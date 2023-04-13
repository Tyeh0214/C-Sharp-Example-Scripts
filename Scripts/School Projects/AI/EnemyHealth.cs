using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.BehaviourTrees;
using UnityEngine.UI;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _minDamageApplied;
    [SerializeField] private float _invincibleOnStartTimer = 5;
    [SerializeField] private GameObject _healthOrbPrefab;

    private float _timer = 0;
    private bool _infiniteHealth = false;
    private bool _invincibleOnStart = true;
    private GameObject _healthOrb;

    public UnityEvent DeathEvent;

    private DamageSounds _damageSounds;

    public float CurrentHealth { get; set; }
    public float MaxHealth => _maxHealth;

    public float HealthPercent => CurrentHealth / 100;
    public bool IsDead => CurrentHealth <= 0;

    private void Awake() 
    {
        ResetHealth();
        _damageSounds = GetComponent<DamageSounds>();
    }

    private void Start()
    {
        if (!_invincibleOnStart) return;
        StartCoroutine(InvincibleOnStartTimer());
    }

    public void ResetHealth()
    {
        //resets health to max
        CurrentHealth = _maxHealth;
    }

    public void SetCurrentHealth( float val )
    {
        CurrentHealth = Mathf.Clamp(val, 0, _maxHealth);
    }

    public void ApplyDamage(float damage)
    {
        //return if either condition is met
        if (_infiniteHealth) return;
        if (IsDead) return;

        //clamp the current health betweem 0 and max health when damage dealt
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);

        //if (CurrentHealth <= 0) SpawnHealthOrb();

        if (IsDead)
        {
            _damageSounds.PlayDamage(true);
            DeathEvent.Invoke();
            return;
        }
        _damageSounds.PlayDamage(false);

    }

    private void SpawnHealthOrb()
    {
        //Spawns health orb
        _healthOrb = Instantiate(_healthOrbPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
    }


    public void InvincibleOnStart(bool Bool)
    {
        _invincibleOnStart = Bool;
    }

    private void Update()
    {
        //Degbug
        if(Input.GetKey(KeyCode.F)) ApplyDamage(_maxHealth);
    }

    private IEnumerator InvincibleOnStartTimer()
    {        
        //ant takes no damage for x seconds
        while (_timer < _invincibleOnStartTimer)
        {
            _infiniteHealth = true;
            _timer += 1;

            if(_timer >= _invincibleOnStartTimer) _infiniteHealth = false;

            yield return new WaitForSeconds(1);
        }        
    }    

    private void OnCollisionEnter(Collision collision)
    {        
        //Checks to see if the damage is greater than the allowed minimum damage, then applies it
        float damage = collision.impulse.magnitude;
        if( damage > _minDamageApplied ) ApplyDamage( damage );
    }

    #region Debug
    /// <summary>
    /// DEBUG SYSTEM
    /// </summary>

    private void OnEnable()
    {
        KillEntities.OnDebugKillEnemies += DebugKill;
    }
    private void OnDisable()
    {
        KillEntities.OnDebugKillEnemies -= DebugKill;

    }
    private void DebugKill()
    {
        ApplyDamage(_maxHealth);
    }

    /// <summary>
    /// END DEBUG SYSTEM
    /// </summary>
    #endregion
}
