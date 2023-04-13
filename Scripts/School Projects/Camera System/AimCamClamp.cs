using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimCamClamp : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnAimIn()
    {
        _playerController._lookInCameraDirection = true;
    }

    private void OnAimOut()
    {
        _playerController._lookInCameraDirection = false;
    }
}
