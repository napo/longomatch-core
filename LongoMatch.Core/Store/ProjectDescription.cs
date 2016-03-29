//
//  Copyright (C) 2009 Andoni Morales Alastruey
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
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
using System;
using Newtonsoft.Json;
using LongoMatch.Core.Store;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Common;
using System.Runtime.Serialization;

namespace VAS.Core.Store
{
	/// <summary>
	/// Describes a project in LongoMatch.
	/// </summary>
	[Serializable]
	[PropertyChanged.ImplementPropertyChanged]
	abstract public class ProjectDescription : IChanged
	{
		DateTime matchDate, lastModified;

		public ProjectDescription ()
		{
			MatchDate = LastModified = DateTime.Now;

			Category = "";
			Competition = "";
			Description = "";
			Group = "";
			Phase = "";
			Season = "";
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public bool IsChanged {
			get;
			set;
		}

		/// <summary>
		/// Title of the project
		/// </summary>
		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		abstract public String Title {
			get;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		abstract public String DateTitle {
			get;
		}

		/// <summary>
		/// Media file asigned to this project
		/// </summary>
		public MediaFileSet FileSet {
			get;
			set;
		}

		/// <summary>
		/// Name of the dashboard in use for this project.
		/// </summary>
		public string DashboardName {
			get;
			set;
		}

		/// <summary>
		/// Season of the game
		/// </summary>
		public String Season {
			get;
			set;
		}

		/// <summary>
		/// Comptetition of the game
		/// </summary>
		public String Competition {
			get;
			set;
		}

		public string Category {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public string Group {
			get;
			set;
		}

		public string Phase {
			get;
			set;
		}

		/// <summary>
		/// Date of the game
		/// </summary>
		public DateTime MatchDate {
			get {
				return matchDate;
			}
			set {
				matchDate = value.ToUniversalTime ();
			}
		}

		public DateTime LastModified {
			get {
				return lastModified;
			}
			set {
				lastModified = value.ToUniversalTime ();
			}
		}

		public bool Search (string text)
		{
			StringComparison sc = StringComparison.InvariantCultureIgnoreCase;

			if (text == "")
				return true;

			if (Title != null && Title.IndexOf (text, sc) > -1)
				return true;
			else if (Season != null && Season.IndexOf (text, sc) > -1)
				return true;
			else if (Competition != null && Competition.IndexOf (text, sc) > -1)
				return true;
			else if (Description != null && Description.IndexOf (text, sc) > -1)
				return true;
			else
				return false;
		}

		static public int Sort (ProjectDescription p1, ProjectDescription p2,
		                        ProjectSortType sortType)
		{
			int ret = 0;
			
			if (p1 == null && p2 == null) {
				ret = 0;
			} else if (p1 == null) {
				ret = -1;
			} else if (p2 == null) {
				ret = 1;
			} else {
				switch (sortType) {
				case ProjectSortType.SortByName:
					{
						ret = String.Compare (p1.Title, p2.Title);
						if (ret == 0) {
							ret = -DateTime.Compare (p1.MatchDate, p2.MatchDate);
						}
						break;
					}
				case ProjectSortType.SortByDate:
					{
						ret = -DateTime.Compare (p1.MatchDate, p2.MatchDate);
						if (ret == 0) {
							ret = String.Compare (p1.Title, p2.Title);
						}
						break;
					}
				case ProjectSortType.SortByModificationDate:
					{
						ret = -DateTime.Compare (p1.LastModified, p2.LastModified);
						break;
					}
				case ProjectSortType.SortBySeason:
					{
						ret = String.Compare (p1.Season, p2.Season);
						if (ret == 0) {
							ret = -DateTime.Compare (p1.MatchDate, p2.MatchDate);
						}
						break;
					}
				case ProjectSortType.SortByCompetition:
					{
						ret = String.Compare (p1.Competition, p2.Competition);
						if (ret == 0) {
							ret = -DateTime.Compare (p1.MatchDate, p2.MatchDate);
						}
						break;
					}
				}
			}
			return ret;
		}
	}
}
