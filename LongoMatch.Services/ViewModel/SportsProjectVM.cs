//
//  Copyright (C) 2016 Fluendo S.A.
//
//
using System;
using LongoMatch.Core.Store;
using VAS.Core.Common;
using VAS.Services.ViewModel;

namespace LongoMatch.Services.ViewModel
{
	public class SportsProjectVM : ProjectVM<ProjectLongoMatch>
	{
		public string Title {
			get {
				return String.Format ("{0} - {1} ({2}-{3})",
									  Model.Description.LocalName, Model.Description.VisitorName,
									  Model.Description.LocalGoals, Model.Description.VisitorGoals);
			}
		}

		public string Description {
			get {
				return Model.Description.Description;
			}
		}

		public string HomeTeamText {
			get {
				return Model.Description.LocalName;
			}
		}

		public string AwayTeamText {
			get {
				return Model.Description.VisitorName;
			}
		}

		public Image HomeTeamShield {
			get {
				return Model.Description.LocalShield;
			}
		}

		public Image AwayTeamShield {
			get {
				return Model.Description.VisitorShield;
			}
		}

		public string DashboardText {
			get {
				return Model.Description.DashboardName;
			}
		}

		public DateTime MatchDate {
			get {
				return Model.Description.MatchDate;
			}
		}

		public string Season {
			get {
				return Model.Description.Season;
			}
		}

		public string Competition {
			get {
				return Model.Description.Competition;
			}
		}

		public string Result {
			get {
				return String.Format ("{0}-{1}", Model.Description.LocalGoals,
							 Model.Description.VisitorGoals);
			}
		}
	}
}

