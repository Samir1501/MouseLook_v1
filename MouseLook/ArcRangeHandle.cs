#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MouseLook
{
    public class ArcRangeHandle
    {
        private readonly ArcHandle _handle = new();
        private readonly float[] _mids = new float[3];
        private readonly Color[] _cols = {new (1f,0.2f,0,0.05f),new (0.3f,1,0,0.05f)};
        private readonly float[] _zAngle = {90, 0};
        public Transform transform;
        private Quaternion _lookAngle;
        private Matrix4x4 _handleMatrix;
        public AngleLimit[] DrawHandle(AngleLimit[] limits,bool[] enableLimits)
        {
            _handle.radius = HandleUtility.GetHandleSize(transform.position)*1.2f;
            
            for (int i = 0; i < limits.Length; i++)
            {
                if (enableLimits[i] == false)
                {
                    _mids[i] = 0;
                } else {
                    _mids[i] = (limits[i].min + limits[i].max) / 2;
                    _lookAngle = Quaternion.Euler(_mids[0], -_mids[1], _zAngle[i]);
                    _handleMatrix = Matrix4x4.TRS(
                        transform.position,
                        transform.rotation * _lookAngle,
                        Vector3.one
                    );
                    using (new Handles.DrawingScope(_handleMatrix))
                    {
                        _cols[i].a = 0.05f;
                        _handle.fillColor = _cols[i];
                        _cols[i].a = 1;
                        _handle.wireframeColor = _cols[i];
                        _handle.angleHandleColor = _cols[i];
                        _handle.angle = limits[i].min - _mids[i];

                        EditorGUI.BeginChangeCheck();
                        _handle.DrawHandle();
                        if (EditorGUI.EndChangeCheck())
                            limits[i].max = Mathf.Clamp(_mids[i] - _handle.angle, _mids[i] - 180, _mids[i] + 180);

                        _handle.angle = limits[i].max - _mids[i];
                        EditorGUI.BeginChangeCheck();
                        _handle.DrawHandle();
                        if (EditorGUI.EndChangeCheck())
                            limits[i].min = Mathf.Clamp(_mids[i] - _handle.angle, _mids[i] - 180, _mids[i] + 180);
                    }
                }
            }
            return limits;
        }
    }
}
#endif