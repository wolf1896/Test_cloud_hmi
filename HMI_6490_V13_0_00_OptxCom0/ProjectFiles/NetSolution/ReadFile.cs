#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using System.IO;
using FTOptix.Report;
using System.Threading;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using FTOptix.Store;
using FluentFTP.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
#endregion

public class ReadFile : BaseNetLogic
{
    private string fileToRead;
    private string fileName;
    private string fileExtension;

    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    
    [ExportMethod]
    public void ClearLog()
    {
        var myStore = Project.Current.Get<Store>("DataStores/EmbeddedDatabase");
        // Get a specific table by name
        var myTable = myStore.Tables.Get<Table>("BeckhoffLOG");
            
        Object[,] ResultSet;
        String[] Header;
        myStore.Query("DELETE FROM BeckhoffLOG", out Header, out ResultSet);
    }

    [ExportMethod]
    public void ReadLogs() // This method creates a report of a file which is read line by line
    {
        fileToRead = new ResourceUri(LogicObject.GetVariable("fileToRead").Value).Uri; // Complete path of the file to read
        fileNameMethod(); // Method whose target is to define file name and file extension
        FileParserForDatabase(); // Method whose target is to create a label inside a vertical layout (path described inside the method)
        Thread.Sleep(100); // Waits 100 milliseconds
    }

    public void fileNameMethod() { // This method returns file name without extension from its path
        string[] filePathNames = fileToRead.Split("\\");
        string[] fileNameExtension = (filePathNames[(filePathNames.Length)-1]).Split(".");
        fileName = fileNameExtension[0]; // Name of the file
        fileExtension = fileNameExtension[1]; // Extension of the file
    }

    
    public void FileParserForDatabase()
    {
        var Label = InformationModel.Make<Label>("Label"); // Creates a "Label" object type
        var VerticalLayout = Owner.Get("VerticalLayout"); // Sets VerticalLayout equal to the object "VerticalLayout" already existing
        string text = ""; // Defines a "string" variable
        
        try
        {
            
            using (StreamReader reader = new StreamReader(fileToRead)) // Creates a new StreamReader in order to read the input file
            {
                text = reader.ReadToEnd(); // Read the whole text of the file
            }
            
            //Hold the file value List[Rows][Columns]
            List<List<string>> listOfLists = new List<List<string>>();

            // Split the text by the marker ";"
            string[] splitText = text.Split('\n');
            
            int i = splitText.Count() -1; // Need to know how many element for database creation

            // Define the input format
            string inputFormat = "yyyy-MM-dd-HH:mm:ss.fff";
            
            // Define the output format
            string outputFormat = "yyyy-MM-dd HH:mm:ss.fff";

            foreach (string part in splitText)
            {
                string[] splitText2 = part.Split(';');
                if (splitText2.Count() >1)
                {
                    // Parse the date string
                    DateTime date = DateTime.ParseExact(splitText2[0], inputFormat, CultureInfo.InvariantCulture);
                
                    // Convert the DateTime to the desired format because this "yyyy-MM-dd-HH:mm:ss.fff" is 
                    splitText2[0] = date.ToString(outputFormat);

                    // Create list to add in ListOfList
                    List<string> riga = new List<string>(splitText2);
                    listOfLists.Add(riga);
                }
            }

            var myStore = Project.Current.Get<Store>("DataStores/EmbeddedDatabase");
            // Get a specific table by name
            var myTable = myStore.Tables.Get<Table>("BeckhoffLOG");
            
            // Prepare the header for the insert query (list of columns)
            string[] columns = { "Time","Circuit", "Program", "Type", "Message" };

            // Create the new object, a bidimensional array where the first element
            // is the number of rows to be added, the second one is the number
            // of columns to be added (same size of the columns array)
            var values = new object[i, 5];
            
            for (int c = 0; c < 5; c++)
            {
                for (int r = 0; r < i; r++)
                {
                    // Set some values for each column
                    values[r,c] = listOfLists[r][c];
                }
            }
            
            myTable.Insert(columns, values);

        }
        catch (Exception ex) // In case StreamReader can't start reading the file...
        {
            Log.Warning("Error reading file: " + ex.Message); // ...throws an exception
        }
    }
}
