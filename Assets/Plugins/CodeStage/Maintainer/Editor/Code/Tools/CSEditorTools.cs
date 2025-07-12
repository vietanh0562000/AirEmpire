#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Text;
	
	using UnityEditor;
	using UnityEngine;
	using Object = UnityEngine.Object;

	using Core;

	public static class CSEditorTools
	{
		private static readonly string[] sizes = { "B", "KB", "MB", "GB" };
		private static TextInfo textInfo;

		internal static CSSceneTools.OpenSceneResult lastRevealSceneOpenResult;

		public static string FormatBytes(double bytes)
		{
			var order = 0;

			while (bytes >= 1024 && order + 1 < 4)
			{
				order++;
				bytes = bytes / 1024;
			}

			return string.Format("{0:0.##} {1}", bytes, sizes[order]);
		}

		public static int GetPropertyHash(SerializedProperty sp)
		{
			var stringHash = new StringBuilder();

			stringHash.Append(sp.type);

			if (sp.isArray)
			{
				stringHash.Append(sp.arraySize);
			}
			else
			{
				switch (sp.propertyType)
				{
					case SerializedPropertyType.AnimationCurve:
						if (sp.animationCurveValue != null)
						{
							stringHash.Append(sp.animationCurveValue.length);
							if (sp.animationCurveValue.keys != null)
							{
								foreach (var key in sp.animationCurveValue.keys)
								{
									stringHash.Append(key.value)
											  .Append(key.time)
#if !UNITY_2018_1_OR_NEWER
											  .Append(key.tangentMode)
#endif
											  .Append(key.outTangent)
											  .Append(key.inTangent);
								}
							}
						}
						break;
					case SerializedPropertyType.ArraySize:
						stringHash.Append(sp.intValue);
						break;
					case SerializedPropertyType.Boolean:
						stringHash.Append(sp.boolValue);
						break;
					case SerializedPropertyType.Bounds:
						stringHash.Append(sp.boundsValue.GetHashCode());
						break;
					case SerializedPropertyType.Character:
						stringHash.Append(sp.intValue);
						break;
					case SerializedPropertyType.Generic: // looks like arrays which we already walk through
						break;
					case SerializedPropertyType.Gradient: // unsupported
						break;
					case SerializedPropertyType.ObjectReference:
						if (sp.objectReferenceValue != null)
						{
							stringHash.Append(sp.objectReferenceValue.name);
						}
						break;
					case SerializedPropertyType.Color:
						stringHash.Append(sp.colorValue.GetHashCode());
						break;
					case SerializedPropertyType.Enum:
						stringHash.Append(sp.enumValueIndex);
						break;
					case SerializedPropertyType.Float:
						stringHash.Append(sp.floatValue);
						break;
					case SerializedPropertyType.Integer:
						stringHash.Append(sp.intValue);
						break;
					case SerializedPropertyType.LayerMask:
						stringHash.Append(sp.intValue);
						break;
					case SerializedPropertyType.Quaternion:
						stringHash.Append(sp.quaternionValue.GetHashCode());
						break;
					case SerializedPropertyType.Rect:
						stringHash.Append(sp.rectValue.GetHashCode());
						break;
					case SerializedPropertyType.String:
						stringHash.Append(sp.stringValue);
						break;
					case SerializedPropertyType.Vector2:
						stringHash.Append(sp.vector2Value.GetHashCode());
						break;
					case SerializedPropertyType.Vector3:
						stringHash.Append(sp.vector3Value.GetHashCode());
						break;
					case SerializedPropertyType.Vector4:
						stringHash.Append(sp.vector4Value.GetHashCode());
						break;
#if UNITY_5_6_OR_NEWER
					case SerializedPropertyType.ExposedReference:
						if (sp.exposedReferenceValue != null)
						{
							stringHash.Append(sp.exposedReferenceValue.name);
						}
						break;
#endif
#if UNITY_2017_2_OR_NEWER
					case SerializedPropertyType.Vector2Int:
						stringHash.Append(sp.vector2IntValue.GetHashCode());
						break;
					case SerializedPropertyType.Vector3Int:
						stringHash.Append(sp.vector3IntValue.GetHashCode());
						break;
					case SerializedPropertyType.RectInt:
						stringHash.Append(sp.rectIntValue.position.GetHashCode()).Append(sp.rectIntValue.size.GetHashCode());
						break;
					case SerializedPropertyType.BoundsInt:
						stringHash.Append(sp.boundsIntValue.GetHashCode());
						break;
#endif
#if UNITY_2018_1_OR_NEWER
					case SerializedPropertyType.FixedBufferSize:
						stringHash.Append(sp.fixedBufferSize);
						break;
#endif
					default:
						Debug.LogWarning(Maintainer.LogPrefix + "Unknown SerializedPropertyType: " + sp.propertyType);
						break;
				}
			}
				
			return stringHash.ToString().GetHashCode();
		}

		public static string GetFullTransformPath(Transform transform)
		{
			var path = transform.name;
			while (transform.parent != null)
			{
				transform = transform.parent;
				path = transform.name + "/" + path;
			}
			return path;
		}

		public static int GetAllSuitableObjectsInFileAssets(List<Object> objects)
		{
			return GetAllSuitableObjectsInFileAssets(objects, null);
		}

		public static int GetAllSuitableObjectsInFileAssets(List<Object> objects, List<string> paths)
		{
			var allAssetPaths = FindAssetsFiltered("t:Prefab, t:ScriptableObject");
			return GetSuitableFileAssetsFromSelection(allAssetPaths, objects, paths);
		}

		public static int GetSuitableFileAssetsFromSelection(string[] selection, List<Object> objects, List<string> paths)
		{
			var selectedCount = 0;

			foreach (var path in selection)
			{
				var assetObject = AssetDatabase.LoadMainAssetAtPath(path);
				if (assetObject is GameObject)
				{
					selectedCount = GetPrefabsRecursive(objects, paths, path, assetObject as GameObject, selectedCount);
				}
				else if (assetObject is ScriptableObject)
				{
					if (paths != null) paths.Add(path);
					objects.Add(assetObject);
					selectedCount ++;
				}
			}

			return selectedCount;
		}

		public static string[] FindAssetsFiltered(string searchMask)
		{
			return FindAssetsFiltered(searchMask, null, null);
		}

		public static string[] FindAssetsFiltered(string searchMask, FilterItem[] includes, FilterItem[] ignores)
		{
			var allAssetsGUIDs = AssetDatabase.FindAssets(searchMask);
			var count = allAssetsGUIDs.Length;

			var paths = new List<string>(count);

			for (var i = 0; i < count; i++)
			{
				var path = AssetDatabase.GUIDToAssetPath(allAssetsGUIDs[i]);
				path = CSPathTools.EnforceSlashes(path);

				var include = false;
				var skip = false;

				if (ignores != null && ignores.Length > 0)
				{
					skip = IsValueMatchesAnyFilter(path, ignores);
				}

				if (skip) continue;

				if (includes != null && includes.Length > 0)
				{
					include = IsValueMatchesAnyFilter(path, includes);
				}

				if (includes != null && includes.Length > 0)
				{
					if (include && !paths.Contains(path)) paths.Add(path);
				}
				else
				{
					if (!paths.Contains(path)) paths.Add(path);
				}
			}

			return paths.ToArray();
		}

		public static int GetDepthInHierarchy(Transform transform, Transform upToTransform)
		{
			if (transform == upToTransform || transform.parent == null) return 0;
			return 1 + GetDepthInHierarchy(transform.parent, upToTransform);
		}

		public static EditorWindow GetInspectorWindow()
		{
			if (CSReflectionTools.inspectorWindowType == null)
			{
				Debug.LogError(Maintainer.ConstructError("Can't find UnityEditor.InspectorWindow type!"));
				return null;
			}

			var inspectorWindow = EditorWindow.GetWindow(CSReflectionTools.inspectorWindowType);
			if (inspectorWindow == null)
			{
				Debug.LogError(Maintainer.ConstructError("Can't get an InspectorWindow!"));
				return null;
			}

			return inspectorWindow;
		}

		public static ActiveEditorTracker GetActiveEditorTrackerForSelectedObject()
		{
			var inspectorWindow = GetInspectorWindow();
			if (inspectorWindow == null) return null;
			if (CSReflectionTools.inspectorWindowType == null) return null;

			inspectorWindow.Repaint();

#if UNITY_5_5_OR_NEWER
			ActiveEditorTracker result;

			var trackerProperty = CSReflectionTools.GetPropertyInfo(CSReflectionTools.inspectorWindowType, "tracker");
			if (trackerProperty == null)
			{
				// may be removed for Unity 5.6 +, since GetTracker method was removed somewhere in 5.5 cycle
				result = GetActiveTrackerUsingMethod(CSReflectionTools.inspectorWindowType, inspectorWindow);
			}
			else
			{
				result = (ActiveEditorTracker)trackerProperty.GetValue(inspectorWindow, null);
			}

			if (result == null)
			{
				Debug.LogError(Maintainer.ConstructError("Can't get ActiveEditorTracker from the InspectorWindow!"));
			}

			return result;
#else
			return GetActiveTrackerUsingMethod(CSReflectionTools.inspectorWindowType, inspectorWindow);
#endif
		}

		public static string[] GetProjectSelections(bool includeFolders)
		{
			var selectedGUIDs = Selection.assetGUIDs;
			var paths = new List<string>(selectedGUIDs.Length);

			foreach (var guid in selectedGUIDs)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				path = CSPathTools.EnforceSlashes(path);

				if (Directory.Exists(path) && !includeFolders) continue;
				paths.Add(path);
			}

			return paths.ToArray();
		}

		public static void RemoveReadOnlyAttribute(string filePath)
		{
			var attributes = File.GetAttributes(filePath);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
		}

		public static bool IsValueMatchesAnyFilter(string value, FilterItem[] filters)
		{
			return IsValueMatchesAnyFilterOfKind(value, filters, FilterKind.NotSet, false);
		}

		public static bool IsValueMatchesAnyFilterOfKind(string value, FilterItem[] filters, FilterKind kind, bool strict = false)
		{
			var match = false;
			var directory = string.Empty;
			var filename = string.Empty;
			var extension = string.Empty;

			foreach (var filter in filters)
			{
				if (kind != FilterKind.NotSet)
				{
					if (filter.kind != kind) continue;
				}

				switch (filter.kind)
				{
					case FilterKind.Path:
					case FilterKind.Type:
						match = FilterMatchHelper(value, filter, strict);
						break;
					case FilterKind.Directory:
						if (directory == string.Empty)
						{
							directory = Path.GetDirectoryName(value);
						}
						if (directory != null)
						{
							directory = CSPathTools.EnforceSlashes(directory);
							match = FilterMatchHelper(directory, filter, strict);
						}
						break;
					case FilterKind.FileName:
						if (filename == string.Empty)
						{
							filename = Path.GetFileName(value);
						}
						if (filename != null)
						{
							filename = CSPathTools.EnforceSlashes(filename);
							match = FilterMatchHelper(filename, filter, strict);
						}
						break;
					case FilterKind.Extension:
						if (extension == string.Empty)
						{
							extension = Path.GetExtension(value);
						}

						match = string.Equals(extension, filter.value, StringComparison.OrdinalIgnoreCase);
						break;
					case FilterKind.NotSet:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (match)
				{
					break;
				}
			}

			return match;
		}

		public static bool TryAddNewItemToFilters(ref FilterItem[] filters, FilterItem newItem)
		{
			foreach (var filterItem in filters)
			{
				if (filterItem.value == newItem.value)
				{
					return false;
				}
			}

			ArrayUtility.Add(ref filters, newItem);
			return true;
		}

		public static Object GetLightmapSettings()
		{
			var mi = CSReflectionTools.GetGetLightmapSettingsMethodInfo();
			if (mi != null)
			{
				return (Object) mi.Invoke(null, null);
			}

			Debug.LogError(Maintainer.ConstructError("Can't retrieve LightmapSettings object via reflection!"));
			return null;
		}

		public static Object GetRenderSettings()
		{
			var mi = CSReflectionTools.GetGetRenderSettingsMethodInfo();
			if (mi != null)
			{
				return (Object) mi.Invoke(null, null);
			}

			Debug.LogError(Maintainer.ConstructError("Can't retrieve RenderSettings object via reflection!"));
			return null;
		}

		public static bool RevealInSettings(AssetSettingsKind settingsKind, string path = null)
		{
			var result = true;

			switch (settingsKind)
			{
				case AssetSettingsKind.NotSettings:
					Debug.LogWarning(Maintainer.ConstructWarning("Can't open settings of kind NotSettings Oo"));
					result = false;
					break;
				case AssetSettingsKind.AudioManager:
					break;
				case AssetSettingsKind.ClusterInputManager:
					break;
				case AssetSettingsKind.DynamicsManager:
					break;
				case AssetSettingsKind.EditorBuildSettings:
					try
					{
						if (EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor")) == null)
						{
							result = false;
						}
					}
					catch (Exception)
					{
						result = false;
					}
					
					if (result == false)
					{
						Debug.LogError(Maintainer.ConstructError("Can't open EditorBuildSettings!"));
					}
					break;
				case AssetSettingsKind.EditorSettings:
					break;
				case AssetSettingsKind.GraphicsSettings:
					if (!CSMenuTools.ShowProjectSettingsGraphics())
					{
						Debug.LogError(Maintainer.ConstructError("Can't open GraphicsSettings!"));
						result = false;
					}
					break;
				case AssetSettingsKind.InputManager:
					break;
				case AssetSettingsKind.NavMeshAreas:
					break;
				case AssetSettingsKind.NavMeshLayers:
					break;
				case AssetSettingsKind.NavMeshProjectSettings:
					break;
				case AssetSettingsKind.NetworkManager:
					break;
				case AssetSettingsKind.Physics2DSettings:
					break;
				case AssetSettingsKind.PlayerSettings:
					if (!CSMenuTools.ShowProjectSettingsPlayer())
					{
						Debug.LogError(Maintainer.ConstructError("Can't open Player Settings!"));
						result = false;
					}
					break;
				case AssetSettingsKind.PresetManager:
					break;
				case AssetSettingsKind.QualitySettings:
					break;
				case AssetSettingsKind.TagManager:
					break;
				case AssetSettingsKind.TimeManager:
					break;
				case AssetSettingsKind.UnityAdsSettings:
					break;
				case AssetSettingsKind.UnityConnectSettings:
					break;
				case AssetSettingsKind.Unknown:
					if (!string.IsNullOrEmpty(path)) EditorUtility.RevealInFinder(path);
					break;
				default:
					throw new ArgumentOutOfRangeException("settingsKind", settingsKind, null);
			}

			return result;
		}

		public static string NicifyName(string name)
		{
			var nicePropertyName = ObjectNames.NicifyVariableName(name);
			if (textInfo == null) textInfo = new CultureInfo("en-US", false).TextInfo;
			return textInfo.ToTitleCase(nicePropertyName);
		}

		public static AssetKind GetAssetKind(string path)
		{
			if (!Path.IsPathRooted(path))
			{
				if (path.IndexOf("Assets/", StringComparison.Ordinal) == 0)
				{
					return AssetKind.Regular;
				}

				if (path.IndexOf("ProjectSettings/", StringComparison.Ordinal) == 0)
				{
					return AssetKind.Settings;
				}

				if (path.IndexOf("Packages/", StringComparison.Ordinal) == 0)
				{
					return AssetKind.FromPackage;
				}
			}
			else
			{
				if (path.IndexOf("/unity/cache/packages/", StringComparison.OrdinalIgnoreCase) > 0)
				{
					return AssetKind.FromPackage;
				}
			}

			return AssetKind.Unsupported;
		}

		private static int GetPrefabsRecursive(List<Object> objects, List<string> paths, string path, GameObject go, int selectedCount)
		{
			if (go.hideFlags == HideFlags.None || go.hideFlags == HideFlags.HideInHierarchy)
			{
				objects.Add(go);
				if (paths != null) paths.Add(path);
				selectedCount++;
			}

			var childCount = go.transform.childCount;

			for (var i = 0; i < childCount; i++)
			{
				var nestedObject = go.transform.GetChild(i).gameObject;
				selectedCount = GetPrefabsRecursive(objects, paths, path, nestedObject, selectedCount);
			}

			return selectedCount;
		}

		// may be removed for Unity 5.6 +, since GetTracker method was removed somewhere in 5.5 cycle
		private static ActiveEditorTracker GetActiveTrackerUsingMethod(Type inspectorWindowType, EditorWindow inspectorWindow)
		{
			var getTrackerMethod = inspectorWindowType.GetMethod("GetTracker");

			if (getTrackerMethod == null)
			{
				Debug.LogError(Maintainer.ConstructError("Can't find an InspectorWindow.GetTracker() method!"));
				return null;
			}

			return (ActiveEditorTracker)getTrackerMethod.Invoke(inspectorWindow, null);
		}

		private static bool FilterMatchHelper(string value, FilterItem filter, bool strict)
		{
			if (strict)
			{
				return string.Equals(value, filter.value, filter.ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			}

			return value.IndexOf(filter.value, filter.ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) != -1;
		}
	}
}