using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A temporary script to get the anteversion and inclination from a recording file.
/// <remark>
/// Delete this when its no longer needed, We now have "ShowAnteversionAndInclination" which may be better then this.
/// </remark>
/// </summary>
public class DecodeData : MonoBehaviour
{

    private KeyFrame.TrackableObjectFrameWithMeasures.Memento TrackableObject(KeyFrame.Memento keyframe, int index) => keyframe.Objects[index];


    int boneIndex = 0;
    int impactorIndex = 1;

    //anteversion
    int boneCondylarVectorIndex = 0;
    int boneCondylarPlaneIndex = 1;

    //inclination
    int boneInclinationVectorIndex = 0;
    int boneInclinationPlaneIndex = 2;

    int impactorVectorIndex = 0;

    public void Awake()


    {

        string filename = "201920050105276";


        ReplayFileManager fileManager = new ReplayFileManager();


        IFile file = null;

        for(int i = 0; i < fileManager.FilesReference.Count;i++)
        {
            if(string.CompareOrdinal(fileManager.FilesReference[i].Name(false),filename)==0 )
            {
                file = fileManager.FilesReference[i];
                break;
            }
        }

        if (file == null)
            throw new System.Exception();

        Recording.Memento memento = fileManager.Load<Recording.Memento>(file);


        int keyframesCount = memento.Keyframes.Count;
        for(int i = 0; i < keyframesCount; i++)
        {
            KeyFrame.Memento keyframe = memento.Keyframes[i];
            KeyFrame.TrackableObjectFrameWithMeasures.Memento bone = TrackableObject(keyframe, boneIndex);
            KeyFrame.TrackableObjectFrameWithMeasures.Memento impactor = TrackableObject(keyframe, impactorIndex);

           
            PrintAnteversion(keyframe.Time, bone, impactor);
            PrintInclination(keyframe.Time, bone, impactor);
        }

    }


    public void PrintAnteversion(float time, KeyFrame.TrackableObjectFrameWithMeasures.Memento bone, KeyFrame.TrackableObjectFrameWithMeasures.Memento impactor)        
    {
        Vector3 condylarWorldValue = bone.Measures[boneCondylarVectorIndex].Value;
        Vector3 impactorVectorWorldValue = impactor.Measures[impactorVectorIndex].Value;
        Vector3 bonePlane = bone.Measures[boneCondylarPlaneIndex].Value;

        float value = GetAngleOnPlane(condylarWorldValue, impactorVectorWorldValue, bonePlane);

        Debug.Log("The anteversion angle at time [" + time.ToString() + "] is [" + value.ToString() + "]");
    }

    public void PrintInclination(float time, KeyFrame.TrackableObjectFrameWithMeasures.Memento bone, KeyFrame.TrackableObjectFrameWithMeasures.Memento impactor)
    {
        Vector3 boneVector = bone.Measures[boneInclinationVectorIndex].Value;
        Vector3 impactorVectorWorldValue = impactor.Measures[impactorVectorIndex].Value;
        Vector3 bonePlane = bone.Measures[boneInclinationPlaneIndex].Value;

        float value = GetAngleOnPlane(boneVector, impactorVectorWorldValue, bonePlane);

        Debug.Log("The Inclination angle at time [" + time.ToString() + "] is [" + value.ToString() + "]");
    }


    public float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal)
    {
        from    = Vector3.ProjectOnPlane(from, planeNormal);
        to      = Vector3.ProjectOnPlane(to  , planeNormal);

        return Vector3.SignedAngle(from, to, planeNormal);
    }
}
