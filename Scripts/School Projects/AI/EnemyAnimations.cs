using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    [SerializeField] private float _dampTime = 0.1f;

    private Animator _animator;
    private Rigidbody _rb;
    private CharacterMovement3D characterMovement;
    private void Start()
    {
        //Grab necessary components
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody>();
        characterMovement = GetComponent<CharacterMovement3D>();
    }

    private void Update()
    {
        // send velocity to animator, ignoring y-velocity
        Vector3 velocity = _rb.velocity;
        Vector3 flattenedVelocity = new Vector3(velocity.x, 0f, velocity.z);
    }
}
