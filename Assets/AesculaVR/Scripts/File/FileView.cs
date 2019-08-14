using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class FileView : MonoBehaviour, IPoolable
{
    protected MasterManager masterManager = MasterManager.GetManager();
    protected IFile file;
    private const string errorStr = "???";

    [SerializeField] private TextMeshProUGUI fname, created, modified, accessed;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Button button;

    void Awake()
    {
        masterManager = MasterManager.GetManager();
        this.button.onClick.AddListener(OnButtonPress);
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
