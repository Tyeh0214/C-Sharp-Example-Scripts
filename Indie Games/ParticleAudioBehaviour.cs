using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FindTheBirds
{
    public class ParticleAudioBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _timeUntilPlay;

        private float _fixedTimeUntilPlay;

        private void Start()
        {
            //stores the fixed time variable
            _fixedTimeUntilPlay = _timeUntilPlay;
        }

        private void Update()
        {
            //if the audio source is playing, return
            if (_audioSource.isPlaying) return;

            //subtracts deltaTime from time variable
            _timeUntilPlay -= Time.deltaTime;

            //if time variable reaches 0, run function
            if (_timeUntilPlay <= 0)
            {
                PlayAudio();
            }
        }

        private void PlayAudio()
        {
            //Play audio source and set time variable back to fixed time variable value
            _audioSource.Play();
            _timeUntilPlay = _fixedTimeUntilPlay;
        }
    }
}

