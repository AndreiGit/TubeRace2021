using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Race
{
    public class RaceResaultViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _place;
        [SerializeField] private TextMeshProUGUI _topSpeed;
        [SerializeField] private TextMeshProUGUI _totalTime;
        [SerializeField] private TextMeshProUGUI _bestLap;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(Bike.BikeStatistics stats)
        {
            gameObject.SetActive(true);

            _place.text ="Place: "+ stats.RacePlace.ToString();
            _topSpeed.text ="Top speed: " + ((int)(stats.TopSpeed)).ToString() + " m/s";
            _totalTime.text ="Time: "+ stats.TotalTime.ToString()+" seconds";
            _bestLap.text = "BestLap:" + stats.BestLapTime.ToString() + " seconds";

        }
    }
}
