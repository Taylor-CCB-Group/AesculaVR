using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AesculaVR.Manipulations
{

    /// <summary>
    /// The manlipulatable moves and rotates with the center if the two controllers, scales with the difference in position.
    /// </summary>
    public class RotateAndScaleManipulation : Manipulation
    {
        private GameObject rotationHelper;

        private Transform orginalParent;
        private Vector3 startPosition;
        private Vector3 startRotation;
        private Vector3 startScale;

        private Vector3 primaryStartPos, secondaryStartPos;

        public RotateAndScaleManipulation(InputManager primary, InputManager secondary, Manipulatable manipulatable, GameObject rotationHelper) : base(primary, secondary, manipulatable)
        {

            this.orginalParent = Manipulatable.transform.parent;
            this.startPosition = Manipulatable.transform.position;
            this.startScale = Manipulatable.transform.localScale;
            this.startRotation = Manipulatable.transform.rotation.eulerAngles;

            this.primaryStartPos = primary.Tip.position;
            this.secondaryStartPos = secondary.Tip.position;

            this.rotationHelper = rotationHelper;

            rotationHelper.transform.position = startPosition;
            rotationHelper.transform.rotation = Quaternion.LookRotation(primaryStartPos - secondaryStartPos, Vector3.up);
            rotationHelper.transform.localScale = (orginalParent == null) ? Vector3.one : orginalParent.lossyScale;

            Manipulatable.transform.SetParent(rotationHelper.transform);

        }

        public override void Update()
        {
            base.Update();

            rotationHelper.transform.position = ((Secondary.Tip.position + Primary.Tip.position) / 2) - ((secondaryStartPos + primaryStartPos) / 2) + startPosition;

            if(Manipulatable.Scalable)
                Manipulatable.transform.localScale = startScale * (Vector3.Distance(Secondary.Tip.position, Primary.Tip.position) / Vector3.Distance(secondaryStartPos, primaryStartPos));

            rotationHelper.transform.rotation = Quaternion.LookRotation(Primary.Tip.position - Secondary.Tip.position, Vector3.up);
        }

        public override void End()
        {
            base.End();

            Vector3 worldRotation = Manipulatable.transform.rotation.eulerAngles;
            EditorManager.transform.SetParent(orginalParent);

            EditorManager.ActionManager.DoAction(new CompoundAction(new IAction[]
            {
                new ScaleAction(Manipulatable, startScale, Manipulatable.transform.localScale),
                new MoveAction (Manipulatable, startPosition, Manipulatable.transform.position),
                new RotateAction (Manipulatable, startRotation, worldRotation)
            }, "Move, Scale and rotate an object."));
        }
    }
}


