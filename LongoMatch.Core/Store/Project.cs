// Project.cs
//
//  Copyright (C) 2007-2009 Andoni Morales Alastruey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using LongoMatch.Core.Common;
using LongoMatch.Core.Migration;
using LongoMatch.Core.Serialization;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Playlists;
using LongoMatch.Core.Store.Templates;
using Newtonsoft.Json;

namespace VAS.Core.Store
{

	/// <summary>
	/// I hold the information needed by a project and provide persistency using
	/// the db4o database.
	/// I'm structured in the following way:
	/// -Project Description (<see cref="LongoMatch.Utils.PreviewMediaFile"/>
	/// -1 Categories Template
	/// -1 Local Team Template
	/// -1 Visitor Team Template
	/// -1 list of <see cref="LongoMatch.Store.MediaTimeNode"/> for each category
	/// </summary>
	///
	[Serializable]
	abstract public class Project : StorableBase, IComparable, IDisposable
	{
		public const int CURRENT_VERSION = 1;
		ObservableCollection<TimelineEvent> timeline;
		ObservableCollection<Period> periods;
		ObservableCollection<Timer> timers;
		ObservableCollection<Playlist> playlists;
		ObservableCollection<EventType> eventTypes;

		#region Constructors


		public Project ()
		{
			ID = System.Guid.NewGuid ();
			Timeline = new ObservableCollection<TimelineEvent> ();
			Dashboard = new Dashboard ();
			Timers = new ObservableCollection<Timer> ();
			Periods = new ObservableCollection<Period> ();
			Playlists = new ObservableCollection<Playlist> ();
			EventTypes = new ObservableCollection<EventType> ();
			Version = Constants.DB_VERSION;
		}

		[OnDeserialized ()]
		internal void OnDeserializedMethod (StreamingContext context)
		{
			foreach (TimelineEvent evt in Timeline) {
				evt.Project = this;
			}
		}

		public void Dispose ()
		{
			Dashboard?.Dispose ();
			foreach (TimelineEvent evt in Timeline) {
				evt.Dispose ();
			}
		}

		#endregion

		#region Properties

		/// <value>
		/// Document version
		/// </value>
		[DefaultValue (0)]
		[JsonProperty (DefaultValueHandling = DefaultValueHandling.Populate)]
		public int Version {
			get;
			set;
		}

		public ObservableCollection<TimelineEvent> Timeline {
			get {
				return timeline;
			}
			set {
				if (timeline != null) {
					timeline.CollectionChanged -= ListChanged;
				}
				timeline = value;
				if (timeline != null) {
					timeline.CollectionChanged += ListChanged;
				}
			}
		}

		[LongoMatchPropertyPreload]
		public ProjectDescription Description {
			get;
			set;
		}

		[JsonProperty (Order = -7)]
		public ObservableCollection<EventType> EventTypes {
			get {
				return eventTypes;
			}
			set {
				if (eventTypes != null) {
					eventTypes.CollectionChanged -= ListChanged;
				}
				eventTypes = value;
				if (eventTypes != null) {
					eventTypes.CollectionChanged += ListChanged;
				}
			}
		}

		/// <value>
		/// Categories template
		/// </value>
		[JsonProperty (Order = -10)]
		public Dashboard Dashboard {
			get;
			set;
		}

		public ObservableCollection<Period> Periods {
			get {
				return periods;
			}
			set {
				if (periods != null) {
					periods.CollectionChanged -= ListChanged;
				}
				periods = value;
				if (periods != null) {
					periods.CollectionChanged += ListChanged;
				}
			}
		}

		public ObservableCollection<Timer> Timers {
			get {
				return timers;
			}
			set {
				if (timers != null) {
					timers.CollectionChanged -= ListChanged;
				}
				timers = value;
				if (timers != null) {
					timers.CollectionChanged += ListChanged;
				}
			}
		}

		public ObservableCollection<Playlist> Playlists {
			get {
				return playlists;
			}
			set {
				if (playlists != null) {
					playlists.CollectionChanged -= ListChanged;
				}
				playlists = value;
				if (playlists != null) {
					playlists.CollectionChanged += ListChanged;
				}
			}
		}


		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public IEnumerable<IGrouping<EventType, TimelineEvent>> EventsGroupedByEventType {
			get {
				return Timeline.GroupBy (play => play.EventType);
			}
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public ProjectType ProjectType {
			get;
			set;
		}

		[JsonIgnore]
		[PropertyChanged.DoNotNotify]
		public bool IsFakeCapture {
			get {
				if (Description != null) {
					MediaFileSet fileSet = Description.FileSet;
					if (fileSet != null) {
						MediaFile file = fileSet.FirstOrDefault ();
						if (file != null)
							return file.IsFakeCapture;
					}
				}
				return false;
			}
		}

		#endregion

		#region Public Methods

		public TimelineEvent AddEvent (EventType type, Time start, Time stop, Time eventTime, Image miniature,
		                               bool addToTimeline = true)
		{
			TimelineEvent evt;
			string count;
			string name;

			count = String.Format ("{0:000}", EventsByType (type).Count + 1);
			name = String.Format ("{0} {1}", type.Name, count);
			evt = new TimelineEvent ();

			evt.Name = name;
			evt.Start = start;
			evt.Stop = stop;
			evt.EventTime = eventTime;
			evt.EventType = type;
			evt.Notes = "";
			evt.Miniature = miniature;
			evt.CamerasConfig = new ObservableCollection<CameraConfig> { new CameraConfig (0) };
			evt.FileSet = Description.FileSet;
			evt.Project = this;

			if (addToTimeline) {
				Timeline.Add (evt);
			}
			return evt;
		}

		public void AddEvent (TimelineEvent play)
		{
			play.FileSet = Description.FileSet;
			play.Project = this;
			Timeline.Add (play);
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
			foreach (TimelineEvent play in plays) {
				Timeline.Remove (play);
			}
		}

		public void CleanupTimers ()
		{
			foreach (Timer t in Timers) {
				t.Nodes.RemoveAll (tn => tn.Start == null || tn.Stop == null);
			}
		}

		abstract public void UpdateEventTypesAndTimers ();

		public List<TimelineEvent> EventsByType (EventType evType)
		{
			return Timeline.Where (p => p.EventType.ID == evType.ID).ToList ();
		}

		public Image GetBackground (FieldPositionType pos)
		{
			switch (pos) {
			case FieldPositionType.Field:
				return Dashboard.FieldBackground;
			case FieldPositionType.HalfField:
				return Dashboard.HalfFieldBackground;
			case FieldPositionType.Goal:
				return Dashboard.GoalBackground;
			}
			return null;
		}

		public void ConsolidateDescription ()
		{
			Description.LastModified = DateTime.UtcNow;
			Description.DashboardName = Dashboard.Name;
		}


		/// <summary>
		/// Resynchronize events with the periods synced with the video file.
		/// Imported projects or fake analysis projects create events assuming periods
		/// don't have gaps between them.
		/// After adding a file to the project and synchronizing the periods with the
		/// video file, all events must be offseted with the new start time of the period.
		/// 
		/// Before sync:
		///   Period 1: start=00:00:00 Period 2: start=00:30:00
		///   evt1 00:10:00            evt2 00:32:00
		/// After sync:
		///   Period 1: start=00:05:00 Period 2: start= 00:39:00
		///   evt1 00:15:00            evt2 00:41:00
		/// </summary>
		/// <param name="periods">The new periods syncrhonized with the video file.</param>
		public void ResyncEvents (IList<Period> periods)
		{
			ObservableCollection<TimelineEvent> newTimeline = new ObservableCollection<TimelineEvent> ();

			if (periods.Count != Periods.Count) {
				throw new IndexOutOfRangeException (
					"Periods count is different from the project's ones");
			}

			for (int i = 0; i < periods.Count; i++) {
				Period oldPeriod = Periods [i];
				TimeNode oldTN = oldPeriod.PeriodNode;
				TimeNode newTN = periods [i].PeriodNode;
				Time diff = newTN.Start - oldTN.Start;

				/* Find the events in this period */
				var periodEvents = Timeline.Where (e =>
					e.EventTime >= oldTN.Start &&
				                   e.EventTime <= oldTN.Stop).ToList ();

				/* Apply new offset and move the new timeline so that the next
				 * iteration for the following period does not use them anymore */
				periodEvents.ForEach (e => {
					e.Move (diff);
					newTimeline.Add (e);
					Timeline.Remove (e);
				});
				foreach (TimeNode tn in oldPeriod.Nodes) {
					tn.Move (diff);
				}
			}
			Timeline = newTimeline;
		}

		public int CompareTo (object obj)
		{
			if (obj is Project) {
				Project project = (Project)obj;
				return ID.CompareTo (project.ID);
			} else
				throw new ArgumentException ("object is not a Project and cannot be compared");
		}

		public static void Export (Project project, string file)
		{
			file = Path.ChangeExtension (file, Constants.PROJECT_EXT);
			Serializer.Instance.Save (project, file);
		}

		public static Project Import ()
		{
			string file = Config.GUIToolkit.OpenFile (Catalog.GetString ("Import project"), null, Config.HomeDir, Constants.PROJECT_NAME,
				              new string[] { "*" + Constants.PROJECT_EXT });
			if (file == null)
				return null;
			return Project.Import (file);
		}

		public static Project Import (string file)
		{
			Project project = null;
			try {
				project = Serializer.Instance.Load<Project> (file);
			} catch (Exception e) {
				Log.Exception (e);
				throw new Exception (Catalog.GetString ("The file you are trying to load " +
				"is not a valid project"));
			}
			ProjectMigration.Migrate (project);
			return project;
		}

	
		void ListChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			IsChanged = true;
		}

		#endregion
	}
}
