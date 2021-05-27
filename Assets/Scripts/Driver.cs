using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Класс водителя в автобусе
/// </summary>
public class Driver : MonoBehaviour
{
    [Header("Автобус которым управляет водитель")]
    [SerializeField] private Bus _bus;

    //Здесь располагаются методы управления состоянием автобуса, остановки и начала движения
    /// <summary>
    /// Начало движения
    /// </summary>
    public void StartMovement()
    {

    }

    /// <summary>
    /// Остановка
    /// </summary>
    public void StopMovement()
    {

    }


}
