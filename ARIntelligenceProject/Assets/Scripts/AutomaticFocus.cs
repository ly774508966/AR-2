using UnityEngine;
using System.Collections;
using Vuforia;

public class AutomaticFocus : MonoBehaviour
{

    //初始化判断是否打开前置摄像头
    bool isOpen;

    void Start()
    {
#if UNITY_ANDROID
        isOpen = true;
        Debug.Log("Android");
#endif
#if UNITY_EDITOR
        //打开摄像头
        isOpen = false;
        Debug.Log("Editor");
#endif
        //自动对焦
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
    void OnGUI()
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (isOpen)
        {
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
            CameraDevice.Instance.Start();
            isOpen = false;
        }
        /*
        if (GUI.Button(new Rect(50, 50, 200, 50), "Select Front Camera"))
        {

            // Stop and deinit camera  
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();

            // Reinit and restart camera, selecting front camera  
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_FRONT);
            CameraDevice.Instance.Start();
        }
        if (GUI.Button(new Rect(50, 150, 200, 50), "Select Back Camera"))
        {
            // Stop tracker  
            // Stop and deinit camera  
            CameraDevice.Instance.Stop();
            CameraDevice.Instance.Deinit();
            // Reinit and restart camera, selecting back camera  
            CameraDevice.Instance.Init(CameraDevice.CameraDirection.CAMERA_BACK);
            CameraDevice.Instance.Start();
        }
        */
    }
}
