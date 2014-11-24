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
using LongoMatch.Core.Interfaces.GUI;
using LongoMatch.Core.Store;
using Gdk;
using Gtk;
using LongoMatch.Core.Common;

namespace LongoMatch.Gui.Component
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PresentationComponent : Gtk.Bin, IPresentationWindow
	{
		Pixbuf listIco, listActiveIco;
		Pixbuf playlistIco, playlistActiveIco;
		Pixbuf filtersIco, filtersActiveIco;
		int currentPage;

		public PresentationComponent ()
		{
			this.Build ();
			LoadIcons ();
			currentPage = 0;
			SetTabProps (leftnotebook, playslisttreewidget1, true);
			SetTabProps (rightnotebook, playlistwidget1, true);
			SetTabProps (rightnotebook, eventsscrolledwindow, false);
			SetTabProps (rightnotebook, teamsscrolledwindow, false);
			rightnotebook.SwitchPage += HandleSwitchPage;
		}

		public IPlayerBin Player {
			get {
				return playerbin1;
			}
		}

		public void Open (Presentation presentation, EventsFilter filter)
		{
			playslisttreewidget1.Project = presentation;
			playslisttreewidget1.Filter = filter;
			playlistwidget1.Project = presentation;
			categoriesfiltertreeview1.SetFilter (filter, presentation);
			playersfiltertreeview1.SetFilter (filter, presentation);
		}

		public void Close ()
		{
		
		}

		public void DetachPlayer ()
		{
		}

		void LoadIcons ()
		{
			int s = StyleConf.NotebookTabIconSize;
 
			listIco = Helpers.Misc.LoadIcon ("longomatch-tab-dashboard", s);
			listActiveIco = Helpers.Misc.LoadIcon ("longomatch-tab-active-dashboard", s);
			filtersIco = Helpers.Misc.LoadIcon ("longomatch-tab-filter", s);
			filtersActiveIco = Helpers.Misc.LoadIcon ("longomatch-tab-active-filter", s);
			playlistIco = Helpers.Misc.LoadIcon ("longomatch-tab-playlist", s);
			playlistActiveIco = Helpers.Misc.LoadIcon ("longomatch-tab-active-playlist", s);
		}

		void SetTabProps (Notebook notebook, Widget widget, bool active)
		{
			Gdk.Pixbuf icon;
			Gtk.Image img;

			img = notebook.GetTabLabel (widget) as Gtk.Image;
			if (img == null) {
				img = new Gtk.Image ();
				img.WidthRequest = StyleConf.NotebookTabSize;
				img.HeightRequest = StyleConf.NotebookTabSize;
				notebook.SetTabLabel (widget, img);
			}

			if (widget == playslisttreewidget1) {
				icon = active ? listActiveIco : listIco;
			} else if (widget == teamsscrolledwindow || widget == eventsscrolledwindow) {
				icon = active ? filtersActiveIco : filtersIco;
			} else if (widget == playlistwidget1) {
				icon = active ? playlistActiveIco : playlistIco;
			} else {
				return;
			}
			img.Pixbuf = icon;
		}

		void HandleSwitchPage (object o, SwitchPageArgs args)
		{
			Notebook notebook = o as Notebook;
			SetTabProps (notebook, notebook.GetNthPage (currentPage), false);
			SetTabProps (notebook, notebook.GetNthPage ((int)args.PageNum), true);
			currentPage = (int)args.PageNum;
		}
	}
}

