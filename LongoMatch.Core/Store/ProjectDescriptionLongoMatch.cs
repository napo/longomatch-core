//
//  Copyright (C) 2016 dfernandez
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
using VAS.Core.Store;
using Newtonsoft.Json;

namespace LongoMatch.Core.Store
{
	[Serializable]
	[PropertyChanged.ImplementPropertyChanged]
	public class ProjectDescriptionLongoMatch : ProjectDescription
	{
		public ProjectDescriptionLongoMatch ()
		{
			LocalGoals = 0;
			LocalName = "";
			VisitorGoals = 0;
			VisitorName = "";
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public override String Title {
			get {
				return String.Format ("{0} - {1} ({2}-{3}) {4} {5}",
					LocalName, VisitorName, LocalGoals, VisitorGoals,
					Competition, Season);
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public override String DateTitle {
			get {
				string ret = String.Format ("{0}-{1} {2}", LocalName, VisitorName,
					             MatchDate.ToShortDateString ());
				if (!String.IsNullOrEmpty (Season)) {
					ret += " " + Season;
				}
				if (!String.IsNullOrEmpty (Competition)) {
					ret += " " + Competition;
				}
				return ret;
			}
		}

		/// <summary>
		/// Name of the local team
		/// </summary>
		public String LocalName {
			get;
			set;
		}

		/// <summary>
		/// Name of the visitor team
		/// </summary>
		public String VisitorName {
			get;
			set;
		}

		/// <summary>
		/// Goals of the local team
		/// </summary>
		public int LocalGoals {
			get;
			set;
		}

		/// <summary>
		/// Goals of the visitor team
		/// </summary>
		public int VisitorGoals {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the shield for the local team.
		/// </summary>
		/// <value>The local team shield.</value>
		public Image LocalShield {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the shield for the visitor team.
		/// </summary>
		/// <value>The visitor team shield.</value>
		public Image VisitorShield {
			get;
			set;
		}

		public bool Search (string text)
		{
			StringComparison sc = StringComparison.InvariantCultureIgnoreCase;

			if (!base.Search (text)) {
				if (LocalName != null && LocalName.IndexOf (text, sc) > -1) {
					return true;
				} else if (VisitorName != null && VisitorName.IndexOf (text, sc) > -1) {
					return true;
				} else {
					return false;
				}
			}
		}
	}
}

