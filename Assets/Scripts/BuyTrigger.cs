using DG.Tweening;
using NTC.MonoCache;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BuyTrigger : MonoCache
{
    public TMP_Text priceText;
    public Image progressImage;
    public GameObject activeObject;
    public Table table;

    private int _price;
    public int remains;
    private int _coins;

    public bool playerTriggered = false;
    private float timerStay = 0;
    public float timeToStartMethod = 0.1f;
    private float scaleFactor = 0;
    private int requiredValue = 1;

    private Canvas _canvas;
    private Clients _clients;

    public void Awake()
    {
        _price = remains;
        _canvas = GetComponentInChildren<Canvas>();
        _clients = FindObjectOfType<Clients>();
        
        _coins = YandexGame.savesData.coins;
        UpdatePrice();
    }

    protected override void Run()
    {
        if (playerTriggered) {
            _canvas.transform.DOScale(new Vector3(0.004469116f, 0.006583653f), 0.5f).SetLink(gameObject);
        } else
        {
            _canvas.transform.DOScale(new Vector3(0.0036082f, 0.005315399f), 0.5f).SetLink(gameObject);
            return;
        }

        if (remains <= 0)
        {
            BuyCurrentObject(activeObject);
            return;
        }
        
        timerStay += Time.deltaTime;
        if (timerStay >= timeToStartMethod)
        {
            MinusPrice();
            timerStay = 0;
        }
    }

    public void MinusPrice()
    {
        if (_coins <= 0) return;

        remains -= 1;
        requiredValue += 1;

        scaleFactor = (float)requiredValue / _price;
        
        scaleFactor = Mathf.Clamp(scaleFactor, 0, 1);

        Vector3 oldScale = progressImage.transform.localScale;
        oldScale.x = scaleFactor;
        progressImage.transform.localScale = oldScale;

        UpdatePrice();
    }

    public void UpdatePrice()
    {
        priceText.text = remains.ToString();
    }

    public void BuyCurrentObject(GameObject @object)
    {
        @object.SetActive(true);
        @object.transform
            .DOLocalMoveY(0, 0.3f)
            .SetLink(@object);
        @object.transform
            .DOScale(1, 0.5f)
            .SetLink(@object);

        foreach (var chair in table.chairs)
        {
            _clients.stulPoints.Add(chair.transform);
        }

        gameObject.SetActive(false);
    }
}
