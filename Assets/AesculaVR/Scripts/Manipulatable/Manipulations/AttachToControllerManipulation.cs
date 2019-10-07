using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AesculaVR.Manipulations
{
    /// <summary>
    /// The Manlipulatable is attached to a single controller, and moves and rotates with it.
    /// </summary>
    public class AttachToControllerManipulation : Manipulation
    {

        private Vector3 startPosition;
        private Vector3 startRotation;

        private Transform orginalParent;

        public AttachToControllerManipulation(InputManager primary, InputManager secondary, Manipulatable manipulatable) : base(primary, secondary, manipulatable)
        {
            this.startPosition = Manipulatable.transform.localPosition;
            this.startRotation = Manipulatable.transform.localRotation.eulerAngles;
            this.orginalParent = Manipulatable.transform.parent;
            Manipulatable.transform.SetParent(primary.Children);
        }

        public override void Update() { base.Update(); }

        public override void End()
        {
            base.End();
            Manipulatable.transform.SetParent(orginalParent);
            EditorManager.ActionManager.DoAction(new CompoundAction(new IAction[] {
                new MoveAction(Manipulatable,startPosition,Manipulatable.transform.localPosition),
                new RotateAction(Manipulatable,startRotation, Manipulatable.transform.localRotation.eulerAngles)
            }, "Move and rotate a   object"), true);
        }
    }
}
