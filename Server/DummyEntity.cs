using System;

namespace Server
{
	public class DummyEntity : IEntity
	{
		private Serial m_Serial;
		private IPoint3D m_Location;
		private IMap m_Map;

		public DummyEntity( Serial serial, IPoint3D loc, IMap map )
		{
			m_Serial = serial;
			m_Location = loc;
			m_Map = map;
		}

		public Serial Serial
		{
			get
			{
				return m_Serial;
			}
		}

		public IPoint3D Location
		{
			get
			{
				return m_Location;
			}
		}

		public IMap Map
		{
			get
			{
				return m_Map;
			}
		}

		public int X
		{
			get
			{
				return m_Location.X;
			}
		}

		public int Y
		{
			get
			{
				return m_Location.Y;
			}
		}

		public int Z
		{
			get
			{
				return m_Location.Z;
			}
		}
	}
}