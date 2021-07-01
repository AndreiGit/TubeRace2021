using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class MainMenuViewController : MonoBehaviour
    {
        [SerializeField] private TrackSelectionViewController _trackSelectionViewController;
        [SerializeField] private OptionsViewController _optionsViewController;
        public void OnButtonNewGame()
        {
            _trackSelectionViewController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnButtonOptions()
        {
            _optionsViewController.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnButtonExit()
        {
            Application.Quit();
        }
    }
}
