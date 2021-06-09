using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Подбор топлива
    /// </summary>
    public class PowerupFuel : Powerup
    {
        [Header("Кол-во топлива которое будет добавлено")]
        [Range(0.0f,100.0f)]
        [SerializeField] private float _fuelAmount;
        public override void OnPickedByBike(Bike bike)
        {
            bike.AddFuel(_fuelAmount);
        }
    }
}
