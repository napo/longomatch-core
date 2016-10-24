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
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using LongoMatch.DB;

namespace LongoMatch.DBFixer
{
	public class DBFixer
	{
		static IStorage DB;
		static string dbPath;
		static string recoveryPath;

		public static void Main (string [] args)
		{
			InitDB ();
			RetrieveData ();
		}

		static void InitDB ()
		{
			//Couchbase.Lite.Storage.SystemSQLite.Plugin.Register ();
			IStorageManager DatabaseManager;
			IStorageManager Manager;
			dbPath = Path.Combine (GetHomeDir (), "db");
			Manager = new CouchbaseManager (dbPath);
			recoveryPath = Path.Combine (dbPath, "recovery");
			if (!Directory.Exists (recoveryPath)) {
				Directory.CreateDirectory (recoveryPath);
			}
			DatabaseManager = Manager;
			Manager.UpdateDatabases ();
			DB = Manager.Databases.FirstOrDefault ();
		}

		static void RetrieveData ()
		{
			IEnumerable<Project> ProjectList = DB.RetrieveAll<Project> ();
			List<Team> retrievedTeams = new List<Team> ();
			List<Dashboard> retrievedDashboards = new List<Dashboard> ();

			foreach (Project project in ProjectList) {
				project.Load ();
				if (!retrievedTeams.Any (t => t.ID == project.LocalTeamTemplate.ID)) {
					SerializeObject (project.LocalTeamTemplate.Name, project.LocalTeamTemplate, Constants.TEAMS_TEMPLATE_EXT);
				}
				if (!retrievedTeams.Any (t => t.ID == project.VisitorTeamTemplate.ID)) {
					SerializeObject (project.VisitorTeamTemplate.Name, project.VisitorTeamTemplate, Constants.TEAMS_TEMPLATE_EXT);
				}
				if (!retrievedDashboards.Any (t => t.ID == project.Dashboard.ID)) {
					SerializeObject (project.Dashboard.Name, project.Dashboard, Constants.CAT_TEMPLATE_EXT);
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
			return Config.homeDirectory = Path.Combine (home, Constants.SOFTWARE_NAME);
		}
	}
}
