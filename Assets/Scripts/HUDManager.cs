using DG.Tweening;
using NTC.MonoCache;
using TMPro;
using UnityEngine;
using YG;

public class HUDManager : MonoCache
{
    [Header("Canvas")]
    public Canvas GameCanvas;
    public Canvas StaticCanvasCoin;

    [Space(2)]
    [Header("Mobile")]

    [Space(2)]
    [Header("Texts")]
    public TMP_Text coinsText;

    private bool volumeOn = true;
    private GameManager _gameManager;

    protected override void OnEnabled() {
        GraphicSettingsYG.onQualityChange += OnQualityChange;
    }

    protected override void OnDisabled() {
        GraphicSettingsYG.onQualityChange -= OnQualityChange;
    }

    private void Awake() {
        YandexGame.GameReadyAPI();
        YandexGame.StickyAdActivity(true);
        _gameManager = GetComponent<GameManager>();
        
        if (PlayerPrefs.HasKey("GQuality")) {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQuality"));
        } OnQualityChange();
        UpdateCoinsText();
    }

    public void OnQualityChange() {
        /* PlayerPrefs.SetInt("GQuality", qualityDropdown.value); */
    }

    public void CloseFullAd() {
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }

    public void OpenFullAd() {
        AudioListener.pause = true;
        AudioListener.volume = 0;
    }

    public void OpenRewardAd(int id)
    {
		YandexGame.RewVideoShow(id);
    }

    public void ShowCanvas(CanvasGroup group) {
        group.DOFade(1, 0.5f).SetLink(group.gameObject);
        group.blocksRaycasts = true;
        group.interactable = true;
    }

    public void HideCanvas(CanvasGroup group) {
        group.DOFade(0, 0.5f).SetLink(group.gameObject);
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public void UpdateCoinsText()
    {
        coinsText.text = _gameManager.coins.ToString();
    }

    public void ChangeSoundState() {
        volumeOn = !volumeOn;
        AudioListener.volume = volumeOn ? 1 : 0;

        // foreach (var sound in soundIcons) {
        //     sound.gameObject.SetActive(false);
        // }

        // soundIcons[volumeOn ? 0 : 1].gameObject.SetActive(true);
    }
}
