using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSV : MonoBehaviour
{
    private string filePath;
    private string delimiter = ","; // Change this if you want to use a different delimiter

    private List<string[]> rowData = new List<string[]>();

    // Add a new row of data to the CSV
    public void AddData(params string[] values)
    {
        rowData.Add(values);
    }

    // Write the data to a CSV file
    public void Save(string fileName)
    {
        string defaultName = "data.csv";
        string directory = Application.persistentDataPath;
        filePath = EditorUtility.SaveFilePanel("Save CSV", directory, defaultName, "csv");

        // Create a StreamWriter to write data to the file
        StreamWriter writer = new StreamWriter(filePath);

        // Write each row of data to the file
        foreach (string[] row in rowData)
        {
            string line = string.Join(delimiter, row);
            writer.WriteLine(line);
        }

        // Close the StreamWriter
        writer.Close();

        Debug.Log("CSV file saved to: " + filePath);
    }
}
