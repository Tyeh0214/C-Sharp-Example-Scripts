using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AccelerationCam : MonoBehaviour
{
    [SerializeField] private CinemachineFollowZoom _playerCam;
    [SerializeField] private CharacterMovement3D _player;
    [SerializeField] private float _rateOfIncrease = 0.5f;
    [SerializeField] private float _minVelocity = 5;
    [SerializeField] private float _maxVelocity = 10;

    private float _zoomWidth;

    private void Start()
    {
        _zoomWidth = _playerCam.m_Width;
        StartCoroutine(AccelerationZoom());
    }

    private IEnumerator AccelerationZoom()
    {
        for (float i = _minVelocity; i < _player.Velocity.magnitude
            && i > _maxVelocity; i++)
        {
            _playerCam.m_Width += _rateOfIncrease;
        }
        yield return null;
    }
}
