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
using NUnit.Framework;
using LongoMatch.Common;
using LongoMatch.Store;


namespace Tests.Core
{
	[TestFixture()]
	public class TestPlayer
	{
		[Test()]
		public void TestSerialization ()
		{
			Player player = new Player {Name="andoni", Position="runner",
				Number = 5, Birthday = new DateTime (1984, 6, 11),
				Nationality = "spanish", Height = 1.73f, Weight = 70,
				Playing = true};
				
			Utils.CheckSerialization (player);
			
			Player newPlayer = Utils.SerializeDeserialize (player);
			Assert.AreEqual (player.Name, newPlayer.Name);
			Assert.AreEqual (player.Position, newPlayer.Position);
			Assert.AreEqual (player.Number, newPlayer.Number);
			Assert.AreEqual (player.Birthday, newPlayer.Birthday);
			Assert.AreEqual (player.Nationality, newPlayer.Nationality);
			Assert.AreEqual (player.Height, newPlayer.Height);
			Assert.AreEqual (player.Weight, newPlayer.Weight);
			Assert.AreEqual (player.Playing, newPlayer.Playing);
		}
		
		[Test()]
		public void TestPhoto ()
		{
			Player player = new Player {Name="andoni", Position="runner",
				Number = 5, Birthday = new DateTime (1984, 6, 11),
				Nationality = "spanish", Height = 1.73f, Weight = 70,
				Playing = true};
				
			player.Photo = null;
			Assert.AreEqual (player.Photo, null);
			/* FIXME: test with real image */
			player.Photo = new DummyImage ("test");
			Utils.CheckSerialization (player);
		}
	}
	[Serializable]
	public class DummyImage: Image
	{
		string text;
		
		public DummyImage (string text): base (null)
		{
			this.text = text;
		}
		
		public byte[] Serialize  () {
			Console.WriteLine ("SER");
			return new byte[] {byte.Parse ("1")};
		}
	}
}

