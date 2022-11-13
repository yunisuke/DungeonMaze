using UnityEngine;
using DG.Tweening;

namespace Scenes.IngameScene
{
    public class Goal : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            transform.DOLocalRotate(new Vector3(0, 720f, 0), 3f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
    }
}
