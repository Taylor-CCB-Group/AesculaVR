using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager manager;

    /// <summary>
    /// Get the EditorManager from within the current scene.
    /// </summary>
    /// <returns></returns>
    public static MainManager GetManager()
    {
        if (manager == null)
        {
            manager = GameObject.FindObjectOfType<MainManager>();
            manager?.Setup();
        }
        return manager;
    }


    private bool isSetup = false;
    private void Setup()
    {
        ///Make sure setup can only be called once, otherwise we'll get a recursive problem.
        if (isSetup)
            return;
        isSetup = true;



    }

}
