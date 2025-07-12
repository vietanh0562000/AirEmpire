﻿#region copyright
//--------------------------------------------------------------------
// Copyright (C) 2015 Dmitriy Yukhanov - focus [http://codestage.net].
//--------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Core
{
	using System;

	public enum Location
	{
		NotFound,
		Invisible,
		GameObject,
		LightingSettings,
		Navigation
	}

	[Serializable]
	public class ReferencingEntryData
	{
		public Location location = Location.GameObject;
		public string label;
		public string transformPath;
		public long objectId = -1L;
		public long componentId = -1L;
		public string propertyName;
	}
}