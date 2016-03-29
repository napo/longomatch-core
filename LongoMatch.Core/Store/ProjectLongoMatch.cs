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

namespace LongoMatch.Core.Store
{
	[Serializable]
	class ProjectLongoMatch : Project
	{
		SubstitutionEventType subsType;

		#region Constructors

		public ProjectLongoMatch ()
		{
			LocalTeamTemplate = new Team ();
			VisitorTeamTemplate = new Team ();
		}

		public void Dispose ()
		{
			base.Dispose ();
			LocalTeamTemplate?.Dispose ();
			VisitorTeamTemplate?.Dispose ();
		}

		#endregion

		#region Properties

		/// <value>
		/// Local team template
		/// </value>
		[JsonProperty (Order = -9)]
		public Team LocalTeamTemplate {
			get;
			set;
		}

		/// <value>
		/// Visitor team template
		/// </value>
		[JsonProperty (Order = -8)]
		public Team VisitorTeamTemplate {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public List<TimelineEvent> ScoreEvents {
			get {
				return Timeline.Where (e => e.EventType is ScoreEventType).ToList ();
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public List<TimelineEvent> PenaltyCardsEvents {
			get {
				return Timeline.Where (e => e.EventType is PenaltyCardEventType).ToList ();
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public SubstitutionEventType SubstitutionsEventType {
			get {
				if (subsType == null) {
					subsType = EventTypes.OfType<SubstitutionEventType> ().FirstOrDefault ();
					if (subsType == null) {
						subsType = new SubstitutionEventType ();
						subsType.SortMethod = SortMethodType.SortByStartTime;
					}
				}
				return subsType;
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public LineupEvent Lineup {
			get {
				LineupEvent lineup = Timeline.OfType <LineupEvent> ().FirstOrDefault ();
				if (lineup == null) {
					lineup = CreateLineupEvent ();
				}
				return lineup;
			}
		}

		#endregion

		#region Public Methods

		public void UpdateScore ()
		{
			Description.LocalGoals = GetScore (LocalTeamTemplate);
			Description.VisitorGoals = GetScore (VisitorTeamTemplate);
		}

		public void CurrentLineup (Time currentTime,
		                           out List<Player> homeFieldPlayers,
		                           out List<Player> homeBenchPlayers,
		                           out List<Player> awayFieldPlayers,
		                           out List<Player> awayBenchPlayers)
		{
			Team homeTeam, awayTeam;
			List<Player> homeTeamPlayers, awayTeamPlayers;

			homeTeamPlayers = Lineup.HomeStartingPlayers.Concat (Lineup.HomeBenchPlayers).ToList ();
			awayTeamPlayers = Lineup.AwayStartingPlayers.Concat (Lineup.AwayBenchPlayers).ToList ();

			foreach (SubstitutionEvent ev in Timeline.OfType<SubstitutionEvent> ().
				Where (e => e.EventTime <= currentTime)) {
				if (ev.In != null && ev.Out != null) {
					if (ev.Teams.Contains (LocalTeamTemplate)) {
						homeTeamPlayers.Swap (ev.In, ev.Out);
					} else {
						awayTeamPlayers.Swap (ev.In, ev.Out);
					}
				}
			}

			homeTeam = new Team {
				Formation = LocalTeamTemplate.Formation,
				List = new ObservableCollection<Player> (homeTeamPlayers)
			};
			awayTeam = new Team {
				Formation = VisitorTeamTemplate.Formation,
				List = new ObservableCollection<Player> (awayTeamPlayers)
			};

			homeFieldPlayers = homeTeam.StartingPlayersList;
			homeBenchPlayers = homeTeam.BenchPlayersList;
			awayFieldPlayers = awayTeam.StartingPlayersList;
			awayBenchPlayers = awayTeam.BenchPlayersList;
		}

		public SubstitutionEvent SubsitutePlayer (Team team, Player playerIn, Player playerOut,
		                                          SubstitutionReason reason, Time subsTime)
		{
			LineupEvent lineup;
			SubstitutionEvent se;

			lineup = Timeline.OfType<LineupEvent> ().FirstOrDefault ();
			if (lineup == null) {
				throw new SubstitutionException (Catalog.GetString ("No lineup events found"));
			}
			if (subsTime < lineup.EventTime) {
				throw new SubstitutionException (Catalog.GetString ("A substitution can't happen before the lineup event"));
			}
			se = new SubstitutionEvent ();
			se.EventType = SubstitutionsEventType;
			se.In = playerIn;
			se.Out = playerOut;
			se.Reason = reason;
			se.EventTime = subsTime;
			se.Teams.Add (team);
			Timeline.Add (se);
			return se;
		}

		public bool LineupChanged (Time start, Time stop)
		{
			return Timeline.OfType<SubstitutionEvent> ().
				Count (s => s.EventTime > start && s.EventTime <= stop) > 0;
		}

		public LineupEvent CreateLineupEvent ()
		{
			Time startTime;
			LineupEvent lineup;

			if (Periods.Count == 0) {
				startTime = new Time (0);
			} else {
				startTime = Periods [0].PeriodNode.Start;
			}

			lineup = new LineupEvent {
				Name = Catalog.GetString ("Lineup"),
				EventType = SubstitutionsEventType,
				HomeStartingPlayers = LocalTeamTemplate.StartingPlayersList,
				HomeBenchPlayers = LocalTeamTemplate.BenchPlayersList,
				AwayStartingPlayers = VisitorTeamTemplate.StartingPlayersList,
				AwayBenchPlayers = VisitorTeamTemplate.BenchPlayersList,
				EventTime = startTime
			};
			AddEvent (lineup);

			return lineup;
		}

		public IEnumerable<TimelineEvent> EventsByTeam (Team team)
		{
			return Timeline.Where (e => e.Teams.Contains (team) || e.Players.Intersect (team.List).Any ());
		}

		public int GetScore (Team team)
		{
			return EventsByTeam (team).Where (e => e.EventType is ScoreEventType).
				Sum (e => (e.EventType as ScoreEventType).Score.Points);
		}

		public TimelineEvent AddEvent (EventType type, Time start, Time stop, Time eventTime, Image miniature,
		                               bool addToTimeline = true)
		{
			base.AddEvent (type, start, stop, eventTime, miniature, addToTimeline);

			if (addToTimeline) {
				if (evt.EventType is ScoreEventType) {
					UpdateScore ();
				}
			}
			return evt;
		}

		public void AddEvent (TimelineEvent play)
		{
			base.AddEvent (play);
			if (play.EventType is ScoreEventType) {
				UpdateScore ();
			}
		}

		/// <summary>
		/// Delete a play from the project
		/// </summary>
		/// <param name="tNode">
		/// A <see cref="MediaTimeNode"/>: play to be deleted
		/// </param>
		/// <param name="section">
		/// A <see cref="System.Int32"/>: category the play belongs to
		/// </param>
		public void RemoveEvents (List<TimelineEvent> plays)
		{
			bool updateScore = false;

			base.RemoveEvents (plays);
			foreach (TimelineEvent play in plays) {
				if (play.EventType is ScoreEventType) {
					updateScore = true;
				}
			}
			if (updateScore) {
				UpdateScore ();
			}
		}

		public override void UpdateEventTypesAndTimers ()
		{
			IEnumerable<EventType> dashboardtypes = new List<EventType> ();
			IEnumerable<EventType> timelinetypes;

			if (Dashboard != null) {
				/* Timers */
				IEnumerable<Timer> timers = Dashboard.List.OfType<TimerButton> ().Select (b => b.Timer);
				Timers.AddRange (timers.Except (Timers));

				/* Update event types list that changes when the user adds or remove a
				 * a new button to the dashboard or after importing a project with events
				 * tagged with a different dashboard */
				dashboardtypes = Dashboard.List.OfType<EventButton> ().Select (b => b.EventType);
			}

			/* Remove event types that have no events and are not in the dashboard anymore */
			foreach (EventType evt in EventTypes.Except (dashboardtypes).ToList ()) {
				if (evt == SubstitutionsEventType) {
					continue;
				}
				if (Timeline.Count (e => e.EventType == evt) == 0) {
					EventTypes.Remove (evt);
				}
			}
			EventTypes.AddRange (dashboardtypes.Except (EventTypes));
			timelinetypes = Timeline.Select (t => t.EventType).Distinct ().Except (EventTypes);
			EventTypes.AddRange (timelinetypes.Except (EventTypes));
			if (!EventTypes.Contains (SubstitutionsEventType)) {
				EventTypes.Add (SubstitutionsEventType);
			}

			/* Remove null EventTypes just in case */
			EventTypes = new ObservableCollection<EventType> (EventTypes.Where (e => e != null));
		}

		public void ConsolidateDescription ()
		{
			base.ConsolidateDescription ();
			Description.LocalName = LocalTeamTemplate.Name;
			Description.LocalShield = LocalTeamTemplate.Shield;
			Description.LocalGoals = GetScore (LocalTeamTemplate);
			Description.VisitorName = VisitorTeamTemplate.Name;
			Description.VisitorShield = VisitorTeamTemplate.Shield;
			Description.VisitorGoals = GetScore (VisitorTeamTemplate);
		}

		#endregion
	}
}

