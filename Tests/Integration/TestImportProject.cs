//
//  Copyright (C) 2017 
using System;
using NUnit.Framework;

namespace Tests.Integration
{
	[TestFixture]
	public class TestImportProject
	{
		[Test]
		public void TestLON1319 ()
		{
			// Being in Basic License

			// Import a project with multicamera, MoveTo Analysis to open it

			// Missing media files dialog

			// Select the first media file

			// When selecting the second media file, the limitation command is launched, and the Limitation popup is MoveToModal'd

			// The controller is already stopped, because we are in the middle of the transition.

			Assert.Fail ();
		}
	}
}
