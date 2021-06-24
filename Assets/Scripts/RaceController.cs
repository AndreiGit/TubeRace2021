using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Race
{
    public class RaceController : MonoBehaviour
    {
        [Header("Трэк")]
        [SerializeField] private RaceTrack _track;

        [Header("Максимальное кол-во кругов")]
        [SerializeField] private int _maxLaps;
        public int MaxLaps => _maxLaps;

        public enum RaceMode
        {
            Laps,
            Time,
            LastStanding
        }

        [Header("Тип гонки")]
        [SerializeField] private RaceMode _raceMode;

        [Header("Событие начала гонки")]
        [SerializeField] private UnityEvent _eventStartFinished;

        [Header("Событие об окончании гонки")]
        [SerializeField] private UnityEvent _eventRaceFinished;

        [Header("Участники")]
        [SerializeField] private Bike[] _bikes;
        public Bike[] Bikes => _bikes;

        [Header("Счетчик в начале")]
        [SerializeField] private int _countdownTimer;
        public int CountdownTimer => _countdownTimer;

        private float _countTimer;
        public float CountTimer => _countTimer;

        public bool IsRaceActive { get; private set; }

        [SerializeField] private RaceCondition[] _conditions;

        private void Start()
        {
            StartRace();
        }

        private void Update()
        {
            if(!IsRaceActive)
            {
                return;
            }

            UpdateBikeRacePositions();
            UpdateRacePrestart();
            UpdateConditions();

        }

        private void UpdateConditions()
        {
            if(IsRaceActive)
            {
                return;
            }

            foreach(var c in _conditions)
            {
                if(!c.IsTriggered)
                {
                    return;
                }
            }

            //race ends
            EndRace();
            Debug.Log("Race end");
            _eventRaceFinished?.Invoke();
        }

        private void UpdateRacePrestart()
        {
            if (_countTimer > 0)
            {
                _countTimer -= Time.deltaTime;

                if (_countTimer < 0)
                {
                    foreach (var e in _bikes)
                    {
                        e.IsMovementControlsActive = true;
                    }
                }
            }
        }

        /// <summary>
        /// Метод начала гонки
        /// </summary>
        public void StartRace()
        {
            _activeBikes = new List<Bike>(_bikes);
            _finishedBikes = new List<Bike>();

            IsRaceActive = true;

            _countTimer = _countdownTimer;

            foreach(var c in _conditions)
            {
                c.OnReceStart();
            }

            foreach( var b in _bikes)
            {
                b.OnRaceStart();
            }

            _eventStartFinished?.Invoke();
        }

        private List<Bike> _activeBikes;
        private List<Bike> _finishedBikes;
        [SerializeField] private RaceResaultViewController _raceResaultViewController;

        /// <summary>
        /// Проверяет проехал ли байк до конца трэка
        /// </summary>
        private void UpdateBikeRacePositions()
        {
            //if(_activeBikes.Count == 0)
            //{
            //    EndRace();
            //    return;
            //}

            foreach(var v in _activeBikes)
            {
                if(_finishedBikes.Contains(v))
                {
                    continue;
                }

                float dist = v.GetDistance();
                float totalRaceDistance = _maxLaps * _track.GetTrackLenght();

                if(dist > totalRaceDistance)
                {
                    _finishedBikes.Add(v);
                    v.Statistics.RacePlace = _finishedBikes.Count;
                    v.OnRaceEnd();

                    if(v.IsplayerBike)
                    {
                        _raceResaultViewController.Show(v.Statistics);
                    }
                }
            }
        }

        /// <summary>
        /// Прерывание гонки
        /// </summary>
        public void EndRace()
        {
            IsRaceActive = false;

            foreach (var c in _conditions)
            {
                c.OnReceEnd();
            }
        }
    }
}
