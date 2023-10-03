#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MouseLook
{
    [CustomEditor(typeof(MouseLook))]
    [CanEditMultipleObjects]
    public class MouseLookInspector : Editor
    {
        private static readonly bool[] ShowBoolList = {true,true,true};
        private bool _isActive;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("inputMode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("axis"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sensitivity"));
            
            Foldout(ref ShowBoolList[0], serializedObject.FindProperty("enableSmoothMove"), ref _isActive);
            
            if (ShowBoolList[0])
            {
                GUILayout.Space(5);
                GUI.enabled = _isActive;
                serializedObject.FindProperty("smoothMoveSensitivity").floatValue = EditorGUILayout.FloatField("Sensitivity",serializedObject.FindProperty("smoothMoveSensitivity").floatValue);
                GUI.enabled = true;
                FooterRect();
            }
            
            Foldout(ref ShowBoolList[1], "Limit settings");
            
            if (ShowBoolList[1])
            {
                GUILayout.Space(5);
                GUILayout.BeginVertical("X Limit",GUI.skin.window);
                _isActive = serializedObject.FindProperty("xLimit").FindPropertyRelative("enabled").boolValue;
                GUILayout.BeginHorizontal();
                _isActive = EditorGUILayout.Toggle(_isActive,GUILayout.Width(16));
                GUI.enabled = _isActive;
                AngleLimit xLimit = (AngleLimit) serializedObject.FindProperty("xLimit").boxedValue;
                GUILayout.Label("Min");
                xLimit.min = EditorGUILayout.FloatField(xLimit.min);
                GUILayout.Label("Max");
                xLimit.max = EditorGUILayout.FloatField(xLimit.max);
                GUILayout.EndHorizontal();
                EditorGUILayout.MinMaxSlider(GUIContent.none, ref xLimit.min,ref xLimit.max,-360,360);
                serializedObject.FindProperty("xLimit").boxedValue = xLimit;
                GUI.enabled = true;
                serializedObject.FindProperty("xLimit").FindPropertyRelative("enabled").boolValue = _isActive;
                GUILayout.EndVertical();
                GUILayout.Space(5);
                
                GUILayout.BeginVertical("Y Limit",GUI.skin.window);
                _isActive = serializedObject.FindProperty("yLimit").FindPropertyRelative("enabled").boolValue;
                GUILayout.BeginHorizontal();
                _isActive = EditorGUILayout.Toggle(_isActive,GUILayout.Width(16));
                GUI.enabled = _isActive;
                AngleLimit yLimit = (AngleLimit)serializedObject.FindProperty("yLimit").boxedValue;
                GUILayout.Label("Min");
                yLimit.min = EditorGUILayout.FloatField(yLimit.min);
                GUILayout.Label("Max");
                yLimit.max = EditorGUILayout.FloatField(yLimit.max);
                GUILayout.EndHorizontal();
                EditorGUILayout.MinMaxSlider(GUIContent.none, ref yLimit.min,ref yLimit.max,-360,360);
                serializedObject.FindProperty("yLimit").boxedValue = yLimit;
                GUI.enabled = true;
                serializedObject.FindProperty("yLimit").FindPropertyRelative("enabled").boolValue = _isActive;
                GUILayout.EndVertical();
                
                FooterRect();
            }

            Foldout(ref ShowBoolList[2], "Invert settings");
            
            if (ShowBoolList[2])
            {
                GUILayout.Space(5);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("invertX"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("invertY"));
                FooterRect();
            }
            
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
        
        private void Foldout(ref bool show, string name)
        {
            var rect = EditorGUILayout.GetControlRect();
            HeaderRect(rect);
            EditorGUI.LabelField(new Rect(rect.x+20,rect.y,rect.width,rect.height), name);
            show = EditorGUI.Toggle(new Rect(rect.x-14,rect.y,rect.width+14,rect.height),show,EditorStyles.foldout);
        }
    
        private void Foldout(ref bool show, SerializedProperty baseProperty, ref bool isActive)
        {
            var rect = EditorGUILayout.GetControlRect();
            HeaderRect(rect);
            baseProperty.boolValue = EditorGUI.Toggle(new Rect(rect.x,rect.y,20,rect.height),baseProperty.boolValue);
            isActive = baseProperty.boolValue;
            EditorGUI.LabelField(new Rect(rect.x+20,rect.y,rect.width,rect.height), baseProperty.displayName);
            show = EditorGUI.Toggle(new Rect(rect.x-14,rect.y,rect.width+14,rect.height),show,EditorStyles.foldout);
        }
        
        private void HeaderRect(Rect rect)
        {
            var headerRect = new Rect(rect.x - 18, rect.y, rect.width + 22, rect.height);
            EditorGUI.DrawRect(headerRect,new Color(0,0,0, EditorGUIUtility.isProSkin ? 0.3f : 0.1f));
            EditorGUI.DrawRect(new Rect(headerRect.x,headerRect.y,headerRect.width,1f),new Color(0,0.7f,1, 0.5f));
        }

        private void FooterRect()
        {
            var rect = EditorGUILayout.GetControlRect();
            var footerRect = new Rect(rect.x - 18, rect.y + 5, rect.width + 22, 1);
            EditorGUI.DrawRect(footerRect,new Color(0,0,0, EditorGUIUtility.isProSkin ? 0.3f : 0.1f));
        }
    }
}
#endif