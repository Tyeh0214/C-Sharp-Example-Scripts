using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    [SerializeField] private Tongue _tongue;
    [SerializeField] private float _newYSpeed = 0.001f;
    [SerializeField] private float _newXSpeed = 0.05f;
    [SerializeField] private CinemachineFreeLook _alternateTargetCam;

    private float _normalYSpeed;
    private float _normalXSpeed;
    private PlayerController _playerController;
    private ToggleAimCam _toggleAimCam;
    private void Start()
    {
        _normalXSpeed = _alternateTargetCam.m_XAxis.m_MaxSpeed;
        _normalYSpeed = _alternateTargetCam.m_YAxis.m_MaxSpeed;
        _playerController = GetComponent<PlayerController>();
        _toggleAimCam = GetComponent<ToggleAimCam>();

        StartCoroutine(ChangeSensitivityOnGrab());
    }

    private IEnumerator ChangeSensitivityOnGrab()
    {
        if (_tongue.HasGrabbedObject)
        {
            _alternateTargetCam.m_YAxis.m_MaxSpeed = _newYSpeed;
            _alternateTargetCam.m_XAxis.m_MaxSpeed = _newXSpeed;
        }
        else
        {
            _alternateTargetCam.m_YAxis.m_MaxSpeed = _normalYSpeed;
            _alternateTargetCam.m_XAxis.m_MaxSpeed = _normalXSpeed;
        }

        if (_tongue.HasGrabbedObject && !_toggleAimCam.AimedIn)
        {
            _playerController._lookInCameraDirection = true;
        }

        if (!_tongue.HasGrabbedObject && !_toggleAimCam.AimedIn)
        {
            _playerController._lookInCameraDirection = false;
        }

        yield return null;
    }





}
