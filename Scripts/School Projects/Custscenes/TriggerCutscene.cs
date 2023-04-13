using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class TriggerCutscene : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cinematicCam;
    [SerializeField] private CinemachineDollyCart _cart;
    [SerializeField] private CinemachineSmoothPath _track;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _cartSpeed = 10;
    
    private float _finalCartPosition;
    private Health _health;
    private PlayerController _input;
    private GameObjectEventCaller _event;
    private bool _hasTriggered;

    private void Start()
    {
        _finalCartPosition = _track.MaxPos;
        _hasTriggered = false;
        _health = _player.GetComponent<Health>();
        _input = _player.GetComponent<PlayerController>();
        _event = GetComponent<GameObjectEventCaller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        //_input.enabled = false;
        _cinematicCam.Priority = 25;
        _hud.SetActive(false);
        _cart.m_Speed = _cartSpeed;
        _event.InvokeEvent();
    }

    private void Update()
    {
        if (_hasTriggered) return;
        if (_cart.m_Position.Equals(_finalCartPosition))
        {
            Reset();
        }
    }

    public void Reset()
    {
        _cinematicCam.Priority = 0;
        _hud.SetActive(true);
        //_input.enabled = true;
        _health.StartRespawn();
        _hasTriggered = true;
    }
}
