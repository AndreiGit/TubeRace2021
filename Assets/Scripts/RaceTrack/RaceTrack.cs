using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Ѕазовый класс который определ€ет нашу трубу дл€ гонок
    /// </summary>
    public abstract class RaceTrack : MonoBehaviour
    {
        [Header("–адиус трубы")]
        [SerializeField] private float _radius;
        public float Radius => _radius;


        /// <summary>
        /// ћетод возвращает длину трека
        /// </summary>
        /// <returns></returns>
        public abstract float GetTrackLenght();

        /// <summary>
        /// ћетод возвращает позицию в 3д кривой цетральной линии трубы
        /// </summary>
        /// <param name="distance">ƒистанци€ от начала трубы до ее GetTrackLength</param>
        /// <returns></returns>
        public abstract Vector3 GetPosition(float distance);

        /// <summary>
        /// ћетод возвращает вращение в 3д кривой цетральной линии трубы
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public virtual Quaternion GetRotation(float distance)
        {
            return Quaternion.identity;
        }


        /// <summary>
        /// ћетод возвращает направление в 3д кривой цетральной линии трубы.
        ///  асательна€ кривой в точке.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public abstract Vector3 GetDirection(float distance);
    }
}
