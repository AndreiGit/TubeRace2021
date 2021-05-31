using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// Подбираемая штука
    /// </summary>
    public abstract class Powerup : MonoBehaviour
    {
        /// <summary>
        /// Срабатывает пр подбирании
        /// </summary>
        public abstract void OnPickedByPlayer();
    }
}
