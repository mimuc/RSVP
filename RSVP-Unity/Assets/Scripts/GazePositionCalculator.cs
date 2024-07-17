using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePositionCalculator : MonoBehaviour
{
    public GameObject gazeTarget;
    public GameObject pointer;
    public bool showPointer;

    private float distanceFromCamera;

    // Start is called before the first frame update
    void Start()
    {
        // How far the text is from the camera
        distanceFromCamera = gazeTarget.transform.position.z;

    }

    void Update()
    {
        // Avoids division with 0
        if (DataScript.GazeDirection.z != 0)
        {
            // Calculating Collision of Gaze Vector with the Text-Plane

            float rValue = (distanceFromCamera - DataScript.GazeOrigin.z) / DataScript.GazeDirection.z;

            Vector3 hitPoint = DataScript.GazeOrigin + rValue * DataScript.GazeDirection;

            // Correct for mirroring
            hitPoint.x = hitPoint.x * -1;

            if (showPointer)
            {
                pointer.transform.position = hitPoint;
            }

            DataScript.HitPoint = hitPoint;
        }
    }
}