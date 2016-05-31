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
using VAS.Core.Common;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.Store.Drawables;

namespace LongoMatch.Drawing.CanvasObjects.Blackboard
{
	public class LineObject: CanvasDrawableObject<Line>
	{
		public LineObject ()
		{
		}

		public LineObject (Line line)
		{
			Drawable = line;
		}

		public override void Draw (IContext context, IEnumerable<Area> areas)
		{
			if (!UpdateDrawArea (context, areas, Drawable.Area)) {
				return;
			}

			context.Begin ();
			context.FillColor = Drawable.FillColor;
			context.StrokeColor = Drawable.StrokeColor;
			context.LineWidth = Drawable.LineWidth;
			context.LineStyle = Drawable.Style;
			context.DrawLine (Drawable.Start, Drawable.Stop);
			context.LineStyle = LineStyle.Normal;
			if (Drawable.Type == LineType.Arrow ||
			    Drawable.Type == LineType.DoubleArrow) {
				context.DrawArrow (Drawable.Start, Drawable.Stop, 5 * Drawable.LineWidth / 2, 0.3, true);
			}
			if (Drawable.Type == LineType.DoubleArrow) {
				context.DrawArrow (Drawable.Stop, Drawable.Start, 5 * Drawable.LineWidth / 2, 0.3, true);
			}
			if (Drawable.Type == LineType.Dot ||
			    Drawable.Type == LineType.DoubleDot) {
				context.DrawPoint (Drawable.Stop);
			}
			if (Drawable.Type == LineType.DoubleDot) {
				context.DrawPoint (Drawable.Start);
			}
			
			if (Selected) {
				DrawCornerSelection (context, Drawable.Start);
				DrawCornerSelection (context, Drawable.Stop);
			}
			context.End ();
		}
	}
}

