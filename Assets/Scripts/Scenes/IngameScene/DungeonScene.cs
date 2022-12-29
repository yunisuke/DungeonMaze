using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Manager;
using Scenes.TitleScene;
using Scenes.IngameScene.DungeonMap;
using Scenes.IngameScene.DungeonMiniMap;

namespace Scenes.IngameScene
{
    public class DungeonScene : MonoBehaviour
    {
        [SerializeField] private DungeonMaker mk;
        [SerializeField] private Player pl;

        [Header("Screen")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GoalScreen goalScreen;
        [SerializeField] private GameObject pauseScreen;

        [Header("UI")]
        [SerializeField] private MiniMap miniMap;
        [SerializeField] private Timer timer;
        [SerializeField] private GameObject controller;

        private MapData map;
        private bool isGoal = false;
        private bool isStart = false;

        void Awake()
        {
            FPSManager.Instance.Initialize ();
            SoundManager.Instance.Initialize ();
            AdManager.Instance.Initialize ();

            DataManager.Instance.Initialize();

            controller.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            AdManager.Instance.ShowAds();

            map = MapReader.ReadFile(IngameSceneParameter.SelectMap);
            mk.MakeDungeon(map);
            miniMap.Update(map);

            timer.SetFloorText(map.MapId.FileName);
            timer.SetStar2Time(map.Star2);
            timer.SetStar3Time(map.Star3);

            startScreen.SetActive(true);
        }

        void Update()
        {
            if (!isStart || isGoal || pl.IsMove) return;

            if (Keyboard.current.upArrowKey.isPressed)
            {
                if (map.EnableMove(true) == false)
                {
                    pl.HitWallAhead();
                    return;
                }

                var c = map.MovePlayer(true);
                pl.GoAhead(() => AfterMove(c));
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                if (map.EnableMove(false) == false)
                {
                    pl.HitWallBack();
                    return;
                }

                var c = map.MovePlayer(false);
                pl.GoBack(() => AfterMove(c));
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                var c = map.TurnRightPlayer();
                pl.TurnRight(() => AfterMove(c));
            }

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                var c = map.TurnLeftPlayer();
                pl.TurnLeft(() => AfterMove(c));
            }
        }

        private bool IsPause()
        {
            return pauseScreen.activeSelf;
        }

        void OnApplicationFocus(bool isFocus)
        {
            if (isStart && isGoal == false && IsPause() == false && isFocus == false)
            {
                OnClickPauseButton();
            }
        }

        public void OnClickStartButton()
        {
            startScreen.SetActive(false);
            timer.StartTimer();
            controller.SetActive(true);

            isStart = true;
        }

        public void OnClickReturnButton()
        {
            AdManager.Instance.HideAds();
            AdManager.Instance.HideMediumAds();
            SceneManager.LoadScene("TitleScene");
        }

        public void OnClickRetryButton()
        {
            AdManager.Instance.HideAds();
            AdManager.Instance.HideMediumAds();
            SceneManager.LoadScene("IngameScene");
        }

        public void OnClickPauseButton()
        {
            AdManager.Instance.HideAds();
            AdManager.Instance.ShowMediumAds();

            timer.StopTimer();
            pauseScreen.SetActive(true);
        }

        public void OnClickContinueButton()
        {
            AdManager.Instance.ShowAds();
            AdManager.Instance.HideMediumAds();

            timer.StartTimer();
            pauseScreen.SetActive(false);
        }

        public void OnClickNextLevel()
        {
            IngameSceneParameter.SelectMap = DataManager.Instance.GetNextStage(IngameSceneParameter.SelectMap);

            AdManager.Instance.HideAds();
            AdManager.Instance.HideMediumAds();
            SceneManager.LoadScene("IngameScene");
        }

        private void AfterMove(BaseCell c)
        {
            c.ExecOnCellEvent();
            miniMap.Update(map);
            if (c.CellType == CellType.Goal) GoalEffect();
        }

        private void GoalEffect()
        {
            isGoal = true;
            timer.StopTimer();

            goalScreen.OpenScreen(map.MapId, timer.TimeText, timer.GetStar);
            SoundManager.Instance.PlaySE(SEType.Goal);

            DataManager.Instance.SaveUserData(map.MapId, timer.GetStar);
        }
    }
}
