//
//  Copyright (C) 2016 Fluendo S.A.
using System;
using VAS.Drawing.CanvasObjects;
using VAS.Core.Store.Drawables;
using VAS.Core.Interfaces.Drawing;
using System.Linq;
using VAS.Core.Common;

namespace LongoMatch.Drawing.CanvasObjects.Blackboard
{
	public class AreaObject : CanvasDrawableObject<MultiPoints>
	{
		public AreaObject ()
		{
		}

		public AreaObject (MultiPoints area)
		{
			Drawable = area;
		}

		public override void Draw (IDrawingToolkit tk, Area area)
		{
			if (!UpdateDrawArea (tk, area, Drawable.Area)) {
				return;
			}

			tk.Begin ();
			tk.FillColor = Drawable.FillColor;
			tk.StrokeColor = Drawable.StrokeColor;
			tk.LineWidth = Drawable.LineWidth;
			tk.LineStyle = Drawable.Style;
			tk.DrawArea (Drawable.Points.ToArray ());
			DrawSelectionArea (tk);
			tk.End ();
		}
	}
}
