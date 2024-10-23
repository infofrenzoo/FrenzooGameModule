using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using System.IO;

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
		//obj = EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Dependency", obj, typeof(GameObject)) as GameObject;

		//if (obj)
		{
			//Object[] roots = new Object[] { obj };

			//if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
			//	Selection.objects = EditorUtility.CollectDependencies(roots).Where(x => x is Texture2D).ToArray<Object>();
			//Right Click Find dependancy of prefab
			EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "SelectObjectCount:", $"{Selection.objects.Count()}");
			if (GUI.Button(new Rect(3, 60, position.width - 6, 20), "Copy Select Object"))
			{
				int count = 0;
				foreach (Object obj in Selection.objects)
				{
					string source = AssetDatabase.GetAssetPath(obj.GetInstanceID());
					source = Path.Combine(Application.dataPath.Replace("Assets", ""), source);
					string fileName = Path.GetFileName(source);
					string Des = Path.Combine(Application.dataPath, $"Modules/Images/{fileName}");
					if (!File.Exists(Des))
					{
						File.Copy(source, Des);

						count++;
					}
					else
					{
						Debug.LogWarning($"{fileName} already there");
					}
					//string to = Path.Combine(Application.dataPath.Replace("Assets", ""), path);
				}
				Debug.Log("Copied: " + count);
			}
			if (GUI.Button(new Rect(3, 90, position.width - 6, 20), "Copy importer settings"))
			{
				//int count = 0;
				foreach (Object obj in Selection.objects)
				{
					string source = AssetDatabase.GetAssetPath(obj.GetInstanceID());
					//source = Path.Combine(Application.dataPath.Replace("Assets", ""), source);
					string fileName = Path.GetFileName(source);
					string Des = Path.Combine(Application.dataPath, $"Modules/Images/{fileName}");
					if (File.Exists(Des))
					{

						TextureImporter orgImporter = (TextureImporter)TextureImporter.GetAtPath(source);
						TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath($"Assets/Modules/Images/{fileName}");

						importer.spriteBorder = orgImporter.spriteBorder;
						importer.textureType = orgImporter.textureType;
						importer.spriteImportMode = orgImporter.spriteImportMode;
						importer.mipmapEnabled = false;

						EditorUtility.SetDirty(importer);
						importer.SaveAndReimport();

					}
					else
					{
						Debug.LogWarning($"{fileName} already there");
					}
					//string to = Path.Combine(Application.dataPath.Replace("Assets", ""), path);
				}
			}
			if (GUI.Button(new Rect(3, 120, position.width - 6, 20), "Find replacement"))
			{
				Image[] images = Selection.gameObjects[0].GetComponentsInChildren<Image>(true);
				int count = 0;
				foreach (Image image in images)
				{
					if (image.sprite != null)
					{
						Sprite t = (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Modules/Images/{image.sprite.name}.png", typeof(Sprite));
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
				//Debug.Log($"Count: {count}");
			}
		}
		//else
		//	EditorGUI.LabelField(new Rect(3, 90, position.width - 6, 20), "Missing:", "Select an object first");



	}

	void OnInspectorUpdate()
	{
		Repaint();
	}
}