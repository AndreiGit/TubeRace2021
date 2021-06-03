using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [System.Serializable]
    public class BikeParameters
    {
        [Header("Масса байка")]
        [Range(0.0f, 10.0f)]
        public float Mass;

        [Header("Ускорение байка")]
        [Range(0.0f, 100.0f)]
        public float Thrust;

        [Header("Ускорение бокового поворота")]
        [Range(0.0f, 100.0f)]
        public float Agility;

        [Header("Максимальная скорость байка")]
        public float MaxSpeed;

        //[Header("Максимальная скорость угла поворота")]
        //public float MaxAngle;

        [Header("Коэффициент сопротивления вперед при отсутствии газа")]
        [Range(0.0f, 1.0f)]
        public float LinearDrag;

        [Header("Коэффициент бокового сопротивления отсутствии газа")]
        [Range(0.0f, 1.0f)]
        public float RotateDrag;

        [Header("Коэффициент отскока при столкнавении с препятствием")]
        [Range(0.0f, 1.0f)]
        public float CollisionBounceFactor;

        public bool Afterburner;

        public GameObject EngineModel;

        public GameObject HullModel;

    }


    public class Bike : MonoBehaviour
    {
        [SerializeField] private BikeParameters _bikeParametersInitial;

        [SerializeField] private BikeViewController _visualController;

        /// <summary>
        /// Управление газом байка. Нормализованное. от-1 до +1.
        /// </summary>
        private float _forwardThrustAxis; 

        /// <summary>
        /// Установка значений педали газа
        /// </summary>
        public void SetForwardThrustAxis(float val)
        {
            _forwardThrustAxis = val;
        }

        /// <summary>
        /// Управление отклонением влево и вправо. Нормализованное. От -1 до +1.
        /// </summary>
        private float _horizontalThrustAxis;

        public void SetHorizontalThrustAxis(float val)
        {
            _horizontalThrustAxis = val;
        }

        [SerializeField] private RaceTrack _track;

        private float _distance;
        private float _velocity;
        private float _rollAngle;

        private void Update()
        {
            //MoveBike();
            UpdateBikePhysics();
        }

        private void UpdateBikePhysics()
        {
            float dt = Time.deltaTime;
            //шаг ускорения вперед
            float dv = dt * _forwardThrustAxis * _bikeParametersInitial.Thrust;

            //шаг бокового ускорения
            float da = dt * _horizontalThrustAxis * _bikeParametersInitial.Agility;

            _velocity += dv;

            _rollAngle += da;

            _velocity = Mathf.Clamp(_velocity, -_bikeParametersInitial.MaxSpeed, _bikeParametersInitial.MaxSpeed);

            float dS = _velocity * dt;

            //collision
            if(Physics.Raycast(transform.position, transform.forward, dS))
            {
                _velocity = - _velocity * _bikeParametersInitial.CollisionBounceFactor;
                dS = _velocity * dt;
                
            }

            _distance += dS;
            
            _velocity += -_velocity * _bikeParametersInitial.LinearDrag * dt;

            _rollAngle += -_rollAngle * _bikeParametersInitial.RotateDrag * dt;

            if (_distance < 0)
            {
                _distance = 0;
            }

            Vector3 bikePosition = _track.GetPosition(_distance);
            Vector3 bikeDirection = _track.GetDirection(_distance);

            //Вращение вокруг оси
            Quaternion q = Quaternion.AngleAxis(_rollAngle, Vector3.forward);
            Vector3 trackOffset = q* (Vector3.up*_track.Radius);

            transform.position = bikePosition - trackOffset;
            transform.rotation = Quaternion.LookRotation(bikeDirection, trackOffset);

        }

        //старый вариант
        private void MoveBike()
        {
            float currentForwardVelocity = _forwardThrustAxis * _bikeParametersInitial.MaxSpeed;
            Vector3 forwardMoveDelta = transform.forward * currentForwardVelocity * Time.deltaTime;

            transform.position += forwardMoveDelta;  
        }
    }
}

