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
using VAS.Core.Interfaces.Drawing;
using VAS.Core.Store.Drawables;
using VAS.Core.Common;
using VAS.Core.Store;
using VAS.Core;
using LongoMatch.Core.Store;

namespace LongoMatch.Drawing.CanvasObjects.Dashboard
{
	public class TimerObject: DashboardButtonObject
	{
		Time currentTime;
		static Image iconImage;
		static Image cancelImage;
		Rectangle cancelRect;
		bool cancelPressed;

		public TimerObject (TimerButtonLongoMatch timer) : base (timer)
		{
			Button = timer;
			Toggle = true;
			CurrentTime = new Time (0);
			if (iconImage == null) {
				iconImage = Resources.LoadImage (StyleConf.ButtonTimerIcon);
			}
			if (cancelImage == null) {
				cancelImage = Resources.LoadImage (StyleConf.CancelButton);
			}
			MinWidth = StyleConf.ButtonMinWidth;
			MinHeight = iconImage.Height + StyleConf.ButtonTimerFontSize;
			cancelRect = new Rectangle ();
		}

		public TimerButtonLongoMatch Button {
			get;
			set;
		}

		public override Image Icon {
			get {
				return iconImage;
			}
		}

		public Time CurrentTime {
			set {
				bool update = false;
				bool wasActive = Active;
				bool isActive = wasActive;

				if (!wasActive) {
					if (Button.StartTime != null) {
						isActive = true;
						update = true;
					}
				} else {
					// In case the node is no longer valid, unactivate it
					if (Button.StartTime == null) {
						isActive = false;
						update = true;
					} else if (value < Button.StartTime) {
						// In case the application seeks to a position before the moment
						// the button was clicked, make sure to cancel such node
						Button.Cancel ();
						update = true;
						isActive = false;
					}
				}

				if (value != null && currentTime != null &&
				    currentTime.TotalSeconds != value.TotalSeconds) {
					update = true;
				}

				currentTime = value;

				if (update) {
					// It is possible that the button is activated but not thtough a click
					Active = isActive;
					ReDraw ();
				}
			}
			get {
				return currentTime;
			}
		}

		Time PartialTime {
			get {
				if (Button.StartTime == null) {
					return new Time (0);
				} else {
					return CurrentTime - Button.StartTime;
				}
			}
		}

		public Image TeamImage {
			get;
			set;
		}

		public override void ClickPressed (Point p, ButtonModifier modif)
		{
			cancelPressed = cancelRect.GetSelection (p) != null;
		}

		public override void ClickReleased ()
		{
			if (Mode == DashboardMode.Edit) {
				return;
			}
			base.ClickReleased ();
			if (Button.StartTime == null) {
				Log.Debug ("Start timer at " + CurrentTime.ToMSecondsString ());
				Button.Start (CurrentTime, null);
			} else {
				if (cancelPressed) {
					Log.Debug ("Cancel timer from button");
					Button.Cancel ();
				} else {
					Log.Debug ("Stop timer at " + CurrentTime.ToMSecondsString ());
					if (Button.StartTime.MSeconds != CurrentTime.MSeconds) {
						Button.Stop (CurrentTime, null);
					} else {
						Button.Cancel ();
					}
				}
			}
		}

		int HeaderHeight {
			get {
				return iconImage.Height + 5;
			}
		}

		int TextHeaderX {
			get {
				return iconImage.Width + 5 * 2;
			}
		}

		public override void Draw (IContext context, IEnumerable<Area> areas)
		{
			if (!UpdateDrawArea (context, areas, Area)) {
				return;
			}

			base.Draw (context, areas);

			context.Begin ();

			cancelRect = new Rectangle (
				new Point ((Position.X + Width) - StyleConf.ButtonRecWidth, Position.Y),
				StyleConf.ButtonRecWidth, HeaderHeight);

			if (Active && Mode != DashboardMode.Edit) {
				context.LineWidth = StyleConf.ButtonLineWidth;
				context.StrokeColor = Button.BackgroundColor;
				context.FillColor = Button.BackgroundColor;
				context.FontWeight = FontWeight.Normal;
				context.FontSize = StyleConf.ButtonHeaderFontSize;
				context.FontAlignment = FontAlignment.Left;
				context.DrawText (new Point (Position.X + TextHeaderX, Position.Y),
					Button.Width - TextHeaderX, iconImage.Height, Button.Timer.Name);
				context.FontWeight = FontWeight.Bold;
				context.FontSize = StyleConf.ButtonTimerFontSize;
				context.FontAlignment = FontAlignment.Center;
				context.DrawText (new Point (Position.X, Position.Y + iconImage.Height),
					Button.Width, Button.Height - iconImage.Height,
					PartialTime.ToSecondsString (), false, true);

				context.FillColor = context.StrokeColor = BackgroundColor;
				context.DrawRectangle (cancelRect.TopLeft, cancelRect.Width, cancelRect.Height);
				context.StrokeColor = TextColor;
				context.FillColor = TextColor;
				context.DrawImage (new Point (cancelRect.TopLeft.X, cancelRect.TopLeft.Y + 5),
					cancelRect.Width, cancelRect.Height - 10, cancelImage, ScaleMode.AspectFit, true);
			} else {
				Text = Button.Name;
				DrawText (context);
				Text = null;
			}
			
			if (TeamImage != null) {
				context.DrawImage (new Point (Position.X + Width - 40, Position.Y + 5), 40,
					iconImage.Height, TeamImage, ScaleMode.AspectFit);
			}
			context.End ();
		}
	}
}

