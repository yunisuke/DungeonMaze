using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private GameObject touchScreenView;
    [SerializeField] private GameObject stageSelectView;

    void Awake()
    {
        FPSManager.Instance.Initialize ();
    }

    public void OnClickTouchScreenView()
    {
        touchScreenView.SetActive(false);
        stageSelectView.SetActive(true);
    }

    public void OnClickStartButton(int level)
    {
        IngameSceneParameter.SelectLevel = level;
        SceneManager.LoadSceneAsync("IngameScene");
    }
}

public static class IngameSceneParameter
{
    public static int SelectLevel;
}
