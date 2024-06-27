#region Using directives
using System;
using FTOptix.HMIProject;
using UAManagedCore;
using FTOptix.NetLogic;
using FTOptix.Core;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using FTOptix.UI;
using FluentFTP;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
#endregion

public class FtpFilesystemBrowserLogic : BaseNetLogic
{
    public override void Start()
    {
        ReadWidgetConfigurationVariables();

        ftpClientWrapper = new FtpClientWrapper(LogicObject);

        ftpClientWrapper.OnConnectionSuccessfullyCompleted += FtpClientWrapper_OnConnectCompleted;
        ftpClientWrapper.OnDisconnectionCompleted += FtpClientWrapper_OnDisconnectionCompleted;
        ftpClientWrapper.OnDownloadCompleted += FtpClientWrapper_OnDownloadCompleted;
        ftpClientWrapper.OnUploadCompleted += FtpClientWrapper_OnUploadCompleted;

        ftpClientWrapper.progressAction = FtpProgressAction;

        pathVariable.VariableChange += PathVariable_VariableChange;
        filesDatagrid.OnUserSelectionChanged += FilesDatagrid_OnUserSelectionChanged;
        ConnectFtpClient();
        periodicTask = new PeriodicTask(UpdateStatisticsTask, 1000, LogicObject);
        periodicTask.Start();
    }

    public override void Stop()
    {
        ftpClientWrapper.OnConnectionSuccessfullyCompleted -= FtpClientWrapper_OnConnectCompleted;
        ftpClientWrapper.OnDisconnectionCompleted -= FtpClientWrapper_OnDisconnectionCompleted;
        ftpClientWrapper.OnDownloadCompleted -= FtpClientWrapper_OnDownloadCompleted;
        ftpClientWrapper.OnUploadCompleted -= FtpClientWrapper_OnUploadCompleted;

        pathVariable.VariableChange -= PathVariable_VariableChange;
        filesDatagrid.OnUserSelectionChanged -= FilesDatagrid_OnUserSelectionChanged;

        DisconnectFtpClient();
    }

    [ExportMethod]
    public void ConnectDisconnect()
    {
        bool isClientRunning = ftpClientIsRunningVariable.Value;
        if (!isClientRunning)
            ConnectFtpClient();
        else
            DisconnectFtpClient();
    }

    [ExportMethod]
    public void ConnectFtpClient()
    {
        var ftpClientOptions = new FtpClientWrapper.FtpOptions
        {
            ftpServerIPAddress = ftpServerIPAddressVariable.Value,
            ftpServerPort = (ushort)ftpServerPortVariable.Value,
            ftpServerUsername = usernameVariable.Value,
            ftpServerUserPassword = passwordVariable.Value,
            connectionTimeout = connectionTimeout,
            clientCertificateFile = new ResourceUri(AddNamespacePrefixToFTOptixRuntimeFolder(clientCertificateFileVariable.Value)).Uri,
            clientPrivateKeyFile = new ResourceUri(AddNamespacePrefixToFTOptixRuntimeFolder(clientPrivateKeyFileVariable.Value)).Uri,
            useFtps = useFtpsVariable.Value,
            tlsVersion = ReadTlsVersionConfiguration()
        };

        ftpClientWrapper.Connect(ftpClientOptions);
    }

    private FtpClientWrapper.TlsVersion ReadTlsVersionConfiguration()
    {
        return (FtpClientWrapper.TlsVersion)(int)tlsVersionVariable.Value;
    }

    [ExportMethod]
    public void DisconnectFtpClient()
    {
        ftpClientWrapper.Disconnect();
    }

    [ExportMethod]
    public void DownloadItemValidation(string localFolderPath, string remoteItemPath)
    {
        if (!DownloadCanBeExecuted())
            return;

        string itemPath;
        try
        {
            var localFolderExpandedPath = new ResourceUri(localFolderPath).Uri;
            itemPath = Path.Combine(localFolderExpandedPath, FtpPath.GetLastToken(remoteItemPath));
        }
        catch (Exception ex)
        {
            Log.Error("FtpFilesystemBrowserLogic", $"An exception occurred while reading the path of the item to be downloaded: {ex.Message}");
            return;
        }

        if (File.Exists(itemPath) || Directory.Exists(itemPath))
            UICommands.OpenDialog(Owner, operationDialogType, downloadDialogContext.NodeId);
        else
            DownloadItem(localFolderPath, remoteItemPath, false);
    }

    [ExportMethod]
    public void DownloadItem(string localFolderPath, string remoteItemPath, bool overwriteItemIfExists)
    {
        if (!DownloadCanBeExecuted())
            return;

        operationInProgressVariable.Value = true;
        ftpClientWrapper.DownloadItem(localFolderPath, remoteItemPath, overwriteItemIfExists);
    }

    [ExportMethod]
    public void UploadItemValidation(string localItemPath, string remoteFolderPath)
    {
        if (!UploadCanBeExecuted())
            return;

        var remoteExpandedItemPath = FtpPath.Combine(remoteFolderPath, Path.GetFileName(localItemPath));
        if (ftpClientWrapper.IsFile(remoteExpandedItemPath) || ftpClientWrapper.IsDirectory(remoteExpandedItemPath))
            UICommands.OpenDialog(Owner, operationDialogType, uploadDialogContext.NodeId);
        else
            UploadItem(localItemPath, remoteFolderPath, false);
    }

    [ExportMethod]
    public void UploadItem(string localItemPath, string remoteFolderPath, bool overwriteItemIfExists)
    {
        if (!UploadCanBeExecuted())
            return;

        operationInProgressVariable.Value = true;
        ftpClientWrapper.UploadItem(localItemPath, remoteFolderPath, overwriteItemIfExists);
    }

    private string AddNamespacePrefixToFTOptixRuntimeFolder(string resourceUriString)
    {
        if (resourceUriString.StartsWith("%APPLICATIONDIR") || resourceUriString.StartsWith("%PROJECTDIR"))
            resourceUriString = $"ns={LogicObject.NodeId.NamespaceIndex};{resourceUriString}";

        return resourceUriString;
    }

    private void FtpClientWrapper_OnConnectCompleted(object sender, EventArgs e)
    {
        Browse(pathVariable.Value);

        ftpClientIsRunningVariable.Value = true;
        filesDatagrid.SelectedItem = NodeId.Empty;
    }

    private void FtpClientWrapper_OnDisconnectionCompleted(object sender, EventArgs e)
    {
        ftpClientIsRunningVariable.Value = false;
        operationInProgressVariable.Value = false;
        etaVariable.Value = string.Empty;
    }

    private void FtpClientWrapper_OnUploadCompleted(object sender, FtpClientWrapper.UploadOperationCompletedEventArgs e)
    {
        if (e.isSuccess)
            Browse(e.remoteFolderPath);

        operationInProgressVariable.Value = false;
    }

    private void FtpClientWrapper_OnDownloadCompleted(object sender, FtpClientWrapper.DownloadOperationCompletedEventArgs e)
    {
        if (e.isSuccess)
            AddFileEntryToLocalFilesystem(e.remoteItemPath, e.itemIsDirectory);

        operationInProgressVariable.Value = false;
    }

    private void AddFileEntryToLocalFilesystem(string remoteItemPath, bool isDirectory)
    {
        var remoteItemName = FtpPath.GetLastToken(remoteItemPath);

        var localDatagridModel = InformationModel.Get(LogicObject.GetVariable("LocalFileSystemNode").Value);
        if (localDatagridModel.Get<FileEntry>(remoteItemName) != null)
            return;

        var downloadedFile = InformationModel.MakeObject<FileEntry>(remoteItemName);
        downloadedFile.FileName = remoteItemName;
        downloadedFile.Size = ftpClientWrapper.GetFileSize(remoteItemPath);
        downloadedFile.IsDirectory = isDirectory;

        localDatagridModel.Add(downloadedFile);
    }

    private void FtpProgressAction(FtpProgress ftpProgress)
    {
        eta = ftpProgress.ETA;
    }

    private void UpdateStatisticsTask()
    {
        etaVariable.Value = eta.ToString(@"hh\:mm\:ss");
    }

    #region FtpFilesystemBrowser browsing methods
    private void FilesDatagrid_OnUserSelectionChanged(object sender, UserSelectionChangedEvent e)
    {
        var nodeId = e.SelectedItem;
        if (nodeId == null || nodeId.IsEmpty)
            return;

        var entry = InformationModel.Get(nodeId) as FtpFileEntry;
        if (entry == null)
            return;

        UpdateCurrentPath(entry.FileName);
    }

    private void UpdateCurrentPath(string lastPathToken)
    {
        var currentFtpPath = pathVariable.Value;

        if (lastPathToken == "..")
            SetPathsToParentFolder(currentFtpPath);
        else
            SetPathsToSelectedItem(currentFtpPath, lastPathToken);
    }

    private void SetPathsToParentFolder(string currentFtpPath)
    {
        var updatedFtpPath = FtpPath.GetParentPath(currentFtpPath);
        if (!ftpClientWrapper.IsDirectory(updatedFtpPath))
        {
            Log.Error("FtpFilesystemBrowserLogic", $"Folder '{updatedFtpPath}' does not exist on the FTP server");
            return;
        }

        pathVariable.Value = updatedFtpPath;
        fullPathVariable.Value = updatedFtpPath;
    }

    private void SetPathsToSelectedItem(string currentFtpPath, string lastPathToken)
    {
        string updatedFtpPath = FtpPath.Combine(currentFtpPath, lastPathToken);
        fullPathVariable.Value = updatedFtpPath;

        // FolderPath variable contains only a path to a folder.
        // It is not necessary to update this variable if the resulting path is a file.
        if (ftpClientWrapper.IsDirectory(updatedFtpPath))
            pathVariable.Value = updatedFtpPath;
    }

    private void PathVariable_VariableChange(object sender, VariableChangeEventArgs e)
    {
        var updatedPathVariableValue = e.NewValue;
        if (updatedPathVariableValue == null)
        {
            Log.Error("FtpFilesystemBrowserLogic", "FolderPath variable cannot be empty");
            return;
        }

        Browse(updatedPathVariableValue);
    }

    private void Browse(string path)
    {
        if (path == string.Empty)
        {
            Log.Warning("FtpFilesystemBrowserLogic", "FolderPath variable cannot be empty");
            return;
        }

        var filesList = LogicObject.GetObject("FtpFilesList");
        if (filesList == null)
        {
            Log.Warning("FtpFilesystemBrowserLogic", "FtpFilesList object not found");
            return;
        }

        // Set datagrid model to Empty
        filesDatagrid.Model = NodeId.Empty;

        InitializeFilesList(path, filesList);

        // Set back datagrid model to the updated filesList
        filesDatagrid.Model = filesList.NodeId;
    }

    private void InitializeFilesList(string path, IUAObject filesList)
    {
        CleanFilesList(filesList);

        var backEntry = InformationModel.MakeObject<FtpFileEntry>("back");
        backEntry.FileName = "..";
        backEntry.IsDirectory = true;
        filesList.Add(backEntry);
        
        string extensions = filterVariable.Value;
        var extensionsList = extensions.Split(';').ToList();

        FillFoldersList(path, filesList);
        FillFilesList(path, filesList, extensionsList);
    }

    private void CleanFilesList(IUAObject filesList)
    {
        filesList.Children.ToList().ForEach((entry) => entry.Delete());
    }

    private void FillFoldersList(string path, IUAObject filesList)
    {
        var directories = ftpClientWrapper.ListAllFolders(path);
        foreach (var dir in directories)
        {
            if (!ItemMustBeListed(dir))
                continue;

            var fileSystemEntry = CreateFilesystemEntry(dir, true);
            filesList.Add(fileSystemEntry);
        }
    }

    private void FillFilesList(string path, IUAObject filesList, List<string> extensionsList)
    {
        var files = ftpClientWrapper.ListAllFiles(path);
        foreach (var file in files)
        {
            if (!ItemMustBeListed(file))
                continue;

            if (!AllFilesFilterSelected(extensionsList) && FileHasToBeFiltered(extensionsList, file))
                continue;

            var fileSystemEntry = CreateFilesystemEntry(file, false);
            filesList.Add(fileSystemEntry);
        }
    }

    private bool BackEntryMustBeAdded(string path)
    {
        return path != "/";
    }

    private FtpFileEntry CreateFilesystemEntry(FtpListItem entry, bool isDirectory)
    {
        var fileSystemEntry = InformationModel.MakeObject<FtpFileEntry>(entry.Name);
        fileSystemEntry.FileName = entry.Name;
        fileSystemEntry.IsDirectory = isDirectory;
        if (!isDirectory)
        {
            fileSystemEntry.Size = (ulong)Math.Round(entry.Size / 1000.0);
        }

        return fileSystemEntry;
    }

    // https://github.com/robinrodricks/FluentFTP/issues/386
    private bool ItemMustBeListed(FtpListItem filelistItem)
    {
        // It is not possible to detect if a file on a Windows server is hidden/junction point
        if (ftpClientWrapper.IsWindowsFtpServer())
            return true;

        bool showHiddenFiles = showHiddenFilesVariable.Value;
        if (!showHiddenFiles && filelistItem.Name.StartsWith("."))
            return false;

        return true;
    }

    // All files are shown if the filter is empty or "*.*" is present in the filter list
    private bool AllFilesFilterSelected(List<string> extensionsList)
    {
        return extensionsList.Contains("*.*") || (extensionsList.Count == 1 && extensionsList.Contains(string.Empty));
    }

    private bool FileHasToBeFiltered(List<string> extensionsList, FtpListItem file)
    {
        string fileName = file.Name;
        var extenstionSeparatorIndex = fileName.LastIndexOf('.');
        if (extenstionSeparatorIndex == -1)
            return false;

        string extension = fileName.Substring(extenstionSeparatorIndex);
        return !extensionsList.Contains($"*{extension}");
    }

    #endregion

    #region FtpClient classes
    private class FtpClientWrapper
    {
        public enum TlsVersion
        {
            Version_1_2 = 0,
            Version_1_3 = 1
        }

        public class FtpOptions
        {
            public string ftpServerIPAddress;
            public int ftpServerPort;
            public string ftpServerUsername;
            public string ftpServerUserPassword;
            public int connectionTimeout;
            public string clientCertificateFile;
            public string clientPrivateKeyFile;
            public bool useFtps;
            public TlsVersion tlsVersion = TlsVersion.Version_1_2;
        }

        public class DownloadOperationCompletedEventArgs : EventArgs
        {
            public string remoteItemPath;
            public string localFolderPath;
            public bool itemIsDirectory;
            public bool isSuccess = false;
        }

        public class UploadOperationCompletedEventArgs : EventArgs
        {
            public string localItemPath;
            public string remoteFolderPath;
            public bool itemIsDirectory;
            public bool isSuccess = false;
        }

        public event EventHandler<EventArgs> OnConnectionSuccessfullyCompleted = delegate { };
        public event EventHandler<EventArgs> OnDisconnectionCompleted = delegate { };
        public event EventHandler<DownloadOperationCompletedEventArgs> OnDownloadCompleted = delegate { };
        public event EventHandler<UploadOperationCompletedEventArgs> OnUploadCompleted = delegate { };

        public Action<FtpProgress> progressAction = delegate { };

        public FtpClientWrapper(IUAObject logicObject)
        {
            this.logicObject = logicObject;
            certificateUtils = new CertificateUtils(logicObject);
        }

        public IEnumerable<FtpListItem> ListAllFolders(string path)
        {
            try
            {
                return ftpClient.GetListing(path)
                    .Where(entry => entry.Type == FtpObjectType.Directory);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read the list of folders from the FTP server: {GetExceptionMessage(ex)}");
            }
        }

        public IEnumerable<FtpListItem> ListAllFiles(string path)
        {
            try
            {
                return ftpClient.GetListing(path)
                    .Where(entry => entry.Type == FtpObjectType.File);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read the list of files from the FTP server: {GetExceptionMessage(ex)}");
            }
        }

        public bool IsWindowsFtpServer()
        {
            try
            {
                return ftpClient.ServerOS == FtpOperatingSystem.Windows;
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Unable to detect the FTP server's operating system: {GetExceptionMessage(ex)}");
                return false;
            }
        }

        public bool IsDirectory(string path)
        {
            try
            {
                return ftpClient.DirectoryExists(path);
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Unable to retrieve folder '{path}' information from the FTP server: {GetExceptionMessage(ex)}");
                return false;
            }
        }

        public bool IsFile(string path)
        {
            try
            {
                return ftpClient.FileExists(path);
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Unable to retrieve file '{path}' information from the FTP server: {GetExceptionMessage(ex)}");
                return false;
            }
        }

        public long GetFileSize(string filePath)
        {
            try
            {
                var size = ftpClient.GetFileSize(filePath);
                if (size == -1)
                    return 0;

                return (long)Math.Round(size / 1000.0, 0);
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Unable to read file size of file '{filePath}': {GetExceptionMessage(ex)}");
                return 0;
            }
        }

        public void Connect(FtpOptions ftpClientOptions)
        {
            connectionTask = new LongRunningTask(ConnectLongRunningTask, ftpClientOptions, logicObject);
            connectionTask.Start();
        }

        public void Disconnect()
        {
            var disconnectLongRunningTask = new LongRunningTask(DisconnectLongRunningTask, logicObject);
            disconnectLongRunningTask.Start();
        }

        public void DownloadItem(string localFolderPath, string remoteItemPath, bool overwriteItemIfExists)
        {
            var localFolderPathResourceUri = new ResourceUri(localFolderPath);

            var downloadLongRunningTaskArguments = new DownloadLongRunningTaskArguments
            {
                localFolderPath = localFolderPathResourceUri.Uri,
                remoteItemPath = remoteItemPath,
                overwriteItemIfExists = overwriteItemIfExists
            };
            var downloadLongRunningTask = new LongRunningTask(DownloadLongRunningTask, downloadLongRunningTaskArguments, logicObject);

            downloadLongRunningTask.Start();
        }

        public void UploadItem(string localItemPath, string remoteFolderPath, bool overwriteItemIfExists)
        {
            var localItemPathResourceUri = new ResourceUri(localItemPath);

            var uploadLongRunningTaskArguments = new UploadLongRunningTaskArguments
            {
                localItemPath = localItemPathResourceUri.Uri,
                remoteFolderPath = remoteFolderPath,
                overwriteItemIfExists = overwriteItemIfExists
            };
            var uploadLongRunningTask = new LongRunningTask(UploadLongRunningTask, uploadLongRunningTaskArguments, logicObject);

            uploadLongRunningTask.Start();
        }

        private void ConnectLongRunningTask(LongRunningTask task, object arguments)
        {
            try
            {
                var ftpClientOptions = arguments as FtpOptions;
                var ftpConfig = new FtpConfig()
                {
                    ConnectTimeout = ftpClientOptions.connectionTimeout,
                    DataConnectionType = FtpDataConnectionType.AutoPassive
                };

                if (ftpClientOptions.useFtps)
                {
                    ftpConfig.EncryptionMode = FtpEncryptionMode.Explicit;

                    switch (ftpClientOptions.tlsVersion)
                    {
                        case TlsVersion.Version_1_3:
                            ftpConfig.SslProtocols = SslProtocols.Tls13;
                            break;
                        case TlsVersion.Version_1_2:
                        default:
                            ftpConfig.SslProtocols = SslProtocols.Tls12;
                            break;
                    }

                    var cert = GetCertificate(ftpClientOptions.clientCertificateFile, ftpClientOptions.clientPrivateKeyFile);
                    ftpConfig.ClientCertificates.Add(cert);
                }

                ftpClient = new FtpClient()
                {
                    Host = ftpClientOptions.ftpServerIPAddress,
                    Port = ftpClientOptions.ftpServerPort,
                    Credentials = new NetworkCredential(ftpClientOptions.ftpServerUsername, ftpClientOptions.ftpServerUserPassword),
                    Config = ftpConfig
                };

                if (ftpClientOptions.useFtps)
                    ftpClient.ValidateCertificate += (control, args) => args.Accept = true;

                ftpClient.Connect();
                if (!ftpClient.IsConnected)
                {
                    Log.Error("FtpClientWrapper", $"FTP client connection to {ftpClient.Host} failed, {ftpClient.LastReply.Message}");
                    DisconnectLongRunningTask();
                    return;
                }

                Log.Info("FtpClientWrapper", $"Successfully connected to the FTP server on endpoint {ftpClient.SocketRemoteEndPoint}");
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Could not connect to the FTP server {ftpClient.Host}: {GetExceptionMessage(ex)}");
                DisconnectLongRunningTask();
                return;
            }

            OnConnectionSuccessfullyCompleted(this, EventArgs.Empty);
        }

        private void DisconnectLongRunningTask()
        {
            try
            {
                ftpClient?.Disconnect();
                ftpClient?.Dispose();

                Log.Info("FtpClientWrapper", "FTP client successfully disconnected");
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"Unable to disconnect FTP client: {GetExceptionMessage(ex)}");
            }
            finally
            {
                ftpClient = null;
                OnDisconnectionCompleted(this, EventArgs.Empty);
            }
        }

        private void DownloadLongRunningTask(LongRunningTask task, object arguments)
        {
            var downloadLongRunningTaskArguments = (DownloadLongRunningTaskArguments)arguments;
            string localFolderPath = downloadLongRunningTaskArguments.localFolderPath;
            string remoteItemPath = downloadLongRunningTaskArguments.remoteItemPath;
            bool overwriteItemIfExists = downloadLongRunningTaskArguments.overwriteItemIfExists;

            var downloadCompletedEventArgs = new DownloadOperationCompletedEventArgs();

            if (!Directory.Exists(localFolderPath))
            {
                Log.Error("FtpClientWrapper", $"Cannot download '{remoteItemPath}' to '{localFolderPath}' because the destination is not a valid folder");
                OnDownloadCompleted(this, downloadCompletedEventArgs);
                return;
            }

            bool itemIsDirectory = IsDirectory(remoteItemPath);

            bool isSuccess;
            if (itemIsDirectory)
                isSuccess = DownloadFolderFromFtpServer(localFolderPath, remoteItemPath, overwriteItemIfExists);
            else
                isSuccess = DownloadFileFromFtpServer(localFolderPath, remoteItemPath, overwriteItemIfExists);

            if (isSuccess)
            {
                downloadCompletedEventArgs.isSuccess = true;
                downloadCompletedEventArgs.itemIsDirectory = itemIsDirectory;
                downloadCompletedEventArgs.remoteItemPath = remoteItemPath;
                downloadCompletedEventArgs.localFolderPath = localFolderPath;
            }

            OnDownloadCompleted(this, downloadCompletedEventArgs);
        }

        private bool DownloadFileFromFtpServer(string localFolderPath, string remoteFilePath, bool overwriteItemIfExists)
        {
            string remoteFileName = FtpPath.GetLastToken(remoteFilePath);
            var fullLocalFilePath = Path.Combine(localFolderPath, remoteFileName);

            Log.Info("FtpClientWrapper", $"Download of file '{remoteFilePath}' to '{fullLocalFilePath}' started");

            try
            {
                FtpLocalExists fileLocalExistsOption = overwriteItemIfExists ? FtpLocalExists.Overwrite : FtpLocalExists.Skip;
                var downloadResult = ftpClient.DownloadFile(fullLocalFilePath, remoteFilePath, fileLocalExistsOption, FtpVerify.None, (progress) =>
                {
                    progressAction(progress);
                });

                if (downloadResult == FtpStatus.Failed)
                {
                    Log.Error("FtpClientWrapper", $"Unable to download file '{remoteFilePath}' to '{fullLocalFilePath}'");
                    return false;
                }

                if (downloadResult == FtpStatus.Skipped)
                {
                    Log.Info("FtpClientWrapper", $"Download of file '{remoteFilePath}' to '{fullLocalFilePath}' was skipped because it already exists");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"An exception occurred while downloading file '{remoteFilePath}': {GetExceptionMessage(ex)}");
                return false;
            }

            Log.Info("FtpClientWrapper", $"File '{remoteFilePath}' downloaded successfully to '{fullLocalFilePath}'");

            return true;
        }

        private bool DownloadFolderFromFtpServer(string localFolderPath, string remoteFolderPath, bool overwriteItemIfExists)
        {
            var remoteFolderName = FtpPath.GetLastToken(remoteFolderPath);
            var fullLocalFolderPath = FtpPath.Combine(localFolderPath, remoteFolderName);

            Log.Info("FtpClientWrapper", $"Download of folder '{remoteFolderPath}' to '{fullLocalFolderPath}' started");

            try
            {
                FtpLocalExists fileLocalExistsOption = overwriteItemIfExists ? FtpLocalExists.Overwrite : FtpLocalExists.Skip;
                var downloadResults = ftpClient.DownloadDirectory(fullLocalFolderPath, remoteFolderPath, FtpFolderSyncMode.Update, fileLocalExistsOption, FtpVerify.None, null, (progress) =>
                {
                    progressAction(progress);
                });

                foreach (var ftpResult in downloadResults)
                {
                    if (ftpResult.IsFailed)
                        Log.Error("FtpClientWrapper", $"Unable to download file '{ftpResult.RemotePath}' to '{fullLocalFolderPath}'");
                }

                Log.Info("FtpClientWrapper", $"Folder '{remoteFolderPath}' downloaded successfully to '{fullLocalFolderPath}'");
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"An exception occurred while downloading folder '{remoteFolderPath}': {GetExceptionMessage(ex)}");
                return false;
            }

            return true;
        }

        private void UploadLongRunningTask(LongRunningTask task, object arguments)
        {
            var uploadLongRunningTaskArguments = (UploadLongRunningTaskArguments)arguments;
            string localItemPath = uploadLongRunningTaskArguments.localItemPath;
            string remoteFolderPath = uploadLongRunningTaskArguments.remoteFolderPath;
            bool overwriteItemIfExists = uploadLongRunningTaskArguments.overwriteItemIfExists;

            var uploadCompletedEventArgs = new UploadOperationCompletedEventArgs();

            if (!IsDirectory(remoteFolderPath))
            {
                Log.Error("FtpClientWrapper", $"Cannot upload '{localItemPath}' to '{remoteFolderPath}' because the destination is not a valid folder");
                OnUploadCompleted(this, uploadCompletedEventArgs);
                return;
            }

            var itemIsDirectory = Directory.Exists(localItemPath);

            bool isSuccess;
            if (itemIsDirectory)
                isSuccess = UploadFolderToFtpServer(localItemPath, remoteFolderPath, overwriteItemIfExists);
            else
                isSuccess = UploadFileToFtpServer(localItemPath, remoteFolderPath, overwriteItemIfExists);

            if (isSuccess)
            {
                uploadCompletedEventArgs.isSuccess = true;
                uploadCompletedEventArgs.itemIsDirectory = itemIsDirectory;
                uploadCompletedEventArgs.localItemPath = localItemPath;
                uploadCompletedEventArgs.remoteFolderPath = remoteFolderPath;
            }

            OnUploadCompleted(this, uploadCompletedEventArgs);
        }

        private bool UploadFileToFtpServer(string localFilePath, string remoteFolderPath, bool overwriteItemIfExists)
        {
            var fullFileRemotePath = FtpPath.Combine(remoteFolderPath, Path.GetFileName(localFilePath));

            Log.Info("FtpClientWrapper", $"Upload of file '{localFilePath}' to '{fullFileRemotePath}' started");

            try
            {
                var fileRemoteExistsOption = overwriteItemIfExists ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip;
                var ftpStatus = ftpClient.UploadFile(localFilePath, fullFileRemotePath, fileRemoteExistsOption, false, FtpVerify.None, (progress) =>
                {
                    progressAction(progress);
                });

                if (ftpStatus == FtpStatus.Failed)
                {
                    Log.Error("FtpClientWrapper", $"Unable to upload file '{localFilePath}' to '{fullFileRemotePath}'");
                    return false;
                }

                if (ftpStatus == FtpStatus.Skipped)
                {
                    Log.Info("FtpClientWrapper", $"Upload of file '{localFilePath}' to '{fullFileRemotePath}' was skipped because it already exists");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"An exception occurred while uploading file '{localFilePath}': {GetExceptionMessage(ex)}");
                return false;
            }

            Log.Info("FtpClientWrapper", $"File '{localFilePath}' uploaded successfully to '{fullFileRemotePath}'");
            return true;
        }

        private bool UploadFolderToFtpServer(string localFolderPath, string remoteFolderPath, bool overwriteItemIfExists)
        {
            var fullFolderRemotePath = FtpPath.Combine(remoteFolderPath, Path.GetFileName(localFolderPath));

            Log.Info("FtpClientWrapper", $"Upload of folder '{localFolderPath}' to '{fullFolderRemotePath}' started");

            try
            {
                var fileRemoteExistsOption = overwriteItemIfExists ? FtpRemoteExists.Overwrite : FtpRemoteExists.Skip;
                var uploadResults = ftpClient.UploadDirectory(localFolderPath, fullFolderRemotePath, FtpFolderSyncMode.Update, fileRemoteExistsOption, FtpVerify.None, null, (progress) =>
                {
                    progressAction(progress);
                });

                foreach (FtpResult ftpResult in uploadResults)
                {
                    if (ftpResult.IsFailed)
                        Log.Error("FtpClientWrapper", $"Unable to upload file '{ftpResult.LocalPath}' to '{fullFolderRemotePath}'");
                }

                Log.Info("FtpClientWrapper", $"Upload of folder '{localFolderPath}' to '{fullFolderRemotePath}' completed");
            }
            catch (Exception ex)
            {
                Log.Error("FtpClientWrapper", $"An exception occurred while uploading file '{localFolderPath}': {GetExceptionMessage(ex)}");
                return false;
            }

            return true;
        }

        private string GetExceptionMessage(Exception ex)
        {
            // FluentFTP library uses sometimes inner exceptions to explain the error.
            var innerExceptionMessage = ex.InnerException?.Message;
            if (string.IsNullOrEmpty(innerExceptionMessage))
                return ex.Message;

            return ex.Message + " InnerException: " + innerExceptionMessage;
        }

        private struct UploadLongRunningTaskArguments
        {
            public string localItemPath;
            public string remoteFolderPath;
            public bool overwriteItemIfExists;
        };

        private struct DownloadLongRunningTaskArguments
        {
            public string localFolderPath;
            public string remoteItemPath;
            public bool overwriteItemIfExists;
        };

        private X509Certificate2 GetCertificate(string clientCertificateFile, string clientPrivateKeyFile)
        {
            if (!string.IsNullOrEmpty(clientCertificateFile) && !string.IsNullOrEmpty(clientPrivateKeyFile))
            {
                return certificateUtils.GetCertificate(clientCertificateFile, clientPrivateKeyFile);
            }
            else
            {
                if (certificateUtils.ShouldGenerateNewCertificate())
                    return new X509Certificate2(certificateUtils.GenerateCertificate());
                else
                    return new X509Certificate2(certificateUtils.GetCertificate());
            }
        }

        #region Certificate Utils
        public class CertificateUtils
        {
            private string certificateFileName;
            private string keyFileName;

            public CertificateUtils(IUANode logicObject)
            {
                var logicObjectId = logicObject.NodeId.Id.ToString();
                certificateFileName = Path.Combine(Project.Current.ApplicationDirectory, "PKI", "Own", "Certs", $"FtpClientCert{logicObjectId}.der");
                keyFileName = Path.Combine(Project.Current.ApplicationDirectory, "PKI", "Own", "Certs", $"FtpClientKey{logicObjectId}.pem");
            }

            public bool ShouldGenerateNewCertificate()
            {
                return File.Exists(certificateFileName) ? false : true;
            }

            public X509Certificate2 GetCertificate(string certificateFile = null, string keyFile = null)
            {
                if (string.IsNullOrEmpty(certificateFile))
                    certificateFile = certificateFileName;
                if (string.IsNullOrEmpty(keyFile))
                    keyFile = keyFileName;

                using (RSA rsaPrivateKey = RSA.Create())
                {
                    var pemPrivateKeyFile = File.ReadAllText(keyFile).ToCharArray();
                    rsaPrivateKey.ImportFromPem(pemPrivateKeyFile);

                    using (X509Certificate2 certificate = new X509Certificate2(certificateFile))
                    using (X509Certificate2 certificateWithPrivateKey = certificate.CopyWithPrivateKey(rsaPrivateKey))
                    {

                        return new X509Certificate2(certificateWithPrivateKey.Export(X509ContentType.Pfx));
                    }
                }
            }


            public X509Certificate2 GenerateCertificate()
            {
                using (var rsa = RSA.Create(4096))
                {
                    CertificateRequest req = new CertificateRequest(
                        "CN=FTPClient",
                        rsa,
                        HashAlgorithmName.SHA256,
                        RSASignaturePadding.Pkcs1);

                    req.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));

                    req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false));

                    req.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.8") }, true));

                    req.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(req.PublicKey, false));

                    Save(keyFileName, rsa.ExportRSAPrivateKey(), "-----BEGIN RSA PRIVATE KEY-----\r\n", "\r\n-----END RSA PRIVATE KEY-----");

                    using (var cert = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(365)))
                    {
                        Save(certificateFileName, cert.Export(X509ContentType.Cert), "-----BEGIN CERTIFICATE-----\r\n", "\r\n-----END CERTIFICATE-----");
                        return GetCertificate();
                    }
                }
            }

            private void Save(string fileName, byte[] certificateRaw, string header, string footer)
            {
                var filePath = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                File.WriteAllText(fileName, $"{header}{Convert.ToBase64String(certificateRaw, Base64FormattingOptions.InsertLineBreaks)}{footer}");
            }
        }
        #endregion

        private FtpClient ftpClient;
        private LongRunningTask connectionTask;
        private readonly IUAObject logicObject;
        private readonly CertificateUtils certificateUtils;
    }

    private static class FtpPath
    {
        public static string Combine(string path1, string path2)
        {
            if (path1 == "/")
                return NormalizePath("/" + path2);

            if (path1.EndsWith("/") || path2.StartsWith("/"))
                return NormalizePath(path1 + path2);

            return NormalizePath(path1 + "/" + path2);
        }

        public static string GetLastToken(string path)
        {
            var normalizedPath = NormalizePath(path);
            var lastForwardSlashIndex = normalizedPath.LastIndexOf("/");
            if (lastForwardSlashIndex == -1)
            {
                Log.Error("FtpClientWrapper", $"The given path '{path}' is not in a valid form");
                return string.Empty;
            }

            var result = normalizedPath.Substring(lastForwardSlashIndex);
            if (result == "/")
                return "FtpRoot";

            return result.Replace("/", string.Empty);
        }

        public static string GetParentPath(string currentPath)
        {
            var normalizedPath = NormalizePath(currentPath);
            var lastForwardSlashIndex = normalizedPath.LastIndexOf("/");
            if (lastForwardSlashIndex == -1)
            {
                Log.Error("FtpClientWrapper", $"The given path '{currentPath}' is not in a valid form");
                return string.Empty;
            }

            // e.g. the parent folder of /myFolder is /
            if (lastForwardSlashIndex == 0)
                return "/";

            return normalizedPath.Substring(0, lastForwardSlashIndex);
        }

        private static string NormalizePath(string ftpPathToNormalize)
        {
            return ftpPathToNormalize.Replace("\\", "/");
        }
    }
    #endregion

    private void ReadWidgetConfigurationVariables()
    {
        pathVariable = LogicObject.GetVariable("Path");
        if (pathVariable == null)
            throw new CoreConfigurationException("Path variable not found in FtpFilesystemBrowserLogic");

        showHiddenFilesVariable = Owner.GetVariable("ShowHiddenFiles");
        if (showHiddenFilesVariable == null)
            throw new CoreConfigurationException("ShowHiddenFiles variable not found");

        filterVariable = Owner.GetVariable("ExtensionFilter");
        if (filterVariable == null)
            throw new CoreConfigurationException("ExtensionFilter variable not found in FtpFilesystemBrowserLogic");

        fullPathVariable = Owner.GetVariable("FullPath");
        if (fullPathVariable == null)
            throw new CoreConfigurationException("FullPath variable not found in FtpFilesystemBrowserLogic");

        operationInProgressVariable = LogicObject.GetVariable("FtpClientOperationInProgress");
        if (operationInProgressVariable == null)
            throw new CoreConfigurationException("FtpClientOperationInProgress variable not found in FtpFilesystemBrowserLogic");

        ftpClientIsRunningVariable = LogicObject.GetVariable("FtpClientIsRunning");
        if (ftpClientIsRunningVariable == null)
            throw new CoreConfigurationException("FtpClientIsRunning variable not found in FtpFilesystemBrowserLogic");

        etaVariable = LogicObject.GetVariable("ETA");
        if (etaVariable == null)
            throw new CoreConfigurationException("ETA variable not found in FtpFilesystemBrowserLogic");

        var operationDialogVariable = LogicObject.GetVariable("OperationDialog");
        if (operationDialogVariable == null)
            throw new CoreConfigurationException("OperationDialog not found in FtpFilesystemBrowserLogic");

        operationDialogType = InformationModel.Get<DialogType>(operationDialogVariable.Value);
        if (operationDialogType == null)
            throw new CoreConfigurationException("OperationDialog type not found in FtpFilesystemBrowserLogic");

        downloadDialogContext = Owner.GetObject("DownloadDialogContext");
        if (downloadDialogContext == null)
            throw new CoreConfigurationException("DownloadDialogContext not found in FtpFilesystemBrowserLogic");

        uploadDialogContext = Owner.GetObject("UploadDialogContext");
        if (uploadDialogContext == null)
            throw new CoreConfigurationException("UploadDialogContext not found in FtpFilesystemBrowserLogic");

        filesDatagrid = Owner.Get<DataGrid>("FtpFilesDataGrid");
        if (filesDatagrid == null)
            throw new CoreConfigurationException("FtpFilesDataGrid not found in FtpFilesystemBrowserLogic");

        // Connection parameters
        var ftpClientWidgetNodeId = LogicObject.GetVariable("FtpClientWidget");
        if (ftpClientWidgetNodeId == null)
            throw new CoreConfigurationException("FtpClientWidget object not found");

        var ftpClientWidget = InformationModel.Get(ftpClientWidgetNodeId.Value);
        if (ftpClientWidget == null)
            throw new CoreConfigurationException("FtpClientWidget node pointer is not set");

        ftpServerIPAddressVariable = ftpClientWidget.GetVariable("FtpServerIPAddress");
        if (ftpServerIPAddressVariable == null)
            throw new CoreConfigurationException("FtpServerIPAddress variable not found in FtpFilesystemBrowserLogic");

        ftpServerPortVariable = ftpClientWidget.GetVariable("FtpServerPort");
        if (ftpServerPortVariable == null)
            throw new CoreConfigurationException("FtpServerPort variable not found in FtpFilesystemBrowserLogic");

        usernameVariable = ftpClientWidget.GetVariable("Username");
        if (usernameVariable == null)
            throw new CoreConfigurationException("Username variable not found in FtpFilesystemBrowserLogic");

        passwordVariable = ftpClientWidget.GetVariable("Password");
        if (passwordVariable == null)
            throw new CoreConfigurationException("Password variable not found in FtpFilesystemBrowserLogic");

        clientCertificateFileVariable = ftpClientWidget.GetVariable("ClientCertificateFile");
        if (clientCertificateFileVariable == null)
            throw new CoreConfigurationException("ClientCertificateFile variable not found");

        clientPrivateKeyFileVariable = ftpClientWidget.GetVariable("ClientPrivateKeyFile");
        if (clientPrivateKeyFileVariable == null)
            throw new CoreConfigurationException("ClientPrivateKeyFile variable not found");

        useFtpsVariable = ftpClientWidget.GetVariable("UseFTPS");
        if (useFtpsVariable == null)
            throw new CoreConfigurationException("UseFTPS variable not found");

        tlsVersionVariable = ftpClientWidget.GetVariable("TLSVersion");
        if (tlsVersionVariable == null)
            throw new CoreConfigurationException("TLSVersion variable not found");
    }

    private bool DownloadCanBeExecuted()
    {
        bool isClientRunning = ftpClientIsRunningVariable.Value;
        if (!isClientRunning)
        {
            Log.Error("FtpFilesystemBrowserLogic", $"Ftp client is not running, cannot download the chosen element");
            return false;
        }

        bool ftpClientIsBusy = operationInProgressVariable.Value;
        if (ftpClientIsBusy)
        {
            Log.Error("FtpFilesystemBrowserLogic", $"Ftp client is busy, cannot download the chosen element");
            return false;
        }

        return true;
    }

    private bool UploadCanBeExecuted()
    {
        bool isClientRunning = ftpClientIsRunningVariable.Value;
        if (!isClientRunning)
        {
            Log.Error("FtpFilesystemBrowserLogic", $"Ftp client is not running, cannot upload the chosen element");
            return false;
        }

        bool ftpClientIsBusy = operationInProgressVariable.Value;
        if (ftpClientIsBusy)
        {
            Log.Error("FtpFilesystemBrowserLogic", $"Ftp client is busy, cannot upload the chosen element");
            return false;
        }

        return true;
    }

    private FtpClientWrapper ftpClientWrapper;

    private IUAVariable pathVariable;
    private IUAVariable fullPathVariable;
    private IUAVariable showHiddenFilesVariable;
    private IUAVariable filterVariable;
    private DialogType operationDialogType;
    private IUAObject downloadDialogContext;
    private IUAObject uploadDialogContext;
    private DataGrid filesDatagrid;
    private IUAVariable ftpServerIPAddressVariable;
    private IUAVariable ftpServerPortVariable;
    private IUAVariable usernameVariable;
    private IUAVariable passwordVariable;
    private IUAVariable ftpClientIsRunningVariable;
    private IUAVariable operationInProgressVariable;
    private IUAVariable etaVariable;
    private IUAVariable clientCertificateFileVariable;
    private IUAVariable clientPrivateKeyFileVariable;
    private IUAVariable useFtpsVariable;
    private IUAVariable tlsVersionVariable;

    private TimeSpan eta = new TimeSpan();
    private PeriodicTask periodicTask;

    private readonly int connectionTimeout = 2000;
}
