using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateCSVEdit : EditorWindow
{
    TextAsset csv = null;
	MonoScript script = null;

	[MenuItem("Window/CreateCSV")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(CreateCSVEdit));
	}

	void OnGUI()
	{
		csv = EditorGUILayout.ObjectField("CSV", csv, typeof(TextAsset), false) as TextAsset;
		script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;

		// buttons
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Generate Code"))
		{
			if (csv != null)
			{
				CreateScript();
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void CreateScript()
    {
		string className = script == null ? "CSVClass" : script.name;		//Ŭ���� �̸�
		string code = CSVTemplate.Generate(csv.text, className);		// �ڵ�
		string path = script == null ? 
			(Application.dataPath +"/" +className + ".cs") : AssetDatabase.GetAssetPath(script);	// ���
		File.WriteAllText(path, code);	// ���Ͽ� ����
    }
}
