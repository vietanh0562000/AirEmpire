#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Issues
{
	using System;
	using System.Text;

	using Core;
	using Tools;

	[Serializable]
	public class BuildSettingsIssueRecord : IssueRecord, IShowableRecord
	{
		public void Show()
		{
			CSEditorTools.RevealInSettings(AssetSettingsKind.EditorBuildSettings);
		}

		internal static BuildSettingsIssueRecord Create(RecordType type, string body)
		{
            return new BuildSettingsIssueRecord(type, body);
		}

		internal override bool MatchesFilter(FilterItem newFilter)
		{
			return false;
		}

		internal override bool CanBeFixed()
		{
			return false;
		}

		protected BuildSettingsIssueRecord(RecordType type, string body):base(type, RecordLocation.BuildSettings)
		{
			bodyExtra = body;
		}

		protected override void ConstructBody(StringBuilder text)
		{
			text.Append("<b>Build Settings</b> issue");
		}
	}
}