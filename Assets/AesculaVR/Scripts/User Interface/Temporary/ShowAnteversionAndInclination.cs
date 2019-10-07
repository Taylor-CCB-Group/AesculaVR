using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// A temporary script to get the anteversion and inclination while the scene is playing.
/// <remark>
/// Delete this script when it's no longer needed, This is NOT a generic script and is too focused for general use. 
/// </remark>
/// </summary>
public class ShowAnteversionAndInclination : MonoBehaviour
{
    private MainManager mainManager;

    public bool isUpdating = false;
    public bool useProjection = true;
    public bool invertAngle = false;

    public int boneObjectIndex;
    public int impactorObjectIndex;

    public int impactorVectorIndex = 0;

    public int boneVectorIndex;
    public int boneAnterversionPlaneIndex;
    public int boneInclinationPlaneIndex;

    public TextMeshProUGUI anterversionText, inclinationText;

    private void Awake()
    {
        mainManager = MainManager.GetManager();
    }


    public void LateUpdate()
    {
        if (isUpdating)
        {
            UpdateAnteversion();
            UpdateInclination();
        }
    }

    public void UpdateAnteversion()
    {
        Vector3 boneVec     = GetMeasureValue(boneObjectIndex, boneVectorIndex);
        Vector3 impactorVec = GetMeasureValue(impactorObjectIndex, impactorVectorIndex);
        Vector3 plane       = GetMeasureValue(boneObjectIndex, boneAnterversionPlaneIndex);

        anterversionText.SetText(InvertAngle(GetAngleOnPlane(impactorVec, boneVec, plane)).ToString());
    }

    public void UpdateInclination()
    {
        Vector3 boneVec = GetMeasureValue(boneObjectIndex, boneVectorIndex);
        Vector3 impactorVec = GetMeasureValue(impactorObjectIndex, impactorVectorIndex);
        Vector3 plane = GetMeasureValue(boneObjectIndex, boneInclinationPlaneIndex);

        inclinationText.SetText(InvertAngle(GetAngleOnPlane(impactorVec, boneVec, plane)).ToString());
    }

    public float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal)
    {
        from = Vector3.ProjectOnPlane(from, planeNormal);
        to = Vector3.ProjectOnPlane(to, planeNormal);

        return Vector3.SignedAngle(from, to, planeNormal);
    }

    public Vector3 GetMeasureValue(int objIndex, int measureIndex) => mainManager.TrackableObjectManager.TrackableObjectsReference[objIndex].MeasuresReference[measureIndex].Value;

    public float InvertAngle(float a) => (!invertAngle) ? a : (180f - Mathf.Abs(a)) * ((a < 0) ? -1 : 1);

}




