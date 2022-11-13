using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using Manager;
using System;

public class StagePanel : MonoBehaviour
{
    [SerializeField] private StageButton buttonPrefab;
    [SerializeField] private RectTransform buttonContainer;

    public UnityAction<int> ButtonEvent;

    void Awake()
    {
        SetStageSelectView();
    }

    public void SetStageSelectView()
    {
        TextAsset[] txt = Resources.LoadAll<TextAsset>("MapFile");
        int clearNum = DataManager.GetClearStageMax();

        for(int i=0; i<txt.Length; i++)
        {
            var f = txt[i];
            CreatePrefab(int.Parse(f.name), DataManager.GetStageInfo(int.Parse(f.name)), clearNum);
        }
    }

    private void CreatePrefab(int no, int getStar, int clearNum)
    {
        StageButton b = GameObject.Instantiate(buttonPrefab, buttonContainer);
        
        if (no > clearNum + 1)
        {
            b.SetButton(no, getStar, null);
        }
        else
        {
            b.SetButton(no, getStar, () => ButtonEvent.Invoke(no));
        }
    }
}
