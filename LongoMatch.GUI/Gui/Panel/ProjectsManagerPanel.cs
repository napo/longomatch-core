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
using System.ComponentModel;
using System.Linq;
using Gtk;
using LongoMatch.Core.Store;
using LongoMatch.Gui.Component;
using LongoMatch.Services.State;
using LongoMatch.Services.ViewModel;
using Pango;
using VAS.Core;
using VAS.Core.Hotkeys;
using VAS.Core.Interfaces.GUI;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.UI.Helpers.Gtk2;
using Misc = VAS.UI.Helpers.Misc;
using VAS.UI;

namespace LongoMatch.Gui.Panel
{
	[System.ComponentModel.ToolboxItem (true)]
	[ViewAttribute (ProjectsManagerState.NAME)]
	public partial class ProjectsManagerPanel : Gtk.Bin, IPanel<SportsProjectsManagerVM>
	{
		SportsProjectsManagerVM viewModel;
		List<VideoFileInfo> videoFileInfos;
		ProjectsTreeView projectsTreeView;

		public ProjectsManagerPanel ()
		{
			this.Build ();
			projectsTreeView = new ProjectsTreeView ();
			projectsTreeView.Show ();
			projectsScrolledWindow.Add (projectsTreeView);

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

			panelheader1.Title = Title;
			panelheader1.ApplyVisible = false;
			panelheader1.BackClicked += async (sender, e) => await App.Current.StateController.MoveBack ();

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
				if (viewModel != null) {
					viewModel.LoadedProject.PropertyChanged -= HandleViewModelChanged;
				}
				viewModel = value;
				if (viewModel == null) {
					return;
				}
				viewModel.LoadedProject.PropertyChanged += HandleViewModelChanged;

				projectsTreeView.ViewModel = viewModel;

				savebutton.Bind (viewModel.SaveCommand, true);
				deletebutton.Bind (viewModel.DeleteCommand, null);
				exportbutton.Bind (viewModel.ExportCommand, null);
				openbutton.Bind (viewModel.OpenCommand, null);
				resyncbutton.Bind (viewModel.ResyncCommand, null);
				seasonentry.Bind (viewModel.LoadedProject, "Season");
				competitionentry.Bind (viewModel.LoadedProject, "Competition");
				desctextview.Bind (viewModel.LoadedProject, "Description");
				templatelabel.Bind (viewModel.LoadedProject, "DashboardText");
				datepicker.Bind (viewModel.LoadedProject, "MatchDate");
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
			gamedescriptionheader1.ProjectDescription = pd;

			foreach (VideoFileInfo vfi in videoFileInfos) {
				videofileinfo_vbox.Remove (vfi);
			}
			videoFileInfos.Clear ();

			int max = project.Description.FileSet.Count;
			// Cap to one media file for non multi camera version
			if (!App.Current.SupportsMultiCamera) {
				max = Math.Min (max, 1);
			}

			for (int i = 0; i < max; i++) {
				MediaFile mf = project.Description.FileSet [i];
				VideoFileInfo vfi = new VideoFileInfo ();

				vfi.SetMediaFileSet (project.Description.FileSet, mf);
				vfi.ShowAll ();

				videoFileInfos.Add (vfi);

				videofileinfo_vbox.PackStart (vfi, true, true, 0);
			}

			projectbox.Visible = true;
		}

		void HandleViewModelChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Model") {
				LoadProject (viewModel.LoadedProject.Model);
			}
		}

		void HandleProjectsSelected (List<ProjectLongoMatch> projects)
		{
			ViewModel.Selection.Replace (projects.Cast<SportsProjectVM> ());
		}
	}
}
