using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Класс отвечающий за звук
    /// </summary>
    public class EngineSfxController : MonoBehaviour
    {
        [SerializeField] private AudioSource _engineSource;

        [SerializeField] private Bike _bike;

        [Range(0,1)]
        [SerializeField] private float _velocityPitchModifier;

        private void Update()
        {
            UpdateEngineSoundsSimple();
        }

        private void UpdateEngineSoundsSimple()
        {
            _engineSource.pitch = 1.0f + _velocityPitchModifier * _bike.GetNormalizedSpeed();
        }
    }
}
