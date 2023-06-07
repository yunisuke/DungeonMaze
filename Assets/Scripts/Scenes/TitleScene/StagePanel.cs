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
        [SerializeField] private GameObject deleteDataButton;
        [SerializeField] private TextMeshProUGUI totalClearStarNumText;
        [SerializeField] private TextMeshProUGUI totalStageStarNumText;

        public UnityAction<MapId> ButtonEvent;

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
            bool canChallengeStage = true;

            for(int i=0; i<stageList.Count; i++)
            {
                MapId mapId = stageList[i];
                ClearData clearData = DataManager.Instance.GetClearData(mapId);

                CreatePrefab(mapId.FileName, clearData.GetStar, canChallengeStage);
                if (clearData.GetStar == 0) canChallengeStage = false;
            }
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
