using NTC.MonoCache;
using UnityEngine;

public class CameraMove : MonoCache
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 offset;

    protected override void LateRun()
    {
        if (_target.position.magnitude > 0) {
            Vector3 camPos = offset + _target.transform.position;
            transform.position = camPos;
        }
    }
}
