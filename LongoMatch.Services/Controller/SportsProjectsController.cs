//
//  Copyright (C) 2016 Fluendo S.A.
//
//
using LongoMatch.Core.Store;
using LongoMatch.Services.ViewModel;
using VAS.Core.MVVMC;
using VAS.Services.Controller;
using LongoMatch.Core.Events;

namespace LongoMatch.Services.Controller
{

	/// <summary>
	/// Controller for sports projects.
	/// </summary>
	[ControllerAttribute ("ProjectsManager")]
	public class SportsProjectsController : ProjectsController<ProjectLongoMatch, SportsProjectVM>
	{

		protected override void HandleOpen (VAS.Core.Events.OpenEvent<ProjectLongoMatch> evt)
		{
			ProjectLongoMatch project = evt.Object;

			if (project == null) {
				return;
			}

			App.Current.EventsBroker.Publish<OpenProjectIDEvent> (
				new OpenProjectIDEvent { Project = project, ProjectID = project.ID }
			);
		}

		async protected override void HandleResync (VAS.Core.Events.ResyncProjectEvent evt)
		{
			ProjectLongoMatch project = evt.Project as ProjectLongoMatch;
			if (project == null) {
				return;
			}

			bool canNavigate = true;
			if (!project.Description.FileSet.CheckFiles ()) {
				// Show message in order to load video.
				canNavigate = App.Current.GUIToolkit.SelectMediaFiles (project.Description.FileSet);
			}

			await App.Current.StateController.MoveTo (ResyncProjectState.Name, project);
		}
	}
}

