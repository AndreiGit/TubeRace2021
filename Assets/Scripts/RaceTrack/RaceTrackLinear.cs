using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Класс линейного трека
    /// </summary>
    public class RaceTrackLinear : RaceTrack
    {
        [Header("Начало трэка")]
        [SerializeField] private Transform _start;

        [Header ("Окончание трэка")]
        [SerializeField] private Transform _end;
        public override Vector3 GetDirection(float distance)
        {
            distance = Mathf.Clamp(distance, 0, GetTrackLenght());

            return (_end.position - _start.position).normalized;
        }

        public override Vector3 GetPosition(float distance)
        {
            distance = Mathf.Clamp(distance, 0, GetTrackLenght());

            Vector3 direction = _end.position - _start.position;
            direction = direction.normalized;

            return _start.position+ direction* distance;
        }

        public override float GetTrackLenght()
        {
            Vector3 direction = _end.position - _start.position;

            return direction.magnitude;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_start.position, _end.position);
        }

        #region Test

        [SerializeField] private float _testDistance;
        [SerializeField] private Transform _testObject;

        private void OnValidate()
        {
            CapsuleMovement();
        }

        /// <summary>
        /// Зацикливает перемещение капсуля
        /// </summary>
        public void TrackLoop()
        {
            if (_testObject.position == _start.position)
            {
                _testDistance = GetTrackLenght();
            }
            else if (_testObject.position == _end.position)
            {
                _testDistance = 0;
            }
        }

        /// <summary>
        /// Перемещение капсуля с зацикливанием
        /// </summary>
        private void CapsuleMovement()
        {
            _testObject.position = GetPosition(_testDistance);
            _testObject.forward = GetDirection(_testDistance);

            TrackLoop();
        }

        /// <summary>
        /// Перемещает капсуль в плеймоде с заданной скоростью
        /// </summary>
        /// <param name="speed"></param>
        public void MovementPlayMode(float speed)
        {
            _testDistance += speed;

            CapsuleMovement();
        }

        #endregion
    }
}
