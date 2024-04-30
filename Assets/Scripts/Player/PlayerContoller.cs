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
    public List<GameObject> items;
    public int itemHave = 0;

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
        _move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _velocity = new Vector3(_move.x, 0, _move.y);

        _animator.SetFloat("Move", _move.magnitude);

        if (_velocity.magnitude > 0) {
            float angle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg;
            Vector3 targetDirection = new Vector3(0, angle, 0);
            Quaternion targetRotation = Quaternion.Euler(targetDirection);
            model.transform.rotation = Quaternion.RotateTowards(model.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            _character.Move(moveSpeed * Time.deltaTime * _velocity);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Item>()) {
            var item = other.gameObject.GetComponent<Item>();
            items.Add(other.gameObject);
            item.transform.SetParent(inventory.transform);
            var scale = item.GetComponent<Transform>().localScale;
            item.transform.DOLocalMove(new Vector3(0, items.Count * scale.y, 0), 0.5f).SetLink(other.gameObject);
        }
    }
}
