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
using System.IO;
using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Store.Templates;
using LongoMatch.DB;
using LongoMatch.Core.Store;

namespace LongoMatch.Services
{
	/// <summary>
	/// This services monitors a directory in the filesystem looking for <see cref="Dashboard"/>
	/// <see cref="Team"/> or <see cref="Project"/> and imports them into the database.
	/// </summary>
	public class ImportMonitorServices: IService
	{
		IDirectoryMonitor monitor;
		string directoryPath;

		public ImportMonitorServices (string directoryPath, IDirectoryMonitor monitor)
		{
			this.monitor = monitor;
			DirectoryPath = directoryPath;
			ImportTeams = true;
			ImportDashboards = true;
			ImportProjects = true;
		}

		public bool ImportProjects {
			get;
			set;
		}

		public bool ImportDashboards {
			get;
			set;
		}

		public bool ImportTeams {
			get;
			set;
		}

		public string DirectoryPath {
			get {
				return directoryPath;
			}
			set {
				directoryPath = value;
			}
		}

		#region IService implementation

		public int Level {
			get {
				return 1;
			}
		}

		public string Name {
			get {
				return "Import monitor";
			}
		}

		public bool Start ()
		{
			if (!Directory.Exists (DirectoryPath)) {
				return false;
			}
			monitor.DirectoryPath = DirectoryPath;
			monitor.FileChangedEvent += HandleFileChangedEvent;
			foreach (string path in Directory.GetFiles (DirectoryPath)) {
				HandleFileChangedEvent (FileChangeType.Created, path);
			}
			monitor.Start ();
			return true;
		}

		public bool Stop ()
		{
			monitor.FileChangedEvent -= HandleFileChangedEvent;
			monitor.Stop ();
			return false;
		}

		#endregion

		void HandleFileChangedEvent (FileChangeType eventType, string path)
		{
			if (eventType == FileChangeType.Created) {
				string ext = Path.GetExtension (path);
				try {
					if (ImportTeams && ext == Constants.TEAMS_TEMPLATE_EXT) {
						Team team = FileStorage.RetrieveFrom<Team> (path);
						Config.TeamTemplatesProvider.Add (team);
					} else if (ImportDashboards && ext == Constants.CAT_TEMPLATE_EXT) {
						Dashboard team = FileStorage.RetrieveFrom<Dashboard> (path);
						Config.CategoriesTemplatesProvider.Add (team);
					} else if (ImportProjects && ext == Constants.PROJECT_EXT) {
						Project project = FileStorage.RetrieveFrom<Project> (path);
						Config.DatabaseManager.ActiveDB.Store<Project> (project, true);
					}
					File.Delete (path);
				} catch (Exception ex) {
					Log.Exception (ex);
				}
			}
		}

	}
}

