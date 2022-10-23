using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager
{
    private static FPSManager _instance;

    private FPSCounter fps;

    // 初期化フラグ
    private bool isInitialized = false;

    private FPSManager () {

    }

    public static FPSManager Instance {get {
        if (_instance == null) _instance = new FPSManager ();
        return _instance;
    }}

    public void Initialize () {
        if (isInitialized) return;

        if (fps == null && Debug.isDebugBuild) {
            var obj = new GameObject("FPS");
            GameObject.DontDestroyOnLoad(obj);
            fps = obj.AddComponent<FPSCounter>();
        }

        Application.targetFrameRate = 60;
        isInitialized = true;
    }

    public void HideFPS () {
        
    }

    public void ShowFPS () {
        
    }
}
