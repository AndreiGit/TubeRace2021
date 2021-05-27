using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной класс сущности автобус
/// </summary>
public class Bus : MonoBehaviour
{
    [Header("Имя автобуса, название модели")]
    [SerializeField] private string _name;

    [Header("Стоимость проезда")]
    public int CostTravel;

    [Header("Вместимость, кол-во человек")]
    public int CapacityPassenger;

    [Header("Скорость автобуса")]
    public float Speed;

    //Список пассажиров в автобусе
    public List<Passenger> Passengers = new List<Passenger>();

}
