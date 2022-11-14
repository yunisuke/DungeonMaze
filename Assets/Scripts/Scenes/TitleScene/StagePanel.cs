using UnityEngine;
using UnityEngine.Events;
using Manager;

namespace Scenes.TitleScene
{
    public class StagePanel : MonoBehaviour
    {
        [SerializeField] private StageButton buttonPrefab;
        [SerializeField] private RectTransform buttonContainer;

        public UnityAction<int> ButtonEvent;

        void Awake()
        {
            SetStageSelectView();
        }

        public void SetStageSelectView()
        {
            TextAsset[] txt = Resources.LoadAll<TextAsset>("MapFile");
            int clearNum = DataManager.Instance.GetClearStageMax();

            for(int i=0; i<txt.Length; i++)
            {
                var f = txt[i];
                CreatePrefab(int.Parse(f.name), DataManager.Instance.GetStageInfo(int.Parse(f.name)), clearNum);
            }
        }

        private void CreatePrefab(int no, int getStar, int clearNum)
        {
            StageButton b = GameObject.Instantiate(buttonPrefab, buttonContainer);
            
            if (no > clearNum + 1)
            {
                b.SetButton(no, getStar, null);
            }
            else
            {
                b.SetButton(no, getStar, () => ButtonEvent.Invoke(no));
            }
        }
    }
}
