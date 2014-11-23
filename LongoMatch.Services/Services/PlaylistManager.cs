// 
//  Copyright (C) 2011 Andoni Morales Alastruey
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
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using LongoMatch.Core.Common;
using LongoMatch.Core.Interfaces;
using LongoMatch.Core.Interfaces.GUI;
using LongoMatch.Core.Store;
using LongoMatch.Core.Store.Playlists;
using Mono.Unix;
using Timer = System.Threading.Timer;
using System;

namespace LongoMatch.Services
{
	public class PlaylistManager
	{
		IGUIToolkit guiToolkit;
		IPlayerBin player;
		IRenderingJobsManager videoRenderer;
		IProject openedProject;
		ProjectType openedProjectType;
		IPlaylistElement loadedElement;
		Playlist loadedPlaylist;
		TimelineEvent loadedPlay;
		EventsFilter filter;

		public PlaylistManager (IGUIToolkit guiToolkit, IRenderingJobsManager videoRenderer)
		{
			this.videoRenderer = videoRenderer;
			this.guiToolkit = guiToolkit;
			BindEvents ();
		}

		void BindEvents ()
		{
			Config.EventsBroker.NewPlaylistEvent += HandleNewPlaylist;
			Config.EventsBroker.AddPlaylistElementEvent += HandleAddPlaylistElement;
			Config.EventsBroker.RenderPlaylist += HandleRenderPlaylist;
			Config.EventsBroker.OpenedProjectChanged += HandleOpenedProjectChanged;
			Config.EventsBroker.PreviousPlaylistElementEvent += HandlePrev;
			Config.EventsBroker.NextPlaylistElementEvent += HandleNext;
			Config.EventsBroker.LoadEventEvent += HandleLoadPlayEvent;
			Config.EventsBroker.PlaylistElementSelectedEvent += HandlePlaylistElementSelected;
			Config.EventsBroker.PlaybackRateChanged += HandlePlaybackRateChanged;
			Config.EventsBroker.TimeNodeChanged += HandlePlayChanged;
			Config.EventsBroker.SeekEvent += HandleSeekEvent;
			Config.EventsBroker.TogglePlayEvent += HandleTogglePlayEvent;
			Config.EventsBroker.KeyPressed += HandleKeyPressed;
		}

		void LoadPlay (TimelineEvent play, Time seekTime, bool playing)
		{
			play.Selected = true;
			player.LoadPlay (play.FileSet, play, seekTime, playing);
			loadedPlay = play;
			if (playing) {
				player.Play ();
			}
		}
		
		void Switch (TimelineEvent play, Playlist playlist, IPlaylistElement element)
		{
			if (loadedElement != null) {
				loadedElement.Selected = false;
			}
			if (loadedPlay != null) {
				loadedPlay.Selected = false;
			}

			loadedPlay = play;
			loadedPlaylist = playlist;
			loadedElement = element;

			if (element != null) {
				element.Selected = true;
			}
			if (play != null) {
				play.Selected = true;
			}
		}

		void HandlePlayChanged (TimeNode tNode, object val)
		{
			/* FIXME: Tricky, create a new handler for categories */
			if (tNode is TimelineEvent && val is Time) {
				LoadPlay (tNode as TimelineEvent, val as Time, false);
			}
			filter.Update ();
		}
		
		void HandleOpenedProjectChanged (Project project, ProjectType projectType,
		                                 EventsFilter filter, IAnalysisWindow analysisWindow)
		{
			openedProject = project;
			openedProjectType = projectType;
			if (project != null) {
				player = analysisWindow.Player;
				this.filter = filter;
			}
		}

		void HandlePlaylistElementSelected (Playlist playlist, IPlaylistElement element)
		{
			Switch (null, playlist, element);
			if (element != null) {
				playlist.SetActive (element);
			}
			player.LoadPlayListPlay (playlist, element);
		}

		void HandleLoadPlayEvent (TimelineEvent play)
		{
			if (play is SubstitutionEvent || play is LineupEvent) {
				Switch (null, null, null);
				Config.EventsBroker.EmitEventLoaded (null);
				player.Seek (play.EventTime, true);
				player.Play ();
			} else {
				Switch (play, null, null);
				if (play != null) {
					LoadPlay (play, play.Start, true);
				} else {
					player.CloseSegment ();
				}
				Config.EventsBroker.EmitEventLoaded (play);
			}
		}

		void HandleNext (Playlist playlist)
		{
			if (playlist != null && playlist.HasNext ()) {
				Config.EventsBroker.EmitPlaylistElementSelected (playlist, playlist.Next());
			}
		}
		
		void HandlePrev (Playlist playlist)
		{
			/* Select the previous element if it's a regular play */
			if (playlist == null && loadedPlay != null) {
				player.Seek (loadedPlay.Start, true);
				return;
			}
			
			if (loadedElement != null) {
				/* Select the previous element if we haven't played 500ms */
				if (loadedElement is PlaylistPlayElement) {
					TimelineEvent play = (loadedElement as PlaylistPlayElement).Play;
					if ((player.CurrentTime - play.Start).MSeconds > 500) {
						player.Seek (play.Start, true);
						return;
					}
				}
				/* Load the next playlist element */
				if (playlist.HasPrev ()) {
					Config.EventsBroker.EmitPlaylistElementSelected (playlist, playlist.Prev());
				}
			}
		}
		
		void HandlePlaybackRateChanged (float rate)
		{
			if (loadedElement != null && loadedElement is PlaylistPlayElement) {
				(loadedElement as PlaylistPlayElement).Rate = rate;
			} else if (loadedPlay != null) {
				loadedPlay.Rate = rate;
			}
		}

		void HandleAddPlaylistElement (Playlist playlist, List<IPlaylistElement> element)
		{
			if (playlist == null) {
				playlist = HandleNewPlaylist (openedProject);
				if (playlist == null) {
					return;
				}
			}
			playlist.Elements.AddRange (element);
			Config.EventsBroker.EmitPlaylistsChanged (this);
		}

		Playlist HandleNewPlaylist (IProject project)
		{
			string name;
			Playlist playlist = null;
			
			name = Config.GUIToolkit.QueryMessage (Catalog.GetString ("Playlist name:"), null,
			                                       Catalog.GetString ("New playlist"));
			if (name != null) {
				playlist = new Playlist {Name = name};
				project.Playlists.Add (playlist);
				Config.EventsBroker.EmitPlaylistsChanged (this);
			}
			return playlist;
		}

		void HandleRenderPlaylist (Playlist playlist)
		{
			List<EditionJob> jobs = guiToolkit.ConfigureRenderingJob (playlist);
			if (jobs == null)
				return;
			foreach (Job job in jobs)
				videoRenderer.AddJob (job);
		}
		
		void HandleSeekEvent (Time pos, bool accurate)
		{
			if (player != null) {
				player.Seek (pos, accurate);
			}
		}
		
		void HandleTogglePlayEvent (bool playing)
		{
			if (player != null) {
				if (playing) {
					player.Play ();
				} else {
					player.Pause ();
				}
			}
		}
		
		void HandleKeyPressed (object sender, HotKey key)
		{
			if (openedProject == null)
				return;

			if (openedProject is Presentation || openedProjectType == ProjectType.FileProject) {
				KeyAction action;
				if (player == null)
					return;

				try {
					action = Config.Hotkeys.ActionsHotkeys.GetKeyByValue (key);
				} catch (Exception ex) {
					/* The dictionary contains 2 equal values for different keys */
					Log.Exception (ex);
					return;
				}
				
				if (action == KeyAction.None) {
					return;
				}

				switch (action) {
				case KeyAction.FrameUp:
					player.SeekToNextFrame ();
					return;
				case KeyAction.FrameDown:
					player.SeekToPreviousFrame ();
					return;
				case KeyAction.JumpUp:
					player.StepForward ();
					return;
				case KeyAction.JumpDown:
					player.StepBackward ();
					return;
				case KeyAction.DrawFrame:
					TimelineEvent evt = loadedPlay;
					if (evt == null && loadedElement is PlaylistPlayElement) {
						evt = (loadedElement as PlaylistPlayElement).Play;
					}
					if (evt != null) {
						Config.EventsBroker.EmitDrawFrame (evt, -1, player.ActiveAngle, true);
					}
					return;
				case KeyAction.TogglePlay:
					player.TogglePlay ();
					return;
				case KeyAction.SpeedUp:
					player.FramerateUp ();
					return;
				case KeyAction.SpeedDown:
					player.FramerateDown ();
					return;
				case KeyAction.CloseEvent:
					Config.EventsBroker.EmitLoadEvent (null);
					return;
				case KeyAction.Prev:
					HandlePrev (loadedPlaylist);
					return;
				case KeyAction.Next:
					HandleNext (loadedPlaylist);
					return;
				}
			} else {
				//if (Capturer == null)
				//	return;
			}
		}
		

	}
}
