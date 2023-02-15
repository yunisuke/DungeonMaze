using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;
using Data;

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

        public void OnClickStartButton(MapId mapId)
        {
            IngameSceneParameter.SelectMap = mapId;
            SceneManager.LoadSceneAsync("DungeonBase");
        }

        public void OnClickDeleteSave()
        {
            DataManager.Instance.DeleteData();
            SceneManager.LoadScene("TitleScene");
        }
    }

    public static class IngameSceneParameter
    {
        public static MapId SelectMap = new MapId("1");
    }
}
