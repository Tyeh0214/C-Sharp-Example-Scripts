using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class ToggleAimCam : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _playerCam;
    [SerializeField] private CinemachineFreeLook _aimCam;
    [SerializeField] private Image _baseReticle;
    [SerializeField] private Image _aimReticle;
    [SerializeField] private Tongue _tongue;

    private bool _aimedIn = false;

    public bool AimedIn => _aimedIn;

    private void Start()
    {
        _baseReticle.enabled = true;
        _aimReticle.enabled = false;
    }

    private void OnAimIn()
    {
        if (!_tongue.tongueIsOut)
        {
            _playerCam.Priority = 0;
            _aimCam.Priority = 20;
            _baseReticle.enabled = false;
            _aimReticle.enabled = true;
            _aimedIn = true;
        }
    }

    private void OnAimOut()
    {
        AimOut();
    }

    private void Update()
    {
        if (!_aimedIn)
        {
            _aimCam.transform.position = _playerCam.transform.position;
            _aimCam.m_YAxis = _playerCam.m_YAxis;
            _aimCam.m_XAxis = _playerCam.m_XAxis;
        }
        else
        {
            _playerCam.transform.position = _aimCam.transform.position;
            _playerCam.m_YAxis = _aimCam.m_YAxis;
            _playerCam.m_XAxis = _aimCam.m_XAxis;
        }
    }

    public void AimOut()
    {
        _playerCam.Priority = 20;
        _aimCam.Priority = 0;
        _baseReticle.enabled = true;
        _aimReticle.enabled = false;
        _aimedIn = false; 
    }
}
