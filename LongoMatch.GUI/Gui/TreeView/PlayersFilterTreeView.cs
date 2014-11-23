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
using Gdk;
using Mono.Unix;

using LongoMatch.Core.Common;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Core.Interfaces;

namespace LongoMatch.Gui.Component
{

	[System.ComponentModel.Category("LongoMatch")]
	[System.ComponentModel.ToolboxItem(true)]
	public class PlayersFilterTreeView: FilterTreeViewBase
	{
		const int PLAYER_COL = 1;
		const int TEAM_COL = 2;
		
		public PlayersFilterTreeView ()
		{
			HeadersVisible = false;
		}
		
		protected override void FillTree () {
			TreeStore store = new TreeStore (typeof (bool), typeof (Player), typeof (TeamTemplate));
			foreach (TeamTemplate team in project.Teams) {
				TreeIter iter = store.AppendValues (false, null, team);
				foreach (Player player in team.List) {
					filter.FilterPlayer (player, true);
					store.AppendValues (iter, true, player, team);
				}
			}
			Model = store;
		}
 
		
		protected override void UpdateSelection(TreeIter iter, bool active) {
			TreeStore store = Model as TreeStore;
			Player player = (Player) store.GetValue(iter, PLAYER_COL);
			
			/* Check all children */
			if (player == null)
			{
				TreeIter child;
				store.IterChildren(out child, iter);
				
				while (store.IterIsValid(child)) {
					Player childPlayer = (Player) store.GetValue(child, PLAYER_COL);
					filter.FilterPlayer (childPlayer, active);
					store.SetValue(child, ACTIVE_COL, active);
					store.IterNext(ref child);
				}
			} else {
				filter.FilterPlayer (player, active);
				if (!active) {
					TreeIter teamIter;
					
					store.IterParent (out teamIter, iter);
					store.SetValue(teamIter, ACTIVE_COL, false);
				}
			}
			
			store.SetValue(iter, ACTIVE_COL, active);
			filter.Update();
		}
		
		protected override void RenderColumn (TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			Player player = model.GetValue (iter, PLAYER_COL) as Player;
			if (player != null) {
				(cell as CellRendererText).Text = player.ToString ();
			} else {
				TeamTemplate team = model.GetValue (iter, TEAM_COL) as TeamTemplate;
				(cell as CellRendererText).Text = team.TeamName;
			}
		}
		
		protected override void Select (bool select_all)
		{
			TreeIter first;
			
			store.GetIterFirst (out first);
			while (store.IterIsValid (first)) {
				UpdateSelection (first, select_all);
				store.IterNext (ref first);
			}
		}
		
		protected override bool OnKeyPressEvent (Gdk.EventKey evnt)
		{
			return false;
		}


	}
}

