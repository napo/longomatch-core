//
//  Copyright (C) 2017 FLuendo
//
//
using System;
using LongoMatch.Core.Store.Templates;
using LongoMatch.Core.ViewModel;
using LongoMatch.Drawing.Widgets;
using VAS.Core.Common;
using VAS.Core.Interfaces;
using VAS.Core.Interfaces.Drawing;
using VAS.Core.Store.Templates;

namespace LongoMatch.Services
{
	/// <summary>
	/// Template preview generator.
	/// </summary>
	public class PreviewService : IPreviewCreator, IService
	{
		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <value>The level.</value>
		public int Level => 10;

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name => nameof (PreviewService);

		/// <summary>
		/// Generates the dashboard preview.
		/// </summary>
		/// <returns>The dashboard preview.</returns>
		/// <param name="dashboard">Dashboard.</param>
		/// <param name="canvas">Canvas.</param>
		public Image CreateDashboardPreview (Dashboard dashboard)
		{
			return null;
		}

		/// <summary>
		/// Generates the team preview.
		/// </summary>
		/// <returns>The team preview.</returns>
		/// <param name="team">Team.</param>
		/// <param name="canvas">Canvas.</param>
		public Image CreateTeamPreview (Team team)
		{
			// load viewmodel and create the view to extract the image 
			LMTeamTaggerVM taggerVM = new LMTeamTaggerVM { HomeTeam = new LMTeamVM { Model = team as LMTeam } };
			taggerVM.AwayTeam = null;
			taggerVM.Background = App.Current.HHalfFieldBackground;

			LMTeamTaggerView taggerView = new LMTeamTaggerView ();
			taggerView.ForceSizeUpdate (320, 192);
			taggerView.SetViewModel (taggerVM);

			// double offsetX = (500 - 320) / 2.0;
			return CreatePreview (taggerView, new Area (new Point (0, 0), 320, 192));
		}

		/// <summary>
		/// Creates the preview from the specified canvas
		/// </summary>
		/// <returns>The preview.</returns>
		/// <param name="canvas">Canvas.</param>
		/// <param name="area">Area.</param>
		public Image CreatePreview (ICanvas canvas, Area area)
		{
			return App.Current.DrawingToolkit.Copy (canvas, area);
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		/// <returns>The start.</returns>
		public bool Start ()
		{
			return true;
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		/// <returns>The stop.</returns>
		public bool Stop ()
		{
			return true;
		}
	}
}
