using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using DG.Tweening;

public class SceneControl : MonoBehaviour
{
    public Image loader;

    void Awake() {
        StartCoroutine(Loading());
    }

    IEnumerator Loading() {
        yield return new WaitForSeconds(1);
        loader.DOFillAmount(1, 2).SetLink(loader.gameObject).OnComplete(InvokedLoad);
    }

    void InvokedLoad() {
        SceneManager.LoadScene($"Level {YandexGame.savesData.level}");
    }
}
