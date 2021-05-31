using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeTrack : MonoBehaviour
{
    [Header("Скорость перемещения капсуля")]
    [SerializeField] private float _speed;

    [SerializeField] private RaceTrackLinear _raceTrackLinear;

    private void Update()
    {
        _raceTrackLinear.MovementPlayMode(_speed);
    }
}
