using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Race.UI
{
    /// <summary>
    /// Класс выводит информацию в ui для байка
    /// </summary>
    public class BikeHudViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _labelSpeed;
        [SerializeField] private TextMeshProUGUI _labelDistance;
        [SerializeField] private TextMeshProUGUI _labelRollAngle;
        [SerializeField] private TextMeshProUGUI _labelLapNumber;
        [SerializeField] private TextMeshProUGUI _labelHeat;
        [SerializeField] private TextMeshProUGUI _labelFuel;

        [SerializeField] private Bike _bike;

        private void Update()
        {
            _labelSpeed.text ="Speed: " + ((int)_bike.GetVelocity()).ToString() + " m/s";
            _labelDistance.text = "Distance: " + ((int)_bike.GetDistance()).ToString() + " m";
            _labelRollAngle.text = "Angle: " + ((int)/*Mathf.Rad2Deg * */_bike.GetRollAngle()).ToString() + " deg";

            int laps =(int)( _bike.GetDistance()/ _bike.Track.GetTrackLenght());
            _labelLapNumber.text ="Lap: "+ (laps+1);

            int heat = (int)(_bike.GetNormalizedHeat()*100.0f);
            _labelHeat.text ="Heat: "+ heat.ToString();

            int fuel = (int) _bike.Fuel;
            _labelFuel.text ="Fuel: "+ fuel;
        }

    }
}
