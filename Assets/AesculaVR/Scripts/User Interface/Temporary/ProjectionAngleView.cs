using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectionAngleView : MonoBehaviour
{
#pragma warning disable 0649
    [System.Serializable]
    private class Index
    {
        [SerializeField] private int objectIndex;
        [SerializeField] private int measureIndex;
                
        public int ObjectIndex  => objectIndex;
        public int MeasureIndex => measureIndex;
    }


    [SerializeField] private bool isUpdating    = false;
    [SerializeField] private bool useProjection = true ;
    [SerializeField] private bool invertAngle   = false;
    [SerializeField] [Range(-180,180)] private float offset       = .0f  ;

    [SerializeField] Index directionA;
    [SerializeField] Index directionB;
    [SerializeField] Index projectionPlane;

    [SerializeField] private TextMeshProUGUI uiOutput;
#pragma warning restore 0649

    private MainManager mainManager;



    private void Awake()
    {
        mainManager = MainManager.GetManager();
    }

    private void Update()
    {
        if (isUpdating)
        {
            uiOutput.SetText(GetAngle.ToString());
        }
    }


    public float GetAngleOnPlane(Vector3 from, Vector3 to, Vector3 planeNormal)
    {
        from    = Vector3.ProjectOnPlane(from , planeNormal);
        to      = Vector3.ProjectOnPlane(to   , planeNormal);

        return Vector3.SignedAngle(from, to, planeNormal);
    }
    public Vector3 GetMeasureValue(int objIndex, int measureIndex) => mainManager.TrackableObjectManager.TrackableObjectsReference[objIndex].MeasuresReference[measureIndex].Value;
    public float InvertAngle(float a) => (!invertAngle) ? a : (180f - Mathf.Abs(a)) * ((a < 0) ? -1 : 1);
    public float AddOffset  (float a)
    {
        a += offset;
        if(a > 180)
            return -180 + (a - 180);
        if (a < -180)
            return 180 - (a + 180);
        return a;
    }
    public float GetAngle => AddOffset
                (
                InvertAngle
                    (
                    GetAngleOnPlane
                        (
                            GetMeasureValue(directionA.ObjectIndex, directionA.MeasureIndex),
                            GetMeasureValue(directionB.ObjectIndex, directionB.MeasureIndex),
                            GetMeasureValue(projectionPlane.ObjectIndex, projectionPlane.MeasureIndex)
                        )
                    )
                );
}


