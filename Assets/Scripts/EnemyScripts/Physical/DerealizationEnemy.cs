using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using System;

public class DerealizationEnemy : EnemyScript
{
    [SerializeField] private CinemachineCamera _camera;
    public override void OnRoomEnter()
    {
        base.OnRoomEnter();
        Invoke(nameof(CameraFlip), 6f);
    }
    public override void OnRoomExit()
    {
        base.OnRoomExit();
        CancelInvoke(nameof(CameraFlip));
        NormalizeCamera();
    }
    protected override void Die()
    {
        CancelInvoke(nameof(CameraFlip));
        NormalizeCamera();
        base.Die();
    }

    private void CameraFlip()
    {
        _camera.Lens.OrthographicSize = -_camera.Lens.OrthographicSize;
        Invoke(nameof(CameraFlip), 6f);
    }
    private void NormalizeCamera()
    {
        _camera.Lens.OrthographicSize = Mathf.Abs(_camera.Lens.OrthographicSize);
    }
}
