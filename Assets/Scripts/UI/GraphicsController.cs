using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    public class GraphicsController : MonoBehaviour
    {
      
        public void SetResolution800x600()
        {
            Screen.SetResolution(800, 600, false);
        }

        public void SetResolution1024x768()
        {
            Screen.SetResolution(1024, 768, false);
        }

    }
}
