using UnityEngine;
using UnityEngine.Events;
using Manager;
using System.Collections.Generic;
using Data;
using TMPro;

namespace Scenes.TitleScene
{
    public class StagePanel : MonoBehaviour
    {
        [SerializeField] private StageButton buttonPrefab;
        [SerializeField] private RectTransform buttonContainer;
        [SerializeField] private TextMeshProUGUI totalClearStarNumText;
        [SerializeField] private TextMeshProUGUI totalStageStarNumText;

        public UnityAction<MapId> ButtonEvent;

        [Header("Debug")]
        [SerializeField] private GameObject deleteDataButton;

        void Awake()
        {
            SetStageSelectView();
            SetTotalClearStarNumText(DataManager.Instance.GetTotalClearStarNum());
            SetTotalStageStarNumText(DataManager.Instance.GetTotalStageNum() * 3);
            if (Debug.isDebugBuild == false) deleteDataButton.SetActive(false);
        }

        public void SetStageSelectView()
        {
            List<MapId> stageList = DataManager.Instance.mapIdList;
            bool canChallengeNextStage = true;

            for(int i=0; i<stageList.Count; i++)
            {
                MapId mapId = stageList[i];
                ClearData clearData = DataManager.Instance.GetClearData(mapId);
                if (mapId.FileName == "34" && CanChallengeLastStage(DataManager.Instance.GetTotalClearStarNum()) == false) continue;

                CreatePrefab(mapId.FileName, clearData.GetStar, canChallengeNextStage);
                if (clearData.GetStar == 0) canChallengeNextStage = false;
            }
        }

        /// <summary>
        /// 最終ステージは99個の星を取得している場合のみ挑戦可能
        /// </summary>
        private bool CanChallengeLastStage(int totalClearStarNum)
        {
            return totalClearStarNum >= 99;
        }

        public void SetTotalClearStarNumText(int totalClearStarNum)
        {
            totalClearStarNumText.text = totalClearStarNum.ToString();
        }

        public void SetTotalStageStarNumText(int totalStageStarNum)
        {
            totalStageStarNumText.text = "/ " + totalStageStarNum.ToString();
        }

        private void CreatePrefab(string fileName, int getStar, bool canChallengeStage)
        {
            StageButton b = GameObject.Instantiate(buttonPrefab, buttonContainer);
            
            if (canChallengeStage)
            {
                b.SetButton(fileName, getStar, () => ButtonEvent.Invoke(new MapId(fileName)));
            }
            else
            {
                b.SetButton(fileName, getStar, null);
            }
        }
    }
}
