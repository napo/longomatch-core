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
using Gtk;
using LongoMatch.Core.Store.Templates;
using System.Collections.Generic;
using LongoMatch.Core.Store;
using EventType = LongoMatch.Core.Store.EventType;
using Misc = LongoMatch.Gui.Helpers.Misc;
using System.Collections.Specialized;


namespace LongoMatch.Gui.Component
{
	[System.ComponentModel.ToolboxItem (true)]
	/// <summary>
	/// A treeview to reorder event types with drag and drop.
	/// </summary>
	public class EventTypesTreeview :TreeView
	{

		ListStore store;
		Dashboard dashboard;
		bool ignoreUpdates;

		public EventTypesTreeview ()
		{
			HeadersVisible = false;
			ShowExpanders = false;
			Reorderable = true;
			
			TreeViewColumn custColumn = new TreeViewColumn ();
			CellRenderer cr = new PlaysCellRenderer ();
			custColumn.PackStart (cr, true);
			custColumn.SetCellDataFunc (cr, RenderElement); 
			AppendColumn (custColumn);
			store = new ListStore (typeof(EventType));
			Model = store;
		}

		public Dashboard Dashboard {
			set {
				if (dashboard != null) {
					dashboard.List.CollectionChanged -= DashboardChanged;
				}
				dashboard = value;
				if (dashboard != null) {
					dashboard.List.CollectionChanged += DashboardChanged;
				}
				UpdateDahsboard ();
			}
			get {
				return dashboard;
			}
		}

		void UpdateDahsboard ()
		{
			store.Clear ();
			foreach (DashboardButton button in dashboard.List) {
				if (button is EventButton) {
					store.AppendValues (button);
				}
			}
		}

		protected override bool OnDragDrop (Gdk.DragContext context, int x, int y, uint time)
		{
			TreeIter iter;
			TreePath path;
			TreeViewDropPosition pos;

			Selection.GetSelected (out iter);
			var srcButton = store.GetValue (iter, 0) as EventButton;

			if (GetDestRowAtPos (x, y, out path, out pos)) {
				ignoreUpdates = true;
				var destButton = store.GetValue (path, 0) as EventButton;
				Dashboard.List.Remove (srcButton);
				int index = Dashboard.List.IndexOf (destButton);
				if (pos == TreeViewDropPosition.After || pos == TreeViewDropPosition.IntoOrAfter) {
					index++;
				}
				Dashboard.List.Insert (index, srcButton);
				ignoreUpdates = false;
			}
			return base.OnDragDrop (context, x, y, time);
		}

		protected void RenderElement (TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			EventButton item = model.GetValue (iter, 0) as EventButton;
			PlaysCellRenderer c = cell as PlaysCellRenderer;
			c.Item = item.EventType;
			c.Count = 0;
		}

		void DashboardChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!ignoreUpdates) {
				UpdateDahsboard ();
			}
		}
	}
}
