using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public abstract class FileView : MonoBehaviour, IPoolable
{
    protected EditorManager editorManager;
    protected MainManager   mainManager;

    protected IFile file;
    private const string errorStr = "???";

#pragma warning disable 0649
    [SerializeField] protected ErrorDialog errorDialog;

    [SerializeField] private TextMeshProUGUI fname, created, modified, accessed;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;
#pragma warning restore 0649

    void Awake()
    {
        editorManager = EditorManager.GetManager();
        mainManager   = MainManager  .GetManager();

        this.button.onClick.AddListener(ErrorCheck);
    }




    /// <summary>
    /// Setup this view.
    /// </summary>
    /// <param name="file">The file we want to represent</param>
    /// <param name="backgroundColor">an alternate background color</param>
    public virtual void SetUp(IFile file, Color backgroundColor, ErrorDialog errorDialog)
    {
        SetUp(file, backgroundColor);
        this.errorDialog = errorDialog;
    }


    /// <summary>
    /// Setup this view.
    /// </summary>
    /// <param name="file">The file we want to represent</param>
    /// <param name="backgroundColor">an alternate background color</param>
    public virtual void SetUp(IFile file, Color backgroundColor)
    {
        SetUp(file);
        this.backgroundImage.color = backgroundColor;
    }

    /// <summary>
    /// Setup this view.
    /// </summary>
    /// <param name="file">The file we want to represent</param>
    public virtual void SetUp(IFile file)
    {
        this.file = file;
        this.fname.SetText(file.Name(false));
        this.created.SetText(DateTimeToString.ToString(file.Created()));
        this.modified.SetText(DateTimeToString.ToString(file.Modified()));
        this.accessed.SetText(DateTimeToString.ToString(file.Accessed()));
    }



    /// <summary>
    /// What happens whem the user presses this view?
    /// </summary>
    public abstract void  OnButtonPress();

    /// <summary>
    /// Wrap the onbutton press code, and handle errors.
    /// </summary>
    private void ErrorCheck()
    {
        if (!errorDialog)
        {
            //if we dont have a dialog, dont check.
            OnButtonPress();
            return;
        }


        try
        {
            OnButtonPress();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
            errorDialog.Show(e.Message);
        }
    }

    #region IPoolable
    void IPoolable.OnPoppedFromPool()
    {
        this.gameObject.SetActive(true);
        this.gameObject.hideFlags = HideFlags.None;
    }

    void IPoolable.OnPushedToPool()
    {
        this.gameObject.SetActive(false);
        this.gameObject.hideFlags = HideFlags.HideInHierarchy;

        this.fname.SetText(errorStr);
        this.created.SetText(errorStr);
        this.modified.SetText(errorStr);
        this.accessed.SetText(errorStr);
    }
    #endregion
}
