#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Tools
{
	using System.IO;

	using TreeEditor;
	using UnityEditor;
	using UnityEngine;

	public static class CSSelectionTools
	{
		public static bool RevealAndSelect(string assetPath, string transformPath, long objectId, long componentId, string propertyPath)
		{
			Object target = null;

			/* selecting a folder */

			if (Directory.Exists(assetPath))
			{
				var instanceId = GetMainAssetInstanceID(assetPath);
				Selection.activeInstanceID = instanceId;

				return true;
			}

			/* selecting asset files or objects on prefabs and in scenes */

			var targetAsset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

			if (objectId == -1)
			{
				Selection.activeObject = targetAsset;
				return true;
			}

			var lookingInScene = targetAsset is SceneAsset;

			if (lookingInScene)
			{
				if (CSEditorTools.lastRevealSceneOpenResult != null)
				{
					if (CSEditorTools.lastRevealSceneOpenResult.scene.isDirty)
					{
						if (!CSSceneTools.SaveDirtyScenesWithPrompt(
							new[] {CSEditorTools.lastRevealSceneOpenResult.scene}))
						{
							return false;
						}
					}
				}

				var newSceneOpenResult = CSSceneTools.OpenScene(assetPath);
				if (newSceneOpenResult.success)
				{
					target = CSObjectTools.FindGameObjectInScene(newSceneOpenResult.scene, objectId, transformPath);

					CSSceneTools.CloseUntitledSceneIfNotDirty();

					if (CSEditorTools.lastRevealSceneOpenResult != null)
					{
						CSSceneTools.CloseOpenedSceneIfNeeded(CSEditorTools.lastRevealSceneOpenResult, assetPath, true);
					}

					CSEditorTools.lastRevealSceneOpenResult = newSceneOpenResult;
				}
			}
			else if (targetAsset is GameObject)
			{
				var targetGo = (GameObject)targetAsset;
				target = CSObjectTools.FindChildGameObjectRecursive(targetGo.transform, targetGo.transform.name, objectId, transformPath);

				// trying to find specific cases -------------------------------------------------------------------------

				if (target == null)
				{
					var allObjectsInPrefab = AssetDatabase.LoadAllAssetsAtPath(assetPath);

					foreach (var objectOnPrefab in allObjectsInPrefab)
					{
						if (objectOnPrefab is BillboardAsset || objectOnPrefab is TreeData)
						{
							var objectOnPrefabId = CSObjectTools.GetUniqueObjectId(objectOnPrefab);

							if (objectOnPrefabId == objectId)
							{
								target = objectOnPrefab;
							}
						}
					}
				}
			}
			else
			{
				target = targetAsset;
			}

			if (target == null)
			{
				Debug.LogError(Maintainer.ConstructError("Couldn't find target Game Object at " + assetPath + " with ObjectID " + objectId + "!"));
				return false;
			}

			if (target is GameObject)
			{
				// workaround for a bug when Unity doesn't expand hierarchy in scene
				if (lookingInScene)
				{
					EditorApplication.delayCall += () =>
					{
						EditorGUIUtility.PingObject(target);
					};
				}

				CSObjectTools.SelectGameObject((GameObject)target, lookingInScene);
			}
			else
			{
				Selection.activeObject = target;
			}

			if (lookingInScene)
			{
				EditorApplication.delayCall += () =>
				{
					EditorGUIUtility.PingObject(targetAsset);
				};
			}
			else
			{
				if (transformPath.Split('/').Length > 2)
				{
					EditorApplication.delayCall += () =>
					{
						EditorGUIUtility.PingObject(targetAsset);
					};
				}
			}

			/* folding all other components if we need to show a component */
			if (componentId != -1)
			{
				var tracker = CSEditorTools.GetActiveEditorTrackerForSelectedObject();
				if (tracker == null)
				{
					Debug.LogError(Maintainer.ConstructError("Can't get active tracker."));
					return false;
				}
				tracker.RebuildIfNecessary();
				
				var editors = tracker.activeEditors;

				var targetFound = false;
				var skipCount = 0;
				
				for (var i = 0; i < editors.Length; i++)
				{
					var editor = editors[i];
					var editorTargetType = editor.target.GetType();
					if (editorTargetType == CSReflectionTools.assetImporterType || editorTargetType == CSReflectionTools.gameObjectType)
					{
						skipCount++;
						continue;
					}

					if (i - skipCount == componentId)
					{
						targetFound = true;

						/* known corner cases when editor can't be set to visible via tracker */

						if (editor.serializedObject.targetObject is ParticleSystemRenderer)
						{
							var renderer = (ParticleSystemRenderer)editor.serializedObject.targetObject;
							var ps = renderer.GetComponent<ParticleSystem>();
							componentId = CSObjectTools.GetComponentIndex(ps);
						}

						break;
					}
				}

				if (!targetFound)
				{
					return false;
				}

				for (var i = 0; i < editors.Length; i++)
				{
					tracker.SetVisible(i, i - skipCount != componentId ? 0 : 1);
				}

				var inspectorWindow2 = CSEditorTools.GetInspectorWindow();
				if (inspectorWindow2 != null)
				{
					inspectorWindow2.Repaint();
				}

				// workaround for bug when tracker selection gets reset after scene open
				// (e.g. revealing TMP component in new scene)
				EditorApplication.delayCall += () =>
				{
					EditorApplication.delayCall += () =>
					{
						for (var i = 0; i < editors.Length; i++)
						{
							tracker.SetVisible(i, i - skipCount != componentId ? 0 : 1);
						}

						var inspectorWindow = CSEditorTools.GetInspectorWindow();
						if (inspectorWindow != null)
						{
							inspectorWindow.Repaint();
						}
					};
				};

				return true;
			}
			return true;
		}

		private static int GetMainAssetInstanceID(string path)
		{
			var mi = CSReflectionTools.GetGetMainAssetInstanceIDMethodInfo();
			if (mi != null)
			{
				return (int)mi.Invoke(null, new object[] { path });
			}

			Debug.LogError(Maintainer.ConstructError("Can't retrieve InstanceID From GUID via reflection!"));
			return -1;
		}
	}
}