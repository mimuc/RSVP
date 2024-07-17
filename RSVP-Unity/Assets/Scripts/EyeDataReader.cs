using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;


// This Script was found here:
// https://forum.htc.com/topic/9341-vive-eye-tracking-at-120hz/

public class EyeDataReader : MonoBehaviour
{
    private static EyeData eyeData = new EyeData();
    private static bool eye_callback_registered = false;

    private void Update()
    {
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
        }
        else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
        {
            SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = false;

        }

    }

    private void OnDisable()
    {
        Release();
    }

    void OnApplicationQuit()
    {
        Release();
    }

    /// <summary>
    /// Release callback thread when disabled or quit
    /// </summary>
    private static void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }

    /// <summary>
    /// Required class for IL2CPP scripting backend support
    /// </summary>
    internal class MonoPInvokeCallbackAttribute : System.Attribute
    {
        public MonoPInvokeCallbackAttribute() { }
    }

    /// <summary>
    /// Eye tracking data callback thread.
    /// Reports data at ~120hz
    /// MonoPInvokeCallback attribute required for IL2CPP scripting backend
    /// </summary>
    /// <param name="eye_data">Reference to latest eye_data</param>
    [MonoPInvokeCallback]
    private static void EyeCallback(ref EyeData eye_data)
    {
        eyeData = eye_data;
        // do stuff with eyeData...

        float leftPupilDiameter = eyeData.verbose_data.left.pupil_diameter_mm;
        float rightPupilDiameter = eyeData.verbose_data.right.pupil_diameter_mm;

        // Check if diameter is not zero and write to DataScript
        if (leftPupilDiameter != -1)
        {
            //Debug.Log("Left Pupil Diameter: " + leftPupilDiameter);
            DataScript.Dilation_L = leftPupilDiameter;
        }

        if (rightPupilDiameter != -1)
        {
            //Debug.Log("Right Pupil Diameter: " + rightPupilDiameter);
            DataScript.Dilation_R = rightPupilDiameter;
        }


        
                // Retrieve the average gaze direction from both eyes
                Vector3 gazeDirection = (eyeData.verbose_data.left.gaze_direction_normalized + eyeData.verbose_data.right.gaze_direction_normalized) / 2;

                if (gazeDirection != Vector3.zero)
                {
                    DataScript.GazeDirection = gazeDirection;
                }

                // Get average gazeOrigin
                Vector3 gazeOrigin = (eyeData.verbose_data.left.gaze_origin_mm + eyeData.verbose_data.right.gaze_origin_mm) / 2;

                if (gazeOrigin != Vector3.zero)
                {
                    DataScript.GazeOrigin = gazeOrigin;
                }
    }
}