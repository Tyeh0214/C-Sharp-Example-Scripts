using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HeightPan : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Tongue _tongue;
    [SerializeField] private CinemachineCameraOffset _playerCam;
    [SerializeField] private float _panStartHeight = 5;
    [SerializeField] private float _maxPanDistance = 5;
    [SerializeField] private float _panRate = 0.25f;
    [SerializeField] private float _decreaseMultiplier = 4;

    private float _playerCamOffset;
    private float _inverseMaxPanDistance;
    private CharacterMovement3D _charMove;

    public bool IsActive { get; private set; }

    private void Start()
    {
        _charMove = _player.GetComponent<CharacterMovement3D>();
        _inverseMaxPanDistance = _maxPanDistance * -1;
        _playerCamOffset = _playerCam.m_Offset.z;
    }

    private void Update()
    {
        // if the player is attached to a rigidbody, return
        if (_tongue.HasGrabbedObject) return;

        //if the player is lower than the set height, decrease the the z offset
        if (_player.transform.position.y < _panStartHeight || _charMove.IsGrounded)
        {
            IsActive = true;
            if(_playerCam.m_Offset.z < _playerCamOffset) _playerCam.m_Offset.z += _panRate * _decreaseMultiplier;
        }

        //if the playerCam z offset is less than the set distance or the player is grounded, return
        if (_playerCam.m_Offset.z <= _inverseMaxPanDistance || _charMove.IsGrounded)
        {
            IsActive = false;
            return;
        }

        //if the player is higher than the set height, increase the the z offset
        if (_player.transform.position.y >= _panStartHeight)
        {
            IsActive = true;
            _playerCam.m_Offset.z -= _panRate;
        }
    }
}
