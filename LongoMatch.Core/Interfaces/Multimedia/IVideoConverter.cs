// ICapturer.cs
//
//  Copyright (C) 2007-2009 Andoni Morales Alastruey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301, USA.
//
//

using System;
using Gdk;

using LongoMatch.Core.Common;
using LongoMatch.Core.Handlers;

namespace LongoMatch.Core.Interfaces.Multimedia
{


	public interface IVideoConverter
	{
		event ErrorHandler Error;
		event ProgressHandler Progress;

		void Start();

		void Cancel();
		
		void AddFile(string filename, long duration, uint width, uint height, double par);
		
		EncodingSettings EncodingSettings {set;}
		
	}
}
