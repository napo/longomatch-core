// 
//  Copyright (C) 2012 Andoni Morales Alastruey
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
using Mono.Unix;

using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;

namespace LongoMatch.Gui.Component
{

    [System.ComponentModel.Category("LongoMatch")]
	[System.ComponentModel.ToolboxItem(true)]
	public class CategoriesFilterTreeView: FilterTreeViewBase
	{
		Project project;
		
		public CategoriesFilterTreeView (): base()
		{
			firstColumnName = Catalog.GetString("Category");
			HeadersVisible = false;
		}
		
		public override void SetFilter (EventsFilter filter, Project project) {
			this.project = project;
			base.SetFilter(filter, project);
		}
		
		protected override void FillTree ()
		{
			store = new TreeStore (typeof(object), typeof(bool));
			
			foreach (EventType evType in project.EventTypes) {
				TreeIter catIter;
				
				catIter = store.AppendValues (evType, true);
				filter.FilterEventType (evType, true);

				if (evType is AnalysisEventType) {
					foreach (Tag tag in (evType as AnalysisEventType).Tags) {
						store.AppendValues(catIter, tag, false);
					}
				}
			}
			Model = store;
		}
		
		void UpdateSelectionPriv(TreeIter iter, bool active, bool checkParents=true, bool recurse=true) {
			TreeIter child, parent;
			
			object o = store.GetValue(iter, 0);
			store.IterParent (out parent, iter);
			
			if (o is Tag) {
				EventType evType = store.GetValue (parent, 0) as EventType;
				filter.FilterEventTag (evType, o as Tag, active);
			} else {
				filter.FilterEventType (o as EventType, active);
			}
			store.SetValue(iter, 1, active);
			
			/* Check its parents */
			if (active && checkParents && store.IterIsValid (parent)) {
				UpdateSelectionPriv (parent, active, true, false);
			}
			
			/* Check/Uncheck all children */
			if (recurse) {
				store.IterChildren(out child, iter);
				while (store.IterIsValid(child)) {
					UpdateSelectionPriv (child, active, false, true);
					store.IterNext(ref child);
				}
			}
			
			if (recurse && checkParents)
				filter.Update();
		}
		
		protected override void UpdateSelection(TreeIter iter, bool active) {
			UpdateSelectionPriv (iter, active, true, true);
		}
 
		protected override void RenderColumn (TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			object obj = store.GetValue(iter, 0);
			string text = "";
			
			if (obj is EventType) {
				EventType evType = obj as EventType;
				text = evType.Name;
			}
			else if (obj is Tag){
				text = (obj as Tag).Value;
			}
			
			(cell as CellRendererText).Text = text;
		}
		
		protected override void Select(bool select_all) {
			TreeIter iter;
			
			filter.Silent = true;
			store.GetIterFirst(out iter);
			while (store.IterIsValid(iter)){
				UpdateSelection(iter, select_all);
				store.IterNext(ref iter);
			}
			filter.Silent = false;
			filter.Update ();
		}
		
		protected override bool OnKeyPressEvent (Gdk.EventKey evnt)
		{
			return false;
		}
	}
}

