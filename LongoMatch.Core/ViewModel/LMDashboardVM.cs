//
//  Copyright (C) 2017 Fluendo S.A.
using System;
using LongoMatch.Core.Store;
using VAS.Core.MVVMC;
using VAS.Core.Store;
using VAS.Core.ViewModel;

namespace LongoMatch.Core.ViewModel
{
	public class LMDashboardVM : DashboardVM
	{
		public override CollectionViewModel<DashboardButton, DashboardButtonVM> CreateSubViewModel ()
		{
			var collection = base.CreateSubViewModel ();
			collection.TypeMappings.Add (typeof (ScoreButton), typeof (ScoreButtonVM));
			collection.TypeMappings.Add (typeof (PenaltyCardButton), typeof (PenaltyCardButtonVM));
			return collection;
		}
	}
}
