using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;

namespace Scenes.TitleScene
{
    public class TitleScene : MonoBehaviour
    {
        [SerializeField] private GameObject touchScreenView;
        [SerializeField] private GameObject stageSelectView;

        void Awake()
        {
            FPSManager.Instance.Initialize ();
            SoundManager.Instance.Initialize ();
            AdManager.Instance.Initialize ();

            DataManager.Instance.Initialize();

            stageSelectView.GetComponent<StagePanel>().ButtonEvent = OnClickStartButton;
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

        public void OnClickDeleteSave()
        {
            DataManager.Instance.DeleteData();
            SceneManager.LoadScene("TitleScene");
        }
    }

    public static class IngameSceneParameter
    {
        public static int SelectLevel = 1;
    }
}
