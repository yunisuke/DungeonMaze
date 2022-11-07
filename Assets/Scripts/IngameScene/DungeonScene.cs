using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Manager;

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

    private Map map;
    private bool isGoal = false;
    private bool isStart = false;

    void Awake()
    {
        FPSManager.Instance.Initialize ();
        SoundManager.Instance.Initialize ();
        AdManager.Instance.Initialize ();

        DataManager.Initialize();

        controller.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        AdManager.Instance.ShowAds();

        map = MapReader.ReadFile(IngameSceneParameter.SelectLevel);
        mk.MakeDungeon(map);
        miniMap.UpdateMap(map);

        timer.SetFloorText(map.MapNo);
        timer.SetStar2Time(map.Star2);
        timer.SetStar3Time(map.Star3);
    }

    void Update()
    {
        if (isNextScene) SceneManager.LoadScene("TitleScene");

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
        AdManager.Instance.ShowIntersitialAd(NextGame);
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
        IngameSceneParameter.SelectLevel++;

        AdManager.Instance.HideAds();
        AdManager.Instance.HideMediumAds();
        SceneManager.LoadScene("IngameScene");
    }

    private bool isNextScene = false;
    private void NextGame(object sender, EventArgs args)
    {
        isNextScene = true;
    }

    private void AfterMove(Cell c)
    {
        miniMap.UpdateMap(map);
        if (c.CellType == CellType.Goal) GoalEffect();
    }

    private void GoalEffect()
    {
        isGoal = true;
        timer.StopTimer();

        goalScreen.OpenScreen(timer.TimeText, timer.GetStar);
        SoundManager.Instance.PlaySE(SEType.Goal);

        DataManager.Save(map.MapNo, timer.GetStar);
    }
}
