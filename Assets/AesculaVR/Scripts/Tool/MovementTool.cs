using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTool : ITool
{

    private EditorManager editorManager;
    private Sprite icon;

    private bool moveX, moveY, moveZ;

    public MovementTool()
    {
        this.editorManager = EditorManager.GetManager();
        icon = (Sprite)Resources.Load<Sprite>("Icons/Movement");

        moveX = true; moveY = true; moveZ = true;
    }

    public MovementTool(float factor)
    {
        this.editorManager = EditorManager.GetManager();
        icon = (Sprite)Resources.Load<Sprite>("Icons/Movement");
        this.factor = factor;

        moveX = true; moveY = true; moveZ = true;

    }

    public MovementTool(float factor, bool x, bool y, bool z)
    {
        this.editorManager = EditorManager.GetManager();
        icon = (Sprite)Resources.Load<Sprite>("Icons/Movement");
        this.factor = factor;

        moveX = x; moveY = y; moveZ = z;

    }


    private Vector3 tipPosition => this.editorManager.ManipulatableManager.Right.Tip.position;

    private Manipulatable manipulatable;
    private Vector3 tipStartPosition, manipulatableStartPositon;

    private float factor = 1f;
    public float Factor { get { return factor; } set { factor = value; } }

    public void EndMove()
    {
        if (!manipulatable)
            return;

        editorManager.ActionManager.DoAction
            (new MoveAction(manipulatable, tipStartPosition, this.manipulatable.transform.position), true);
        manipulatable = null;
    }

    #region ITool
    public Sprite Icon() => icon;

    public void OnDeselected()
    {
        EndMove();
    }

    public void OnSelected()
    {
        EndMove();
    }
    
    public void OnUpdate() {}

    public void TriggerDown()
    {
        manipulatable = this.editorManager.ManipulatableManager.GetManipulatableUnderRightController();
        if (manipulatable == null)
            return;

        tipStartPosition = tipPosition;
        manipulatableStartPositon = manipulatable.transform.position;
    }

    public void TriggerUp()
    {
        EndMove();
    }

    public void TriggerUpdate()
    {
        if (this.manipulatable == null)
            return;

        Vector3 pos = manipulatableStartPositon + ((tipPosition - tipStartPosition) * factor);
        if (!moveX) pos[0] = manipulatableStartPositon[0];
        if (!moveY) pos[1] = manipulatableStartPositon[1];
        if (!moveZ) pos[2] = manipulatableStartPositon[2];
        this.manipulatable.transform.position = pos;
    }
    #endregion
}
