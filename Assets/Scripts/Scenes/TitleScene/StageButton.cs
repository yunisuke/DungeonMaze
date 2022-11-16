using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Scenes.TitleScene
{
    public class StageButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI noText;
        [SerializeField] private GameObject[] stars;
        [SerializeField] private GameObject mask;
        [SerializeField] private Button button;

        public void SetButton(string stageName, int getStar, UnityAction callback)
        {
            noText.text = stageName;
            for (int i=0; i<getStar; i++)
            {
                stars[i].SetActive(true);
            }

            if (callback == null)
            {
                mask.SetActive(true);
            }
            else
            {
                mask.SetActive(false);
                button.onClick.AddListener(callback);
            }
        }
    }
}
