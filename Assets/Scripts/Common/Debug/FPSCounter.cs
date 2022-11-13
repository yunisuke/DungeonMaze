using System;
using UnityEngine;
 
namespace Common.Debug
{
    public class FPSCounter : MonoBehaviour {
    
        // 変数
        int frameCount;
        float prevTime;
        float fps;

        private GUIStyleState state;
        private GUIStyle style;
    
        // 初期化処理
        void Start()
        {
            // 変数の初期化
            frameCount = 0;
            prevTime = 0.0f;

            state = new GUIStyleState ();
            state.textColor = Color.white;

            style = new GUIStyle();
            style.fontSize = 60;
            style.normal = state;
        }
    
        // 更新処理
        void Update()
        {
            frameCount++;
            float time = Time.realtimeSinceStartup - prevTime;
    
            if (time >= 0.5f) {
                fps = frameCount / time;

                frameCount = 0;
                prevTime = Time.realtimeSinceStartup;
            }
        }
    
        // 表示処理
        private void OnGUI()
        {
            GUILayout.Label(string.Format (" {0} FPS", Math.Floor(fps)), style);
        }
    }
}
