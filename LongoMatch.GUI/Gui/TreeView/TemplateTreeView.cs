//
//  Copyright (C) 2016 Fluendo S.A.
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
using Gdk;
using Gtk;
using LongoMatch.Core;
using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Gui;

namespace LongoMatch.Gui
{
	public delegate void TemplateChangedHandler<T> (T template);

	public class TemplateTreeView<T>: TreeView where T:ITemplate
	{
		public event TemplateChangedHandler<T> SelectionChanged;

		protected string defaultIcon, duplicateErrorMessage, loadErrorMessage;
		protected ITemplateProvider<T> provider;
		protected TreeViewColumn iconColumn;

		const int COL_PIXBUF = 0;
		const int COL_NAME = 1;
		const int COL_EDITABLE = 2;
		const int COL_TEMPLATE = 3;

		bool dragging, dragStarted;
		LongoMatch.Core.Common.Point start;
		TargetList targets;
		ListStore store;
		TreeIter selectedIter;
		T loadedTemplate;

		public TemplateTreeView ()
		{
			store = new ListStore (typeof(Pixbuf), typeof(string), typeof(bool), typeof(T));
			iconColumn = AppendColumn ("Icon", new CellRendererPixbuf (), "pixbuf", COL_PIXBUF);
			var cell = new CellRendererText { SizePoints = 14.0 };
			cell.Edited += HandleEdited;
			var col = AppendColumn ("Text", cell, "text", COL_NAME);
			col.AddAttribute (cell, "editable", COL_EDITABLE);

			SearchColumn = COL_NAME;
			EnableGridLines = TreeViewGridLines.None;
			HeadersVisible = false;
			Model = store;

			CursorChanged += HandleSelectionChanged;

			TargetEntry[] targetEntry = { new TargetEntry ("file/uri-list", TargetFlags.OtherApp, 0) };
			targets = new TargetList (targetEntry);
			EnableModelDragSource (ModifierType.None, targetEntry, DragAction.Default);
		}

		public List<T> Templates {
			get;
			protected set;
		}

		public T Selected {
			get {
				return loadedTemplate;
			}
		}

		public void AddTemplate (T template)
		{
			TreeIter iter = AddTemplateInt (template);
			Selection.SelectIter (iter);
			HandleSelectionChanged (null, null);
		}

		public void DeleteSelected ()
		{
			store.Remove (ref selectedIter);
			selectedIter = TreeIter.Zero;
			Selection.SelectPath (new TreePath ("0"));
			Templates.Remove (loadedTemplate);
		}

		public void UpdateLoadedTemplate ()
		{
			Model.SetValue (selectedIter, COL_TEMPLATE, loadedTemplate);
		}

		public void Load (T template, TreeIter iter)
		{
			loadedTemplate = template;
			selectedIter = iter;
		}

		public void Load (string templateName)
		{
			TreeIter templateIter = TreeIter.Zero;
			bool first = true;

			Templates = new List<T> ();
			store.Clear ();
			foreach (T template in provider.Templates) {
				TreeIter iter;

				iter = AddTemplateInt (template);
				if (first || template.Name == templateName) {
					templateIter = iter;
				}
				first = false;
			}
			if (store.IterIsValid (templateIter)) {
				Selection.SelectIter (templateIter);
				HandleSelectionChanged (null, null);
			}
		}

		TreeIter AddTemplateInt (T template)
		{
			Pixbuf img;
			string name;

			if (template.Image != null) {
				img = template.Image.Scale (50, 50).Value;
			} else {
				img = Helpers.Misc.LoadIcon (defaultIcon, 50);
			}

			name = template.Name;
			if (template.Static) {
				name += " (" + Catalog.GetString ("System") + ")";
			} else {
				Templates.Add (template);
			}
			return store.AppendValues (img, name, !template.Static, template);
		}

		protected override bool OnMotionNotifyEvent (EventMotion evnt)
		{
			if (dragging && !dragStarted) {
				if (start.Distance (new LongoMatch.Core.Common.Point (evnt.X, evnt.Y)) > 5) {
					Gtk.Drag.Begin (this, targets, DragAction.Default, 1, evnt);
					dragStarted = true;
				}
			}
			return base.OnMotionNotifyEvent (evnt);
		}

		protected override void OnDragEnd (DragContext context)
		{
			base.OnDragEnd (context);
		}

		protected override void OnDragDataGet (DragContext context, SelectionData selection_data, uint info, uint time)
		{
			base.OnDragDataGet (context, selection_data, info, time);
		}

		void HandleEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			store.GetIter (out iter, new TreePath (args.Path));

			T template = (T)store.GetValue (iter, COL_TEMPLATE);
			if (template.Name != args.NewText) {
				if (Templates.Any (d => d.Name == args.NewText)) {
					Config.GUIToolkit.ErrorMessage (Catalog.GetString (duplicateErrorMessage), this);
					args.RetVal = false;
				} else {
					try {
						template.Name = args.NewText;
						provider.Save (template);
						store.SetValue (iter, 1, args.NewText);
					} catch (Exception ex) {
						Config.GUIToolkit.ErrorMessage (ex.Message);
					}
				}
			}
		}

		void HandleSelectionChanged (object sender, EventArgs e)
		{
			T newTemplate;
			TreeIter iter;

			Selection.GetSelected (out iter);
			try {
				T dashboard = (T)store.GetValue (iter, COL_TEMPLATE);
				newTemplate = dashboard.Clone ();
			} catch (Exception ex) {
				Log.Exception (ex);
				Config.GUIToolkit.ErrorMessage (loadErrorMessage);
				return;
			}
			if (SelectionChanged != null) {
				SelectionChanged (newTemplate);
			}
			Load (newTemplate, iter);
		}
	}


	[System.ComponentModel.Category ("LongoMatch")]
	[System.ComponentModel.ToolboxItem (true)]
	public class TeamsTreeView: TemplateTreeView<Team>
	{

		public TeamsTreeView ()
		{
			defaultIcon = "longomatch-default-shield";
			duplicateErrorMessage = Catalog.GetString ("A team with the same name already exists");
			loadErrorMessage = Catalog.GetString ("Could not load team");
			provider = Config.TeamTemplatesProvider;
		}
	}

	[System.ComponentModel.Category ("LongoMatch")]
	[System.ComponentModel.ToolboxItem (true)]
	public class DashboardsTreeView: TemplateTreeView<Dashboard>
	{

		public DashboardsTreeView ()
		{
			defaultIcon = "longomatch";
			duplicateErrorMessage = Catalog.GetString ("A dashboard with the same name already exists");
			loadErrorMessage = Catalog.GetString ("Could not load dashboard");
			provider = Config.CategoriesTemplatesProvider;
			iconColumn.Visible = false;
		}
	}
}

