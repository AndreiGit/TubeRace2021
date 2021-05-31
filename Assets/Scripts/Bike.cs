using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    [System.Serializable]
    public class BikeParameters
    {
        [Header("Масса байка")]
        [Range(0.0f, 10.0f)]
        public float Mass;

        [Header("Ускорение байка")]
        [Range(0.0f, 100.0f)]
        public float Thrust;

        [Range(0.0f, 100.0f)]
        public float Agility;

        [Header("Максимальная скорость байка")]
        public float MaxSpeed;

        public bool Afterburner;

        public GameObject EngineModel;

        public GameObject HullModel;

    }


    public class Bike : MonoBehaviour
    {
        [SerializeField] private BikeParameters _bikeParametersInitial;

        [SerializeField] private BikeViewController _visualController;
    }
}

