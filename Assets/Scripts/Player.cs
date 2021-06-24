using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Игрок гонки
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private string _nickname;
        public string Nickname => _nickname;

        [SerializeField] private Bike _activeBike;

        private void Update()
        {
            ControlBike();
        }

        private void ControlBike()
        {
            _activeBike.SetForwardThrustAxis(0);
            _activeBike.SetHorizontalThrustAxis(0);

            if (!_activeBike.IsMovementControlsActive)
            {
                return;
            }

            if(Input.GetKey(KeyCode.W))
            {
                _activeBike.SetForwardThrustAxis(1);
            }

            if(Input.GetKey(KeyCode.S))
            {
                _activeBike.SetForwardThrustAxis(-1);
            }

            if (Input.GetKey(KeyCode.A))
            {
                _activeBike.SetHorizontalThrustAxis(-1);
            }

            if (Input.GetKey(KeyCode.D))
            {
                _activeBike.SetHorizontalThrustAxis(1);
            }

            _activeBike.EnableAfterburner = Input.GetKey(KeyCode.Space);

        }

    }
}
