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
using System.Linq;
using System.Collections.Generic;
using Gdk;
using Gtk;
using LongoMatch.Core.Common;
using LongoMatch.Core.Handlers;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Interfaces.GUI;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Gui.Helpers;
using Mono.Unix;
using Pango;
using LongoMatch.Gui.Dialog;

namespace LongoMatch.Gui.Panel
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class SportsTemplatesPanel : Gtk.Bin, IPanel
	{
		public event BackEventHandle BackEvent;

		Dashboard loadedDashboard;
		ICategoriesTemplatesProvider provider;
		DashboardsTreeView dashboardsTreeview;

		public SportsTemplatesPanel ()
		{
			this.Build ();
			provider = Config.CategoriesTemplatesProvider;

			// Assign images
			panelheader1.ApplyVisible = false;
			panelheader1.Title = Catalog.GetString ("ANALYSIS DASHBOARDS MANAGER");
			panelheader1.BackClicked += (sender, o) => {
				Save (true);
				if (BackEvent != null)
					BackEvent ();
			};

			templateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-header", 54);
			categoryheaderimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-category-header", 47);
			newtemplateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-add", 36);
			importtemplateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-import", 36);
			exporttemplateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-export", 36);
			deletetemplateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-delete", 36);
			savetemplateimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-template-save", 36);
			addcategoryimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-tag-category", 36);
			addtagimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-tag-tag", 36);
			scoreimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-tag-score", 36);
			cardimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-tag-card", 36);
			timerimage.Pixbuf = Helpers.Misc.LoadIcon ("longomatch-tag-timer", 36);
			vseparatorimage.Pixbuf = Helpers.Misc.LoadIcon ("vertical-separator", 54);

			// Connect buttons from the bar
			newtemplatebutton.Entered += HandleEnterTemplateButton;
			newtemplatebutton.Left += HandleLeftTemplateButton;
			importtemplatebutton.Entered += HandleEnterTemplateButton;
			importtemplatebutton.Left += HandleLeftTemplateButton;
			exporttemplatebutton.Entered += HandleEnterTemplateButton;
			exporttemplatebutton.Left += HandleLeftTemplateButton;
			deletetemplatebutton.Entered += HandleEnterTemplateButton;
			deletetemplatebutton.Left += HandleLeftTemplateButton;
			savetemplatebutton.Entered += HandleEnterTemplateButton;
			savetemplatebutton.Left += HandleLeftTemplateButton;

			addcategorybutton.Entered += HandleEnterTagButton;
			addcategorybutton.Left += HandleLeftTagButton;
			addcategorybutton.Clicked += (object sender, EventArgs e) =>
				buttonswidget.AddButton ("Category");
			addtagbutton1.Entered += HandleEnterTagButton;
			addtagbutton1.Left += HandleLeftTagButton;
			addtagbutton1.Clicked += (object sender, EventArgs e) =>
				buttonswidget.AddButton ("Tag");
			scorebutton.Entered += HandleEnterTagButton;
			scorebutton.Left += HandleLeftTagButton;
			scorebutton.Clicked += (object sender, EventArgs e) =>
				buttonswidget.AddButton ("Score");
			cardbutton.Entered += HandleEnterTagButton;
			cardbutton.Left += HandleLeftTagButton;
			cardbutton.Clicked += (object sender, EventArgs e) =>
				buttonswidget.AddButton ("Card");
			timerbutton.Entered += HandleEnterTagButton;
			timerbutton.Left += HandleLeftTagButton;
			timerbutton.Clicked += (object sender, EventArgs e) =>
				buttonswidget.AddButton ("Timer");

			templatesvbox.WidthRequest = 160;

			dashboardsTreeview = new DashboardsTreeView ();
			dashboardsTreeview.SelectionChanged += HandleSelectionChanged;
			scrolledwindow1.Add (dashboardsTreeview);
			dashboardsTreeview.Visible = true;

			buttonswidget.Sensitive = false;
			buttonswidget.ButtonsVisible = false;
			buttonswidget.Mode = DashboardMode.Edit;
			newtemplatebutton.Visible = true;
			deletetemplatebutton.Visible = false;
			
			newtemplatebutton.Clicked += HandleNewTemplateClicked;
			importtemplatebutton.Clicked += HandleImportTemplateClicked;
			exporttemplatebutton.Clicked += HandleExportTemplateClicked;
			deletetemplatebutton.Clicked += HandleDeleteTemplateClicked;
			savetemplatebutton.Clicked += (sender, e) => Save (false);
			
			editdashboardslabel.ModifyFont (FontDescription.FromString (Config.Style.Font + " 9"));
			editbuttonslabel.ModifyFont (FontDescription.FromString (Config.Style.Font + " 9"));

			dashboardsTreeview.Load (null);
		}

		protected override void OnDestroyed ()
		{
			buttonswidget.Destroy ();
			base.OnDestroyed ();
		}

		void SaveLoadedDashboard ()
		{
			if (loadedDashboard == null)
				return;
			if (!SaveTemplate (loadedDashboard)) {
				return;
			}
			dashboardsTreeview.UpdateLoadedTemplate ();
			buttonswidget.Edited = false;
		}

		bool SaveTemplate (Dashboard dashboard)
		{
			try {
				provider.Save (dashboard);
				return true;
			} catch (InvalidTemplateFilenameException ex) {
				Config.GUIToolkit.ErrorMessage (ex.Message, this);
				return false;
			}
		}

		void SaveStatic ()
		{
			string msg = Catalog.GetString ("System dashboards can't be edited, do you want to create a copy?");
			if (Config.GUIToolkit.QuestionMessage (msg, null, this).Result) {
				string newName;
				while (true) {
					newName = Config.GUIToolkit.QueryMessage (Catalog.GetString ("Name:"), null,
						loadedDashboard.Name + "_copy", this).Result;
					if (newName == null)
						break;
					if (dashboardsTreeview.Templates.Any (d => d.Name == newName)) {
						msg = Catalog.GetString ("A dashboard with the same name already exists"); 
						Config.GUIToolkit.ErrorMessage (msg, this);
					} else {
						break;
					}
				}
				if (newName == null) {
					return;
				}
				Dashboard newtemplate = loadedDashboard.Clone ();
				newtemplate.Name = newName;
				newtemplate.Static = false;
				if (SaveTemplate (newtemplate)) {
					dashboardsTreeview.Load (newtemplate.Name);
				}
			}
		}

		void Save (bool prompt)
		{
			if (loadedDashboard != null && buttonswidget.Edited) {
				if (loadedDashboard.Static) {
					/* prompt=false when we click the save button */
					if (!prompt) {
						SaveStatic ();
					}
				} else {
					string msg = Catalog.GetString ("Do you want to save the current dashboard");
					if (!prompt || Config.GUIToolkit.QuestionMessage (msg, null, this).Result) {
						SaveLoadedDashboard ();
					}
				}
			}
		}

		void HandleEnterTemplateButton (object sender, EventArgs e)
		{
			if (sender == newtemplatebutton) {
				editdashboardslabel.Markup = Catalog.GetString ("New dashboard");
			} else if (sender == importtemplatebutton) {
				editdashboardslabel.Markup = Catalog.GetString ("Import dashboard");
			} else if (sender == exporttemplatebutton) {
				editdashboardslabel.Markup = Catalog.GetString ("Export dashboard");
			} else if (sender == deletetemplatebutton) {
				editdashboardslabel.Markup = Catalog.GetString ("Delete dashboard");
			} else if (sender == savetemplatebutton) {
				editdashboardslabel.Markup = Catalog.GetString ("Save dashboard");
			}
		}

		void HandleLeftTemplateButton (object sender, EventArgs e)
		{
			editdashboardslabel.Markup = Catalog.GetString ("Manage dashboards");
		}

		void HandleEnterTagButton (object sender, EventArgs e)
		{
			if (sender == addcategorybutton) {
				editbuttonslabel.Markup = Catalog.GetString ("Add category button");
			} else if (sender == addtagbutton1) {
				editbuttonslabel.Markup = Catalog.GetString ("Add tag button");
			} else if (sender == scorebutton) {
				editbuttonslabel.Markup = Catalog.GetString ("Add score button");
			} else if (sender == timerbutton) {
				editbuttonslabel.Markup = Catalog.GetString ("Add timer button");
			} else if (sender == cardbutton) {
				editbuttonslabel.Markup = Catalog.GetString ("Add card button");
			}
		}

		void HandleLeftTagButton (object sender, EventArgs e)
		{
			editbuttonslabel.Markup = Catalog.GetString ("Manage dashboard buttons");
		}

		void HandleSelectionChanged (Dashboard dashboard)
		{
			deletetemplatebutton.Visible = dashboard != null;
			buttonswidget.Sensitive = dashboard != null;
			if (dashboard != null) {
				Save (true);
				deletetemplatebutton.Sensitive = !dashboard.Static;
				buttonswidget.Template = dashboard;
			}
			loadedDashboard = dashboard;
		}

		void HandleDeleteTemplateClicked (object sender, EventArgs e)
		{
			if (loadedDashboard != null) {
				string msg = Catalog.GetString ("Do you really want to delete the dashboard: ") + loadedDashboard.Name;
				if (MessagesHelpers.QuestionMessage (this, msg, null)) {
					provider.Delete (loadedDashboard);
					dashboardsTreeview.DeleteSelected ();
					dashboardsTreeview.Load (null);
				}
			}
		}

		void HandleImportTemplateClicked (object sender, EventArgs e)
		{
			string fileName, filterName;
			string[] extensions;

			Log.Debug ("Importing dashboard");
			filterName = Catalog.GetString ("Dashboard files");
			extensions = new [] { "*" + Constants.CAT_TEMPLATE_EXT };
			/* Show a file chooser dialog to select the file to import */
			fileName = Config.GUIToolkit.OpenFile (Catalog.GetString ("Import dashboard"), null, Config.HomeDir,
				filterName, extensions);

			if (fileName == null)
				return;

			try {
				Dashboard new_dashboard = provider.LoadFile (fileName);

				if (new_dashboard != null) {
					bool abort = false;

					while (provider.Exists (new_dashboard.Name) && !abort) {
						string name = Config.GUIToolkit.QueryMessage (Catalog.GetString ("Dashboard name:"),
							              Catalog.GetString ("Name conflict"), new_dashboard.Name + "#").Result;
						if (name == null) {
							abort = true;
						} else {
							new_dashboard.Name = name;
						}
					}

					if (!abort) {
						provider.Save (new_dashboard);
						dashboardsTreeview.AddTemplate (new_dashboard);
					}
				}
			} catch (Exception ex) {
				Config.GUIToolkit.ErrorMessage (Catalog.GetString ("Error importing template:") +
				"\n" + ex.Message);
				Log.Exception (ex);
				return;
			}
		}

		void HandleExportTemplateClicked (object sender, EventArgs e)
		{
			string fileName, filterName;
			string[] extensions;

			Log.Debug ("Exporting dashboard");
			filterName = Catalog.GetString ("Dashboard files");
			extensions = new [] { "*" + Constants.CAT_TEMPLATE_EXT };
			/* Show a file chooser dialog to select the file to export */
			fileName = Config.GUIToolkit.SaveFile (Catalog.GetString ("Export dashboard"),
				System.IO.Path.ChangeExtension (loadedDashboard.Name, Constants.CAT_TEMPLATE_EXT), Config.HomeDir,
				filterName, extensions);

			if (fileName != null) {
				bool succeeded = true;
				fileName = System.IO.Path.ChangeExtension (fileName, Constants.CAT_TEMPLATE_EXT);
				if (System.IO.File.Exists (fileName)) {
					string msg = Catalog.GetString ("A file with the same name already exists, do you want to overwrite it?");
					succeeded = Config.GUIToolkit.QuestionMessage (msg, null).Result;
				}

				if (succeeded) {
					Serializer.Instance.Save (loadedDashboard, fileName);
					string msg = Catalog.GetString ("Dashboard exported correctly");
					Config.GUIToolkit.InfoMessage (msg);
				}
			}
		}

		void HandleNewTemplateClicked (object sender, EventArgs e)
		{
			bool create = false;

			EntryDialog dialog = new EntryDialog (Toplevel as Gtk.Window);
			dialog.ShowCount = true;
			dialog.Title = dialog.Text = Catalog.GetString ("New dasboard");
			dialog.SelectText ();
			dialog.CountText = Catalog.GetString ("Event types:");
			dialog.AvailableTemplates = dashboardsTreeview.Templates.Select (d => d.Name).ToList ();
			
			while (dialog.Run () == (int)ResponseType.Ok) {
				if (dialog.Text == "") {
					MessagesHelpers.ErrorMessage (dialog, Catalog.GetString ("The dashboard name is empty."));
					continue;
				} else if (dialog.Text == dialog.SelectedTemplate) {
					/* The new template has the same name as the orignal one,
					 * just reload it as if we where copying it */
					dashboardsTreeview.Load (dialog.Text);
					break;
				} else if (provider.Exists (dialog.Text)) {
					var msg = Catalog.GetString ("The dashboard already exists. " +
					          "Do you want to overwrite it?");
					if (MessagesHelpers.QuestionMessage (this, msg)) {
						create = true;
						break;
					}
				} else {
					create = true;
					break;
				}
			}
			
			if (create) {
				Dashboard dashboard;

				if (dialog.SelectedTemplate != null) {
					try {
						dashboard = provider.Copy (dashboardsTreeview.Templates.FirstOrDefault (
							d => d.Name == dialog.SelectedTemplate), dialog.Text);
					} catch (InvalidTemplateFilenameException ex) {
						Config.GUIToolkit.ErrorMessage (ex.Message, this);
						dialog.Destroy ();
						return;
					}
				} else {
					dashboard = Dashboard.DefaultTemplate (dialog.Count);
					dashboard.Name = dialog.Text;
					if (!SaveTemplate (dashboard)) {
						dialog.Destroy ();
						return;
					}
				}
				dashboardsTreeview.AddTemplate (dashboard);
			}
			dialog.Destroy ();
		}
	}
}


