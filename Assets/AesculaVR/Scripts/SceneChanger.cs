using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneType { Editor, Recorder }

    [SerializeField] private Dialog diaglog;
    [SerializeField] private SceneType changeTo;

    [SerializeField] private string editor, recorder;

    public void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(OnPress);
    }

    public void OnPress()
    {
        diaglog.Show(ChangeScene(), "Change scene?", DialogMessage());
    }

    public UnityAction ChangeScene()
    {
        return () => { SceneManager.LoadScene(changeTo == SceneType.Editor ? editor : recorder); };  
    }

    public string DialogMessage()
    {
        switch (changeTo)
        {
            case SceneType.Editor:
                return "Do you want to change to the editor scene.";
            case SceneType.Recorder:
                return "Do you want to change to the recording scene.";
            default:
                throw new System.NotImplementedException();
        }
    }
}
