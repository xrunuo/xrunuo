using System;

namespace Server
{
	public class DummyEntity : IEntity
	{
		public DummyEntity( Serial serial, IPoint3D loc, IMap map )
		{
			Serial = serial;
			Location = loc;
			Map = map;
		}

		public Serial Serial { get; }

		public IPoint3D Location { get; }

		public IMap Map { get; }

		public int X => Location.X;

		public int Y => Location.Y;

		public int Z => Location.Z;
	}
}