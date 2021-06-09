using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Race
{
    /// <summary>
    /// Подбираемая штука
    /// </summary>
    public abstract class Powerup : MonoBehaviour
    {

        [SerializeField] private RaceTrack _track;

        [SerializeField] private float _rollAngle;
        [SerializeField] private Transform _rollIndicatorMin;
        [SerializeField] private Transform _rollIndicatorMax;

        [Header("Крайние зоны для проверки угла прохождения байка")]
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float _rollAngleMin;
        [Range(-180.0f, 180.0f)]
        [SerializeField] private float _rollAngleMax;

        [Header("Позиция штуки на трэке")]
        [SerializeField] private float _distance;

        private void OnValidate()
        {
            SetPowerPosition();
        }

        private void Update()
        {
            UpdateBikes();
        }

        /// <summary>
        /// Логика обработки всех байков
        /// </summary>
        private void UpdateBikes()
        {
            foreach(var bikeObject in GameObject.FindGameObjectsWithTag(Bike.Tag))
            {
                Bike bike = bikeObject.GetComponent<Bike>();
                float prev = bike.PrevDistance;
                float curr = bike.GetDistance();

                if(prev < _distance && curr > _distance)
                {

                    float bikeAngle = bike.GetRollAngle();

                    if (bikeAngle > 0 && bikeAngle < 180)
                    {
                        if (_rollAngleMin < bikeAngle && _rollAngleMax > bikeAngle)
                        {
                            Debug.Log("yes");
                            //в этом месте байк проезжает powerup
                            OnPickedByBike(bike);
                        }
                    }
                    else if(bikeAngle < 0 && bikeAngle > -180)
                    {
                        if (_rollAngleMin > bikeAngle && _rollAngleMax < bikeAngle)
                        {
                            Debug.Log("yes");
                            //в этом месте байк проезжает powerup
                            OnPickedByBike(bike);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Срабатывает пр подбирании
        /// </summary>
        public abstract void OnPickedByBike(Bike bike);

        private void SetPowerPosition()
        {
            Vector3 obstaclePosition = _track.GetPosition(_distance);
            Vector3 obstacleDirection = _track.GetDirection(_distance);

            //Вращение вокруг оси
            Quaternion q = Quaternion.AngleAxis(_rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up * (0));

            transform.position = obstaclePosition - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDirection, trackOffset);

            _rollIndicatorMin.position = transform.position;
            _rollIndicatorMax.position = transform.position;

            _rollIndicatorMin.transform.rotation = Quaternion.Euler(0,0, _rollAngleMin);
            _rollIndicatorMax.transform.rotation = Quaternion.Euler(0, 0, _rollAngleMax);
        }
    }
}
