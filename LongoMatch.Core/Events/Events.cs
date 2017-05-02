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
using LongoMatch.Core.Common;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Templates;
using VAS.Core.Common;
using VAS.Core.Events;
using VAS.Core.Interfaces.GUI;
using VAS.Core.Store;

namespace LongoMatch.Core.Events
{
	public class OpenProjectIDEvent : Event
	{
		public Guid ProjectID { get; set; }

		public LMProject Project { get; set; }
	}

	public class ImportProjectEvent : Event
	{
	}

	/// <summary>
	/// Event to request openning a file in the application.
	/// </summary>
	public class OpenFileEvent : Event
	{
		/// <summary>
		/// Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
		public string FilePath { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:LongoMatch.Core.Events.OpenFileEvent"/>
		/// will only import the file if it can be imported and it will not open it. For example, if a user
		/// double clicks in a Team file, it will import the team and open it in the teams editor by default.
		/// In case of a service monitoring a directory, it will only import the file.
		/// </summary>
		/// <value><c>true</c> if import only; otherwise, <c>false</c>.</value>
		public bool ImportOnly { get; set; }
	}

	public class ExportProjectEvent : Event
	{
		public LMProject Project { get; set; }
	}

	public class ManageJobsEvent : Event
	{
	}

	public class ManageDatabasesEvent : Event
	{
	}

	public class MigrateDBEvent : Event
	{
	}

	public class ShowProjectStatsEvent : Event
	{
		public Project Project { get; set; }
	}

	public class PlayerSubstitutionEvent : Event
	{
		public LMTeam Team { get; set; }

		public LMPlayer Player1 { get; set; }

		public LMPlayer Player2 { get; set; }

		public SubstitutionReason SubstitutionReason { get; set; }

		public Time Time { get; set; }
	}

	public class TeamTagsChangedEvent : Event
	{
	}

	public class QueryToolsEvent : Event
	{
		public List<ITool> Tools { get; set; }
	}
}

