using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalText : MonoBehaviour
{
    [SerializeField] private float delay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        yield return new WaitForSeconds(delay);

        var seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.Append(transform.DOLocalMoveY(0.3f, 0.3f));
        seq.SetLoops(-1, LoopType.Yoyo);
    }
}
