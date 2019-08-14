using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.UI;

/// <summary>
/// A view that represents an .obj file.
/// </summary>
public class ObjectFileView : FileView
{

    public override void OnButtonPress()
    {
        masterManager.ActionManager.DoAction(
            masterManager.ObjectManager.GenerateObject(this.file)
            );
    }

}
