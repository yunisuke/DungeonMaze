using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Manager;
using Data;

namespace Scenes.IngameScene
{
    public class GoalScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clear;
        [SerializeField] private TextMeshProUGUI clearTime;

        [SerializeField] private GameObject[] stars;
        [SerializeField] private GameObject footer;
        [SerializeField] private GameObject nextGameButton;

        void Awake()
        {
            foreach (var obj in stars)
            {
                obj.SetActive(false);
            }
            footer.SetActive(false);

            clearTime.alpha = 0;
        }

        public void OpenScreen(MapId mapId, string time, int getStar)
        {
            gameObject.SetActive(true);
            nextGameButton.SetActive(DataManager.Instance.ExistNextGame(mapId));
            clearTime.text = time;

            StartCoroutine(Effect(getStar));
            AdManager.Instance.HideAds();
        }

        private IEnumerator Effect(int getStar)
        {
            yield return StartCoroutine(EffectClear());
            yield return StartCoroutine(EffectTime());
            yield return StartCoroutine(EffectStar(getStar));
            yield return StartCoroutine(AfterEffect());
        }

        private IEnumerator EffectClear()
        {
            clear.transform.localScale = new Vector3(0, 0, 0);
            clear.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutElastic);
            yield return new WaitForSeconds(1);
        }

        private IEnumerator EffectTime()
        {
            clearTime.DOFade(1, 1f);
            AdManager.Instance.ShowMediumAds();
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator EffectStar(int getStar)
        {
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

            yield return new WaitForSeconds(1.0f);
        }

        private IEnumerator AfterEffect()
        {
            AdManager.Instance.ShowIntersitialAd();
            yield return new WaitForSeconds(0.1f);

            footer.SetActive(true);
            yield return null;
        }
    }
}
