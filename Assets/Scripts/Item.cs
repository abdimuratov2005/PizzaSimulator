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

        FreezeDefault();
    }

    #region Rigibody Methods
    private void FreezeDefault()
    {
        _rbody.constraints = RigidbodyConstraints.FreezePositionZ;
        _rbody.constraints = RigidbodyConstraints.FreezePositionX;
        _rbody.constraints = RigidbodyConstraints.FreezeRotationX;
        _rbody.constraints = RigidbodyConstraints.FreezeRotationY;
        _rbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    }
    public void FreezeRigibody()
    {
        _rbody.useGravity = false;
        _rbody.constraints = RigidbodyConstraints.FreezePositionY;
        _bCollider.isTrigger = true;
    }
    public void UnFreezeRigibody()
    {
        _rbody.useGravity = true;
        _rbody.constraints = RigidbodyConstraints.None;
        FreezeDefault();
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
