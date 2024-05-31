using DG.Tweening;
using NTC.MonoCache;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoCache
{
    public Image image;
    public float time = 0.4f;

    public void OnEnter()
    {
        image.DOColor(Color.green, time);
        image.transform.DOScale(1.1f, time);
    }

    public void OnExit()
    {
        image.DOColor(Color.white, time);
        image.transform.DOScale(1, time);
    }
}
