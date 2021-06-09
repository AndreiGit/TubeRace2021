using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Подбор охлаждения
    /// </summary>
    public class PowerupCoolant : Powerup
    {
        public override void OnPickedByBike(Bike bike)
        {
            bike.CoolAfterburner();
        }
    }
}
