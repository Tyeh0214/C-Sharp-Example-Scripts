using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject _enemyToSpawn;
    [SerializeField] private float _maxSpawnRadius = 4;
    [SerializeField] private float _minSpawnRadius = 2;
    [SerializeField] private int _numberOfEnemiesToSpawn = 3;
    [SerializeField] private float _spawnDelay = 1;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private bool _spawnOnStart = true;
    [SerializeField] private bool _invincibleOnSpawn = true;
    [SerializeField] private GameObject _patrolPointContainer;

    [Header("Scale Settings")]
    [SerializeField] private float _minScale = 1;
    [SerializeField] private float _maxScale = 5;

    private int _enemyCount;
    private float _xPosition;
    private float _yPosition;
    private float _zPosition;
    private GameObject _ant;
    private AntAttacks _antAttacks;
    private EnemyHealth _enemyHealth;
    private EnemyController _enemyController;
    private PatrolPoint[] _patrolPoints;

    public GameObject Target { get; set; }

    private void Start()
    {
        // Get all patrol point children and add them to an array
        _patrolPoints = _patrolPointContainer.GetComponentsInChildren<PatrolPoint>();
       
        if (!_spawnOnStart) return;
        StartCoroutine(StartDelay(_spawnDelay));        
    }

    private IEnumerator SpawnEnemies()
    {
        //While the enemy count is less than the number of enemies to be spawned, spawn another enemy
        while (_enemyCount < _numberOfEnemiesToSpawn)
        {
            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hitInfo, _ground))
            {
                //Sets y position
                _yPosition = gameObject.transform.position.y + hitInfo.transform.position.y;
            }
            //Sets the position of the spawned enemy to random within a radius
            _xPosition = Random.Range(gameObject.transform.position.x +_minSpawnRadius, gameObject.transform.position.x + _maxSpawnRadius);
            _zPosition = Random.Range(gameObject.transform.position.z + _minSpawnRadius, gameObject.transform.position.z + _maxSpawnRadius);
            _ant = Instantiate(_enemyToSpawn, new Vector3(_xPosition, _yPosition, _zPosition), Quaternion.identity);
            //_ant = PoolSingleton.Instance.Get(_enemyToSpawn, new Vector3(_xPosition, _yPosition, _zPosition), Quaternion.identity);
            //Randomizes the scale of the enemy
            RandomizeScale(_ant);
            //Sets all required variables and components for the spawned enemey
            _antAttacks = _ant.GetComponent<AntAttacks>();
            _antAttacks.SetTarget(Target);
            _enemyHealth = _ant.GetComponent<EnemyHealth>();
            _enemyHealth.InvincibleOnStart(_invincibleOnSpawn);
            _enemyController = _ant.GetComponent<EnemyController>();
            _enemyController.SetPatrolPoints(_patrolPoints);
            _enemyCount += 1;


            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(SpawnEnemies());
    }

    public void SpawnWave()
    {
        _enemyCount = 0;
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        //Degbug
        if (Input.GetKeyDown(KeyCode.F)) SpawnWave();
    }

    private void RandomizeScale(GameObject gameObject)
    {
        float scale = Mathf.Round(Random.Range(_minScale, _maxScale) * 10) * 0.1f;
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
    }

    private void OnDrawGizmos()
    {
        float yPosition = gameObject.transform.position.y + 0.25f;
        Gizmos.color = new Color(0, 255, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, yPosition, transform.position.z), new Vector3(0.5f, 0.5f, 0.5f));
    }
}
