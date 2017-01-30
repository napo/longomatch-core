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
//

using System;
using System.Threading.Tasks;
using LongoMatch.Core;
using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using Microsoft.HockeyApp.Services;

namespace VAS.KPI.Services
{
	public class DeviceService : IDeviceService
	{
		IDevice device = new LMDevice ();

		public string GetDeviceModel ()
		{
			return "unknown";
		}

		public Task<string> GetDeviceType ()
		{
			return Task.FromResult ("unknown");
		}

		public string GetDeviceUniqueId ()
		{
			return device.ID.ToString ();
		}

		public string GetHostSystemLocale ()
		{
			return "unknown";
		}

		public int GetNetworkType ()
		{
			return 0;
		}

		public Task<string> GetOemName ()
		{
			return Task.FromResult ("unknown");
		}

		public string GetOperatingSystemName ()
		{
			return Utils.OS.ToString ();
		}

		public Task<string> GetOperatingSystemVersionAsync ()
		{
			return Task.FromResult (Utils.OSVersion);
		}
	}
}
