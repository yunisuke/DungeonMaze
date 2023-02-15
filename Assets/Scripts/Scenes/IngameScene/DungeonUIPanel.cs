using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Scenes.IngameScene.DungeonMiniMap;
using Scenes.IngameScene.DungeonMap;

namespace Scenes.IngameScene
{
    public class DungeonUIPanel : MonoBehaviour
    {
        [Header("Screen")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GoalScreen goalScreen;
        [SerializeField] private GameObject pauseScreen;

        [Header("UI")]
        [SerializeField] private MiniMap miniMap;
        [SerializeField] private Timer timer;
        [SerializeField] private GameObject controller;
        [SerializeField] public Image whiteout;

        public UnityAction OnClickStartButtonAction;
        public UnityAction OnClickHomeButtonAction;
        public UnityAction OnClickRetryButtonAction;
        public UnityAction OnClickPlayButtonAction;
        public UnityAction OnClickNextStageButtonAction;
        public UnityAction OnClickPauseButtonAction;

        public bool IsActivePauseScreen()
        {
            return pauseScreen.activeSelf;
        }

        public void SetActiveController(bool isActive)
        {
            controller.SetActive(isActive);
        }

        public void UpdateMinimap(MapData map)
        {
            miniMap.UpdateMinimap(map);
        }

        public void SetTimer(MapData map)
        {
            timer.SetFloorText(map.MapId.FileName);
            timer.SetStar2Time(map.Star2);
            timer.SetStar3Time(map.Star3);
        }

        public void SetActiveStartScreen(bool isActive)
        {
            startScreen.SetActive(isActive);
        }

        public void SetActivePauseScreen(bool isActive)
        {
            pauseScreen.SetActive(isActive);
        }

        public void StartTimer()
        {
            timer.StartTimer();
        }

        public void StopTimer()
        {
            timer.StopTimer();
        }

        public void OpenGoalScreen(MapData map)
        {
            goalScreen.OpenScreen(map.MapId, timer.TimeText, timer.GetStar);
        }

        public int GetTimerStar()
        {
            return timer.GetStar;
        }

        public void OnClickStartButton()
        {
            OnClickStartButtonAction.Invoke();
        }

        public void OnClickHomeButton()
        {
            OnClickHomeButtonAction.Invoke();
        }

        public void OnClickRetryButton()
        {
            OnClickRetryButtonAction.Invoke();
        }

        public void OnClickPlayButton()
        {
            OnClickPlayButtonAction.Invoke();
        }

        public void OnClickNextStageButton()
        {
            OnClickNextStageButtonAction.Invoke();
        }

        public void OnClickPauseButton()
        {
            OnClickPauseButtonAction.Invoke();
        }
    }
}
