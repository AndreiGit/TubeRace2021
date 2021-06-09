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

        [Header("Дополнительное ускорение байка")]
        public float AfterburnerThrust;

        [Header("Ускорение бокового поворота")]
        [Range(0.0f, 100.0f)]
        public float Agility;

        [Header("Максимальная скорость байка")]
        public float MaxSpeed;

        [Header("Максимальная скорость ускорителя")]
        public float AfterburnerMaxSpeedBonus;

        [Header("Охлаждение в секунду")]
        public float AfterburnerCoolSpeed;

        [Header("Тепловыделение в секунду")]
        public float AfterburnerHeatGeneration;

        [Header("Добавляется к температуре двигателя при столкновении")]
        public float HeatCollision;

        [Header("Максимальный перегрев")]
        public float AfterburnerMaxHeat;

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

        /// <summary>
        /// Вкл/Выкл доп ускорения
        /// </summary>
        public bool EnableAfterburner { get; set; }

        public void SetHorizontalThrustAxis(float val)
        {
            _horizontalThrustAxis = val;
        }

        [SerializeField] private RaceTrack _track;
        public RaceTrack Track => _track;

        private float _distance;
        private float _velocity;
        private float _rollAngle;

        /// <summary>
        /// Поблучный метод для возвращения дистанции
        /// </summary>
        public float GetDistance()
        {
            return _distance;
        }

        /// <summary>
        /// Поблучный метод для возвращения скорости
        /// </summary>
        public float GetVelocity()
        {
            return _velocity;
        }

        /// <summary>
        /// Поблучный метод для возвращения угла поворота
        /// </summary>
        public float GetRollAngle()
        {
            return _rollAngle;
        }

        private void Update()
        {
            UpdateAfterburnerHeat();
            UpdateBikePhysics();
        }

        private float _afterburnerHeat;

        /// <summary>
        /// Нормализованное значение перегрева двигателя
        /// </summary>
        public float GetNormalizedHeat()
        {
            if (_bikeParametersInitial.AfterburnerMaxHeat > 0)
            {
                float t = _afterburnerHeat / _bikeParametersInitial.AfterburnerMaxHeat;
                return t;
            }

            return 0;
        }

        /// <summary>
        /// Охлаждение ускорителя
        /// </summary>
        public void CoolAfterburner()
        {
            _afterburnerHeat = 0;
        }

        /// <summary>
        /// Обновление информации о перегреве
        /// </summary>
        private void UpdateAfterburnerHeat()
        {
            _afterburnerHeat -= _bikeParametersInitial.AfterburnerCoolSpeed*Time.deltaTime;
            
            if(_afterburnerHeat < 0)
            {
                _afterburnerHeat = 0;
            }
        }



        private void UpdateBikePhysics()
        {
            float dt = Time.deltaTime;


            float FthrustMax = _bikeParametersInitial.Thrust;
            float Vmax = _bikeParametersInitial.MaxSpeed;
            float F = _forwardThrustAxis * _bikeParametersInitial.Thrust;

            if (EnableAfterburner && ConsumeFuelForAfterburner(1.0f * Time.deltaTime))
            {

                _afterburnerHeat += _bikeParametersInitial.AfterburnerHeatGeneration * Time.deltaTime;

                F += _bikeParametersInitial.AfterburnerThrust;
                Vmax += _bikeParametersInitial.AfterburnerMaxSpeedBonus;
                FthrustMax += _bikeParametersInitial.AfterburnerThrust;
            }

            //drag
            F += -_velocity * (FthrustMax / Vmax);

            //шаг ускорения вперед
            float dv = dt * F;

            //шаг бокового ускорения
            float da = dt * _horizontalThrustAxis * _bikeParametersInitial.Agility;

            _velocity += dv;

            _rollAngle += da;

           // _velocity = Mathf.Clamp(_velocity, -_bikeParametersInitial.MaxSpeed, _bikeParametersInitial.MaxSpeed);

            float dS = _velocity * dt;

            //collision
            if(Physics.Raycast(transform.position, transform.forward, dS))
            {
                //Приращение температуры при столкновении
                _afterburnerHeat += _bikeParametersInitial.HeatCollision;

                _velocity = - _velocity * _bikeParametersInitial.CollisionBounceFactor;
                dS = _velocity * dt;
                
            }

            _prevDistance = _distance;

            _distance += dS;
            
         //   _velocity += -_velocity * _bikeParametersInitial.LinearDrag * dt;

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

        private float _prevDistance;
        public float PrevDistance => _prevDistance;
        //топливо 0 - 100
        private float _fuel;

        public float Fuel => _fuel;

        /// <summary>
        /// Добавление топлива
        /// </summary>
        public void AddFuel(float amount)
        {
            _fuel += amount;
            _fuel = Mathf.Clamp(_fuel, 0, 100);
        }

        /// <summary>
        /// Снижение скорости
        /// </summary>
        public void SpeedDown(float amount)
        {
            _velocity -= amount;
            _velocity = Mathf.Clamp(_velocity, 0, _bikeParametersInitial.MaxSpeed);
        }

        public static readonly string Tag = "Bike";

        /// <summary>
        /// Метод дающий понять, можем ли мы использовать ускорение
        /// </summary>
        public bool ConsumeFuelForAfterburner(float amount)
        {
            if(_fuel <= amount)
            {
                return false;
            }

            _fuel -= amount;

            return true;
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

