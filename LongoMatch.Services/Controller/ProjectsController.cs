//
//  Copyright (C) 2016 Fluendo S.A.
//
//
using System;
using VAS.Core;
using VAS.Core.MVVMC;
using VAS.Services.Controller;
using LongoMatch.Core.Store;
using LongoMatch.Services.ViewModel;

namespace LongoMatch.Services.Controller
{
	[ControllerAttribute ("ProjectsManager")]
	public class SportsProjectsController : ProjectsController<ProjectLongoMatch, SportsProjectVM>
	{
	}
}

