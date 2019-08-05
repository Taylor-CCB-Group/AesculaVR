using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A view that allows us to browse all the .obj files.
/// </summary>
public class ObjectBrowserView : LateObserver
{

    /// <summary>
    /// concreate pool for the file object views.   
    /// </summary>
    private class FileViewPool : ObjectPool
    {
        private FileObjectView viewPrefab;

        public FileViewPool(FileObjectView viewPrefab) : base()
        {
            this.viewPrefab = viewPrefab;
        }

        public override void Fill(int amount = 1)
        {
            FileObjectView obj = GameObject.Instantiate(viewPrefab).GetComponent<FileObjectView>();
            this.Push(obj);
        }
    }


    [SerializeField] private Transform contentRoot;

    [SerializeField] private SortByDropdown  sortByDropdown;
    [SerializeField] private OrderByDropdown orderByDropdown;


    private FileViewPool filePool;
    [SerializeField] private FileObjectView fileViewPrefab;
    private Stack<FileObjectView> activeViews;

    private ObjectManager objectManager;

    [SerializeField] private Color fileColor1, fileColor2;

    protected  void Awake()
    {
        objectManager = new ObjectManager(); //This should be passed in from a master model at some point.
        objectManager.AddObserver(this);

        filePool = new FileViewPool(fileViewPrefab);
        activeViews = new Stack<FileObjectView>();

        sortByDropdown .AddObserver(this);
        orderByDropdown.AddObserver(this);

        objectManager.FileManager.SetSortBy       (sortByDropdown .Value());
        objectManager.FileManager.SetSortDirection(orderByDropdown.Value());


        this.Notify(this, null);
        
    }

    public override void LateNotify(object Sender, EventArgs args)
    {
        while(activeViews.Count > 0)
            filePool.Push(activeViews.Pop());

        if(Sender is SortByDropdown)
            objectManager.FileManager.SetSortBy(sortByDropdown.Value());

        if (Sender is OrderByDropdown)
            objectManager.FileManager.SetSortDirection(orderByDropdown.Value());


        List<IFile> files = objectManager.FileManager.FilesReference;
        for(int i = 0; i < files.Count; i++)
        {
            FileObjectView view = filePool.Pop() as FileObjectView;
            view.transform.SetParent(contentRoot);

            view.transform.localPosition = Vector3.zero;
            view.transform.localScale = Vector3.one;

            view.SetUp(files[i], ((i%2==0)?fileColor1:fileColor2) );

            activeViews.Push(view);

        }

    }
}
