using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;
using System.IO;

public class Normal_Reading : MonoBehaviour
{
    public TMP_Text display;
    public int startPause;

    private string directory;

    // Start is called before the first frame update
    void Start()
    {
        DataScript.StartPause = startPause;

        directory = "Assets/TextFiles/";
        string randomOrderFile = directory + "RandomOrder.txt";

        string[] textFiles = new string[] { "a", "b" };

        ShuffleArray(textFiles);

        Debug.Log("Random Order generated!");
        DataScript.ActiveTextFile = "NormalReading";

        // Clear the txt file from previous uses
        using (FileStream fileStream = new FileStream(randomOrderFile, FileMode.Truncate))
        {
            // Set the length of the file stream to 0 to clear its contents
            fileStream.SetLength(0);
        }

        // Write the randomly generated order into a txt file for use in the RSVP scene
        foreach (var file in textFiles)
        {
            using (StreamWriter writer = new StreamWriter(randomOrderFile, true))
            {
                writer.WriteLine(file);
            }
        }

        StartCoroutine(Display());
    }

    // Shuffles the array using the Fisher-Yates algorithm
    static void ShuffleArray<T>(T[] array)
    {
        System.Random random = new System.Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            SwapElements(array, i, j);
        }
    }

    static void SwapElements<T>(T[] array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    string readFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string content = reader.ReadToEnd();
            return content;
        }
    }


    IEnumerator Display()
    {
        // Specify active phase before starting wait-timer
        DataScript.Phase = "calibration";
        // Wait before start
        yield return new WaitForSeconds(startPause);

        // Specify active phase before starting RSVP
        DataScript.Phase = "test";

        display.text = readFile(directory + "MainStudy_2/NormalReading.txt");
    }
}
