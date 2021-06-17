using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class CameraController : MonoBehaviour
    {
        [Header("Байк")]
        [SerializeField] private Bike _targetBike;

        [Header("Нижняя граница отклонения камеры")]
        [SerializeField] private float _minFov = 60;
        [Header("Верхняя граница отклонения камеры")]
        [SerializeField] private float _maxFov = 85;

        [Header("Тряска камеры")]
        [SerializeField] private float _shakeFactor = 0.1f;

        [SerializeField] private AnimationCurve _shakeCurve;

        private Vector3 _initialLocalPosition;

        private void Start()
        {
            _initialLocalPosition = Camera.main.transform.localPosition;
        }

        private void Update()
        {
            UpdateFov();
            UpdateCameraShake();
        }

        private void UpdateCameraShake()
        {
            var cam = Camera.main;

            var t = _targetBike.GetNormalizedSpeed();
            var curveValue = _shakeCurve.Evaluate(t);

            var randomVector = UnityEngine.Random.insideUnitSphere * _shakeFactor;
            randomVector.z = 0;

            cam.transform.localPosition = _initialLocalPosition + randomVector * curveValue;
        }

        private void UpdateFov()
        {
            var cam = Camera.main;

            var t = _targetBike.GetNormalizedSpeed();
            cam.fieldOfView = Mathf.Lerp(_minFov, _maxFov, t);
        }


    }
}