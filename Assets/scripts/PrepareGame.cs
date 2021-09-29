using CameraScript;
using MLAPI;
using UnityEngine;

public class PrepareGame : NetworkBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public override void NetworkStart()
    {
        if (_mainCamera != null)
        {
            _mainCamera.GetComponent<CameraController>().Targets.Add(OwnerClientId, gameObject.transform);
        }
    }

    private void OnDestroy()
    {
        if (_mainCamera != null)
        {
            _mainCamera.GetComponent<CameraController>().Targets.Remove(OwnerClientId);
        }
    }
}