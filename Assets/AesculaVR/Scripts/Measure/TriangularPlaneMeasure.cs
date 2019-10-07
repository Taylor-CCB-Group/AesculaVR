using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A measure for fine plane manipulation.
/// </summary>
public class TriangularPlaneMeasure : Measure   
{

#pragma warning disable 0649
    [SerializeField] private MeshFilter meshFilterA, meshFilterB;

    [SerializeField] private Transform normalPointA, normalPointB, normalLink;
    [SerializeField] private float normalLength = 0.25f;
#pragma warning restore 0649

    public virtual Manipulatable PointC => points[2];

    public new static MeasureManager.MeasureType Type => MeasureManager.MeasureType.TriangularPlane;
    public override Vector3 Value => CalculateNormal();

    public override Manipulatable PointB => points[1];

    public override void SetColor(Color color)
    {
        base.SetColor(color);
        meshFilterA.GetComponent<Renderer>().material.color = color;
        meshFilterB.GetComponent<Renderer>().material.color = color;

        normalPointA.GetComponent<Renderer>().material.color = color;
        normalPointB.GetComponent<Renderer>().material.color = color;
        normalLink  .GetComponent<Renderer>().material.color = color;
    }

    public override void SetManipulatablesEnabled(bool value)
    {
        base.SetManipulatablesEnabled(value);
    }

    private void Awake()
    {
        Debug.Assert(points.Count == 3);
    }
    public void LateUpdate()
    {
        UpdateMeshes();
        UpdateNormal();
    }

    /// <summary>
    /// Calculate the normal of a->b and a->c.
    /// </summary>
    /// <returns>The normal of to the triangle formed from a,b and c</returns>
    public Vector3 CalculateNormal()
    {
        return Vector3.Cross(
            (PointB.transform.position - PointA.transform.position),
            (PointC.transform.position - PointA.transform.position)
            ).normalized;
    }

    /// <summary>
    /// Update the mesh between a,b and c.
    /// </summary>
    public void UpdateMeshes()
    {

        List<Vector3> verts = new List<Vector3>{
            meshFilterA.transform.InverseTransformPoint(PointA.transform.position),
            meshFilterA.transform.InverseTransformPoint(PointB.transform.position),
            meshFilterA.transform.InverseTransformPoint(PointC.transform.position)
        };
        int[] indicesA = { 0, 1, 2 };
        int[] indicesB = { 2, 1, 0 };
        Mesh meshA, meshB;

        meshA = new Mesh();
        meshB = new Mesh();

        meshA.SetVertices(verts);
        meshB.SetVertices(verts);

        meshA.SetTriangles(indicesA,0);
        meshB.SetTriangles(indicesB, 0);

        meshA.RecalculateNormals();
        meshB.RecalculateNormals();

        meshFilterA.mesh = meshA;
        meshFilterB.mesh = meshB;
    }

    /// <summary>
    /// update how the normal is displayed.
    /// </summary>
    public void UpdateNormal()
    {
        Vector3 center = (PointA.transform.position + PointB.transform.position + PointC.transform.position) / 3;
        Vector3 norm = Value* normalLength;

        normalPointA.transform.position = center + norm;
        normalPointB.transform.position = center - norm;

        DrawLinkBetweenPoints();

    }

    /// <summary>
    /// draw the link for the two normal points.
    /// </summary>
    private void DrawLinkBetweenPoints()
    {
        //this was ripped from bable VR.

        //get the positions.
        Vector3 pa = normalPointA.transform.position;
        Vector3 pb = normalPointB.transform.position;

        //get the position between the two points.
        Vector3 pos = Vector3.zero;
        pos[0] = (pa[0] + pb[0]) * 0.5f;
        pos[1] = (pa[1] + pb[1]) * 0.5f;
        pos[2] = (pa[2] + pb[2]) * 0.5f;
        normalLink.transform.position = pos;

        //set the scale to be reletive to the links hierarchy.
        Transform tsfm = normalLink.transform.parent;
        if (tsfm)
        {
            pa = tsfm.InverseTransformPoint(pa);
            pb = tsfm.InverseTransformPoint(pb);
        }

        //set pos[2] to be the distance bwteen the two points.
        Vector3 smallestPoint = pa;
        smallestPoint[0] -= pb[0]; smallestPoint[1] -= pb[1]; smallestPoint[2] -= pb[2];
        pos[2] = (smallestPoint).magnitude;

        //get the smallest point
        pa = points[0].transform.lossyScale;
        pb = points[1].transform.lossyScale;
        smallestPoint = (pa.sqrMagnitude < pb.sqrMagnitude) ? pa : pb;

        //set the cross section of the link to be propotional to the smallest point.
        pos[0] = smallestPoint[0] * normalLength;
        pos[1] = smallestPoint[1] * normalLength;

        normalLink.transform.localScale = pos;
        normalLink.transform.LookAt(normalPointB.transform);
    }

    #region IMementoOriginator

    public class TriangularPlaneMemento : Memento
    {
        public TriangularPlaneMemento(TriangularPlaneMeasure measure) : base(measure)
        {
            this.type = (int)TriangularPlaneMeasure.Type;
        }

    }

    public override IMemento SaveMemento() => new TriangularPlaneMemento(this);

    public override void RestoreMemento(IMemento memento)
    {
        base.RestoreMemento(memento);
    }


    #endregion
}
