using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public enum GameState {
    Game,
    Ready,
    Menu,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public int coins;

    private void Awake() {
        coins = YandexGame.savesData.coins;
    }
}
