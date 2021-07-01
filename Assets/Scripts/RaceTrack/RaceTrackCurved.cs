using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Race
{
#if UNITY_EDITOR
    [CustomEditor(typeof(RaceTrackCurved))]
    public class RaceTrackCurvedEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Generate"))
            {
               (target as RaceTrackCurved).GenerateTrackDate();
            }
        }
    }
#endif

    public class RaceTrackCurved : RaceTrack
    {
        [Header("Массив точек образующих трэк")]
        [SerializeField] private CurvedTrackPoint[] _trackPoints;

        [Header("Точность отрисовки точек на треке")]
        [SerializeField] private int _division;

        [Header("Линейние вращения")]
        [SerializeField] private Quaternion[] _trackSampledRotations;

        [Header("Массив сгенерированных точек на треке")]
        [SerializeField] private Vector3 [] _trackSampledPoints;

        [Header("Расстояния между парами точек на трэке")]
        [SerializeField] private float[] _trackSampledSegmentLenghts;

        [Header("Общая длина трэка")]
        [SerializeField] private float _trackSampledLength;

        [Header("Отрисовка в редакторе трека бизье")]
        [SerializeField] private bool _debugDrawBezier;

        [Header("Отрисовка в редакторе сгенерированных точек")]
        [SerializeField] private bool _debugDrawSampledPoints;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(_debugDrawBezier)
            {
                DrawBezierCurve();
            }

            if (_debugDrawSampledPoints)
            {
                DrawSampledTrackPoint();
            }
        }

        public void GenerateTrackDate()
        {
            Debug.Log("Generating");
            if (_trackPoints.Length < 3)
            {
                return;
            }
            else
            {
                List<Vector3> points = new List<Vector3>();
                List<Quaternion> rotations = new List<Quaternion>();

                for (int i = 0; i < _trackPoints.Length - 1; i++)
                {
                    CurvedTrackPoint a = _trackPoints[i];
                    CurvedTrackPoint b = _trackPoints[i + 1];

                    var newPoints = GenerateBezierPoints(a, b, _division);
                    var newRotations = GenerateRotations(a.transform, b.transform, newPoints);

                    rotations.AddRange(newRotations);
                    points.AddRange(newPoints);
                }
                //last points
                var lastPoints = GenerateBezierPoints(_trackPoints[_trackPoints.Length - 1], _trackPoints[0], _division);
                var lastRotations = GenerateRotations(_trackPoints[_trackPoints.Length - 1].transform, _trackPoints[0].transform, lastPoints);

                rotations.AddRange(lastRotations);
                points.AddRange(lastPoints);

                _trackSampledRotations = rotations.ToArray();
                _trackSampledPoints = points.ToArray();

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
                //сохранение длины трэка
                PlayerPrefs.SetFloat("TrackSampledLength", _trackSampledLength);
                PlayerPrefs.Save();

                //Debug.Log("Тест загрузка - "+PlayerPrefs.GetFloat("TrackSampledLength"));

                // сообщение редактору что скрипт изменен
                EditorUtility.SetDirty(this);
            }
        }

        /// <summary>
        /// Отрисовка в редакторе сгенерированных точек
        /// </summary>
        private void DrawSampledTrackPoint()
        {
            Handles.DrawAAPolyLine(_trackSampledPoints);
        }

        /// <summary>
        /// Возвращает массив сгенерированных точек между двумя точками
        /// </summary>
        private Vector3[] GenerateBezierPoints(CurvedTrackPoint a, CurvedTrackPoint b, int division)
        {
              return  Handles.MakeBezierPoints(a.transform.position, b.transform.position,
                a.transform.position + a.transform.forward * a.Lenght,
                b.transform.position - b.transform.forward * b.Lenght, division);
            
        }

        /// <summary>
        /// Возвращает массив вращений
        /// </summary>
        private Quaternion[] GenerateRotations(Transform a, Transform b, Vector3[] points)
        {
            List <Quaternion> rotations = new List<Quaternion>();
            float t = 0;

            for(int i = 0; i < points.Length -1; i++)
            {
                Vector3 dir = (points[i + 1] - points[i]).normalized;

                Vector3 up = Vector3.Lerp(a.up, b.up, t);

                Quaternion rotation = Quaternion.LookRotation(dir, up);

                rotations.Add(rotation);

                t += 1.0f / (points.Length - 1);
            }

            rotations.Add(b.rotation);
            return rotations.ToArray();
        }

        /// <summary>
        /// Метод отрисовки кривой бизье в редакторе
        /// </summary>
        private void DrawBezierCurve()
        {
            if (_trackPoints.Length < 3)
            {
                return;
            }
            else
            {
                for (int i = 0; i < _trackPoints.Length - 1; i++)
                {
                    CurvedTrackPoint a = _trackPoints[i];
                    CurvedTrackPoint b = _trackPoints[i + 1];

                    DeawTrackPartGizmo(a, b);
                }

                DeawTrackPartGizmo(_trackPoints[_trackPoints.Length - 1], _trackPoints[0]);
            }
        }

        private void DeawTrackPartGizmo(CurvedTrackPoint a, CurvedTrackPoint b)
        {
            Handles.DrawBezier(a.transform.position, b.transform.position,
                a.transform.position + a.transform.forward * a.Lenght,
                b.transform.position - b.transform.forward * b.Lenght,
                Color.green,
                Texture2D.whiteTexture,
                1.0f);
        }
#endif
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

            for(int i =0; i < _trackSampledSegmentLenghts.Length; i++)
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
                    return Quaternion.Slerp(_trackSampledRotations[i], _trackSampledRotations[i + 1],t);                  
                }
                else
                {
                    distance -= _trackSampledSegmentLenghts[i];
                }
            }
          return  Quaternion.identity;
        }

        public override float GetTrackLenght()
        {
            return _trackSampledLength;
        }
    }
}
