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
	public class TagsAndLayersIssueRecord : IssueRecord, IShowableRecord
	{
		public void Show()
		{
			CSMenuTools.ShowProjectSettingsTagsAndLayers();
		}

		internal static TagsAndLayersIssueRecord Create(RecordType type, string body)
		{
            return new TagsAndLayersIssueRecord(type, body);
		}

		internal override bool MatchesFilter(FilterItem newFilter)
		{
			return false;
		}

		internal override bool CanBeFixed()
		{
			return false;
		}

		protected TagsAndLayersIssueRecord(RecordType type, string body):base(type, RecordLocation.TagsAndLayers)
		{
			bodyExtra = body;
		}

		protected override void ConstructBody(StringBuilder text)
		{
			text.Append("<b>Tags and Layers</b> issue");
		}
	}
}