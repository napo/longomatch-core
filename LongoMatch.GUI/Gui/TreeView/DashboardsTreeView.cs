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
using LongoMatch.Core;
using LongoMatch.Core.Store.Templates;

namespace LongoMatch.Gui
{
	[System.ComponentModel.Category ("LongoMatch")]
	[System.ComponentModel.ToolboxItem (true)]
	public class DashboardsTreeView: TemplateTreeView<Dashboard>
	{

		public DashboardsTreeView ()
		{
			defaultIcon = "longomatch";
			duplicateErrorMessage = Catalog.GetString ("A dashboard with the same name already exists");
			loadErrorMessage = Catalog.GetString ("Could not load dashboard");
		}
	}

}

