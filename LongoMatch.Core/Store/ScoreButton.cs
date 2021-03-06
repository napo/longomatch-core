﻿//
//  Copyright (C) 2008-2015 Andoni Morales Alastruey
//  Copyright (C) 2016 Fluendo S.A.
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
using System;
using Newtonsoft.Json;
using VAS.Core.Serialization;
using VAS.Core.Store;

namespace LongoMatch.Core.Store
{
	[Serializable]
	public class ScoreButton : EventButton
	{

		public ScoreButton ()
		{
			EventType = new ScoreEventType ();
		}

		// We keep it for backwards compatibility, to be able to retrieve the Score from this object
		[JsonProperty ("Score")]
		[Obsolete ("Do not use, it's only kept here for migrations")]
		public Score OldScore {
			set;
			get;
		}

		[CloneIgnoreAttribute]
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public Score Score {
			get {
				return ScoreEventType?.Score;
			}
			set {
				if (ScoreEventType != null) {
					ScoreEventType.Score = value;
				}
			}
		}


		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public ScoreEventType ScoreEventType {
			get {
				return EventType as ScoreEventType;
			}
		}
	}
}

