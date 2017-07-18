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
using LongoMatch.Core.Filters;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Core;

namespace LongoMatch.Gui.Component
{

	[System.ComponentModel.Category ("LongoMatch")]
	[System.ComponentModel.ToolboxItem (true)]
	public class PlayersFilterTreeView: FilterTreeViewBase
	{
		Team local, visitor;
		Player localTeam, visitorTeam;
		TreeIter localIter, visitorIter;
		EventsFilter filter;

		public PlayersFilterTreeView () : base ()
		{
			visitorTeam = new Player ();
			localTeam = new Player ();
			HeadersVisible = false;
		}

		public void SetFilter (EventsFilter filter, Project project)
		{
			this.local = project.LocalTeamTemplate;
			this.visitor = project.VisitorTeamTemplate;
			localTeam.Name = local.TeamName;
			visitorTeam.Name = visitor.TeamName;
			this.filter = filter;
			FillTree ();
		}

		public override void ToggleAll (bool active)
		{
			TreeIter current;
			store.GetIterFirst (out current);
			ToggleAll (current, active, false);
		}

		protected override void UpdateSelection (TreeIter iter, bool active)
		{
			TreeStore store = Model as TreeStore;
			var selected = store.GetValue (iter, COL_VALUE);

			/* Check all children */
			if (selected == local || selected == visitor) {
				TreeIter child;
				store.IterChildren (out child, iter);

				filter.IgnoreUpdates = true;
				while (store.IterIsValid (child)) {
					Player childPlayer = (Player)store.GetValue (child, COL_VALUE);
					FilterPlayer (childPlayer, active);
					store.SetValue (child, COL_ACTIVE, active);
					store.IterNext (ref child);
				}
				filter.IgnoreUpdates = false;
			} else {
				FilterPlayer (selected as Player, active);
				if (!active) {
					TreeIter team;
					/* Uncheck the team check button */
					if (local.List.Contains (selected as Player) || selected == localTeam)
						team = localIter;
					else
						team = visitorIter;
					store.SetValue (team, COL_ACTIVE, false);
				}
			}

			store.SetValue (iter, COL_ACTIVE, active);
			filter.Update ();
		}

		void FilterPlayer (Player player, bool active)
		{
			if (player == localTeam) {
				filter.FilterTeam (local, active);
			} else if (player == visitorTeam) {
				filter.FilterTeam (visitor, active);
			} else {
				filter.FilterPlayer (player as Player, active);
			}
		}

		void FillTree ()
		{
			localIter = store.AppendValues (localTeam.Name, false, local);
			visitorIter = store.AppendValues (visitorTeam.Name, false, visitor);

			filter.IgnoreUpdates = true;
			store.AppendValues (localIter, Catalog.GetString ("Team tagged"), false, localTeam);
			foreach (Player player in local.PlayingPlayersList) {
				store.AppendValues (localIter, player.ToString (), false, player);
			}
			
			store.AppendValues (visitorIter, Catalog.GetString ("Team tagged"), false, visitorTeam);
			foreach (Player player in visitor.PlayingPlayersList) {
				store.AppendValues (visitorIter, player.ToString (), false, player);
			}
			filter.IgnoreUpdates = false;
			filter.Update ();
		}


	}
}