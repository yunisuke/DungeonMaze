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
        [SerializeField] private Player pl;

        public DungeonUIPanel uiPanel;

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

            uiPanel.SetActiveController(false);
            uiPanel.OnClickStartButtonAction = OnClickStartButton;
            uiPanel.OnClickHomeButtonAction = OnClickHomeButton;
            uiPanel.OnClickRetryButtonAction = OnClickRetryButton;
            uiPanel.OnClickNextStageButtonAction = OnClickNextStage;
            uiPanel.OnClickPlayButtonAction = OnClickPlayButton;
            uiPanel.OnClickPauseButtonAction = OnClickPauseButton;
        }

        // Start is called before the first frame update
        void Start()
        {
            AdManager.Instance.ShowAds();

            map = MapReader.ReadFile(IngameSceneParameter.SelectMap);
            SetPlayerPosition(map.Position);
            uiPanel.UpdateMinimap(map);
            uiPanel.SetTimer(map);
            uiPanel.SetActiveStartScreen(true);
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

        private void SetPlayerPosition(PlayerPosition p)
        {
            pl.transform.position = new Vector3(p.x, 1, -p.y);
            pl.transform.eulerAngles = GetPlayerRotation(p.d);
        }

        private Vector3 GetPlayerRotation(Direction d)
        {
            int dic;
            switch(d)
            {
                case Direction.North:
                    dic = 0;
                    break;
                case Direction.South:
                    dic = 180;
                    break;
                case Direction.West:
                    dic = 270;
                    break;
                case Direction.East:
                    dic = 90;
                    break;
                default:
                    dic = 0;
                    break;
            }
            return new Vector3(0, dic, 0);
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
            return uiPanel.IsActivePauseScreen();
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
            uiPanel.SetActiveStartScreen(false);
            uiPanel.StartTimer();
            uiPanel.SetActiveController(true);

            isStart = true;
        }

        public void OnClickHomeButton()
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

            uiPanel.StopTimer();
            uiPanel.SetActivePauseScreen(true);
        }

        public void OnClickPlayButton()
        {
            AdManager.Instance.ShowAds();
            AdManager.Instance.HideMediumAds();

            uiPanel.StartTimer();
            uiPanel.SetActivePauseScreen(false);
        }

        public void OnClickNextStage()
        {
            IngameSceneParameter.SelectMap = DataManager.Instance.GetNextStage(IngameSceneParameter.SelectMap);

            AdManager.Instance.HideAds();
            AdManager.Instance.HideMediumAds();
            SceneManager.LoadScene("DungeonBase");
        }

        private IEnumerator AfterMove(BaseCell c, bool isMoveCell)
        {
            if (isMoveCell) {
                var func = c.ExecOnCellEvent(this);
                if (func != null)
                {
                    uiPanel.UpdateMinimap(map);
                    yield return StartCoroutine(func.Invoke());
                }
            }
            uiPanel.UpdateMinimap(map);
            if (c.CellType == CellType.Goal) GoalEffect();
            this.isMoveCell = false;
        }

        private void GoalEffect()
        {
            isGoal = true;
            uiPanel.StopTimer();

            uiPanel.OpenGoalScreen(map);
            SoundManager.Instance.PlaySE(SEType.Goal);
            DataManager.Instance.SaveUserData(map.MapId, uiPanel.GetTimerStar());
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
            var whiteout = uiPanel.whiteout;

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
