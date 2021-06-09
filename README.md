# Aescular 

---

## What Is AesculaVR?

AesculaVR is a piece of VR software designed to track and record positions, distances and rotations using the HTC Vive Trackers.

## The Basics	

### Editor

When you open AesculaVR you’ll be dropped into the editor. From you can load in objects and set up your measurements, before saving them to a “Trackable Object”

#### Trackable Objects:

Trackable objects are the things you’ll load into the recorder. They are a combination of measures and objects. Each trackable object is tied to a single tracker

#### Measures:
Measures represent the values you want to record. At the moment there are four types of measures which are Vectors, Points, Planes and Triangular planes.

* Vectors represent a line or direction.
* Points represent a position.
* Planes represent a flat surface. Specifically, a plane represents a Normal to a surface. The difference between a triangular plane and plane is how you create and modify them.

#### Objects: 

Objects are models you want to see. If you want to see a representation of a Hip, Ship or any other physical object, you can load them in as an object.

#### Tools: 

You can use tools to help you move objects and measurements into the position you want.

* The Fine movement tool can allow you to move measures and objects slower or faster than normal. You can also constrain which Axis you want to move the measures and objects on.

* The Smooth tool allows you to move measures and objects smoothly. 

#### Trackers:

 Here you can select which tracker is active. Measures and Objects are created and attached to the active tracker.

### Recorder

After you have set up your trackable objects, you’ll want to record some data and the recorder will let you do just that.
A recording will save out the values for each of your trackable objects.

---

## Tutorial

### Purpose

The purpose of the setup is to get two angles between a femur and an impactor.  To measure the angle between the femur and impactor, we must project them onto a plane.
The planes need to replicate the Sagittal (longitudinal) and Axial (horizontal) planes. 

![Image](https://www.researchgate.net/profile/Masahiko-Nakamoto-2/publication/220876519/figure/fig2/AS:305605077159943@1449873336277/Definition-of-the-femur-coordinate-system-1-Sagittal-view-2-Coronal-view-3-Axial.png)

### Setup

Goto Editor Scene.
Start Aescular from Unity.
Select Tracker.

Select Tracker For the Impactor,
1. Create a Vector going down the shaft. (So its parallel to the sagittal plane)
2. Create a Vector going Perpendicular to the Shaft,Upwards.  (So it's parallel to the Axial plane)

Goto the Trackable Objects menu, and save it.
Clear the Scene by pressing Undo repeatedly.

Select Tracker For the Femur.
1. Create a Plane Going down the femur. So the plane lies flat on it.
2. Create a Plane Perpendicular to the femur, So the Plane’s Normal Faces down the Femur.
3. Create a Vector going down the femur
4. Create a Vector going up from the femur, So its Perpendicular to femur, upwards.


Goto the Trackable Objects menu, and save it.

Goto the Recorder Scene.
Select the Femur and Load in the Femur Trackable Object.
Select the Impactor and load in the Impactor Trackable Object.

Goto the Unity Inspector.
Select “Display values '' in the Hierarchy
Fill in the Projection Angle View,

#### The Project Angle View.

The Order you created the measures on the trackable objects, and the order you loaded them into the recorder matters.

Direction A and B are the Vectors from the angle. 
The Projection Plane is the plane on which the angle is measured.

Object index: Which object are we looking at? Depending on the order we loaded the trackable objects.

Measure Index: On the trackable Object, Which measure are we using? Depending on the order we create the measures when making the trackable object.

A Projection plane should look like:

```
Direction A:
    Object Index: 0
    Measure Index: 2
Direction B:
    Object Index: 1
    Measure Index: 0
Projection Plane:
    Object Index: 0
    Measure Index: 0
```
Then turn on “Is Updating”.
You’ll then see your angle updating.

#### Explanation of Why we need to project the Angle.

Suppose we want to take an angle on the Axial Plane; e.g we want to measure how much the “Scientific name here”.  We’ll need two vectors and a plane. An angle is a space between two intersecting lines; These lines are the vectors. If we use the DOT product; we'll have an angle between the two vectors; but it Won't be the angle on the Axial Plane.  To get the angle on the axial plane, we have to Project the vectors to it first; and then measure the angle.

If we rotate the figure below around the Z axis, We can see the XZ angle changes from obtuse to acute; Therefore we know we have to project the Vectors onto a Plane. In this example the plane is the page you are reading.

![Image](https://i.stack.imgur.com/zEYI0.png)
See how the Angle Between XZ changes.
