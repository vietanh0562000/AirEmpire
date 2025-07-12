#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using UnityEngine;
	using UnityEditor;
	using UnityEngine.SceneManagement;
	using Object = UnityEngine.Object;

	public class CSObjectTools
	{
		public static long GetUniqueObjectId(Object unityObject)
		{
			var id = -1L;
			var siblingId = 0;

			if (unityObject == null) return id;

			var go = unityObject as GameObject;
			if (go != null)
			{
				siblingId = go.transform.GetSiblingIndex();
			}

			if (AssetDatabase.Contains(unityObject))
			{
				var path = AssetDatabase.GetAssetPath(unityObject);
				path = CSPathTools.EnforceSlashes(path);
				if (!string.IsNullOrEmpty(path))
				{
					if (AssetDatabase.IsMainAsset(unityObject))
					{
						var pathBytes = Encoding.UTF8.GetBytes(path);
						id = xxHash.CalculateHash(pathBytes, pathBytes.Length, 230887);
					}
					else
					{
						id = GetLocalIdentifierInFile(unityObject);
					}
				}
				else
				{
					Debug.LogError(Maintainer.ConstructError("Can't get path to the asset " + unityObject.name));
				}
			}
			else
			{
				var prefabType = PrefabUtility.GetPrefabType(unityObject);
				if (prefabType != PrefabType.None)
				{
#if UNITY_2018_2_OR_NEWER
					var parentObject = PrefabUtility.GetCorrespondingObjectFromSource(unityObject);
#else
					var parentObject = PrefabUtility.GetPrefabParent(unityObject);
#endif
					if (parentObject != null)
					{
						id = GetUniqueObjectId(parentObject);
						return id + siblingId;
					}
				}

				id = GetLocalIdentifierInFile(unityObject);
				if (id <= 0)
				{
					id = unityObject.GetInstanceID();
				}
			}
			
			if (id <= 0)
			{
				id = siblingId;
			}

			if (id <= 0)
			{
				id = unityObject.name.GetHashCode();
			}

			return id;
		}

		public static int GetComponentIndex(Component component)
		{
			if (component == null) return -1;

			var allComponents = component.GetComponents<Component>();
			for (var i = 0; i < allComponents.Length; i++)
			{
				if (allComponents[i] == component)
				{
					return i;
				}
			}

			return -1;
		}

		public static string GetNativeObjectType(Object unityObject)
		{
			string result;

			try
			{
				var fullName = unityObject.ToString();
				var openingIndex = fullName.IndexOf('(') + 1;
				if (openingIndex != 0)
				{
					var closingIndex = fullName.LastIndexOf(')');
					result = fullName.Substring(openingIndex, closingIndex - openingIndex);
				}
				else
				{
					result = null;
				}
			}
			catch
			{
				result = null;
			}

			return result;
		}

		public static void SelectGameObject(GameObject go, bool inScene)
		{
			if (inScene)
			{
				Selection.activeTransform = go == null ? null : go.transform;
			}
			else
			{
				Selection.activeGameObject = go;
			}
		}

		public static GameObject FindGameObjectInScene(Scene scene, long objectId, string transformPath = null)
		{
			GameObject result = null;
			var rootObjects = scene.GetRootGameObjects();

			foreach (var rootObject in rootObjects)
			{
				result = FindChildGameObjectRecursive(rootObject.transform, rootObject.transform.name, objectId, transformPath);
				if (result != null)
				{
					break;
				}
			}

			return result;
		}

		public static GameObject[] GetAllGameObjectsInScene(Scene scene)
		{
			var gameObjects = new List<GameObject>();

			var rootObjects = scene.GetRootGameObjects();
			foreach (var rootObject in rootObjects)
			{
				GetSceneObjectsRecursive(ref gameObjects, rootObject);
			}

			return gameObjects.ToArray();
		}

		private static void GetSceneObjectsRecursive(ref List<GameObject> gameObjects, GameObject parentObject)
		{
			var parentTransform = parentObject.transform;

			gameObjects.Add(parentObject);

			for (var i = 0; i < parentTransform.childCount; i++)
			{
				var childTransform = parentTransform.GetChild(i);
				GetSceneObjectsRecursive(ref gameObjects, childTransform.gameObject);
			}
		}

		public static GameObject FindChildGameObjectRecursive(Transform parent, string currentTransformPath, long objectId, string transformPath = null)
		{
			GameObject result = null;
			var skipObjectIdCheck = false;

			if (!string.IsNullOrEmpty(transformPath))
			{
				if (currentTransformPath != transformPath)
				{
					skipObjectIdCheck = true;
				}
			}

			if (!skipObjectIdCheck)
			{
				var currentObjectId = GetUniqueObjectId(parent.gameObject);
				if (currentObjectId == objectId)
				{
					result = parent.gameObject;
					return result;
				}
			}

			for (var i = 0; i < parent.childCount; i++)
			{
				var childTransform = parent.GetChild(i);
				result = FindChildGameObjectRecursive(childTransform, currentTransformPath + "/" + childTransform.name, objectId, transformPath);
				if (result != null) break;
			}

			return result;
		}

		public static string GetArrayItemNameAndIndex(string fullPropertyPath)
		{
			var propertyPath = fullPropertyPath.Replace(".Array.data", "").Replace("].", "] / ").Replace("[", " [Element ");
			return propertyPath;
		}

		public static string GetArrayItemName(string fullPropertyPath)
		{
			var name = GetArrayItemNameAndIndex(fullPropertyPath);
			var lastOpeningBracketIndex = name.LastIndexOf('[');
			return name.Substring(0, lastOpeningBracketIndex);
		}

		public static string GetScriptPathFromObject(Object unityObject)
		{
			if (unityObject == null) return null;

			MonoScript monoScript = null;

			var monoBehaviour = unityObject as MonoBehaviour;
			if (monoBehaviour != null)
			{
				monoScript = MonoScript.FromMonoBehaviour(monoBehaviour);
			}

			var scriptableObject = unityObject as ScriptableObject;
			if (scriptableObject != null)
			{
				monoScript = MonoScript.FromScriptableObject(scriptableObject);
			}

			return monoScript == null ? null : AssetDatabase.GetAssetPath(monoScript);
		}

		private static long GetLocalIdentifierInFile(Object unityObject)
		{
			var serializedObject = new SerializedObject(unityObject);
			try
			{
				CSReflectionTools.SetInspectorToDebug(serializedObject);
				var serializedProperty = serializedObject.FindProperty("m_LocalIdentfierInFile");
				return serializedProperty.longValue;
			}
			catch (Exception e)
			{
				Debug.LogWarning(Maintainer.ConstructWarning("Couldn't get data from debug inspector for object " + unityObject.name + " due to this error:\n" + e));
				return -1;
			}
		}
	}
}