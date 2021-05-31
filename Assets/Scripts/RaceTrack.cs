using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Базовый класс который определяет нашу трубу для гонок
    /// </summary>
    public abstract class RaceTrack : MonoBehaviour
    {
        [Header("Радиус трубы")]
        [SerializeField] private float _radius;
        public float Radius => _radius;


        /// <summary>
        /// Метод возвращает длину трека
        /// </summary>
        /// <returns></returns>
        public abstract float GetTrackLenght();

        /// <summary>
        /// Метод возвращает позицию в 3д кривой цетральной линии трубы
        /// </summary>
        /// <param name="distance">Дистанция от начала трубы до ее GetTrackLength</param>
        /// <returns></returns>
        public abstract Vector3 GetPosition(float distance);

        /// <summary>
        /// Метод возвращает направление в 3д кривой цетральной линии трубы.
        /// Касательная кривой в точке.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public abstract Vector3 GetDirection(float distance);
    }
}
