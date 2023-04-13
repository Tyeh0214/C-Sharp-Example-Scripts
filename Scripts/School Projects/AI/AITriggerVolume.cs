using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITriggerVolume : MonoBehaviour
{

    //[SerializeField] private bool _enableEncounter = true;
    //[SerializeField] private GameObject _patrolPointContainer;

    //[Header("Add Spawners if Enabling Encounter")]
    [SerializeField] private EnemySpawner[] _enemySpawners;

    private MeshRenderer _meshRenderer;
    private bool _hasTriggered;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
        _hasTriggered = false;
        //_patrolPointContainer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;

        //_patrolPointContainer.SetActive(_enableEncounter);
        //if (!_enableEncounter) return;
        foreach(EnemySpawner enemySpawner in _enemySpawners)
        {
            enemySpawner.SpawnWave();
        }
        _hasTriggered = true;
    }
}
