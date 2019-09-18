using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DeviceManager : MonoBehaviour
{

    private static int NumberOfDifferentDeviceTypes => 6; //How big is the DeviceType enum?
    private static int MaxinumNumberOfDevices = 16;       //How many devices does steam VR allow.

    public enum DeviceType { None = 0, Headset = 1, Controller = 2, BaseStation = 3, Tracker = 4, Other = 5}
    
    private DeviceType[] indexToType;
    private List<int>[] deviceTypeToIndexes;

#pragma warning disable 0649
    [SerializeField] private SteamVR_TrackedObject leftController, rightController;
    [SerializeField] private List<SteamVR_TrackedObject> trackers;
#pragma warning restore 0649
    /// <summary>
    /// Get the device type form an index.
    /// </summary>
    /// <param name="index">The index you want to know the device type for.</param>
    /// <returns>The device type.</returns>
    public DeviceType GetDeviceTypeFromIndex(int index) => indexToType[index];

    /// <summary>
    /// Get the indexs for a type of device.
    /// </summary>
    /// <param name="deviceType">The type of device you want the indexes for.</param>
    /// <returns>A list of indexes.</returns>
    public List<int> GetIndexesForDeviceType(DeviceType deviceType) => new List<int>(deviceTypeToIndexes[(int)deviceType]);


    private void Start()
    {
        indexToType = new DeviceType[MaxinumNumberOfDevices];
        deviceTypeToIndexes = new List<int>[NumberOfDifferentDeviceTypes];
        for (int i = 0; i < NumberOfDifferentDeviceTypes; i++)
            deviceTypeToIndexes[i] = new List<int>();

        //fill in the data.
        ETrackedPropertyError error = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i < MaxinumNumberOfDevices; i++)
        {
            SetupIndexToType(i, ref error);
            deviceTypeToIndexes[(int)indexToType[i]].Add((int)i);
        }

        SetUpControllers();
        SetupTrackers();

        StartCoroutine("ActivateTrackers");
    }

    /// <summary>
    /// Get the string for device i, set its device type from it. 
    /// </summary>
    /// <param name="i">The index</param>
    /// <param name="error">The error</param>
    private void SetupIndexToType(uint i, ref ETrackedPropertyError error)
{
        var result = new System.Text.StringBuilder((int)64);
        OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);

        string id = result.ToString();
        if (id.Contains("hmd"))
            indexToType[i] = DeviceType.Headset;
        else if (id.Contains("controller"))
            indexToType[i] = DeviceType.Controller;
        else if (id.Contains("basestation"))
            indexToType[i] = DeviceType.BaseStation;
        else if (id.Contains("tracker"))
            indexToType[i] = DeviceType.Tracker;
        else if (id == string.Empty)
            indexToType[i] = DeviceType.None;
        else
            indexToType[i] = DeviceType.Other;
    }

    /// <summary>
    /// Setup the controllers so they have the correct device ids, and models.
    /// </summary>
    void SetUpControllers()
    {

        List<int> controllerIndexes = deviceTypeToIndexes[(int)DeviceType.Controller];
        if(controllerIndexes.Count == 0)
        {
            //we have no controllers connected
        }
        else if (controllerIndexes.Count == 1)
        {
            //we always assume the first controller we find is the Right controller, as that has all the interaction tied to it.
            rightController.SetDeviceIndex(controllerIndexes[0]);
            rightController.gameObject.SetActive(true);

            SteamVR_RenderModel rm = rightController.transform.GetComponentInChildren<SteamVR_RenderModel>();
            rm.gameObject.SetActive(true);
            rm.SetDeviceIndex(controllerIndexes[0]);
        }
        else
        {            
            //both contollers.
            rightController.SetDeviceIndex(controllerIndexes[0]);
            rightController.gameObject.SetActive(true);

            SteamVR_RenderModel rm = rightController.transform.GetComponentInChildren<SteamVR_RenderModel>();
            rm.gameObject.SetActive(true);
            rm.SetDeviceIndex(controllerIndexes[0]);
           
            leftController.SetDeviceIndex(controllerIndexes[1]);
            leftController.gameObject.SetActive(true);

            rm = leftController.transform.GetComponentInChildren<SteamVR_RenderModel>();
            rm.SetDeviceIndex(controllerIndexes[1]);
        }     

    }

    /// <summary>
    /// Setup the trackers so they have the correct device ids, and models.
    /// </summary>
    void SetupTrackers()
    {
        List<int> trackerIndexes = deviceTypeToIndexes[(int)DeviceType.Tracker];

        for(int i = 0; i < trackers.Count; i++)
        {
            if (i < trackerIndexes.Count)
            {
                //set the device ID, and set it to have a model.
                trackers[i].SetDeviceIndex(trackerIndexes[i]);

                SteamVR_RenderModel rm = trackers[i].transform.GetComponentInChildren<SteamVR_RenderModel>();
                rm.gameObject.SetActive(true);
                rm.SetDeviceIndex(trackerIndexes[i]);
            }
            else
            {

                //not a "giant" issue; Some trackers will be ignored.
                Debug.LogWarning("there're more tracker indexes then objects.");
            }

        }
    }

    IEnumerator ActivateTrackers()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForEndOfFrame();

        for(int i = 0; i < trackers.Count; i++)
        {
            trackers[i].gameObject.SetActive(true);
        }
    }
}
