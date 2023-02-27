using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scenes.TitleScene;

namespace Scenes.IngameScene
{
    public class DungeonBase : MonoBehaviour
    {
        private DungeonUIPanel dungeonUiPanel = null;
        private GameObject dungeonMap = null;

        public DungeonScene dungeonScene;

        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadSceneAsync(IngameSceneParameter.SelectMap.FileName, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("DungeonUIScene", LoadSceneMode.Additive);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (IsLoadScene()) {
                Debug.Log("load is finished ...");
                dungeonScene.uiPanel = dungeonUiPanel;
                dungeonScene.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public bool IsLoadScene()
        {
            if (dungeonUiPanel == null )
            {
                var p = GameObject.Find("DungeonUIPanel");
                if (p != null) dungeonUiPanel = p.GetComponent<DungeonUIPanel>();
            }
            if (dungeonMap == null)
            {
                dungeonMap = GameObject.Find("DungeonMap");
            }

            return dungeonUiPanel != null && dungeonMap != null;
        }
    }
}
