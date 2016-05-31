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
using LongoMatch.Core.Store;
using VAS.Core.Common;
using VAS.Core.Interfaces.Drawing;

namespace LongoMatch.Drawing.CanvasObjects.Dashboard
{
	public class CardObject: TimedTaggerObject
	{
		public CardObject (PenaltyCardButton card) : base (card)
		{
			Button = card;
		}

		public PenaltyCardButton Button {
			get;
			set;
		}

		public override void Draw (IContext context, IEnumerable<Area> areas)
		{
			Color front, back;
			int width;

			if (!UpdateDrawArea (context, areas, Area)) {
				return;
			}
			context.Begin ();

			if (Active) {
				context.LineWidth = StyleConf.ButtonLineWidth;
				context.StrokeColor = BackgroundColor;
				context.FillColor = TextColor;
			} else {
				context.LineWidth = 0;
				context.StrokeColor = TextColor;
				context.FillColor = BackgroundColor;
			}

			/* Draw Shape */
			switch (Button.PenaltyCard.Shape) {
			case CardShape.Rectangle:
				context.DrawRoundedRectangle (Button.Position, Button.Width, Button.Height, 3);
				break;
			case CardShape.Circle:
				context.DrawCircle (new Point (Button.Position.X + Button.Width / 2,
					Button.Position.Y + Button.Height / 2),
					Math.Min (Button.Width, Button.Height) / 2);
				break;
			case CardShape.Triangle:
				context.DrawTriangle (new Point (Button.Position.X + Button.Width / 2, Button.Position.Y),
					Button.Width, Button.Height, SelectionPosition.Top);
				break;
			}

			/* Draw header */
			context.LineWidth = 2;
			context.FontSize = StyleConf.ButtonNameFontSize;
			context.FontWeight = FontWeight.Light;
			context.FontAlignment = FontAlignment.Center;
			if (Recording) {
				context.DrawText (Position, Button.Width, Button.Height, (CurrentTime - Start).ToSecondsString ());
			} else {
				context.DrawText (Position, Button.Width, Button.Height, Button.PenaltyCard.Name);
			}
			DrawSelectionArea (context);
			if (ShowLinks) {
				GetAnchor (null).Draw (context, areas);
			}
			context.End ();
		}
	}
}

