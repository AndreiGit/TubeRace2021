using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{/// <summary>
/// Генерирует визуальные участки трэка
/// </summary>
    public class ObjectPlacer : MonoBehaviour
    {
        [Header("Генерируемый префаб объекта")]
        [SerializeField] private GameObject _prefab;

        [Header("Количество объектов")]
        [SerializeField] private int _numObjects;

        [Header("Трэк")]
        [SerializeField] private RaceTrack _track;

        [Header("Одинаковое смещение вращения для всех объектов")]
        [Range(0, 360)]
        [SerializeField] private float _rotationOffset;

        [Header("Шаг расположения объектов (при = 1 шаг по умолчанию по умолчанию)")]
        [Range(0.1f, 2)]
        [SerializeField] private float _positionOffset;



        [Header("Рандом поворота участков при генерации")]
        [SerializeField] private bool _randomizeRotation;

        [Header("Рандом позиций участков при генерации")]
        [SerializeField] private bool _randomizePosition;

        [Header("Настройка рандома позиции")] 
        [Range(0,1)]
        [SerializeField] private float _randomPositionKoef;

        private void Start()
        {
            float distance = 0;

            float step = _track.GetTrackLenght() / _numObjects;

            for (int i = 0; i < _numObjects; i++)
            {
                var e = Instantiate(_prefab);

                if (_randomizePosition)
                {
                    float randomDistanceOffset = step * UnityEngine.Random.Range(-_randomPositionKoef, _randomPositionKoef);
                    e.transform.position = _track.GetPosition(distance + randomDistanceOffset);
                }
                else
                {
                    //distance += step * _positionOffset;
                    e.transform.position = _track.GetPosition(distance);
                    
                }

                e.transform.rotation = _track.GetRotation(distance);

                if(_randomizeRotation)
                {
                    e.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0, 360), Space.Self);
                }
                else
                {
                    e.transform.Rotate(Vector3.forward, _rotationOffset, Space.Self);
                }

                
                distance += step* _positionOffset;          
            }
        }
    }
}
