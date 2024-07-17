using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataExport : MonoBehaviour
{
    public int participant_ID;
    private string activeScene;
    private string filename;

    // Start is called before the first frame update
    void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;

        // Set the filename based on the participant ID
        filename = "Participant_" + participant_ID + ".csv";

        // Create the CSV file if it doesn't exist and write the headers
        if (!File.Exists(GetFilePath(filename)))
        {
            string[] headers = { "startPause", "activeScene", "textFile", "speed","phase","pupilDilation_L", "pupilDilation_R", "gazePosition" };
            AppendToCSV(filename, headers);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Change the "," in the float to a "." so it doesn't mess with the CSV
        string dilation_L = DataScript.Dilation_L.ToString().Replace(",", ".");
        string dilation_R = DataScript.Dilation_R.ToString().Replace(",", ".");

        // Create a string array with the data to append
        string[] data = {
            DataScript.StartPause.ToString(),
            activeScene,
            DataScript.ActiveTextFile,
            DataScript.Wpm.ToString(),
            DataScript.Phase,
            dilation_L,
            dilation_R,
            DataScript.HitPoint.ToString()
        };

        // Append the data to the CSV file
        AppendToCSV(filename, data);
    }

    private void AppendToCSV(string filename, string[] data)
    {
        // Combine the directory path and filename
        string filePath = GetFilePath(filename);

        // Append the data to the CSV file
        using (StreamWriter writer = File.AppendText(filePath))
        {
            string dataLine = string.Join(",", data);
            writer.WriteLine(dataLine);
        }
    }

    private string GetFilePath(string filename)
    {
        // Create the directory path if it doesn't exist
        string directoryPath = Path.Combine(Application.dataPath, "CSV");
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // Combine the directory path and filename
        return Path.Combine(directoryPath, filename);
    }
}
