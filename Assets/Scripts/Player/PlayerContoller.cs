using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PlayerController : TransformUtils
{
    [Header("Model")]
    public GameObject model;

    [Space(2)] 
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] public float rotationSpeed = 1f;
    [SerializeField] public bool isDead = false;
    [SerializeField] public Vector3 offset;
    private CharacterController _character;
    private Animator _animator;
    private Matrix4x4 _isometricInputMatrix;

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
    [SerializeField] Quaternion _look;
    [SerializeField] Vector2 _move;

    private void Awake() {
        _isometricInputMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
        _character = GetComponent<CharacterController>();
        _animator = model.GetComponent<Animator>();
    }
    protected override void Run()
    {
        #region Player Movement
        var _xInput = SimpleInput.GetAxis("Horizontal");
        var _yInput = SimpleInput.GetAxis("Vertical");
        _move = new Vector2(_xInput, _yInput);
        _velocity = _isometricInputMatrix.MultiplyPoint3x4(new Vector3(_move.x, 0.0f, _move.y));
        _animator.SetFloat("Move", _move.magnitude);

        if (_velocity.magnitude > 0)
        {
            var relative = (transform.position + _velocity) - transform.position;
            _look = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, _look, Time.deltaTime * rotationSpeed).normalized;
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
        if (other.GetComponent<Zavod>())
        {
            other.GetComponent<Zavod>().playerStaying = true;
            ZavodMethod(other.GetComponent<Zavod>());
        }

        if (other.GetComponent<AutoZavod>())
        {

            AutoZavodMethod(other.GetComponent<AutoZavod>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Zavod>())
        {
            other.GetComponent<Zavod>().playerStaying = false;
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
                TransformItem(zavod.getItemsList, getLastItem.transform, () =>
                {
                    isLocked = false;
                });
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
                TransformItem(items, item.transform, () =>
                {
                    isLocked = false;
                });
            }
        }
    }
}
