using UnityEngine;

namespace Common.Light
{
    public class AdjustCamera : MonoBehaviour
    {
        [SerializeField] private float baseWidth = 9.0f;
        [SerializeField] public float baseHeight = 16.0f;

        void Awake()
        {
            var camera = GetComponent<Camera>();

            // アスペクト比固定
            var scale = Mathf.Min(Screen.height / this.baseHeight, Screen.width / this.baseWidth);
            var width = (this.baseWidth * scale) / Screen.width;
            var height = (this.baseHeight * scale) / Screen.height;
            camera.rect = new Rect((1.0f - width) * 0.5f, (1.0f - height) * 0.5f, width, height);
        }
    }
}
