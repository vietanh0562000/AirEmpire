#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Settings
{
	using System;

#if UNITY_5_6_OR_NEWER
	using UnityEditor.IMGUI.Controls;
#endif

	[Serializable]
	public class ReferencesFinderPersonalSettings
	{
		public bool showAssetsWithoutReferences;
		public bool selectedFindClearsResults;

		public bool fullProjectScanWarningShown;
		public string searchString;

#if UNITY_5_6_OR_NEWER
		public TreeViewState treeViewState;
		public MultiColumnHeaderState multiColumnHeaderState;
#endif
	}
}