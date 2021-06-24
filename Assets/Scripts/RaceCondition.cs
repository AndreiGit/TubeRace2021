using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Race
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RaceCondition : MonoBehaviour
    {
        public bool IsTriggered { get; protected set; }

        public virtual void OnReceStart()
        {

        }

        public virtual void OnReceEnd()
        {

        }

    }
}
