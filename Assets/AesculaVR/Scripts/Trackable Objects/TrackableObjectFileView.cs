using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectFileView : FileView
{

    private TrackableObjectView mainView;

    public override void OnButtonPress() => mainView.Load(file);

    public void Setup(IFile file, Color backgroundColor, TrackableObjectView mainView)
    {
        this.SetUp(file, backgroundColor);
        this.mainView = mainView;
    }
}
