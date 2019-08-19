using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneVectorView : MonoBehaviour
{

    /* Vector mode.
     *
     *--Create mode
     * + Create two manlipulatable spheres that the user can move around
     * + Click down ->create first, Click Up -> Create seconed.
     * + Vectors are stored (runtime) as two transforms (one for each sphere).
     * 
     * 
     * --Destroy mode
     * + Delete vector if either the spheres or the link between them is grabbed.
     * +
     */

    /* Plane mode.
     * 
     * -Create mode
     * + Create two manlipulatable spheres that the user can move around, these for the normal to the plane.
     * + The plane can be represented by a quad.
     * + works the same way as a vector
     * + 
     */








#pragma warning disable 0649
    [SerializeField] private RadioToggle planeOrVectorToggle, createOrDestroyToggle;
#pragma warning restore 0649

    private enum PlaneOrVector { Plane, Vector }
    private enum CreateOrDestroy { Create, Destroy }

    private PlaneOrVector planeOrVector => planeOrVectorToggle.Value == 0 ? PlaneOrVector.Plane : PlaneOrVector.Vector;
    private CreateOrDestroy createOrDestroy => createOrDestroyToggle.Value == 0 ? CreateOrDestroy.Create : CreateOrDestroy.Destroy;
}
