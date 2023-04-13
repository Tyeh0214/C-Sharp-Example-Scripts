using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetLookDirection : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _playerCam;

    public void SetCameraDirection(LookDirectionGizmo checkpoint)
    {
        _playerCam.m_XAxis.Value = checkpoint.transform.eulerAngles.y;
        _playerCam.m_YAxis.Value = 0.4f;
    }
}
