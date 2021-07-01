using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Race
{
    public class PauseViewController : MonoBehaviour
    {
        public static readonly string MainMenuScene = "scene_main_menu";
        [SerializeField] private RectTransform _content;
        [SerializeField] private RaceController _raceController;

        private void Start()
        {
            _content.gameObject.SetActive(false);   
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_raceController.IsRaceActive)
                {
                    _content.gameObject.SetActive(!_content.gameObject.activeInHierarchy);
                    UpdateGameActivity(!_content.gameObject.activeInHierarchy);
                }
            }
        }


        private void UpdateGameActivity(bool flag)
        {
            if(flag)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        public void OnButtonContinue()
        {
            UpdateGameActivity(true);
            _content.gameObject.SetActive(false);
        }

        public void OnButtonEndRace()
        {
            SceneManager.LoadScene(MainMenuScene);
        }

    }
}
