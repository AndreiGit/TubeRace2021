using Race;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Race
{
#if UNITY_EDITOR
    [CustomEditor(typeof(RaceTrackRound))]
    public class RaceTrackRoundEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                (target as RaceTrackRound).GenerateTrackDate();
            }
        }
    }
#endif


    public class RaceTrackRound : RaceTrack
    {

        [Header("Радиус трэка")]
        [SerializeField] private float _radiusTrack;

        [Header("Общая длина трэка")]
        [SerializeField] private float _trackSampledLength;

        [Header("Точность отрисовки точек на треке")]
        [SerializeField] private int _division;

        [Header("Линейние вращения")]
        [SerializeField] private Quaternion[] _trackSampledRotations;

        [Header("Массив сгенерированных точек на треке")]
        [SerializeField] private Vector3[] _trackSampledPoints;

        [Header("Расстояния между парами точек на трэке")]
        [SerializeField] private float[] _trackSampledSegmentLenghts;

        [Header("Отрисовка в редакторе окружности трэка")]
        [SerializeField] private bool _debugDrawRound;

        [Header("Отрисовка в редакторе сгенерированных точек")]
        [SerializeField] private bool _debugDrawSampledPoints;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_debugDrawRound)
            {
                DrawRound();
            }

            if (_debugDrawSampledPoints)
            {
                DrawSampledTrackPoint();
            }
        }

        /// <summary>
        /// Метод отрисовки окружности в редакторе
        /// </summary>
        private void DrawRound()
        {
            Handles.DrawWireArc(this.transform.position, this.transform.up, -this.transform.right, 360, _radiusTrack);
        }


        public void GenerateTrackDate()
        {
            Debug.Log("Generating");

            List<Vector3> points = new List<Vector3>();
            List<Quaternion> rotations = new List<Quaternion>();

            //нахождение точек на окружности
            for (int i = 0; i < _division; i++)
            {
                float angle = i * Mathf.PI * 2f / _division;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * _radiusTrack, this.transform.position.y, Mathf.Sin(angle) * _radiusTrack);

                points.Add(newPos);
            }

            _trackSampledPoints = points.ToArray();

            float t = 0;

            //нахождение вращений для точек на окружности
            for (int i = 0; i < _trackSampledPoints.Length - 1; i++)
            {
                Vector3 a = _trackSampledPoints[i];
                Vector3 b = _trackSampledPoints[i + 1];

                Vector3 dir = (_trackSampledPoints[i + 1] - _trackSampledPoints[i]).normalized;

                Vector3 up = Vector3.Lerp(a, b, t);

                Quaternion rotation = Quaternion.LookRotation(dir, up);

                rotations.Add(rotation);

                t += 1.0f / (_trackSampledPoints.Length - 1);

            }

            //нахождение вращения для последней точки на окружности
            Vector3 al = _trackSampledPoints[_trackSampledPoints.Length - 1];
            Vector3 bl = _trackSampledPoints[0];

            Vector3 dirl = (_trackSampledPoints[0] - _trackSampledPoints[_trackSampledPoints.Length - 1]).normalized;
            Vector3 upl = Vector3.Lerp(al, bl, t);
            Quaternion rotationl = Quaternion.LookRotation(dirl, upl);

            rotations.Add(rotationl);

            _trackSampledRotations = rotations.ToArray();



            //precompute lengths
            {
                _trackSampledSegmentLenghts = new float[_trackSampledPoints.Length - 1];

                _trackSampledLength = 0;

                for (int i = 0; i < _trackSampledPoints.Length - 1; i++)
                {
                    Vector3 a = _trackSampledPoints[i];
                    Vector3 b = _trackSampledPoints[i + 1];

                    float segmentLength = (b - a).magnitude;

                    _trackSampledSegmentLenghts[i] = segmentLength;

                    _trackSampledLength += segmentLength;
                }
            }

            // сообщение редактору что скрипт изменен
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Отрисовка в редакторе сгенерированных точек
        /// </summary>
        private void DrawSampledTrackPoint()
        {
            Handles.DrawAAPolyLine(_trackSampledPoints);
        }

#endif
        public override Quaternion GetRotation(float distance)
        {
            distance = Mathf.Repeat(distance, _trackSampledLength);

            for (int i = 0; i < _trackSampledSegmentLenghts.Length; i++)
            {
                float diff = distance - _trackSampledSegmentLenghts[i];

                if (diff < 0)
                {

                    //retern position
                    float t = distance / _trackSampledSegmentLenghts[i];
                    // находится вращение на самом сегменте
                    return Quaternion.Slerp(_trackSampledRotations[i], _trackSampledRotations[i + 1], t);
                }
                else
                {
                    distance -= _trackSampledSegmentLenghts[i];
                }
            }
            return Quaternion.identity;
        }




        public override Vector3 GetDirection(float distance)
        {
            
            distance = Mathf.Repeat(distance, _trackSampledLength);

            for (int i = 0; i < _trackSampledSegmentLenghts.Length; i++)
            {
                float diff = distance - _trackSampledSegmentLenghts[i];

                if (diff < 0)
                {

                    return (_trackSampledPoints[i + 1] - _trackSampledPoints[i]).normalized;
                }
                else
                {
                    distance -= _trackSampledSegmentLenghts[i];
                }
            }
            
            return Vector3.forward;
        }




        public override Vector3 GetPosition(float distance)
        {
            distance = Mathf.Repeat(distance, _trackSampledLength);

            for (int i = 0; i < _trackSampledSegmentLenghts.Length; i++)
            {
                float diff = distance - _trackSampledSegmentLenghts[i];

                if (diff < 0)
                {

                    //retern position
                    float t = distance / _trackSampledSegmentLenghts[i];
                    // находится позиция на самом сегменте
                    return Vector3.Lerp(_trackSampledPoints[i], _trackSampledPoints[i + 1], t);

                }
                else
                {
                    distance -= _trackSampledSegmentLenghts[i];
                }
            }

            return Vector3.zero;
        }




        public override float GetTrackLenght()
        {
            return _trackSampledLength;
        }
    }
}
