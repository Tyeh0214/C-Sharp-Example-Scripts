using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _viewDistance = 10f;
    [SerializeField] private float _viewHalfAngle = 60f;
    [SerializeField] private LayerMask _targetingMask;

    [Header("Visibility")]
    [SerializeField] private LayerMask _occlusionMask;

    [Header("Patrol")]
    [SerializeField] private float _patrolPointReachedDistance = 1f;
    [SerializeField] private float _patrolSpeed = 0.3f;

    [Header("Fire Ant")]
    [SerializeField] private bool _isFireAnt = false;
    [SerializeField] private float _minHealthToExplode = 20;

    private CharacterMovement3D _characterMovement;
    private PatrolPoint[] _patrolPoints;
    private IEnumerator _currentState;
    private AntAttacks _antAttacks;
    private bool _hasTarget = false;
    private float _targetDistance;
    private GameObject _target;
    private EnemyHealth _health;

    private void Start()
    {
        _characterMovement = GetComponent<CharacterMovement3D>();
        _antAttacks = GetComponent<AntAttacks>();
        _health = GetComponent<EnemyHealth>();
        
        // finds all PartolPoints in the scene, and adds them to an array (expensive operation! perform as alittle as possible)
        //_patrolPoints = FindObjectsOfType<PatrolPoint>();

        // start in initial state
        NextState(InitialState());
    }

    private IEnumerator InitialState()
    {
        yield return new WaitForSeconds(1);
        NextState(PatrolState());
    }

    public void SetPatrolPoints (PatrolPoint[] patrolPoints)
    {
        _patrolPoints = patrolPoints;
    }

    private void NextState(IEnumerator nextState)        
    {
        // stop current state (if it exists)
        if (_currentState != null) StopCoroutine(_currentState);

        // Reset the path on start
        _characterMovement.ResetPath();

        // assign current state and start
        _currentState = nextState;
        StartCoroutine(_currentState);
    }

    private IEnumerator PatrolState()
    {
        PatrolPoint patrolPoint = _patrolPoints[Random.Range(0, _patrolPoints.Length)];

        _characterMovement.ResetPath();
        _characterMovement.MoveSpeedMultiplier = _patrolSpeed;
        _characterMovement.MoveTo(patrolPoint.transform.position);

        while (true)
        {
            float patrolPointDistance = Vector3.Distance(transform.position, patrolPoint.transform.position);
            if (patrolPointDistance < _patrolPointReachedDistance)
            {
                _characterMovement.ResetPath();
                patrolPoint = _patrolPoints[Random.Range(0, _patrolPoints.Length)];
                _characterMovement.MoveTo(patrolPoint.transform.position);
            }
            
            Debug.DrawLine(transform.position, patrolPoint.transform.position, Color.magenta);

            TryFindTarget();
            if (_hasTarget) NextState(ChaseState());

            yield return null;
        }
    }

    private IEnumerator ChaseState()
    {
        _characterMovement.MoveSpeedMultiplier = 1f;
        

        while (_hasTarget)
        {
            _targetDistance = Vector3.Distance(transform.position, _target.transform.position);
            if (_targetDistance > _attackDistance || !TestVisibility(_target.transform.position))
            {
                _characterMovement.ResetPath();
                Debug.DrawLine(_target.transform.position, transform.position, Color.cyan);
                _characterMovement.MoveTo(_target.transform.position);
            }
            else
            {
                NextState(AttackState());
            }

            yield return null;
        }

        NextState(PatrolState());
    }

    private IEnumerator AttackState()
    {
        while (_hasTarget)
        {
            if (TestVisibility(_target.transform.position) && Vector3.Distance(transform.position, _target.transform.position) < _attackDistance)
            {
                // stop and look at player
                Vector3 directionToTarget = (_target.transform.position - transform.position).normalized;
                _characterMovement.SetLookDirection(directionToTarget);
                _antAttacks.Attack();               
            }
            else
            {
                NextState(ChaseState());
            }

            yield return null;
        }

        NextState(PatrolState());
    }

    private IEnumerator ExplodeState()
    {
        while (_hasTarget)
        {
            if (TestVisibility(_target.transform.position) && Vector3.Distance(transform.position, _target.transform.position) < _attackDistance)
            {
                // stop and look at player
                Vector3 directionToTarget = (_target.transform.position - transform.position).normalized;
                _characterMovement.SetLookDirection(directionToTarget);
                _antAttacks.Explode();

            }
            else
            {
                NextState(ChaseState());
            }

            yield return null;
        }

        NextState(DeadState());
    }

    private IEnumerator DeadState()
    {
        _characterMovement.Stop();
        yield return null;
    }

    private void Update()
    {
        if (!_isFireAnt) return;

        if (_health.CurrentHealth <= _minHealthToExplode) NextState(ExplodeState());
    }

    private void TryFindTarget()
    {
        // find all colliders within radius, using layermask
        Collider[] hits = Physics.OverlapSphere(transform.position, _viewDistance, _targetingMask);

        // iterate through all possible targets
        foreach (Collider hit in hits)
        {            
            // check for valid targets
            if (hit.CompareTag("Player") && TestVisibility(hit.transform.position))
            {
                _hasTarget = true;
                _target = hit.gameObject;
                break;
            }
            else
            {
                _hasTarget = false;
            }
        }
    }

    private bool TestVisibility(Vector3 targetPosition)
    {
        // check if position is within view distance
        Vector3 vectorToTarget = targetPosition - transform.position;
        float distance = vectorToTarget.magnitude;
        if (distance > _viewDistance) return false;

        // check if position is within view angle
        Vector3 directionToTarget = vectorToTarget.normalized;
        float halfAngleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (halfAngleToTarget > _viewHalfAngle) return false;

        Vector3 start = transform.position;
        // LineCast uses a start and end point rather than a direction
        // returns true if anything in occlusionMask was hit
        if (Physics.Linecast(start, targetPosition, _occlusionMask))
        {
            // a wall was hit, target is not visible
            return false;
        }

        // assume target is visible if nothing was hit
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);
    }
}
