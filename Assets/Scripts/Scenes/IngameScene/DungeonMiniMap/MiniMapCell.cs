using UnityEngine;
using UnityEngine.UI;
using Scenes.IngameScene.DungeonMap;

namespace Scenes.IngameScene.DungeonMiniMap
{
    [RequireComponent(typeof(Image))]
    public class MiniMapCell : MonoBehaviour
    {
        private Image img;

        void Awake()
        {
            img = GetComponent<Image>();
        }

        public void DrawCell(Color color)
        {
            img.color = color;
        }

        public void ClearCell()
        {
            img.color = Color.black;
        }
    }
}
