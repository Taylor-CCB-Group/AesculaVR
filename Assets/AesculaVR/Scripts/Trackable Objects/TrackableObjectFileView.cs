using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectFileView : FileView
{
    private TrackableObjectView mainView;

    public override void OnButtonPress()
    {
        if (mainView.Mode == TrackableObjectView.FilingMode.Delete)
            mainView.Delete(this.file);
        else
            mainView.Load(this.file);
    }

    public void Setup(IFile file, Color backgroundColor, TrackableObjectView mainView)
    {
        this.SetUp(file, backgroundColor);
        this.mainView = mainView;
    }


}
