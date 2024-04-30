using NTC.MonoCache;
using TMPro;

public class Score : MonoCache
{
    public int score;
    public TMP_Text tMP_Text;

    void Awake() {
        tMP_Text.text = score.ToString();
    }
}
