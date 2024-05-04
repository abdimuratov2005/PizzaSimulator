using NTC.MonoCache;
using UnityEngine;

public enum ItemType {
    Small,
    Big,
    None
}

public class Item : MonoCache
{
    public ItemType itemType;

    private Rigidbody _rbody;
    private BoxCollider _bCollider;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _bCollider = GetComponent<BoxCollider>();
    }

    #region Rigibody Methods
    public void FreezeRigibody()
    {
        _rbody.useGravity = false;
        _rbody.constraints = RigidbodyConstraints.FreezeAll;
        _bCollider.isTrigger = true;
    }
    public void UnFreezeRigibody()
    {
        _rbody.useGravity = true;
        _rbody.constraints = RigidbodyConstraints.None;
        _rbody.constraints = RigidbodyConstraints.FreezeRotation;
        _bCollider.isTrigger = false;
    }
    #endregion

    #region Transform
    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }
    #endregion
}
