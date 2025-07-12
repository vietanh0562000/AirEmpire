#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Tools
{
	using UnityEditor;

	public class CSMenuTools
	{
		public static bool ShowProjectSettingsTagsAndLayers()
		{
			return EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
		}

		public static bool ShowProjectSettingsGraphics()
		{
			return EditorApplication.ExecuteMenuItem("Edit/Project Settings/Graphics");
		}

		public static bool ShowProjectSettingsPlayer()
		{
			return EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
		}

		public static bool ShowSceneSettingsLighting()
		{
#if UNITY_2018_2_OR_NEWER
			return EditorApplication.ExecuteMenuItem("Window/Rendering/Lighting Settings");
#else
			return EditorApplication.ExecuteMenuItem("Window/Lighting/Settings");
#endif
		}

		public static bool ShowSceneSettingsNavigation()
		{
#if UNITY_2018_2_OR_NEWER
			return EditorApplication.ExecuteMenuItem("Window/AI/Navigation");
#else
			return EditorApplication.ExecuteMenuItem("Window/Navigation");
#endif
		}
	}
}