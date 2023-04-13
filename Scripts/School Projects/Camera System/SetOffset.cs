using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetOffset : MonoBehaviour
{
    [SerializeField] private float _zOffset = -0.5f;

    private CinemachineCameraOffset _playerCam;

    // Start is called before the first frame update
    void Awake()
    {
        _playerCam = GetComponent<CinemachineCameraOffset>();
        _playerCam.m_Offset.z = _zOffset;
    }
}
