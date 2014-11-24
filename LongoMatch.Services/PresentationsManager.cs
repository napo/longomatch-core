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
using LongoMatch.Core.Store;
using LongoMatch.Core.Interfaces.GUI;
using Mono.Unix;
using LongoMatch.Core.Handlers;
using LongoMatch.Core.Common;

namespace LongoMatch.Services
{
	public class PresentationsManager
	{
		Presentation openedPresentation;
		IPresentationWindow presentationWindow;
		
		public PresentationsManager ()
		{
			Config.EventsBroker.NewPresentationEvent += HandleNewPresentation;
			Config.EventsBroker.OpenPresentationEvent += HandleOpenPresentation;
			Config.EventsBroker.CloseOpenedProjectEvent += HandleCloseOpenedProject ;
			Config.EventsBroker.Detach += HandleDetach;
		}

		void HandleNewPresentation ()
		{
			if (Config.EventsBroker.EmitCloseOpenedProject ()) {
				Config.GUIToolkit.CreateNewPresentation ();
			}
		}

		void HandleOpenPresentation (Presentation presentation)
		{
			if (Config.EventsBroker.EmitCloseOpenedProject ()) {
				EventsFilter eventsFilter = new EventsFilter (presentation);
				Config.GUIToolkit.OpenPresentation (presentation, eventsFilter, out presentationWindow);
				Config.EventsBroker.EmitOpenedProjectChanged (presentation, ProjectType.PresentationProject,
				                                              eventsFilter, presentationWindow);
			}
		}
		
		void HandleCloseOpenedProject (RetEventArgs args)
		{
			if (openedPresentation != null) {
				bool ret;
				ret = Config.GUIToolkit.QuestionMessage (
					Catalog.GetString ("Do you want to close the current project?"), null);
				if (ret) {
					openedPresentation = null;
					presentationWindow.Close ();
					presentationWindow = null;
				} else {
					args.ReturnValue = false;
				}
			}
		}
		
		void HandleDetach ()
		{
			if (presentationWindow != null) {
				presentationWindow.DetachPlayer ();
			}
		}

		
	}
}

