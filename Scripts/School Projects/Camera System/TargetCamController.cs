using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetCamController : MonoBehaviour
{
    [SerializeField] private Tongue _tongue;

    [Header("Target Camera Settings")]
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private float _playerTargetWeight = 10;
    [SerializeField] private float _grabbedObjectWeight = 2;

    public bool UseTargetCamSystem { get; set; }
    public bool UseAlternateTargetCam { get; set; }

    private void Start()
    {
        _targetGroup.m_Targets[0].weight = _playerTargetWeight;
        _targetGroup.m_Targets[1].weight = 0;
    }    

    private void Update()
    {        
        if (!UseTargetCamSystem || UseAlternateTargetCam) return;

        if (!_tongue.HasGrabbedObject)
        {
            TargetCam(0);           
        }
        
        if (_tongue.HasGrabbedObject)
        {
           TargetCam(_grabbedObjectWeight);
        }
        
    }

    private void TargetCam(float weight)
    {
        _targetGroup.m_Targets[1].weight = weight;
    }
}
