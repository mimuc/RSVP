# About #
This adaptive RSVP demo was created by Philipp Thalhammer and can be used by anyone.  
For the setup a *HTC Vive Pro Eye* was used.

# General Setup #
You will need to install the following software:
- Unity
- SRanipal Eye Tracking SDK (for Unity)
(if Unity crashed try using a older version of the SDK like described here:  
https://forum.htc.com/topic/14152-vive-eye-tracking-sdk-makes-unity-project-crashing/)
- SRanipal Runtime
(if you get an error message try the approach described here:  
https://forum.htc.com/topic/10439-vive-sranipal-installer-installation-failed-error-1001-the-system-cannot-find-the-file-specified)
- Steam VR

For the basic setup on your PC AND in Unity just follow the tutorial found here:  
https://developer.tobii.com/xr/develop/xr-sdk/getting-started/vive-pro-eye/

# Scenes #
Under *Assets/ViveSR/Scenes/Eye* you can find two Sample scenes provided by the SDK that show a basic mirror avatar to check if eye tracking is working properly.  
The other relevant scenes can be found under *Assets/Scenes*

## RSVP ##
All data this Scene collects is saved under *Assets/CSV*. Currently the *startPause*, *activeScene*, *textFile*, *speed*, *phase*, *pupil dilation L/R* and *gaze position* are saved under *participant_ID.csv*.
This scene offers some customization within the editor. Everything can be adjusted on the *Manager*-Object within the scene.  

### Simple_RSVP ###
#### Speed ####
Adjust the speed in words per minute

#### Start Pause ####
Adjust the initial pause timer at the beginning (in seconds)

#### Text File ####
Specify a text-file (0-9) to be used during testing. Only works if *Use Specific File* is set to true.

#### Use Specific File ####
Uses a specific text-file (specified with the value above) instead of a following the randomized order.
Careful: The random order only works if the txt-file *RandomOrder* contains a random order that was generated in the *Normal_Reading* Scene.

### Data Export ###
#### Participant_ID ####
Specifies the Participant ID in the created *CSV-File*

### Gaze Position Calculator ###
#### Gaze Target ####
Needs to be assigned the Text-Object.

#### Pointer ####
Needs to be assigned the Pointer Object (could be any 3D object).

#### Show Pointer ####
Toggle Pointer on or off.

## NormalReading ##
This scene is used as a baseline measurement of reading speed without RSVP. Upon activation a random order of 9 (a - i) text-files is generated and saved into the *RandomOrder* txt file to use in the *RSVP* scene. The *NormalReading* scene always uses the same text-file (NormalReading.txt).


*All other variables are analog to the ones specified above.*

## Adaptive RSVP simple ##
All data this Scene collects is saved under *Assets/CSV*. Currently the *startPause*, *activeScene*, *textFile*, *speed*, *phase*, *pupil dilation L/R* and *gaze position* are saved under *participant_ID.csv*.
This scene offers some customization within the editor. Everything can be adjusted on the *Manager*-Object within the scene.

This is a crude adaptive prototype, that will compare the average pupil dilation of x amount of data-points to each other. If the dilation is increasing, the speed gets increased, if its decreasing the speed gets lowered.

### Simple_Adaptive_RSVP ###
#### Speed Increment ####
The increment in words per minute the speed gets changed.

#### Number of Data-Points ####
The amount of data-points that gets compared in the adaptive system.

#### Start Speed ####
The speed at which the adaptive system starts.

*All other variables are analog to the ones specified above.*