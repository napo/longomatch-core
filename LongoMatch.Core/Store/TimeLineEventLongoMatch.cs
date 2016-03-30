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
using System.Collections.ObjectModel;
using LongoMatch.Core.Store.Templates;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LongoMatch.Core.Store
{
	[Serializable]
	public class TimelineEventLongoMatch : TimelineEvent
	{
		ObservableCollection<Player> players;
		ObservableCollection<Tag> tags;
		ObservableCollection<Team> teams;

		public TimelineEventLongoMatch ()
		{
			Players = new ObservableCollection<Player> ();
			Tags = new ObservableCollection<Tag> ();
			Teams = new ObservableCollection<Team> ();
		}

		/// <summary>
		/// List of players tagged in this event.
		/// </summary>
		[LongoMatchPropertyIndex (0)]
		public ObservableCollection<Player> Players {
			get {
				return players;
			}
			set {
				if (players != null) {
					players.CollectionChanged -= ListChanged;
				}
				players = value;
				if (players != null) {
					players.CollectionChanged += ListChanged;
				}
			}
		}

		[PropertyChanged.DoNotNotify]
		[Obsolete ("Use Teams instead of Team to tag a team in a TimelineEvent")]
		public TeamType Team {
			get;
			set;
		}

		/// <summary>
		/// A list of teams tagged in this event.
		/// </summary>
		[LongoMatchPropertyIndex (3)]
		public ObservableCollection<Team> Teams {
			get {
				return teams;
			}
			set {
				if (teams != null) {
					teams.CollectionChanged -= ListChanged;
				}
				teams = value ?? new ObservableCollection<Team> ();
				if (teams != null) {
					teams.CollectionChanged += ListChanged;
				}
			}
		}

		[JsonIgnore]
		public List<Team> TaggedTeams {
			get {
				if (Project == null) {
					return Teams.ToList ();
				}
				List<Team> teams = new List<Team> ();
				if (Teams.Contains (Project.LocalTeamTemplate) ||
				    Players.Intersect (Project.LocalTeamTemplate.List).Any ()) {
					teams.Add (Project.LocalTeamTemplate);
				}
				if (Teams.Contains (Project.VisitorTeamTemplate) ||
				    Players.Intersect (Project.VisitorTeamTemplate.List).Any ()) {
					teams.Add (Project.VisitorTeamTemplate);
				}
				return teams;
			}
		}


		/// <summary>
		/// List of tags describing this event.
		/// </summary>
		/// <value>The tags.</value>
		public ObservableCollection<Tag> Tags {
			get {
				return tags;
			}
			set {
				if (tags != null) {
					tags.CollectionChanged -= ListChanged;
				}
				tags = value;
				if (tags != null) {
					tags.CollectionChanged += ListChanged;
				}
			}
		}

		public string TagsDescription ()
		{
			return String.Join ("-", Tags.Select (t => t.Value));
		}

		/// <summary>
		/// An event in the game for a penalty.
		/// </summary>
		[Serializable]
		[Obsolete ("Create a TimelineEvent with a PenaltyCardEventType")]
		public class PenaltyCardEvent: TimelineEvent
		{
			[JsonIgnore]
			[PropertyChanged.DoNotNotify]
			public PenaltyCardEventType PenaltyCardEventType {
				get {
					return EventType as PenaltyCardEventType;
				}
			}

			public PenaltyCard PenaltyCard {
				get;
				set;
			}
		}

		[Serializable]
		[Obsolete ("Create a TimelineEvent with a ScoreEventType")]
		public class ScoreEvent: TimelineEvent
		{
			[JsonIgnore]
			[PropertyChanged.DoNotNotify]
			public ScoreEventType ScoreEventType {
				get {
					return EventType as ScoreEventType;
				}
			}

			public Score Score {
				get;
				set;
			}
		}

		[Serializable]
		public class LineupEvent: StatEvent
		{
			public List<Player> HomeStartingPlayers {
				get;
				set;
			}

			public List<Player> HomeBenchPlayers {
				get;
				set;
			}

			public List<Player> AwayStartingPlayers {
				get;
				set;
			}

			public List<Player> AwayBenchPlayers {
				get;
				set;
			}
		}
	}
}

