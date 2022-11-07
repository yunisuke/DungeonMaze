using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class StageButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI noText;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private Button button;

    public void SetButton(int stageNo, int getStar, UnityAction callback)
    {
        noText.text = stageNo.ToString();
        for (int i=0; i<getStar; i++)
        {
            stars[i].SetActive(true);
        }

        button.onClick.AddListener(callback);
    }
}