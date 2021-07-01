using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Race
{
    public class TrackEntryViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _trackName;

        [Header("Вывод длины трэка")]
        [SerializeField] private TextMeshProUGUI _trackLenght;

        [SerializeField] private TrackDescription _trackDescription;

        private TrackDescription _activeDescription;

        private void Start()
        {
            if(_trackDescription !=null)
            {
                SetViewValues(_trackDescription);

                //вывод длины трэка
                float lenght = 0;
                lenght = PlayerPrefs.GetFloat("TrackSampledLength");
                _trackLenght.text = lenght.ToString() + " м";
            }
        }

        public void SetViewValues(TrackDescription desc)
        {
            _activeDescription = desc;
            _trackName.text = desc.TrackName;
        }

        public void OnButtonStartLevel()
        {
            SceneManager.LoadScene(_activeDescription.SceneNickname);
        }
    }


}