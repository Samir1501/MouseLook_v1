#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace MouseLook
{
    [EditorTool("Platform Tool",typeof(MouseLook))]
    class MouseLookHandle : EditorTool
    {
        private readonly ArcRangeHandle _arcRangeHandle = new ();
        private AngleLimit[] _limits = new AngleLimit[2];
        private MouseLook mouseLook;
        [Shortcut("Activate Mouse Look Tool", typeof(SceneView), KeyCode.U)]
        static void PlatformToolShortcut()
        {
            if (Selection.GetFiltered<MouseLook>(SelectionMode.TopLevel).Length > 0)
                ToolManager.SetActiveTool<MouseLookHandle>();
            else 
                Debug.Log("No platforms selected!");
        }

        public override GUIContent toolbarIcon {
            get
            {
                return EditorGUIUtility.IconContent(AssetDatabase.GUIDToAssetPath("b4fb54b4ff4003642a8e560b266f9426"));
            }
        }
        
        public override void OnActivated()
        {
            mouseLook = (MouseLook)target;
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent($"Entering Mouse Look Tool\n \n {(!mouseLook.xLimit.enabled && !mouseLook.yLimit.enabled ? "Disabled all limits":"")}"), .2f);
        }

        public override void OnWillBeDeactivated()
        {
            SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Exiting Mouse Look Tool"), .1f);
        }

        public override void OnToolGUI(EditorWindow window)
        {
            _arcRangeHandle.transform = mouseLook.transform;
            _limits = _arcRangeHandle.DrawHandle(
                new [] {mouseLook.GetXLimit(), mouseLook.GetYLimit()},
                new []{mouseLook.xLimit.enabled,mouseLook.yLimit.enabled});
            mouseLook.xLimit = _limits[0];
            mouseLook.yLimit = _limits[1];
        }
    }
}
#endif