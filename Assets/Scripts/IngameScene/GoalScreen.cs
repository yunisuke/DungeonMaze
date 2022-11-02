using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GoalScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clear;
    [SerializeField] private TextMeshProUGUI clearTime;

    [SerializeField] private GameObject[] stars;

    void Awake()
    {
        foreach (var obj in stars)
        {
            obj.SetActive(false);
        }
    }

    public void OpenScreen(string time, int getStar)
    {
        gameObject.SetActive(true);
        EffectClear();
        StartCoroutine(EffectStar(getStar));
        AdManager.Instance.HideAds();
        AdManager.Instance.ShowMediumAds();
        clearTime.text = time;
    }

    private void EffectClear()
    {
        clear.transform.localScale = new Vector3(0, 0, 0);
        clear.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutElastic);
    }

    private IEnumerator EffectStar(int getStar)
    {
        yield return new WaitForSeconds(1.0f);
        stars[0].SetActive(true);

        if (getStar >= 2) {
            yield return new WaitForSeconds(0.2f);
            stars[1].SetActive(true);
        }

        if (getStar >= 3)
        {
            yield return new WaitForSeconds(0.2f);
            stars[2].SetActive(true);
        }
    }
}
