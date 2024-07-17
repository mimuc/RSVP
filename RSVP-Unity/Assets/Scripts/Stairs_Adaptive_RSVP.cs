using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using TMPro;
using System.Linq;

public class Stairs_Adaptive_RSVP : MonoBehaviour
{

    public TMP_Text display;
    public int startPause;
    public string textFile;
    public bool useSpecificFile;
    public int speedIncrement;
    public int startSpeed;

    private string[] inputArray;
    private int counter;
    private float pupilBaseline;
    private List<float> calibrationData;
    private int numberOfDataPoints;
    private float idealDilation;
    private float threshhold;
    private bool fineTunig;


    private void Start()
    {
        string directory = "Assets/TextFiles/";
        string randomOrderFile = directory + "RandomOrder.txt";
        DataScript.StartPause = startPause;
        // Starting with 200 wpm as start speed
        DataScript.Wpm = startSpeed;

        counter = 0;
        pupilBaseline = 0;
        calibrationData = new List<float>();
        fineTunig = false;

        // Waiting Period between steps
        numberOfDataPoints = 450;
        idealDilation = 0.21515628F;
        threshhold = 0.05F;

        // Either use a specific File or the randomly generated order
        if (useSpecificFile)
        {
            DataScript.ActiveTextFile = textFile;

            inputArray = readFile(directory + "MainStudy_1/" + textFile + ".txt").Split(' ');
            Debug.Log("Using File " + textFile);

            StartCoroutine(RSVP_Display());
        }
        else
        {
            // Get the random Order from the txt file and convert it to an array while removing initial linebreaks or whitespaces
            string randomOrder = readFile(randomOrderFile).Trim();
            char[] randomOrderArray = randomOrder.ToCharArray();

            if (randomOrderArray.Length != 0)
            {
                // Specify the used thext file for CSV export
                DataScript.ActiveTextFile = randomOrderArray[0].ToString();
                textFile = randomOrderArray[0].ToString();

                inputArray = readFile(directory + "MainStudy_1/" + textFile + ".txt").Split(' ');
                Debug.Log("Using File " + textFile);

                // Delete the first item in the array that has just been "used"
                DeleteFirstItem(ref randomOrderArray);

                // Clear the txt file from previous uses
                using (FileStream fileStream = new FileStream(randomOrderFile, FileMode.Truncate))
                {
                    // Set the length of the file stream to 0 to clear its contents
                    fileStream.SetLength(0);
                }

                // Write the randomly generated order into a text file with one less file name
                using (StreamWriter writer = new StreamWriter(randomOrderFile))
                {
                    foreach (var file in randomOrderArray)
                    {
                        writer.Write(file);
                    }
                }

                StartCoroutine(RSVP_Display());
            }
            else
            {
                Debug.Log("No more files left!");
            }
        }
    }

    private void Update()
    {
        if (DataScript.Phase == "calibration")
        {
            calibrationData.Add(DataScript.Dilation_L);
        }
        else
        {
            if (pupilBaseline == 0)
            {
                // Drop the first 450 values
                calibrationData.RemoveRange(0, 449);

                float standardDeviation = CalculateStandardDeviation(calibrationData);

                Debug.Log("Current standard deviation: " + standardDeviation);

                float lowerEnd = calibrationData.Average() - 3 * standardDeviation;
                float upperEnd = calibrationData.Average() + 3 * standardDeviation;

                List<float> filteredCalibrationData = calibrationData
                    .Where(dataPoint => dataPoint >= lowerEnd && dataPoint <= upperEnd)
                    .ToList();

                pupilBaseline = filteredCalibrationData.Average();

                Debug.Log("Current baseline: " + pupilBaseline);
            }

            // Check if the number of datapoints has been reached already
            if (counter != numberOfDataPoints)
            {
                counter++;
            }
            else
            {
                // Reset the counter
                counter = 0;

                float relativeDilation = DataScript.Dilation_L - pupilBaseline;

                // Increase the speed if the pupil dilation is higher then the old value, decrease it if its lower
                if (relativeDilation <= idealDilation && !fineTunig)
                {
                    DataScript.Wpm = DataScript.Wpm + speedIncrement;

                    Debug.Log("Speed changed to " + DataScript.Wpm);
                    Debug.Log("Current relative Dilation: " + (DataScript.Dilation_L - pupilBaseline));
                }
                else
                {
                    Debug.Log("Entering finetuning");
                    fineTunig = true;

                    if (relativeDilation >= idealDilation + threshhold)
                    {
                        DataScript.Wpm = DataScript.Wpm - speedIncrement / 2;

                        Debug.Log("Speed finetuned to " + DataScript.Wpm);
                    }
                    else
                    {
                        if (relativeDilation <= idealDilation - threshhold)
                        {
                            DataScript.Wpm = DataScript.Wpm + speedIncrement / 2;

                            Debug.Log("Speed finetuned to " + DataScript.Wpm);
                        }

                        // else do nothing, because we are within the ideal range
                    }
                }
            }
        }
    }


    IEnumerator RSVP_Display()
    {
        // Specify active phase before starting wait-timer
        DataScript.Phase = "calibration";
        // Wait before start
        yield return new WaitForSeconds(startPause);

        // Specify active phase before starting RSVP
        DataScript.Phase = "test";

        foreach (var word in inputArray)
        {
            display.text = word;
            yield return new WaitForSeconds(1 / (DataScript.Wpm / 60f));

        }

        // Specify phase as "finished" once RSVP is done
        DataScript.Phase = "finished";
    }

    string readFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string content = reader.ReadToEnd();
            return content;
        }
    }

    static void DeleteFirstItem<T>(ref T[] array)
    {
        T[] newArray = new T[array.Length - 1];
        Array.Copy(array, 1, newArray, 0, newArray.Length);
        array = newArray;
    }

    static float CalculateStandardDeviation(IEnumerable<float> values)
    {
        float avg = (float)values.Average();
        return (float)Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
    }
}
