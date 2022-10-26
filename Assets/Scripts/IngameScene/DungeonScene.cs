using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Manager;

public class DungeonScene : MonoBehaviour
{
    [SerializeField] private DungeonMaker mk;
    [SerializeField] private Player pl;

    [SerializeField] private GameObject goalObj;

    [SerializeField] private MiniMap miniMap;

    private Map map;
    private bool isGoal = false;

    void Awake()
    {
        FPSManager.Instance.Initialize ();
        SoundManager.Instance.Initialize ();
        AdManager.Instance.Initialize ();
    }

    // Start is called before the first frame update
    void Start()
    {
        AdManager.Instance.ShowAds();

        map = MapReader.ReadFile(IngameSceneParameter.SelectLevel);
        mk.MakeDungeon(map);
        miniMap.UpdateMap(map);
    }

    void Update()
    {
        if (isNextScene) SceneManager.LoadScene("TitleScene");

        if (Keyboard.current.upArrowKey.isPressed)
        {
            if (pl.IsMove || isGoal) return;
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
            if (pl.IsMove || isGoal) return;
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
            if (pl.IsMove || isGoal) return;
            var c = map.TurnRightPlayer();
            pl.TurnRight(() => AfterMove(c));
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            if (pl.IsMove || isGoal) return;
            var c = map.TurnLeftPlayer();
            pl.TurnLeft(() => AfterMove(c));
        }
    }

    public void OnClickReturnButton()
    {
        AdManager.Instance.HideAds();
        AdManager.Instance.ShowIntersitialAd(NextGame);
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
        goalObj.SetActive(true);
        SoundManager.Instance.PlaySE(SEType.Goal);
    }
}
