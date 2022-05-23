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
		string className = script == null ? "CSVClass" : script.name;		//클래스 이름
		string code = CSVTemplate.Generate(csv.text, className);		// 코드
		string path = script == null ? 
			(Application.dataPath +"/" +className + ".cs") : AssetDatabase.GetAssetPath(script);	// 경로
		File.WriteAllText(path, code);	// 파일에 저장
    }
}
