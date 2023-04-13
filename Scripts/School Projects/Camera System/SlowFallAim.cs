using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFallAim : MonoBehaviour
{
    [SerializeField] private float _slowTimeScale = .5f;
    [SerializeField] private float _lerpTime;
    [SerializeField] private Material _SlowMoMat;
    [SerializeField] private Tongue _tongue;

    private float _timeScale = 1;
    private CharacterMovement3D _charMove;
    private ToggleAimCam _toggleAimCam;
    private bool _isPaused = false;

    public bool IsPaused { get; set; } = false;
    private float _targetTimeScale;
    private Coroutine _lerpCoroutine;

    private void Start()
    {
        _charMove = GetComponent<CharacterMovement3D>();
        _toggleAimCam = GetComponent<ToggleAimCam>();
    }

    //set material Strength to 0 on enable and disable to make 
    //sure player cannot get stuck with the effect on
    private void OnEnable() => SetMatStrength( 0 );
    private void OnDisable() => SetMatStrength( 0 );

    private void SetMatStrength( float val )
    {
        if ( _SlowMoMat == null ) return;
        _SlowMoMat.SetFloat("EffectStrength", val );
    }

    private void OnAimIn()
    {
        LerpTimeTo( _slowTimeScale );      
    }

    private void OnAimOut()
    {
        //Time.timeScale = _timeScale;
        LerpTimeTo( _timeScale );
    }

    private void OnFireTongue()
    {
        _toggleAimCam.AimOut();
        //Time.timeScale = _timeScale;
        LerpTimeTo( _timeScale );
    }

    private void LerpTimeTo( float newTimescale )
    {
        //stop old coroutine, and start a new one so that they don't overlap
        if ( _lerpCoroutine != null ) StopCoroutine( _lerpCoroutine );
        _lerpCoroutine = StartCoroutine( LerpTimeToCoroutine( newTimescale ) );
        _targetTimeScale = newTimescale;
    }

    private void UpdateScreenEffectAmount()
    {
        if ( _SlowMoMat == null ) return;

        //remap current timescale to 0-1, based on min and max timescale
        float output =  (Time.timeScale - _timeScale) / (_slowTimeScale - _timeScale);

        //apply to material
        SetMatStrength( output );
    }

    private IEnumerator LerpTimeToCoroutine( float newTime )
    {
        float timer = 0;
        float startingTime = Time.timeScale;
        float lerpLength = GetLerpLength( newTime );

        while (Time.timeScale != newTime)
        {
            Time.timeScale = Mathf.Lerp(startingTime, newTime, timer / lerpLength);
            UpdateScreenEffectAmount();
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void GamePaused(bool paused)
    {
        _isPaused = paused;
    }

    private float GetLerpLength(float newTime)
    {
        float distance = Mathf.Abs(newTime - Time.timeScale);

        float percentOfRange = (distance - _slowTimeScale) / (_timeScale - _slowTimeScale);
        float lengthOfTime = _lerpTime * percentOfRange;
        return lengthOfTime;
    }

    private void Update()
    {
        if (_isPaused) return;

        if (_charMove.IsGrounded)
        {
            //only start new coroutine if correct timescale is not allready targeted
            if ( _targetTimeScale != _timeScale ) LerpTimeTo( _timeScale );
        }

        if (_tongue.tongueIsOut)
        {
            _toggleAimCam.AimOut();

            //only start new coroutine if correct timescale is not allready targeted
            if ( _targetTimeScale != _timeScale ) LerpTimeTo( _timeScale );
        }
    }
}
