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
    public GameState gameState;
    public int totalLevelCoin = 0;
    public int score = 0;
    public int currentLevel;
    public string LEADERBOARD_NAME = "ScoreLeaderboard";

    private void Awake() {
        // YandexGame.ResetSaveProgress();
        // YandexGame.SaveProgress();
        totalLevelCoin = 0;
        score = 0;
        currentLevel = YandexGame.savesData.level;

        if (SceneManager.GetActiveScene().buildIndex != 0) {
            
            gameState = GameState.Menu;
        }
    }
}
