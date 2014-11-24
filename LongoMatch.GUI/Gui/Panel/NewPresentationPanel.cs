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
using System.Collections.Generic;
using System.Linq;
using Gtk;
using LongoMatch.Core.Handlers;
using LongoMatch.Core.Interfaces.GUI;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using Misc = LongoMatch.Gui.Helpers.Misc;
using Mono.Unix;
using LongoMatch.Core.Common;

namespace LongoMatch.Gui.Panel
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class NewPresentationPanel : Gtk.Bin, IPanel
	{
		public event BackEventHandle BackEvent;

		Dictionary <Guid, Project> projects;
		Dictionary <Guid, TeamTemplate> teams;
		Dictionary <Guid, EventType> eventTypes;
		Dictionary <Guid, int> teamsRefs;
		Dictionary <Guid, int> eventTypesRefs;
		ListStore teamsStore;
		ListStore eventTypesStore;
		ListStore projectsStore;

		public NewPresentationPanel ()
		{
			this.Build ();
			panelheader1.ApplyVisible = false;
			panelheader1.BackClicked += HandleClicked;
			panelheader1.ApplyClicked += HandleApplyClicked;
			panelheader1.Title = "PRESENTATION PROJECT";
			projects = new Dictionary<Guid, Project> ();
			eventTypes = new Dictionary<Guid, EventType> ();
			teams = new Dictionary<Guid, TeamTemplate> ();
			eventTypesRefs = new Dictionary<Guid, int> ();
			teamsRefs = new Dictionary<Guid, int> ();
			teamsStore = new ListStore (typeof(bool), typeof(string), typeof(TeamTemplate));
			eventTypesStore = new ListStore (typeof(bool), typeof(string), typeof(EventType));
			projectsStore = new ListStore (typeof(bool), typeof(Gdk.Pixbuf), typeof(Gdk.Pixbuf),
			                               typeof(string), typeof(ProjectDescription));

			teamstreeview.Model = teamsStore;
			teamstreeview.HeadersVisible = false;
			teamstreeview.Selection.Mode = SelectionMode.None;
			teamstreeview.AppendColumn ("Selected", new CellRendererToggle (), "active", 0); 
			teamstreeview.AppendColumn ("Name", new CellRendererText (), "text", 1); 
			teamstreeview.ButtonPressEvent += HandleButtonPressEvent;

			eventstreeview.Model = eventTypesStore;
			eventstreeview.HeadersVisible = false;
			eventstreeview.Selection.Mode = SelectionMode.None;
			eventstreeview.AppendColumn ("Selected", new CellRendererToggle (), "active", 0); 
			eventstreeview.AppendColumn ("Name", new CellRendererText (), "text", 1);
			eventstreeview.ButtonPressEvent += HandleButtonPressEvent;
			
			projectstreeview.Model = projectsStore;
			projectstreeview.HeadersVisible = false;
			projectstreeview.Selection.Mode = SelectionMode.None;
			projectstreeview.AppendColumn ("Selected", new CellRendererToggle (), "active", 0);
			projectstreeview.AppendColumn ("Home", new CellRendererPixbuf (), "pixbuf", 1); 
			projectstreeview.AppendColumn ("Away", new CellRendererPixbuf (), "pixbuf", 2); 
			projectstreeview.AppendColumn ("Desc", new CellRendererText (), "text", 3); 
			projectstreeview.ButtonPressEvent += HandleButtonPressEvent;

			foreach (ProjectDescription pdesc in Config.DatabaseManager.ActiveDB.GetAllProjects()) {
				Gdk.Pixbuf homeShield, awayShield;

				if (pdesc.LocalShield != null) {
					homeShield = pdesc.LocalShield.Scale (50, 50).Value;
				} else {
					homeShield = Misc.LoadIcon ("longomatch-default-shield", 50);
				}
				if (pdesc.VisitorShield != null) {
					awayShield = pdesc.VisitorShield.Scale (50, 50).Value;
				} else {
					awayShield = Misc.LoadIcon ("longomatch-default-shield", 50);
				}
				
				projectsStore.AppendValues (false, homeShield, awayShield, pdesc.Description, pdesc);
			}
		}

		List<TeamTemplate> SelectedTeams {
			get {
				List<TeamTemplate> teams = new List<TeamTemplate> ();
				TreeIter iter;
				
				teamsStore.GetIterFirst (out iter);
				while (teamsStore.IterIsValid (iter)) {
					if ((bool)teamsStore.GetValue (iter, 0)) {
						teams.Add (teamsStore.GetValue (iter, 2) as TeamTemplate);
					}
					teamsStore.IterNext (ref iter);
				}
				return teams;
			}
		}

		List<EventType> SelectedEvents {
			get {
				List<EventType> eventTypes = new List<EventType> ();
				TreeIter iter;
				
				eventTypesStore.GetIterFirst (out iter);
				while (eventTypesStore.IterIsValid (iter)) {
					if ((bool)eventTypesStore.GetValue (iter, 0)) {
						eventTypes.Add (eventTypesStore.GetValue (iter, 2) as EventType);
					}
					eventTypesStore.IterNext (ref iter);
				}
				return eventTypes;
			}
		}

		void AddProject (Project project)
		{ 
			projects.Add (project.ID, project);
			AddTeam (project.LocalTeamTemplate);
			AddTeam (project.VisitorTeamTemplate);
			
			foreach (EventType evt in project.EventTypes) {
				AddEventType (evt);
			}
			panelheader1.ApplyVisible = true;
		}

		void RemoveProject (Project project)
		{
			projects.Remove (project.ID);
			RemoveTeam (project.LocalTeamTemplate);
			RemoveTeam (project.VisitorTeamTemplate);
			foreach (EventType evt in project.EventTypes) {
				RemoveEventType (evt);
			}
			if (projects.Count == 0) {
				panelheader1.ApplyVisible = false;
			}
		}

		void AddTeam (TeamTemplate template)
		{
			if (!teams.ContainsKey (template.ID)) {
				teams.Add (template.ID, template);
				teamsRefs.Add (template.ID, 1);
				teamsStore.AppendValues (true, template.TeamName, template);
			} else {
				teamsRefs [template.ID] ++;
			}
		}

		void RemoveTeam (TeamTemplate template)
		{
			teamsRefs [template.ID] --;
			
			if (teamsRefs [template.ID] == 0) {
				TreeIter first;

				teamsRefs.Remove (template.ID);
				teams.Remove (template.ID);
				teamsStore.GetIterFirst (out first);
				while (teamsStore.IterIsValid (first)) {
					TeamTemplate team = teamsStore.GetValue (first, 2) as TeamTemplate;
					if (team.ID == template.ID) {
						teamsStore.Remove (ref first);
						break;
					}
					teamsStore.IterNext (ref first);
				}
			}
		}

		void AddEventType (EventType evt)
		{
			if (evt.ID == Constants.SubsID) {
				return;
			} 
			if (!eventTypes.ContainsKey (evt.ID)) {
				eventTypes.Add (evt.ID, evt);
				eventTypesRefs.Add (evt.ID, 1);
				eventTypesStore.AppendValues (true, evt.Name, evt);
			} else {
				eventTypesRefs [evt.ID] ++;
			}
		}

		void RemoveEventType (EventType evt)
		{
			eventTypesRefs [evt.ID] --;
			
			if (eventTypesRefs [evt.ID] == 0) {
				TreeIter first;
				
				eventTypes.Remove (evt.ID);
				eventTypesRefs.Remove (evt.ID);
				eventTypesStore.GetIterFirst (out first);
				while (eventTypesStore.IterIsValid (first)) {
					EventType evnt = eventTypesStore.GetValue (first, 2) as EventType;
					if (evnt.ID == evt.ID) {
						eventTypesStore.Remove (ref first);
						break;
					}
					eventTypesStore.IterNext (ref first);
				}
			}
		}

		void HandleClicked (object sender, EventArgs e)
		{
			if (BackEvent != null)
				BackEvent ();
		}

		[GLib.ConnectBefore]
		void HandleButtonPressEvent (object o, ButtonPressEventArgs args)
		{
			TreeView treeview = (o as TreeView);
			TreePath path;
			TreeIter iter;
			bool selected;
			
			treeview.GetPathAtPos ((int)args.Event.X, (int)args.Event.Y, out path);
			if (path == null)
				return;
			treeview.Model.GetIter (out iter, path);
			selected = !(bool)treeview.Model.GetValue (iter, 0);
			treeview.Model.SetValue (iter, 0, selected);
			if (treeview == projectstreeview) {
				ProjectDescription pd = treeview.Model.GetValue (iter, 4) as ProjectDescription; 
				Project project = Config.DatabaseManager.ActiveDB.GetProject (pd.ID);
				if (selected) {
					AddProject (project);
				} else {
					RemoveProject (project);
				}
			}
		}

		void HandleApplyClicked (object sender, EventArgs e)
		{
			string msg = Catalog.GetString ("Creating presentation..."); 
			IBusyDialog dialog = Config.GUIToolkit.BusyDialog (msg, this);
			dialog.Show ();
			Presentation presentation = Presentation.CreateFromData (projects.Values.ToList (),
			                                                         SelectedTeams,
			                                                         SelectedEvents,
			                                                         dialog);
			dialog.Destroy ();
			Config.EventsBroker.EmitOpenPresentation (presentation);
		}

		void HandleProjectsSelected (List<ProjectDescription> pds)
		{
			foreach (ProjectDescription pd in pds) {
				if (!projects.ContainsKey (pd.ID)) {
					AddProject (Config.DatabaseManager.ActiveDB.GetProject (pd.ID));
				}
			}

			var ps = projects.Where (p => !pds.Select (pd => pd.ID).
			                         Contains (p.Key)).Select (p => p.Value).ToList (); 
			foreach (Project project in ps) {
				RemoveProject (project);
			}
		}
	}
}

