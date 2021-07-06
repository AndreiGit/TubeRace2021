using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class ComplexEngineSfxController : MonoBehaviour
    {
        [SerializeField] private Bike _bike;

        [SerializeField] private AudioSource _sfxLow;
        [SerializeField] private AudioSource _sfxHight;
        [SerializeField] private AudioSource _sfxLoud;
        [SerializeField] private AudioSource _sfxSonicBoom;

        [SerializeField] private AnimationCurve _curveLow; //0
        [SerializeField] private AnimationCurve _curveHight; //0.5
        [SerializeField] private AnimationCurve _curveLoud; // 1
        [SerializeField] private AnimationCurve _curveSonicBoom;

        public const float PitchFactor = 1f;

        [SerializeField] private float _superSonicSpeed;

        public bool IsSuperSonic { get; private set; }

        public void SetsuperSonic( bool flag)
        {
            if (!IsSuperSonic && flag)
            {
                _sfxSonicBoom.Play();
            }

            IsSuperSonic = flag;
        }

        private void Update()
        {
            SetsuperSonic(Mathf.Abs(_bike.GetVelocity()) > _superSonicSpeed);

            if(_sfxSonicBoom.isPlaying)
            {
                var t = Mathf.Clamp01(_sfxSonicBoom.time / _sfxSonicBoom.clip.length);

                _sfxSonicBoom.volume = _curveSonicBoom.Evaluate(t);
            }

            UpdateDynamicEngineSound();
        }

        private void UpdateDynamicEngineSound()
        {
            if (IsSuperSonic)
            {
                _sfxLow.volume = 0;
                _sfxHight.volume = 0;
                _sfxLoud.volume = 0;
                return;
            }
            //var t = _bike.GetNormalizedSpeed();
            var t = Mathf.Clamp01( _bike.GetVelocity() / _superSonicSpeed);

                _sfxLow.volume = _curveLow.Evaluate(t);
                _sfxLow.pitch = 1.0f + PitchFactor * t;

                _sfxHight.volume = _curveHight.Evaluate(t);
                _sfxHight.pitch = 1.0f + PitchFactor * t;


                _sfxLoud.volume = _curveLoud.Evaluate(t);
        }


    }
}
