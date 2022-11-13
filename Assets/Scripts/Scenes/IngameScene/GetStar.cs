using UnityEngine;
using DG.Tweening;

namespace Scenes.IngameScene
{
    public class GetStar : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.localScale = new Vector3(0, 0, 0);
            transform.DOScale(Vector3.one, 1).SetEase(Ease.OutElastic);
        }
    }
}
