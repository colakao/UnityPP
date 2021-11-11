using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ToggleObjects))]
public class ToggleObjectsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		ToggleObjects toggleObjects = (ToggleObjects)target;

		if (GUILayout.Button("Find objects", GUILayout.Width(100)))
			toggleObjects.FindObjects();
		GUILayout.Space(10);
		if (GUILayout.Button("Enable/Disable objects", GUILayout.Width(150)))
			toggleObjects.EnableGameObjects(toggleObjects.setEnabled);

	}
}
