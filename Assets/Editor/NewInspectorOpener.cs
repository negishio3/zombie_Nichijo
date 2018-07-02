using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class NewInspectorOpener
{
    [MenuItem("Assets/Open New Inspector %i")]
    private static void OpenNewInspector()
    {
        var editorType = typeof(Editor);
        var inspectorWindowType = editorType.Assembly.GetType("UnityEditor.InspectorWindow");
        var bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var flipLockedMethod = inspectorWindowType.GetMethod("FlipLocked", bindingAttr);
        var inspector = ScriptableObject.CreateInstance(inspectorWindowType) as EditorWindow;

        inspector.Show(true);
        inspector.Repaint();
        flipLockedMethod.Invoke(inspector, null);
    }
}