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
using System.Collections.Generic;
using System.Linq;
using LongoMatch.Drawing.CanvasObjects.Timeline;
using VAS.Core.Common;
using VAS.Core.Handlers;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.Store;
using VAS.Core.Store.Drawables;
using VAS.Core.ViewModel;
using VAS.Drawing;
using VAS.Drawing.CanvasObjects.Timeline;
using Timer = VAS.Core.Store.Timer;

namespace LongoMatch.Drawing.Widgets
{
	public class CamerasTimeline : SelectionCanvas, ICanvasView<ProjectVM>
	{
		public event ShowTimerMenuHandler ShowTimerMenuEvent;

		double secondsPerPixel;
		Time currentTime;

		List<TimelineView> timelines;
		ProjectVM viewModel;

		public CamerasTimeline (IWidget widget) : base (widget)
		{
			secondsPerPixel = 0.1;
			Accuracy = VAS.Drawing.Constants.TIMELINE_ACCURACY;
			SelectionMode = MultiSelectionMode.MultipleWithModifier;
			currentTime = new Time (0);
		}

		public CamerasTimeline () : this (null)
		{
		}

		public ProjectVM ViewModel {
			get {
				return viewModel;
			}
			set {
				viewModel = value;
				timelines = new List<TimelineView> ();
				ClearObjects ();
				FillCanvas ();
				widget?.ReDraw ();
			}
		}

		public PeriodsTimelineView PeriodsTimeline {
			get;
			set;
		}

		public Time CurrentTime {
			set {
				Area area;
				double start, stop;

				foreach (TimelineView tl in timelines) {
					tl.CurrentTime = value;
				}
				if (currentTime < value) {
					start = VAS.Drawing.Utils.TimeToPos (currentTime, SecondsPerPixel);
					stop = VAS.Drawing.Utils.TimeToPos (value, SecondsPerPixel);
				} else {
					start = VAS.Drawing.Utils.TimeToPos (value, SecondsPerPixel);
					stop = VAS.Drawing.Utils.TimeToPos (currentTime, SecondsPerPixel);
				}
				currentTime = value;
				if (widget != null) {
					area = new Area (new Point (start - 1, 0), stop - start + 2, widget.Height);
					widget.ReDraw (area);
				}
			}
		}

		public double SecondsPerPixel {
			set {
				secondsPerPixel = value;
				Update ();
			}
			get {
				return secondsPerPixel;
			}
		}

		public CameraView SelectedCamera {
			get {
				Selection sel = Selections.FirstOrDefault ();

				if (sel != null && sel.Drawable is CameraView)
					return sel.Drawable as CameraView;
				else
					return null;
			}
		}

		public void SetViewModel (object viewModel)
		{
			ViewModel = (ProjectVM)viewModel;
		}

		void Update ()
		{
			int height = 0;
			if (ViewModel == null)
				return;

			double width = ViewModel.FileSet.Duration.TotalSeconds / SecondsPerPixel + StyleConf.TimelinePadding;
			foreach (TimelineView tl in timelines) {
				tl.Width = width;
				tl.SecondsPerPixel = SecondsPerPixel;
				height += (int)tl.Height;
			}
			WidthRequest = (int)width;
			HeightRequest = height;
		}

		void AddTimeLine (TimelineView tl)
		{
			AddObject (tl);
			timelines.Add (tl);
		}

		void FillCanvas ()
		{
			int i = 0;
			// Add the timeline for periods
			PeriodsTimeline = new PeriodsTimelineView {
				ShowLine = true,
				Duration = ViewModel.FileSet.Duration,
				DraggingMode = NodeDraggingMode.All,
				Height = StyleConf.TimelineCameraHeight,
				OffsetY = 0,
				LineColor = App.Current.Style.PaletteBackgroundLight,
				BackgroundColor = App.Current.Style.PaletteBackground
			};
			PeriodsTimeline.ViewModel = ViewModel.Periods;
			AddTimeLine (PeriodsTimeline);
			i++;

			// Now add the timeline for the secondary cameras.
			// The main camera does not have a timeline since its the master camera and secondary cameras are synced
			// with respect of the main camera.
			foreach (MediaFileVM fileVM in ViewModel.FileSet.Skip (1)) {
				CameraTimelineView cameraTimeLine = new CameraTimelineView {
					ShowName = false,
					ShowLine = true,
					Height = StyleConf.TimelineCameraHeight,
					OffsetY = i * StyleConf.TimelineCameraHeight,
					LineColor = App.Current.Style.PaletteBackgroundLight,
					BackgroundColor = App.Current.Style.PaletteBackground,
				};
				cameraTimeLine.ViewModel = fileVM;
				AddTimeLine (cameraTimeLine);
				i++;
			}
			Update ();
		}

		protected override void StartMove (Selection sel)
		{
			if (sel == null || sel.Drawable as TimeNodeView == null)
				return;

			(sel.Drawable as TimeNodeView).ClippingMode = NodeClippingMode.NoStrict;

			if (sel.Position == SelectionPosition.All) {
				widget.SetCursor (CursorType.Selection);
			} else {
				widget.SetCursor (CursorType.DoubleArrow);
			}
		}

		protected override void StopMove (bool moved)
		{
			widget.SetCursor (CursorType.Arrow);
		}

		protected override void ShowMenu (Point coords)
		{
			if (ShowTimerMenuEvent != null &&
				coords.Y >= PeriodsTimeline.OffsetY &&
				coords.Y <= PeriodsTimeline.OffsetY + PeriodsTimeline.Height) {
				Timer t = null;
				if (Selections.Count > 0) {
					TimerTimeNodeView to = Selections.Last ().Drawable as TimerTimeNodeView;
					t = to.Timer.Model;
				}
				ShowTimerMenuEvent (t, VAS.Drawing.Utils.PosToTime (coords, SecondsPerPixel));
			}
		}
	}
}
