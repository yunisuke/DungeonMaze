using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Manager;

public class Player : MonoBehaviour
{
    public bool IsMove {get; private set;}

    public void GoAhead(UnityAction callback)
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalMove(transform.localPosition + transform.forward * 1, 0.3f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            SoundManager.Instance.PlaySE(SEType.Step);
            IsMove = false;
            callback.Invoke();
        });
        seq.Play();
    }

    public void GoBack(UnityAction callback)
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalMove(transform.localPosition - transform.forward * 1, 0.3f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            SoundManager.Instance.PlaySE(SEType.Step);
            IsMove = false;
            callback.Invoke();
        });
        seq.Play();
    }

    public void TurnRight(UnityAction callback)
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalRotate(new Vector3(0, transform.localRotation.eulerAngles.y + 90, 0), 0.3f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            IsMove = false;
            callback.Invoke();
        });
        seq.Play();
    }

    public void TurnLeft(UnityAction callback)
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalRotate(new Vector3(0, transform.localRotation.eulerAngles.y - 90, 0), 0.3f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            IsMove = false;
            callback.Invoke();
        });
        seq.Play();
    }

    public void HitWallAhead()
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalMove(transform.localPosition + transform.forward * 1 * 0.2f, 0.1f).SetEase(Ease.Linear));
        seq.Append(transform.DOLocalMove(transform.localPosition, 0.1f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            SoundManager.Instance.PlaySE(SEType.HitWall);
            IsMove = false;
        });
        seq.Play();
    }

    public void HitWallBack()
    {
        var seq = DOTween.Sequence();
        seq.OnStart(() => IsMove = true);
        seq.Append(transform.DOLocalMove(transform.localPosition - transform.forward * 1 * 0.2f, 0.1f).SetEase(Ease.Linear));
        seq.Append(transform.DOLocalMove(transform.localPosition, 0.1f).SetEase(Ease.Linear));
        seq.OnComplete(() => {
            SoundManager.Instance.PlaySE(SEType.HitWall);
            IsMove = false;
        });
        seq.Play();
    }
}
