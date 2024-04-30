using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class InternationalText : MonoBehaviour
{
    [SerializeField] string _en;
    [SerializeField] string _ru;

    void Start()
    {
        if (GetComponent<TextMeshProUGUI>()){
            GetComponent<TextMeshProUGUI>().text = YandexGame.EnvironmentData.language
                switch
            {
                "en" => _en,
                "ru" => _ru,
                _ => _en,
            };
        } else {
            GetComponent<Text>().text = YandexGame.EnvironmentData.language
                switch
            {
                "en" => _en,
                "ru" => _ru,
                _ => _en,
            };
        }
    }
}
