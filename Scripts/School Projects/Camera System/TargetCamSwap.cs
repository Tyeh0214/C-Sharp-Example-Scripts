using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamSwap : MonoBehaviour
{
    [SerializeField] private bool _useTargetCamSystem;
    [SerializeField] private Tongue _tongue;
    [SerializeField] private CinemachineFreeLook _playerCam;
    [SerializeField] private CinemachineFreeLook _targetCam;
    [SerializeField] private CinemachineFreeLook _alternateTargetCam;

    [Header("Swap between separate target cam or alternate option")]
    [SerializeField] private bool _useAlternateTargetCam = false;

    private TargetCamController _controller;
    private CinemachineFreeLook _camera;

    private void Start()
    {
        _controller = GetComponent<TargetCamController>();
    }

    private void Update()
    {
        _controller.UseAlternateTargetCam = _useAlternateTargetCam;
        _controller.UseTargetCamSystem = _useTargetCamSystem;

        if (_useAlternateTargetCam)
        {
            _camera = _alternateTargetCam;
        }
        else _camera = _targetCam;

        if (!_useTargetCamSystem) return;

        if (_tongue.HasGrabbedObject)
        {
            _playerCam.Priority = 0;
            _camera.Priority = 20;
            _playerCam.transform.position = _camera.transform.position;
            _playerCam.m_YAxis = _camera.m_YAxis;
            _playerCam.m_XAxis = _camera.m_XAxis;
        }
        else
        {
            _playerCam.Priority = 20;
            _camera.Priority = 0;
            _camera.transform.position = _playerCam.transform.position;
            _camera.m_YAxis = _playerCam.m_YAxis;
            _camera.m_XAxis = _playerCam.m_XAxis;
        }


    }
}
