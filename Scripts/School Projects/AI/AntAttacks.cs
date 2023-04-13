using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AntAttacks : MonoBehaviour
{
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damage = 5;

    [Header("Fire Ant Only")]
    [SerializeField] private ExplosionController _explosionController;
    [SerializeField] private LayerMask _knockBackLayer;
    [SerializeField] private float _explosionForce = 0.5f;
    [SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _explosionUpwardsModifier = 1;
    
    private CinemachineImpulseSource _impusleSource;
    private EnemyHealth _health;

    public GameObject Target { get; private set;}
    public Event TargetPositionUpdated;


    private void Start()
    {
        //Grab necessary components
        _impusleSource = GetComponent<CinemachineImpulseSource>();
        _health = GetComponent<EnemyHealth>();
    }

    public void SetTarget(GameObject target)
    {
        //Sets the target variable
        Target = target;
        _playerHealth = target.GetComponent<Health>();
    }

    public void Attack()
    {
        //Play Attack animation
        _animator.Play("Attack");
    }


    public void Explode()
    {
        //Reworked this so it can hit multiple objects at once
        //Currently uses a overlapSphere to get all colliders then iterates through all of them
        Collider[] objects = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider collider in objects)
        {
            //If the object has the destructable wall component it will trigger the burn
            if (collider.TryGetComponent(out DestructableWall wall))
            {
                wall.StartBurn();
            }

            //If the object has a rigidbody then it will knock it back
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(_explosionForce, gameObject.transform.position, _explosionRadius, _explosionUpwardsModifier, ForceMode.Impulse);
                rb.AddForce(gameObject.transform.forward, ForceMode.Impulse);
            }
            //Triggers particles after null check
            if (_explosionController == null) return;
            _explosionController.Explode();
        }

        _impusleSource.GenerateImpulseAt(gameObject.transform.position, Vector3.forward);
        //_health.ApplyDamage(_health.MaxHealth);
    }
}
