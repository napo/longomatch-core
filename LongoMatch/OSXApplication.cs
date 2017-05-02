//
//  Copyright (C) 2017 Andoni Morales Alstruey
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

//#if OSTYPE_OSX
#if true

using AppKit;
using Gtk;
using LongoMatch.Core.Events;
using LongoMatch.Gui;
using VAS.Core.Common;

namespace LongoMatch
{
	/// <summary>
	/// Handles all the OSX's application specific setup, such as menus and openning associated files
	/// with double-click or open-with.
	/// </summary>
	class OSXApplication : NSApplicationDelegate
	{
		static OSXApplication instance;
		GtkOSXApplication app;
		bool inited, ready;

		OSXApplication ()
		{
		}

		public static OSXApplication Instance {
			get {
				// This single implementation is different because NSApplication.Init () must be called before
				// any NSObject is created.
				if (instance == null) {
					NSApplication.Init ();
					instance = new OSXApplication ();
				}
				return instance;
			}
		}

		/// <summary>
		/// Initializes the application, this should be called as soon as possible be after initialization
		/// GTK, otherwise the NSApplication.SharedInstance is <c>null</c>.
		/// </summary>
		public void Init ()
		{
			if (inited) {
				return;
			}
			inited = true;
			app = new GtkOSXApplication ();
			NSApplication.SharedApplication.Delegate = new OSXApplication ();
		}

		/// <summary>
		/// Updates the OSX menu and notifies that application is ready. The OpenFile events are reaceived
		/// right after calling this function.
		/// This function must be called after all services are initialzied.
		/// </summary>
		public void Ready ()
		{
			if (ready) {
				return;
			}
			ready = true;
			MainWindow window = App.Current.GUIToolkit.MainController as MainWindow;
			window.QuitMenu.Visible = false;
			window.Menu.Visible = false;

			// GtkOSXApplication is still useful to bind the menu's, for the rest we use AppKit
			app.SetMenuBar (window.Menu);
			app.InsertAppMenuItem (window.AboutMenu, 0);
			app.InsertAppMenuItem (new SeparatorMenuItem (), 1);
			app.InsertAppMenuItem (window.PreferencesMenu, 2);
			app.UseQuartzAccelerators = false;

			NSApplication.SharedApplication.FinishLaunching ();
		}

		public override bool OpenFile (NSApplication sender, string filename)
		{
			// For some unknown reason the first file is the LongoMatch.exe binary, just ignore it
			if (filename.EndsWith ("LongoMatch.exe")) {
				return true;
			}
			Log.Verbose ($"Application requested to open file {filename}");
			App.Current.EventsBroker.Publish (new OpenFileEvent { FilePath = filename });
			return true;
		}

		public override NSApplicationTerminateReply ApplicationShouldTerminate (NSApplication sender)
		{
			Log.Verbose ($"Application requested to close");
			MainWindow window = App.Current.GUIToolkit.MainController as MainWindow;
			if (window.CloseAndQuit ().Result) {
				return NSApplicationTerminateReply.Now;
			}
			return NSApplicationTerminateReply.Cancel;
		}
	}
}
#endif