using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NTC.MonoCache;
using UnityEngine;

public class PlayerController : MonoCache
{
    [Header("Model")]
    public GameObject model;

    [Space(2)] 
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float rotationSpeed = 1f;
    [SerializeField] public bool isDead = false;
    private CharacterController _character;
    private Animator _animator;

    [Space(2)] 
    [Header("Iventory")]
    public GameObject inventory;
    public List<Item> items;
    public ItemType hasItemType = ItemType.None;
    public int itemMaxCount = 10;
    public bool isLocked = false;
    public float lockTimer = 0;

    [Space(2)] 
    [Header("Input Info")]
    [SerializeField] Vector3 _velocity;
    [SerializeField] Vector2 _move;

    private void Awake() {
        _character = GetComponent<CharacterController>();
        _animator = model.GetComponent<Animator>();
    }

    protected override void Run()
    {
        #region Player Movement
        _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _velocity = new Vector3(_move.x, 0, _move.y);

        _animator.SetFloat("Move", _move.magnitude);

        if (_velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg;
            Vector3 targetDirection = new Vector3(0, angle, 0);
            Quaternion targetRotation = Quaternion.Euler(targetDirection);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _character.Move(moveSpeed * Time.deltaTime * _velocity);
        }
        #endregion

        #region TimerItem
        if (isLocked)
        {
            lockTimer += Time.deltaTime;
            if (lockTimer > 0.3f) {
                isLocked = false;
                lockTimer = 0;
            }
        }

        #endregion
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Zavod>())
        {
            ZavodMethod(other.gameObject.GetComponent<Zavod>());
        }

        if (other.gameObject.GetComponent<AutoZavod>())
        {
            AutoZavodMethod(other.gameObject.GetComponent<AutoZavod>());
        }
    }

    void AutoZavodMethod(AutoZavod autoZavod)
    {
        if (autoZavod.items.Count > 0)
        {
            var item = autoZavod.items[^1];
            autoZavod.GetItem();
            GetItem(item);
        }
    }

    void ZavodMethod(Zavod zavod)
    {
        if (hasItemType == zavod.requireItemType)
        {
            if (items.Count > 0)
            {
                if (zavod.getItemPaused) return;

                var getLastItem = items[^1];
                zavod.getItemsList.Add(getLastItem);
                getLastItem.SetParent(zavod.getItemPoint);
                TransformItem(zavod.getItemsList, getLastItem.transform);
                items.Remove(getLastItem);
            }
        }
    }

    void GetItem(Item item)
    {
        if (items.Count < itemMaxCount)
        {
            isLocked = true;
            if (hasItemType == ItemType.None)
                hasItemType = item.itemType;

            if (hasItemType == item.itemType)
            {
                items.Add(item);
                item.FreezeRigibody();
                item.SetParent(inventory.transform);
                TransformItem(items, item.transform);
            }
        }
    }

    void SetItem(Zavod zavod)
    {
        
    }

    void TransformItem(List<Item> items, Transform item)
    {
        var scale = item.localScale;
        item
            .DOLocalMove(new Vector3(0, items.Count * scale.y, 0), 0.2f)
            .OnComplete(() =>
            {
                isLocked = false;
            })
            .SetLink(item.gameObject);
    }
}
