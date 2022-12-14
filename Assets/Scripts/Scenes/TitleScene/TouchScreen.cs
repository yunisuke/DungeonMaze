using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Scenes.TitleScene
{
    public class TouchScreen : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var tmp = GetComponent<TextMeshProUGUI>();

            DOTween.ToAlpha(
                () => tmp.color,
                color => tmp.color = color,
                0f,
                1f
            ).SetEase(Ease.OutCubic).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
