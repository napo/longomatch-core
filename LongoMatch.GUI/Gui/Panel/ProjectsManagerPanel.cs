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
using LongoMatch.Core.Events;
using LongoMatch.Core.Store;
using LongoMatch.Gui.Component;
using LongoMatch.Services.State;
using LongoMatch.Services.ViewModel;
using Pango;
using VAS.Core;
using VAS.Core.Common;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces;
using VAS.Core.Interfaces.GUI;
using VAS.Core.MVVMC;
using VAS.Core.Serialization;
using VAS.Core.Store;
using Constants = LongoMatch.Core.Common.Constants;
using Helpers = VAS.UI.Helpers;
using Misc = VAS.UI.Helpers.Misc;
using VAS.UI.Helpers.Gtk2;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace LongoMatch.Gui.Panel
{
	[System.ComponentModel.ToolboxItem (true)]
	[ViewAttribute (ProjectsManagerState.NAME)]
	public partial class ProjectsManagerPanel : Gtk.Bin, IPanel<SportsProjectsManagerVM>
	{
		SportsProjectsManagerVM viewModel;
		ProjectLongoMatch loadedProject;
		List<ProjectLongoMatch> selectedProjects;
		List<VideoFileInfo> videoFileInfos;
		IStorage DB;
		IGUIToolkit gkit;
		bool edited;

		public ProjectsManagerPanel ()
		{
			this.DB = App.Current.DatabaseManager.ActiveDB;
			this.gkit = App.Current.GUIToolkit;
			this.Build ();

			this.videoFileInfos = new List<VideoFileInfo> ();

			savebuttonimage.Pixbuf = Misc.LoadIcon ("longomatch-save", 34);
			exportbuttonimage.Pixbuf = Misc.LoadIcon ("longomatch-export", 34);
			resyncbuttonimage.Pixbuf = Misc.LoadIcon ("longomatch-project-resync", 34);
			deletebuttonimage.Pixbuf = Misc.LoadIcon ("longomatch-delete", 34);
			openbuttonimage.Pixbuf = Misc.LoadIcon ("longomatch-open", 34);

			// Force tooltips to be translatable as there seems to be a bug in stetic 
			// code generation for translatable tooltips.
			savebutton.TooltipMarkup = Catalog.GetString ("Save");
			exportbutton.TooltipMarkup = Catalog.GetString ("Export");
			openbutton.TooltipMarkup = Catalog.GetString ("Open");
			deletebutton.TooltipMarkup = Catalog.GetString ("Delete");

			notebook1.ShowTabs = false;
			notebook1.ShowBorder = false;
			projectlistwidget1.SelectionMode = SelectionMode.Multiple;
			projectlistwidget1.ProjectsSelected += HandleProjectsSelected;

			resyncbutton.Clicked += HandleResyncClicked;
			datepicker.ValueChanged += HandleDateChanged;

			notebook1.Page = 0;
			panelheader1.Title = Title;
			panelheader1.ApplyVisible = false;
			panelheader1.BackClicked += HandleBackClicked;

			projectlistwidget1.ViewMode = ProjectListViewMode.List;

			// Only visible when multi camera is supported. Indeed periods can be edited in the timeline of the project.
			resyncbutton.Visible = App.Current.SupportsMultiCamera;

			SetStyle ();
		}

		protected override void OnDestroyed ()
		{
			OnUnload ();
			base.OnDestroyed ();
		}

		public override void Dispose ()
		{
			Destroy ();
			base.Dispose ();
		}

		public string Title {
			get {
				return Catalog.GetString ("PROJECTS MANAGER");
			}
		}

		public SportsProjectsManagerVM ViewModel {
			set {
				viewModel = value;
				projectlistwidget1.Fill (viewModel.Model.ToList ());
				viewModel.PropertyChanged += HandleViewModelChanged;
				viewModel.LoadedProject.PropertyChanged += HandleLoadedProjectChanged;
				savebutton.Bind (viewModel.SaveCommand, true);
				deletebutton.Bind (viewModel.DeleteCommand, null);
				exportbutton.Bind (viewModel.ExportCommand, null);
				openbutton.Bind (viewModel.OpenCommand, null);
				resyncbutton.Bind (viewModel.ResyncCommand, null);
				seasonentry.Bind (viewModel, "Season");
				competitionentry.Bind (viewModel, "Competition");
				desctextview.Bind (viewModel, "Description");
			}
			get {
				return viewModel;
			}
		}

		public void OnLoad ()
		{

		}

		public void OnUnload ()
		{

		}

		public KeyContext GetKeyContext ()
		{
			return new KeyContext ();
		}

		public void SetViewModel (object viewModel)
		{
			ViewModel = (SportsProjectsManagerVM)viewModel;
		}

		void SetStyle ()
		{
			FontDescription desc = FontDescription.FromString (App.Current.Style.Font + " 18");
			infoeventbox.ModifyBg (StateType.Normal, Misc.ToGdkColor (App.Current.Style.PaletteBackgroundDark));
			infolabel.ModifyFg (StateType.Normal, Misc.ToGdkColor (App.Current.Style.PaletteText));
			infolabel.ModifyFont (desc);
			videoseventbox.ModifyBg (StateType.Normal, Misc.ToGdkColor (App.Current.Style.PaletteBackgroundDark));
			videoslabel.ModifyFg (StateType.Normal, Misc.ToGdkColor (App.Current.Style.PaletteText));
			videoslabel.ModifyFont (desc);
		}

		void LoadProject (ProjectLongoMatch project)
		{
			ProjectDescription pd = project.Description;

			loadedProject = null;
			gamedescriptionheader1.ProjectDescription = pd;
			seasonentry.Text = pd.Season;
			competitionentry.Text = pd.Competition;
			datepicker.Date = pd.MatchDate;
			templatelabel.Text = pd.DashboardName;
			desctextview.Buffer.Clear ();
			desctextview.Buffer.InsertAtCursor (pd.Description ?? "");
			loadedProject = project;

			foreach (VideoFileInfo vfi in videoFileInfos) {
				videofileinfo_vbox.Remove (vfi);
			}
			videoFileInfos.Clear ();

			resyncbutton.Sensitive = project.Description.FileSet.Count > 1;

			int max = project.Description.FileSet.Count;
			// Cap to one media file for non multi camera version
			if (!App.Current.SupportsMultiCamera) {
				max = Math.Min (max, 1);
			}

			for (int i = 0; i < max; i++) {
				MediaFile mf = project.Description.FileSet [i];
				VideoFileInfo vfi = new VideoFileInfo ();

				vfi.SetMediaFileSet (project.Description.FileSet, mf);
				vfi.Changed += HandleChanged;

				vfi.ShowAll ();

				videoFileInfos.Add (vfi);

				videofileinfo_vbox.PackStart (vfi, true, true, 0);
			}

			projectbox.Visible = true;
			edited = false;
		}

		void HandleBackClicked (object sender, EventArgs e)
		{
			if (notebook1.Page == 0) {
				App.Current.StateController.MoveBack ();
			} else {
				projectperiods1.Pause ();
				/* FIXME: we don't support adding new cameras, so there is nothing
				 * to fix or update */
				//projectperiods1.SaveChanges (false);

				// We need to reload project details
				LoadProject (loadedProject);
				// And remember that the project has changed
				edited = true;

				notebook1.Page--;
			}
		}

		void HandleViewModelChanged (object sender, PropertyChangedEventArgs e)
		{
		}

		void HandleLoadedProjectChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Model") {
				LoadProject (viewModel.LoadedProject.Model);
			}
		}

		void HandleProjectsSelected (List<ProjectLongoMatch> projects)
		{
			ViewModel.Selection.Replace (projects.Cast<SportsProjectVM> ());
		}

		void HandleResyncClicked (object sender, EventArgs e)
		{
			bool canNavigate = true;
			if (!loadedProject.Description.FileSet.CheckFiles ()) {
				// Show message in order to load video.
				canNavigate = gkit.SelectMediaFiles (loadedProject.Description.FileSet);
			}

			if (canNavigate) {
				notebook1.Page = 1;

				// Load data in the project periods widget.
				projectperiods1.Project = loadedProject;
			}
		}

		void HandleDateChanged (object sender, EventArgs e)
		{
			if (loadedProject == null)
				return;

			loadedProject.Description.MatchDate = datepicker.Date;
			edited = true;
		}

	}
}
