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
using VAS.Core.Common;
using VAS.Core.Handlers.Drawing;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.Store.Drawables;
using System.Collections.Generic;

namespace LongoMatch.Drawing.CanvasObjects
{
	public abstract class CanvasObject: ICanvasObject
	{
		public event CanvasHandler ClickedEvent;
		public event RedrawHandler RedrawEvent;

		bool disposed;
		bool highlighted;
		bool selected;

		protected CanvasObject ()
		{
			Visible = true;
		}

		~CanvasObject ()
		{
			if (!disposed) {
				Log.Error (String.Format ("Canvas object {0} not disposed correctly", this));
				Dispose (true);
			}
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			disposed = true;
		}

		public virtual string Description {
			get;
			set;
		}

		public bool Visible {
			get;
			set;
		}

		public virtual bool Highlighted {
			get {
				return highlighted;
			}
			set {
				bool changed = value != highlighted;
				highlighted = value;
				if (changed) {
					ReDraw ();
				}
			}
		}

		public virtual bool Selected {
			get {
				return selected;
			}
			set {
				bool changed = value != selected;
				selected = value;
				if (changed) {
					ReDraw ();
				}
			}
		}

		public virtual void ResetDrawArea ()
		{
			DrawArea = null;
		}

		public Area DrawArea {
			get;
			protected set;
		}

		public virtual void ReDraw ()
		{
			List<Area> areas = null;

			if (DrawArea != null) {
				areas = new List<Area> { DrawArea };
			}
			EmitRedrawEvent (this, areas);
		}

		public virtual void ClickPressed (Point p, ButtonModifier modif)
		{
		}

		public virtual void ClickReleased ()
		{
		}

		protected void EmitClickEvent ()
		{
			if (ClickedEvent != null) {
				ClickedEvent (this);
			}
		}

		protected void EmitRedrawEvent (CanvasObject co, IEnumerable<Area> areas)
		{
			if (RedrawEvent != null) {
				RedrawEvent (co, areas);
			}
		}

		protected bool NeedsRedraw (IEnumerable<Area> areas)
		{
			if (DrawArea == null) {
				return true;
			}
			foreach (Area area in areas) {
				if (area == null || area.IntersectsWith (DrawArea)) {
					return true;
				}
			}
			return false;
		}

		protected virtual bool UpdateDrawArea (IContext context, IEnumerable<Area> redrawAreas, Area drawArea)
		{
			if (NeedsRedraw (redrawAreas)) {
				DrawArea = context.UserToDevice (drawArea);
				return true;
			}
			return false;
		}

		public abstract void Draw (IContext context, IEnumerable<Area> areas);
	}

	/// <summary>
	/// An object that has a fixed size, which does not depend of any parameter other than the Width and Height set
	/// in its properties.
	/// </summary>
	public abstract class FixedSizeCanvasObject: CanvasObject
	{
		/// <summary>
		/// Gets or sets the position of the object.
		/// </summary>
		/// <value>The position.</value>
		public virtual Point Position {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the width of the object.
		/// </summary>
		/// <value>The width.</value>
		public virtual double Width {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the height of the object.
		/// </summary>
		/// <value>The height.</value>
		public virtual double Height {
			get;
			set;
		}

	}

	public abstract class CanvasButtonObject: FixedSizeCanvasObject
	{
		bool active;

		public bool Toggle {
			get;
			set;
		}

		public virtual bool Active {
			get {
				return active;
			}
			set {
				bool changed = active != value;
				active = value;
				if (changed) {
					ReDraw ();
				}
			}
		}

		public void Click ()
		{
			ClickPressed (new Point (Position.X + 1, Position.Y + 1),
				ButtonModifier.None);
			
			ClickReleased ();
		}

		public override void ClickPressed (Point p, ButtonModifier modif)
		{
			Active = !Active;
		}

		public override void ClickReleased ()
		{
			if (!Toggle) {
				Active = !Active;
			}
			EmitClickEvent ();
		}
	}

	public abstract class CanvasDrawableObject<T>: CanvasObject, ICanvasDrawableObject where T: IBlackboardObject
	{
		
		int selectionSize = 3;

		public IBlackboardObject IDrawableObject {
			get {
				return Drawable;
			}
			set {
				Drawable = (T)value;
			}
		}

		public T Drawable {
			get;
			set;
		}

		public override bool Selected {
			get {
				return Drawable.Selected;
			}
			set {
				bool changed = value != Drawable.Selected;
				Drawable.Selected = value;
				if (changed) {
					ReDraw ();
				}
			}
		}

		public Selection GetSelection (Point point, double precision, bool inMotion = false)
		{
			Selection sel = Drawable.GetSelection (point, precision, inMotion);
			if (sel != null) {
				sel.Drawable = this;
			}
			return sel;
		}

		public void Move (Selection s, Point p, Point start)
		{
			s.Drawable = Drawable;
			Drawable.Move (s, p, start);
			s.Drawable = this;
		}

		protected void DrawCornerSelection (IContext context, Point p)
		{
			context.StrokeColor = context.FillColor = Constants.SELECTION_INDICATOR_COLOR;
			context.LineStyle = LineStyle.Normal;
			context.LineWidth = 0;
			context.DrawRectangle (new Point (p.X - selectionSize,
				p.Y - selectionSize),
				selectionSize * 2, selectionSize * 2);
		}

		protected void DrawCenterSelection (IContext context, Point p)
		{
			context.StrokeColor = context.FillColor = Constants.SELECTION_INDICATOR_COLOR;
			context.LineWidth = 0;
			context.LineStyle = LineStyle.Normal;
			context.DrawCircle (p, selectionSize);
		}

		protected override bool UpdateDrawArea (IContext context, IEnumerable<Area> redrawAreas, Area drawArea)
		{
			if (NeedsRedraw (redrawAreas)) {
				DrawArea = context.UserToDevice (drawArea);
				DrawArea.Start.X -= selectionSize + 2;
				DrawArea.Start.Y -= selectionSize + 2;
				DrawArea.Width += selectionSize * 2 + 4;
				DrawArea.Height += selectionSize * 2 + 4;
				return true;
			} else {
				return false;
			}
		}

		protected void DrawSelectionArea (IContext context)
		{
			Area area;
			
			area = Drawable.Area;
			if (!Selected || area == null) {
				return;
			}
			context.StrokeColor = Constants.SELECTION_INDICATOR_COLOR;
			context.StrokeColor = Config.Style.PaletteActive;
			context.FillColor = null;
			context.LineStyle = LineStyle.Dashed;
			context.LineWidth = 2;
			context.DrawRectangle (area.Start, area.Width, area.Height);
			foreach (Point p in area.Vertices) {
				DrawCornerSelection (context, p);
			}
			foreach (Point p in area.VerticesCenter) {
				DrawCenterSelection (context, p);
			}
		}
	}
}