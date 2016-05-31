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
	public class CounterObject: CanvasDrawableObject<Counter>
	{

		public CounterObject ()
		{
		}

		public CounterObject (Counter counter)
		{
			Drawable = counter;
		}

		public override void Draw (IContext context, IEnumerable<Area> areas)
		{
			Area darea;
			
			darea = Drawable.Area;
			if (!UpdateDrawArea (context, areas, darea)) {
				return;
			}
			;
			context.Begin ();
			context.FillColor = Drawable.FillColor;
			context.StrokeColor = Drawable.StrokeColor;
			context.LineWidth = Drawable.LineWidth;
			context.DrawCircle (Drawable.Center, Drawable.Radius);
			context.StrokeColor = Drawable.TextColor;
			context.FontAlignment = FontAlignment.Center;
			context.FontSize = (int)Drawable.AxisX;
			context.DrawText (darea.Start, darea.Width, darea.Height,
				Drawable.Count.ToString ());
			DrawSelectionArea (context);
			context.End ();
		}
	}
}

