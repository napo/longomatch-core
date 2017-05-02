//
//  Copyright (C) 2017 Andoni Morales Alastruey
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
using LongoMatch.Core.Events;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Core.ViewModel;
using LongoMatch.Services.State;
using LongoMatch.Services.States;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.Store.Templates;
using VAS.DB;

namespace LongoMatch.Services.Service
{
	public class FileOpenerService : IService
	{
		IFileStorage storage;

		public FileOpenerService ()
		{
		}

		#region IService implementation

		public int Level {
			get {
				return 1;
			}
		}

		public string Name {
			get {
				return "File opener";
			}
		}

		public bool Start ()
		{
			storage = App.Current.DependencyRegistry.Retrieve<IFileStorage> (InstanceType.Default, null);
			App.Current.EventsBroker.Subscribe<OpenFileEvent> (HandleOpenFile);
			return true;
		}

		public bool Stop ()
		{
			App.Current.EventsBroker.Unsubscribe<OpenFileEvent> (HandleOpenFile);
			storage = null;
			return false;
		}

		#endregion

		void HandleOpenFile (OpenFileEvent obj)
		{
			string fileExtension = Path.GetExtension (obj.FilePath);

			Log.Information ($"Requested to open file {obj.FilePath} with ImportOnly: {obj.ImportOnly}");


			// FIXME: This could more flexible if we could register IFileImporter in the dependency service with
			// file extensions associated to it.
			if (fileExtension == Core.Common.Constants.TEAMS_TEMPLATE_EXT) {
				LMTeam team = storage.RetrieveFrom<LMTeam> (obj.FilePath);
				App.Current.TeamTemplatesProvider.Add (team);
				if (!obj.ImportOnly) {
					App.Current.StateController.MoveTo (DashboardsManagerState.NAME, team);
				}
			} else if (fileExtension == Core.Common.Constants.CAT_TEMPLATE_EXT) {
				Dashboard team = storage.RetrieveFrom<Dashboard> (obj.FilePath);
				if (!obj.ImportOnly) {
					App.Current.StateController.MoveTo (TeamsManagerState.NAME, team);
				}
				App.Current.CategoriesTemplatesProvider.Add (team as LMDashboard);
			} else if (fileExtension == Core.Common.Constants.PROJECT_EXT) {
				LMProject project = storage.RetrieveFrom<LMProject> (obj.FilePath);
				App.Current.DatabaseManager.ActiveDB.Store<LMProject> (project, true);
				Log.Information ($"Stored file {obj.FilePath}");
				LMProjectVM projectVM = new LMProjectVM { Model = project };
				projectVM.ProjectType = ProjectType.FileProject;
				LMStateHelper.OpenProject (projectVM);
				if (!obj.ImportOnly) {
					try {
						LMStateHelper.OpenProject (new LMProjectVM { Model = project });
					} catch (Exception ex) {
						Log.Exception (ex);
					}
				}
			}

		}

	}
}
