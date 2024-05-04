using UnityEngine;
using NTC.MonoCache;

public class LookToPlayerText : MonoCache
{
    Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }
    protected override void Run()
    {
        transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward,
            _cam.transform.rotation * Vector3.up);
    }
}
