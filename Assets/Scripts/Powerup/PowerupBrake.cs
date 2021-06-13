using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Race
{
    /// <summary>
    /// Реализует торможение при наезде
    /// </summary>
    public class PowerupBrake : Powerup
    {
        [Header("Величина на которую будет происходить торможение")]
        [SerializeField] private float _speedAmount;

        public override void OnPickedByBike(Bike bike)
        {
            bike.SpeedDown(_speedAmount);
        }
    }
}
