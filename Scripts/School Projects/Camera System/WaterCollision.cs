using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaterCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _waterCollision;
    [SerializeField] private LayerMask _noWaterCollision;

    private WaterDamage[] _water;

    private void Awake()
    {
        _water = FindObjectsOfType<WaterDamage>();
    }
    private void Start()
    {
        foreach (WaterDamage water in _water)
        {
            water.gameObject.layer = _waterCollision;
        }
    }

    public void SetCollision(bool inWater)
    {
        if (inWater)
        {
            foreach (WaterDamage water in _water)
            {
                water.gameObject.layer = _noWaterCollision;
            }
        }
        else
        {
            foreach (WaterDamage water in _water)
            {
                water.gameObject.layer = _waterCollision;
            }
        }
    }
}
