using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Переключает камеры первого и третьего лица
/// </summary>
public class CameraSelect : MonoBehaviour
{
    [SerializeField] private Camera _firstPerson;
    [SerializeField] private Camera _secondPerson;

    [SerializeField] private GameObject _canvasSecondPerson;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(_firstPerson.gameObject.activeSelf == true)
            {
                _firstPerson.gameObject.SetActive(false);

                _secondPerson.gameObject.SetActive(true);
                _canvasSecondPerson.SetActive(true);
            }
            else
            {
                _firstPerson.gameObject.SetActive(true);

                _secondPerson.gameObject.SetActive(false);
                _canvasSecondPerson.SetActive(false);
            }

        }
        
    }
}
