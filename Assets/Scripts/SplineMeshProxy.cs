using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SplineMeshProxy))]
public class SplineMeshProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update"))
        {
            (target as SplineMeshProxy).UpdatePoints();
        }
    }
}
#endif



    [RequireComponent(typeof (SplineMesh.Spline))]
    public class SplineMeshProxy : MonoBehaviour
    {
        [SerializeField] private RaceTrackCurved _track;

        [SerializeField] private CurvedTrackPoint _pointA;
        [SerializeField] private CurvedTrackPoint _pointB;
        //[SerializeField] private CurvedTrackPoint _pointC;
        //[SerializeField] private CurvedTrackPoint _pointD;


        public void UpdatePoints()
        {
            var spline = GetComponent<SplineMesh.Spline>();

            var n0 = spline.nodes[0];
            n0.Position = _pointA.transform.position;
            n0.Direction = _pointA.transform.position +  _pointA.transform.forward * _pointA.Lenght;

            var n1 = spline.nodes[1];
            n1.Position = _pointB.transform.position;
            n1.Direction = _pointB.transform.position + _pointB.transform.forward * _pointB.Lenght;

            //var n2 = spline.nodes[2];
            //n2.Position = _pointC.transform.position;
            //n2.Direction = _pointC.transform.position + _pointC.transform.forward * _pointC.Lenght;

            //var n3 = spline.nodes[3];
            //n3.Position = _pointD.transform.position;
            //n3.Direction = _pointD.transform.position + _pointD.transform.forward * _pointD.Lenght;

            //var n4 = spline.nodes[4];
            //n4.Position = _pointA.transform.position;
            //n4.Direction = _pointA.transform.position + _pointA.transform.forward * _pointA.Lenght;


        }

    }
}
