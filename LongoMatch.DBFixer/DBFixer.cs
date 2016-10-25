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
using System.IO;
using System.Linq;
using LongoMatch.Core.Common;
using VAS.Core.Interfaces;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using VAS.DB;
using VAS.Core.Serialization;
using LongoMatch.DB;
using VAS.Core.Store.Templates;
using VAS.Core.Common;

namespace LongoMatch.DBFixer
{
	public class DBFixer
	{
		static IStorage DB;
		static string dbPath;
		static string recoveryPath;

		//static public Registry DependencyRegistry = new Registry ("App Registry");

		public static void Main (string [] args)
		{
			InitDB ();
			RetrieveData ();
		}

		static void InitDB ()
		{
			Couchbase.Lite.Storage.SystemSQLite.Plugin.Register ();
			IStorageManager Manager;
			dbPath = Path.Combine (GetHomeDir (), "db");
			Manager = new CouchbaseManagerLongoMatch (dbPath);
			recoveryPath = Path.Combine (dbPath, "recovery");
			if (!Directory.Exists (recoveryPath)) {
				Directory.CreateDirectory (recoveryPath);
			}
			Manager.UpdateDatabases ();
			DB = Manager.Databases.FirstOrDefault ();

			//DependencyRegistry.Register<IStorageManager, CouchbaseManagerLongoMatch> (1);
			//Manager = DependencyRegistry.Retrieve<IStorageManager> (InstanceType.New, dbPath);
			//Manager.UpdateDatabases ();
			//DB = Manager.Databases.FirstOrDefault ();
		}

		static void RetrieveData ()
		{
			IEnumerable<ProjectLongoMatch> ProjectList = DB.RetrieveAll<ProjectLongoMatch> ();
			List<SportsTeam> retrievedTeams = new List<SportsTeam> ();
			List<Dashboard> retrievedDashboards = new List<Dashboard> ();

			foreach (ProjectLongoMatch project in ProjectList) {
				project.Load ();
				if (!retrievedTeams.Any (t => t.ID == project.LocalTeamTemplate.ID)) {
					SerializeObject (project.LocalTeamTemplate.Name, project.LocalTeamTemplate, Core.Common.Constants.TEAMS_TEMPLATE_EXT);
					retrievedTeams.Add (project.LocalTeamTemplate);
				}
				if (!retrievedTeams.Any (t => t.ID == project.VisitorTeamTemplate.ID)) {
					SerializeObject (project.VisitorTeamTemplate.Name, project.VisitorTeamTemplate, Core.Common.Constants.TEAMS_TEMPLATE_EXT);
					retrievedTeams.Add (project.VisitorTeamTemplate);
				}
				if (!retrievedDashboards.Any (t => t.ID == project.Dashboard.ID)) {
					SerializeObject (project.Dashboard.Name, project.Dashboard, Core.Common.Constants.CAT_TEMPLATE_EXT);
					retrievedDashboards.Add (project.Dashboard);
				}
			}
		}

		static void SerializeObject (string templateName, object template, string extension)
		{
			string fileName = Path.Combine (recoveryPath, templateName);
			fileName = Path.ChangeExtension (fileName, extension);
			Serializer.Instance.Save (template, fileName);
		}

		static string GetHomeDir ()
		{
			string home = null;
			home = Environment.GetEnvironmentVariable ("LONGOMATCH_HOME");
			if (home == null) {
				home = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			}
			return Path.Combine (home, Core.Common.Constants.SOFTWARE_NAME);
		}
	}
}
