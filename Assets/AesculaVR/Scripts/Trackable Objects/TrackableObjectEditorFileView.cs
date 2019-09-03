using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackableObjectEditorFileView : FileView
{
    private TrackableObjectEditorView mainView;

    public override void OnButtonPress()
    {
        if (mainView.Mode == TrackableObjectEditorView.FilingMode.Delete)
            mainView.Delete(this.file);
        else
            mainView.Load(this.file);
    }

    public void Setup(IFile file, Color backgroundColor, TrackableObjectEditorView mainView)
    {
        this.SetUp(file, backgroundColor);
        this.mainView = mainView;
    }


}
