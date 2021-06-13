using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Race
{
    public class CurvedTrackPoint : MonoBehaviour
    {
        [Header("Длина")]
        [SerializeField] private float _lenght = 1.0f;
        public float Lenght => _lenght;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position,5);
        }

    }
}
