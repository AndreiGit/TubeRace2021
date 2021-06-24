using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class RaceConditionLaps : RaceCondition
    {
        [SerializeField] private RaceController _raceController;

        private int _currentLap;

        private void Update()
        {
            if(!_raceController.IsRaceActive && IsTriggered)
            {
                return;
            }

            Bike[] bikes = _raceController.Bikes;

            foreach(var bike in bikes)
            {
                int laps = (int)(bike.GetDistance() / bike.Track.GetTrackLenght());

                if(laps!= _currentLap)
                {
                    bike.OnBestLapCheck();
                    Debug.Log("X " + laps);
                    _currentLap = laps;
                }
               
                if(laps < _raceController.MaxLaps)
                {
                    return;
                }
            }

            IsTriggered = true;
        }
    }
}
