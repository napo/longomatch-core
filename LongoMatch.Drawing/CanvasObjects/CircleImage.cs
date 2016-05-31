//
//  Copyright (C) 2015 Fluendo S.A.
//
//
using System;
using VAS.Core.Interfaces.Drawing;
using LongoMatch.Core.Common;
using VAS.Core.Common;
using System.Collections.Generic;

namespace LongoMatch.Drawing.CanvasObjects
{
	/// <summary>
	/// An object that draws circular image image.
	/// </summary>
	public class CircleImage: FixedSizeCanvasObject
	{
		/// <summary>
		/// Gets or sets the image to display.
		/// </summary>
		public Image Image {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color of the background for the circle. This can be usefull when the image has
		/// transparencies, to draw a circle with this color and achieve the same circular effect.
		/// </summary>
		public Color BackgroundColor {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the backup text used when the image can't be loaded or is empty/null.
		/// </summary>
		public string BackupText {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the color of the text used when the backup character is in use.
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor {
			get;
			set;
		}

		public override void Draw (IContext context, IEnumerable<Area> areas)
		{
			context.Begin ();
			var center = new Point (Position.X + Width / 2, Position.Y + Height / 2);
			var radius = Math.Min (Width, Height) / 2;

			context.FillColor = BackgroundColor;
			context.StrokeColor = BackgroundColor;
			context.LineWidth = 0;

			if (Image != null) {
				context.DrawCircleImage (center, radius, Image);
			} else {
				context.DrawCircle (center, radius);
				if (!String.IsNullOrEmpty (BackupText)) {
					context.FontSize = (int)(radius * 1.3);
					context.FontWeight = FontWeight.Bold;
					context.FontAlignment = FontAlignment.Center;
					context.StrokeColor = TextColor;
					context.DrawText (Position, Width, Height, BackupText [0].ToString ());
				}
			}
			context.End ();
		}
	}
}

