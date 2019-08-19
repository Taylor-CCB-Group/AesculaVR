using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using AesculaVR.Manipulations;


public class ManipulatableManager : MonoBehaviour
{

    /* private vars */

    [SerializeField]
    private InputManager right = null, left = null;
    private GameObject rotationHelper;

    private MasterManager masterManager;


    /* public vars */

    /// <summary>
    /// Get the Right Input Controller.
    /// </summary>
    public InputManager Right { get { return right; } }

    /// <summary>
    /// Get the Left Input Controller.
    /// </summary>
    public InputManager Left { get { return left; } }

    /* methods */

    public void Start()
    {
        masterManager = MasterManager.GetManager();
        rotationHelper = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rotationHelper.GetComponent<Renderer>().enabled = false;

        right.Setup();
        left.Setup();

        right.OnGripClicked += OnGripDown;
        left.OnGripClicked += OnGripDown;

        right.OnGripUnclicked += OnGripReleased;
        left.OnGripUnclicked += OnGripReleased;
    }

    public void Update()
    {
        right.Update();
        left.Update();

        if (right.IsManlipulating && left.IsManlipulating)
        {
            if (right.Manlipulation == left.Manlipulation)
            {
                //both inputs are moveing the same object; Only execute the manlipulation once

                right.Manlipulation.Update();
                right.Manlipulation.Manipulatable.OnTransformation();
            }
            else
            {
                //both doing seperate objects
                right.Manlipulation.Update();
                left.Manlipulation.Update();

                right.Manlipulation.Manipulatable.OnTransformation();
                left.Manlipulation.Manipulatable.OnTransformation();
            }
        }
        else if (right.IsManlipulating)
        {
            right.Manlipulation.Update();
            right.Manlipulation.Manipulatable.OnTransformation();
        }
        else if (left.IsManlipulating)
        {
            left.Manlipulation.Update();
            left.Manlipulation.Manipulatable.OnTransformation();
        }

    }

    /// <summary>
    /// Return the other input manager, e.g. if we have the left one, return the right one
    /// </summary>
    /// <param name="inputManager">The input manager you have</param>
    /// <returns>the opposite input manager</returns>
    private InputManager GetOther(InputManager inputManager)
    {
        return (inputManager.Type == InputManager.Controller.Left) ? right : left;
    }

    /// <summary>
    ///What do we do, when the user holds the grip down?
    /// </summary>
    /// <param name="inputManager"> The input manager thats holding the grip down. </param>
    private void OnGripDown(InputManager inputManager)
    {
        InputManager other = GetOther(inputManager);
        Manipulatable manipulatable = GetSelectedTarget(inputManager);

        if (!manipulatable)
        {
            //nothing found.
            return;
        }
        else if (other.IsManlipulating && (other.Manlipulation.Manipulatable == manipulatable))
        {
            //both hands mode

            EndManlipulation(other);
            Manipulation manlipulation = new RotateAndScaleManipulation(other, inputManager, manipulatable, rotationHelper);

            inputManager.Manlipulation = manlipulation;
            other.Manlipulation = manlipulation;

            return;
        }
        else
        {
            //single hand mode.
            manipulatable.OnTransformationStarted();
            inputManager.Manlipulation = new AttachToControllerManipulation(inputManager, other, manipulatable);
            return;
        }

    }


    /// <summary>
    /// What do we do when the grip is released?
    /// </summary>
    /// <param name="inputController">The input manager that had the grip released.</param>
    private void OnGripReleased(InputManager inputController)
    {
        InputManager other = GetOther(inputController);

        if (!inputController.IsManlipulating)
        {
            //I was doing Nothing.
            return;
        }
        else if (other.IsManlipulating && (other.Manlipulation.Manipulatable == inputController.Manlipulation.Manipulatable))
        {
            //we're both manlipulating the same object
            Manipulation manlipulation = inputController.Manlipulation;
            inputController.Manlipulation = null; other.Manlipulation = null;
            manlipulation.End();

            other.Manlipulation = new AttachToControllerManipulation(other, inputController, manlipulation.Manipulatable);
            manlipulation = null;

        }
        else
        {
            //I was manlipulating a single object
            inputController.Manlipulation.Manipulatable.OnTransformationEnded();
            EndManlipulation(inputController);
        }

    }

    /// <summary>
    /// Get the manlipulatable object from an objects ancestry
    /// </summary>
    /// <param name="target">The object we want to get the manlipulatable from</param>
    /// <returns>Null if the ancestry does not have an manipulatable, otherwise return the first manipulatable we find. </returns>
    public Manipulatable GetManipulatableFromAncestors(GameObject target)
    {
        //recursivley go up from the GO, and return the first Manliplulateable found.
        Manipulatable manipulatable = target.GetComponent<Manipulatable>();
        if (manipulatable && manipulatable.enabled)
            return manipulatable;
        else
        {
            if (target.transform.parent)
                return GetManipulatableFromAncestors(target.transform.parent.gameObject);
            else
                return null;
        }
    }

    /// <summary>
    /// Gets the manlipulatable object under the tip.
    /// </summary>
    /// <param name="inputManager">The input manager for the tip.</param>
    /// <param name="getManipulatableFromAncestors">Do we want to get the manipulatable object from objects ancestors.</param>
    /// <returns>A manipulatable object under the tip.</returns>
    private Manipulatable GetSelectedTarget(InputManager inputManager, bool getManipulatableFromAncestors = true)
    {
        //get all objects under the cursor.
        Collider[] objectsUnderTheCursor;
        objectsUnderTheCursor = Physics.OverlapSphere(inputManager.Tip.position, inputManager.Tip.localScale.magnitude);


        Manipulatable best = null; //best manlipulatable we've found so far
        Manipulatable selected = null; //the one we're currently checking.

        float bestDistance = float.PositiveInfinity;

        //go through each object under the cursor.
        for (int i = 0; i < objectsUnderTheCursor.Length; i++)
        {
            //Does the object (or one if its ancestors/parents) have a manlipulatable componenet?
            if (getManipulatableFromAncestors)
                selected = GetManipulatableFromAncestors(objectsUnderTheCursor[i].gameObject);
            else
                selected = objectsUnderTheCursor[i].gameObject.GetComponent<Manipulatable>();


            //if it does, and it's active; then is it better then what we;ve got?
            if (selected && selected.isActiveAndEnabled)
            {
                if ((inputManager.Tip.position - selected.transform.position).sqrMagnitude < bestDistance)
                {
                    //is the object a better target, and is it closer?
                    best = selected;
                    bestDistance = (inputManager.Tip.position - best.transform.position).sqrMagnitude;
                }
            }
        }

        return best;
    }

    /// <summary>
    /// end a manlipulation
    /// </summary>
    /// <param name="inputManager"> the input manager we want to end the manlipulation on</param>
    private void EndManlipulation(InputManager inputManager)
    {
        if (inputManager.Manlipulation != null)
            inputManager.Manlipulation.End(); inputManager.Manlipulation = null;
    }


}
