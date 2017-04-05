using System;

namespace Server
{
	public class Point3DList
	{
		private Point3D[] m_List;

		public Point3DList()
		{
			m_List = new Point3D[8];
			Count = 0;
		}

		public int Count { get; private set; }

		public void Clear()
		{
			Count = 0;
		}

		public Point3D Last => m_List[Count - 1];

		public Point3D this[int index] => m_List[index];

		public void Add( int x, int y, int z )
		{
			if ( ( Count + 1 ) > m_List.Length )
			{
				Point3D[] old = m_List;
				m_List = new Point3D[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_List[i] = old[i];
			}

			m_List[Count].X = x;
			m_List[Count].Y = y;
			m_List[Count].Z = z;
			++Count;
		}

		public void Add( Point3D p )
		{
			if ( ( Count + 1 ) > m_List.Length )
			{
				Point3D[] old = m_List;
				m_List = new Point3D[old.Length * 2];

				for ( int i = 0; i < old.Length; ++i )
					m_List[i] = old[i];
			}

			m_List[Count].X = p.X;
			m_List[Count].Y = p.Y;
			m_List[Count].Z = p.Z;
			++Count;
		}

		private static readonly Point3D[] m_EmptyList = new Point3D[0];

		public Point3D[] ToArray()
		{
			if ( Count == 0 )
				return m_EmptyList;

			Point3D[] list = new Point3D[Count];

			for ( int i = 0; i < Count; ++i )
				list[i] = m_List[i];

			Count = 0;

			return list;
		}
	}
}