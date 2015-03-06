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
using System.Collections.Generic;
using Mono.Unix;
using LongoMatch.Core.Store;
using LongoMatch.Core.Common;
using Gtk;

namespace LongoMatch.Gui.Component
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class MediaFileSetSelection : Gtk.Bin
	{
		MediaFileSet fileSet;
		List<MediaFileChooser> fileChoosers;
		bool editable = true;

		public MediaFileSetSelection ()
		{
			this.Build ();

			fileChoosers = new List<MediaFileChooser> ();
		}

		public MediaFileSet FileSet {
			set {
				fileSet = value;

				if (fileSet.Count > 0) {
					editable = false;

					// Create all choosers
					foreach (MediaFile mf in fileSet) {
						AddMediaFileChooser (mf.Name);
					}
				} else {
					// Add the first media file chooser for main camera
					AddMediaFileChooser (Catalog.GetString ("Main camera angle"));
				}
			}
			get {
				return fileSet;
			}
		}

		void AddMediaFileChooser (String name)
		{
			Alignment alignment = new Alignment (0.0f, 0.5f, 0.0f, 0.0f);
			MediaFileChooser chooser = new MediaFileChooser (name);

			chooser.ChangedEvent += HandleFileChangedEvent;
			alignment.Add (chooser);
			alignment.ShowAll ();

			mfss_vbox.PackStart (alignment, true, false, 0);

			fileChoosers.Add (chooser);
		}

		/// <summary>
		/// Here we just map 1 to 1 from the choosers list to our set. We make sure to use Replace method on the set
		/// to preserve important metadata.
		/// </summary>
		void UpdateFileSet ()
		{
			for (var i = 0; i < fileChoosers.Count; i++) {
				MediaFile mf = fileChoosers [i].MediaFile;
				if (mf != null) {
					fileSet.Replace (fileSet [i], mf);	
				}
			}
		}

		/// <summary>
		/// Editable FileSet will just clear the set and recreate from the choosers. It will also figure out
		/// if some choosers should be removed or added to allow more files to come in.
		/// </summary>
		void UpdateEditableFileSet ()
		{
			bool have_empty_chooser = false;
			List<MediaFileChooser> to_remove = new List<MediaFileChooser> ();

			fileSet.Clear ();

			foreach (MediaFileChooser chooser in fileChoosers) {
				if (chooser.MediaFile != null) {
					fileSet.Add (chooser.MediaFile);
				} else {
					if (!have_empty_chooser) {
						have_empty_chooser = true;
					} else {
						// Mark for removal as we only want one empty file chooser at most
						to_remove.Add (chooser);
					}
				}
			}

			foreach (MediaFileChooser chooser in to_remove) {
				chooser.ChangedEvent -= HandleFileChangedEvent;
				fileChoosers.Remove (chooser);
				mfss_vbox.Remove (chooser.Parent);
			}

			to_remove.Clear ();

			if (!have_empty_chooser) {
				AddMediaFileChooser (String.Format ("{0} {1}", Catalog.GetString ("Angle"), fileChoosers.Count));
			}
		}

		void HandleFileChangedEvent (object sender, EventArgs e)
		{
			if (editable) {
				UpdateEditableFileSet ();
			} else {
				UpdateFileSet ();
			}
		}
	}
}

