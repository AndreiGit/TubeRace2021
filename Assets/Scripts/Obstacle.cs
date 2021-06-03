using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Препятствие
    /// </summary>
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private RaceTrack _track;

        [Header("Боковое положение препятствия на треке")]
        [SerializeField] private float _rollAngle;

        [Header("Скорость вращения вокруг оси")]
        [SerializeField] private float _rotationSpeed;

        [Header("Позиция препятствия на трэке")]
        [SerializeField] private float _distance;

        [Header("Смещение от центральной оси")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _radiusModifier = 1;

        private void OnValidate()
        {
            SetObstaclePosition();
        }

        private void Update()
        {
            UpdateRotateObstacle();
        }

        /// <summary>
        /// Метод вращения препятствия вокруг оси трэка
        /// </summary>
        private void UpdateRotateObstacle()
        {
            float dt = Time.deltaTime;

            float da = dt * _rotationSpeed;

            if(_rollAngle >= 360)
            {
                _rollAngle -= 360;
            }

            if(_rollAngle <= -360)
            {
                _rollAngle += 360;
            }

            _rollAngle += da;

            SetObstaclePosition();
        }

        private void SetObstaclePosition()
        {
            Vector3 obstaclePosition = _track.GetPosition(_distance);
            Vector3 obstacleDirection = _track.GetDirection(_distance);

            //Вращение вокруг оси
            Quaternion q = Quaternion.AngleAxis(_rollAngle, Vector3.forward);
            Vector3 trackOffset = q * (Vector3.up *(_radiusModifier * _track.Radius));

            transform.position = obstaclePosition - trackOffset;
            transform.rotation = Quaternion.LookRotation(obstacleDirection, trackOffset);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 centerLinePosition = _track.GetPosition(_distance);
            Gizmos.DrawSphere(centerLinePosition, _track.Radius);
        }
    }
}
