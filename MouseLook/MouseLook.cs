using System;
using UnityEngine;

namespace MouseLook
{
    public enum Axis { XAxis,YAxis,Both }
    public enum Mode { AutoUpdate,CallFunction }
    public enum InputMode {Mouse, Touch, Set}
    
    [Serializable]
    public struct AngleLimit
    {
        public bool enabled;
        public float min;
        public float max;
    }
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private Mode mode;
        [SerializeField] private InputMode inputMode;
        [SerializeField] private Axis axis;
        [SerializeField] private float sensitivity;
    
        [SerializeField] private bool enableSmoothMove;
        [SerializeField] private float smoothMoveSensitivity;

        [SerializeField] internal AngleLimit xLimit;
        [SerializeField] internal AngleLimit yLimit;

        [SerializeField] private bool invertY;
        [SerializeField] private bool invertX;
    
        [SerializeField] private Vector3 resultRotation;
        [SerializeField] private Vector2 input;

        internal AngleLimit GetXLimit()
        {
            xLimit.min = Mathf.Clamp(xLimit.min, xLimit.max-360, xLimit.max);
            xLimit.max = Mathf.Clamp(xLimit.max, xLimit.min, xLimit.min + 360);
            
            xLimit.min = Mathf.Clamp(xLimit.min, -360, 360);
            xLimit.max = Mathf.Clamp(xLimit.max, -360, 360);
            
            return xLimit;
        }
        internal AngleLimit GetYLimit()
        {
            yLimit.min = Mathf.Clamp(yLimit.min, yLimit.max-360, yLimit.max);
            yLimit.max = Mathf.Clamp(yLimit.max, yLimit.min, yLimit.min + 360);
            
            yLimit.min = Mathf.Clamp(yLimit.min, -360, 360);
            yLimit.max = Mathf.Clamp(yLimit.max, -360, 360);
            
            return yLimit;
        }
        
        private void Update()
        {
            if(mode == Mode.AutoUpdate) Look();
        }

        public void Look(Vector2 inputValue = default)
        {
            input = inputMode switch
            {
                InputMode.Mouse => new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")),
                InputMode.Touch => Input.touchCount > 0 ? Input.touches[0].deltaPosition : Vector2.zero,
                InputMode.Set => inputValue,
                _ => throw new ArgumentOutOfRangeException()
            };
    
            InputFunction(); 
 
            if (xLimit.enabled) resultRotation.x = Mathf.Clamp(resultRotation.x, xLimit.min, xLimit.max);
            if (yLimit.enabled) resultRotation.y = Mathf.Clamp(resultRotation.y, yLimit.min, yLimit.max);
        
            transform.localRotation = CalculatedRotation();
        }

        private Quaternion CalculatedRotation()
        {
            return enableSmoothMove 
                ? Quaternion.Lerp(transform.localRotation, Quaternion.Euler(resultRotation), smoothMoveSensitivity * Time.deltaTime)
                : Quaternion.Euler(resultRotation);
        }

        private void InputFunction()
        {
            switch (axis)
            {
                case Axis.YAxis: resultRotation.y += (invertY ? input.x : -input.x) * sensitivity; break;
                case Axis.XAxis: resultRotation.x += (invertX ? input.y : -input.y) * sensitivity; break;
                case Axis.Both:
                    resultRotation.x += (invertX ? input.y : -input.y) * sensitivity;
                    resultRotation.y += (invertY ? input.x : -input.x) * sensitivity;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}