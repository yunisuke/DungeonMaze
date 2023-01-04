using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Manager;
using Scenes.TitleScene;
using Scenes.IngameScene.DungeonMap;
using Scenes.IngameScene.DungeonMiniMap;
using DG.Tweening;

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
        [SerializeField] private Image whiteout;

        private MapData map;
        private bool isGoal = false;
        private bool isStart = false;
        private bool isMoveCell = false;

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
            miniMap.UpdateMinimap(map);

            timer.SetFloorText(map.MapId.FileName);
            timer.SetStar2Time(map.Star2);
            timer.SetStar3Time(map.Star3);

            startScreen.SetActive(true);
        }

        void Update()
        {
            if (IsInputInvalidTime()) return;

            if (Keyboard.current.upArrowKey.isPressed)
            {
                isMoveCell = true;
                if (map.EnableMove(true) == false)
                {
                    pl.HitWallAhead();
                    isMoveCell = false;
                    return;
                }

                var c = map.MovePlayer(true);
                pl.GoAhead(() => StartCoroutine(AfterMove(c, true)));
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                isMoveCell = true;
                if (map.EnableMove(false) == false)
                {
                    pl.HitWallBack();
                    isMoveCell = false;
                    return;
                }

                var c = map.MovePlayer(false);
                pl.GoBack(() => StartCoroutine(AfterMove(c, true)));
            }

            if (Keyboard.current.rightArrowKey.isPressed)
            {
                var c = map.TurnRightPlayer();
                pl.TurnRight(() => StartCoroutine(AfterMove(c, false)));
            }

            if (Keyboard.current.leftArrowKey.isPressed)
            {
                var c = map.TurnLeftPlayer();
                pl.TurnLeft(() => StartCoroutine(AfterMove(c, false)));
            }
        }

        private bool IsInputInvalidTime()
        {
            // ゲームスタート待ち
            if (!isStart) return true;

            // ポーズ中
            if (IsPause()) return true;

            // プレイヤーが動いている最中
            if (pl.IsMove || isMoveCell) return true;

            // ゴールした
            if (isGoal) return true;

            return false;
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

        private IEnumerator AfterMove(BaseCell c, bool isMoveCell)
        {
            if (isMoveCell) {
                var func = c.ExecOnCellEvent(this);
                if (func != null)
                {
                    miniMap.UpdateMinimap(map);
                    yield return StartCoroutine(func.Invoke());
                }
            }
            miniMap.UpdateMinimap(map);
            if (c.CellType == CellType.Goal) GoalEffect();
            this.isMoveCell = false;
        }

        private void GoalEffect()
        {
            isGoal = true;
            timer.StopTimer();

            goalScreen.OpenScreen(map.MapId, timer.TimeText, timer.GetStar);
            SoundManager.Instance.PlaySE(SEType.Goal);

            DataManager.Instance.SaveUserData(map.MapId, timer.GetStar);
        }

        public IEnumerator Warp(int warpNumber, WarpCell fromCell)
        {
            SoundManager.Instance.PlaySE(SEType.Warp);
            yield return StartCoroutine(WarpEffect(() => WarpExec(fromCell)));
        }

        private void WarpExec(WarpCell fromCell)
        {
            for(int y=0; y<map.Max_Y; y++)
            {
                for (int x=0; x<map.Max_X; x++)
                {
                    var cell = map.Cells[y, x];
                    if (cell.GetType() == typeof(WarpCell) && cell != fromCell)
                    {
                        pl.transform.localPosition = new Vector3(x, 1, -y);
                        map.Warp(x, y);
                    }
                }
            }
        }

        private IEnumerator WarpEffect(UnityAction callback)
        {
            // 初期化
            whiteout.gameObject.SetActive(true);
            var col = whiteout.color;
            col.a = 0;
            whiteout.color = col;

            // ホワイトアウト
            bool isEndEffect = false;
            whiteout.DOFade(1, 0.5f).OnComplete(()=>isEndEffect = true);
            while(!isEndEffect) {
                yield return null;
            }
            
            // コールバック実行
            callback.Invoke();

            // ホワイトアウト解除
            isEndEffect = false;
            whiteout.DOFade(0, 0.5f).OnComplete(()=>isEndEffect = true);
            while(!isEndEffect) {
                yield return null;
            }

            yield return null;
        }
    }
}
