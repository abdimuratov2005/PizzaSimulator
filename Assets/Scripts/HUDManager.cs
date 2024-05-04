using System.Collections;
using DG.Tweening;
using NTC.MonoCache;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class HUDManager : MonoCache
{
    public Canvas GameCanvas;
    public Canvas StaticCanvasCoin;
    private GameManager gameManager;
    private AudioController audioController;
    // public Image[] soundIcons;
    public GameObject joysticks;
    // public ButtonHandler[] buttonHandler;
    private bool volumeOn = true;

    protected override void OnEnabled() {
        GraphicSettingsYG.onQualityChange += OnQualityChange;
    }

    protected override void OnDisabled() {
        GraphicSettingsYG.onQualityChange -= OnQualityChange;
    }

    private void Awake() {
        YandexGame.GameReadyAPI();
        YandexGame.StickyAdActivity(true);
        audioController = FindObjectOfType<AudioController>();
        gameManager = FindObjectOfType<GameManager>();
        if (PlayerPrefs.HasKey("GQuality")) {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQuality"));
        } OnQualityChange();
    }

    public void OnQualityChange() {
        // PlayerPrefs.SetInt("GQuality", qualityDropdown.value);
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

    public void GoToMainMenu(CanvasGroup group) {
        group.blocksRaycasts = false;
        group.interactable = false;
        group.DOFade(0, 0.5f).OnComplete(() => {
            gameManager.score = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }).SetLink(group.gameObject);
    }

    public void PlayButton() {
        StaticCanvasCoin.gameObject.SetActive(false);
        gameManager.totalLevelCoin = 0;
        gameManager.score = 0;
        gameManager.gameState = GameState.Ready;
    }

    public void PerkPlayerAdd(int coin) {
        if (YandexGame.savesData.coins >= coin) {
            if (YandexGame.savesData.players < 5) {
                audioController.audioSource.PlayOneShot(audioController.audioClips[8]);
                YandexGame.savesData.coins -= coin;
                YandexGame.SaveProgress();
            } 
        }
    }

    public void PerkHealthAdd(int coin) {
        if (YandexGame.savesData.coins >= coin) {
            if (YandexGame.savesData.health < 10) {
                audioController.audioSource.PlayOneShot(audioController.audioClips[8]);
                YandexGame.savesData.coins -= coin;
                YandexGame.SaveProgress();
            }
        }
    }

    public void NextLevel(CanvasGroup group) {
        group.blocksRaycasts = false;
        group.interactable = false;
        group.DOFade(0, 0.5f).OnComplete(() => {
            gameManager.score = 0;
            SceneManager.LoadScene("Loading");
        }).SetLink(group.gameObject);
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
