//
//  Copyright (C) 2014 Andoni Morales Alastruey
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
using LongoMatch.Core.Interfaces;
using System.Collections.Generic;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Core.Store.Playlists;
using Mono.Unix;
using System.Linq;
using Newtonsoft.Json;

namespace LongoMatch.Core.Store
{
	public class Presentation: IProject
	{
		public Presentation ()
		{
			ID = new Guid ();
			Description = Catalog.GetString ("Presentation");
			Timeline = new List<TimelineEvent> ();
			EventTypes = new List<EventType> ();
			Teams = new List<TeamTemplate> ();
			Playlists = new List<Playlist> ();
		}

		public Guid ID {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public List<TimelineEvent> Timeline {
			get;
			set;
		}

		public List<EventType> EventTypes {
			get;
			set;
		}

		public List<TeamTemplate> Teams {
			get;
			set;
		}

		public List<Playlist> Playlists {
			get;
			set;
		}

		public List<Guid> ProjectsID {
			get;
			set;
		}

		[JsonIgnore]
		public IEnumerable<IGrouping<EventType, TimelineEvent>> EventsGroupedByType {
			get {
				return Timeline.GroupBy (e => e.EventType);
			}
		}

		public static Presentation CreateFromData (List<Project> projects, List<TeamTemplate> teams,
		                                           List<EventType> eventTypes)
		{
			Dictionary <Guid, EventType> eventTypesDict = eventTypes.ToDictionary (t => t.ID, t => t);
			Dictionary <Guid, Player> playersDict = new Dictionary<Guid, Player> ();
			List<TimelineEvent> timeline = new List<TimelineEvent> ();
			Presentation presentation = new Presentation ();

			foreach (TeamTemplate team in teams) {
				foreach (Player player in team.List) {
					playersDict [player.ID] = player;
				}
			}
			
			foreach (Project project in projects) {
				foreach (TimelineEvent evt in project.Timeline) {
					List<Player> players = new List<Player> ();
					if (!eventTypesDict.ContainsKey (evt.ID)) {
						continue;
					}
					/* FIXME: filter team */
					evt.EventType = eventTypesDict [evt.ID];
					foreach (Player player in evt.Players) {
						if (playersDict.ContainsKey (player.ID)) {
							players.Add (playersDict [player.ID]);
						}
					}
					evt.Players = players;
					timeline.Add (evt);
				}
			}
			
			presentation.Timeline = timeline;
			presentation.EventTypes = eventTypes;
			presentation.Teams = teams;
			presentation.ProjectsID = projects.Select (p => p.ID).ToList ();
			return presentation;
		}
	}
}

