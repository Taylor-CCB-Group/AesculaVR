# **What Is AesculaVR?**
AesculaVR is a piece of VR software designed to track and record positions, distances and rotations using the HTC Vive Trackers.

**Setting Up VR:**
**Download SteamVR:**
* [This is written on the 01//04/2020, Things may have changed]*
Before using Virtual Reality *(VR)*, you’ll need to download SteamVR. To do so, Go to[ https://store.steampowered.com/](https://store.steampowered.com/) and click on “Install Steam”.  *(At the time of writing, it’s a green box at the top right of the page)*.  You should then follow their instructions.
Once you have created an account and have logged into the steam client, go to the Library Tab, click on “Games” (*On the left side of the screen*) and change it to “Tools”. Go through the List until and find “SteamVR” and install it.

## **Download AesculaVR:**
To set up AesculaVR you’ll need to download the software from GitHub. Once that is done, you’ll need to decompress the files into their own folder. *(It doesn’t matter where)*. Once that is done, you can click on the .exe file to open AesculaVR.
**Loading in your own Objects:**
To load in your own objects, You’ll need to go to your documents folder *(After running AesculaVR at least once). *You should see a folder called “AesculaVR”, Open it, then go to “Objects”. You should move your object files into this folder before running AesculaVR.
Note: AesculaVR only supports the ”.Obj” file type.

(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

**The Editor:**
When you open AesculaVR you’ll be dropped into the editor. From you can load in objects and set up your measurements, before saving them to a “Trackable Object”

## **Trackable Objects:**
Trackable objects are the *things* you’ll load into the recorder. They are a combination of measures and objects. Each trackable object is tied to a single tracker.

## **Measures:**
Measures represent the values you want to record. At the moment there are four types of measures which are Vectors, Points, Planes and Triangular planes.
| 
### **Vectors:** | ![](WARN_REPLACE_IMG_URL) | 
| --- | --- |
| Vectors represent a line or direction. |  | 

### 
| 
### **Points:** | ![](WARN_REPLACE_IMG_URL) | 
| --- | --- |
| Points represent a position. |  | 

| 
### **Planes and Triangular Planes.** | ![](WARN_REPLACE_IMG_URL)![](WARN_REPLACE_IMG_URL) | 
| --- | --- |
| Planes represent a flat surface. Specifically, a plane represents a Normal to a surface. The difference between a triangular plane and plane is how you create and modify them.
 |  | 

## 

## **Objects:**
Objects are models you want to see. If you want to see a representation of a Hip, Ship or any other physical object, you can load them in as an object.

## **Tools:**
You can use tools to help you move objects and measurements into the position you want.
| 
### **Fine Movement Tool:** |  | 
| --- | --- |
| The Fine movement tool can allow you to move measures and objects slower or faster than normal. You can also constrain which Axis you want to move the measures and objects on. |  | 

| 
### **Smooth Transformation Tool:** |  | 
| --- | --- |
| The Fine movement tool can allow you to move measures and objects slower or faster than normal. You can also constrain which Axis you want to move the measures and objects on. |  | 

(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

**Trackers:**
Here you can select which tracker is active. Measures and Objects are created and attached to the active tracker.

## **Moving to the Recorder:**
To move the recorder mode, Press the button that’s shaped like a stopwatch.

(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

**The Recorder:**
After you have set up your trackable objects, you’ll want to record some data and the recorder will let you do just that.
A recording will save out the values for each of your trackable objects.

## **Trackable Objects:**
Trackable objects are the combinations of measures and objects you created in the editor.

## **Recorder**
In this tab, you can create new recordings. When recording is saved it’s named after the time it was created.

## **Moving to the Editor:**
To move the Editor, Press the button that’s shaped like a screwdriver and a wench.

# 

(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

# **The Recording File Type:**
Once you have created a recording, You’ll probably want to process the data. The data is stored to a “.recording” file, but under the hood it’s just a Json file. You can even use some text editors to see the raw data itself.
The recording files are saved to the “AesculaVR/Recordings” folder in your user documents.
**The JSON Format:**
| **Type** | **Name** | **Description** | 
| --- | --- | --- |
| String [] | indexToPaths | A list of objects that’s being used. | 
| KeyFrame[] | keyframes | A List of Keyframes. | 

The index to paths stores the name of the trackable object being used. These objects should be in your Trackable objects folder.
The Key frames are described below.

### **The Key Frame:**
Each key frame represents a moment in time.
| **Type** | **Name** | **Description** | 
| --- | --- | --- |
| Number | time | The number of seconds, since the recording started. | 
| TrackableObject[] | objects | A list of trackable objects in a scene | 

### 
(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

### **The Trackable Object:**
A Trackable Object represents the combination of measures and objects that you created in the editor. The index is the position of the trackable object file, in the “index to paths” array mentioned earlier. The position and rotation values are of the tracker that the trackable object is attached to.
| **Type** | **Name** | **Description** | 
| --- | --- | --- |
| Number | index | The index to the object in “indexToPaths” | 
| Vector3 | position | The position of the object  (x y z). | 
| Vector3 | rotation | The rotation of the object  (x y z). | 
| Measure[] | measures | The measures on the object. | 

**Measures:**
Measures give the values of the *measures* attached to the trackable object, the type of measure is represented by a number between 0 to 3, but each output a Vector3.
Planes give you the normal to the surface.
Vectors give you the distance between two points.
A Point is just a position.
| **Type** | **Name** | **Description** | 
| --- | --- | --- |
| Number | Type | The type of measure:
0: Plane
1: Vector
2: Point
3 Triangular Plane | 
| Vector3 | Value | The value of the measurement | 

## 
(WARN_UNRECOGNIZED_ELEMENT: PAGE_BREAK)

## **Can we see the values in real time?**
AesculaVR is open source. You can download the project and make changes to it if you want to. Its written in the Unity using C#. You can get Unity for free at Unity.com, and there’s plenty of tutorials out there on how to use it.
Note: Aescular is was written with Unity version 2019.2.0a8, however it *Should* work the a 2019 LTS *(Long Term Support) *version. You can try it in different versions but there’s no guarantee it’ll work.# AesculaVR
