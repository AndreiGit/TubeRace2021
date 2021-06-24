using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Race
{
    public class CountdownViewController : MonoBehaviour
    {
        [SerializeField] private RaceController _raceController;

        [SerializeField] private TextMeshProUGUI _label;

        private float _count = 0.8f;
        private void Update()
        {
            int t = (int)_raceController.CountTimer;

            _label.text = (t).ToString();

            if(_raceController.CountTimer <= 0)
            {
                _label.text = "Старт!";
                _count -= Time.deltaTime;
            }

            if(_count<0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
