//
//  Copyright (C) 2016 Fluendo S.A.
//
//
using System;
using LongoMatch.Core.Store;
using VAS.Core.MVVMC;
using VAS.Services.ViewModel;

namespace LongoMatch.Services.ViewModel
{
	[ViewAttribute ("ProjectsManager")]
	public class SportsProjectsManagerVM : ProjectsManagerVM<ProjectLongoMatch, SportsProjectVM>
	{
	}
}

