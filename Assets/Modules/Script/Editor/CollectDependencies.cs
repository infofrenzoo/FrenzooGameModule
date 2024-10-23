using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;

public class CollectDependencies : EditorWindow
{
	static GameObject obj = null;


	[MenuItem("Tools/Collect Dependencies")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		CollectDependencies window = (CollectDependencies)EditorWindow.GetWindow(typeof(CollectDependencies));
		window.Show();
	}

	void OnGUI()
	{
		obj = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Dependency", obj, typeof(GameObject)) as GameObject;

		if (obj)
		{
			Object[] roots = new Object[] { obj };

			if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
				Selection.objects = EditorUtility.CollectDependencies(roots).Where(x => x is Texture2D).ToArray<Object>();

			if (GUI.Button(new Rect(3, 60, position.width - 6, 20), "Find replacement"))
			{
				Image[] images = obj.GetComponentsInChildren<Image>(true);
				int count = 0;
				foreach (Image image in images)
				{
					if (image.sprite != null)
					{
						Sprite t = (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Modules/Images2/{image.sprite.name}.png", typeof(Sprite));
						if (t != null)
						{
							image.sprite = t;
							//Debug.Log($"Found: {t.name}");
							count++;
						}
						else
						{
							Debug.LogWarning($"No {image.sprite.name} Found.");
						}
					}
				}
				Debug.Log($"Count: {count}");
			}
		}
		else
			EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
	}

	void OnInspectorUpdate()
	{
		Repaint();
	}
}