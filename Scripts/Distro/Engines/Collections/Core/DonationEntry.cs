using System;

namespace Server.Engines.Collections
{
	public class DonationEntry
	{
		private Type m_Type;
		private double m_Award;
		private int m_Cliloc;
		private int m_Tile, m_Hue;
		private string m_Name;

		private int m_X, m_Y, m_Width, m_Height;

		public Type Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}

		public double Award
		{
			get { return m_Award; }
			set { m_Award = value; }
		}

		public int Cliloc
		{
			get { return m_Cliloc; }
			set { m_Cliloc = value; }
		}

		public int Tile
		{
			get { return m_Tile; }
			set { m_Tile = value; }
		}

		public int Hue
		{
			get { return m_Hue; }
			set { m_Hue = value; }
		}

		public int X { get { return m_X; } }
		public int Y { get { return m_Y; } }
		public int Width { get { return m_Width; } }
		public int Height { get { return m_Height; } }

		public string Name { get { return m_Name; } }

		public DonationEntry( Type type, double award, int cliloc, int tile, int hue )
		{
			m_Type = type;
			m_Award = award;
			m_Cliloc = cliloc;
			m_Tile = tile;
			m_Hue = hue;

			Rectangle2D bounds = ItemBounds.Table[m_Tile];

			m_X = bounds.X;
			m_Y = bounds.Y;
			m_Width = bounds.Width;
			m_Height = bounds.Height;
		}

		public DonationEntry( Type type, double award, string name, int tile, int hue )
		{
			m_Type = type;
			m_Award = award;
			m_Name = name;
			m_Cliloc = -1;
			m_Tile = tile;
			m_Hue = hue;

			Rectangle2D bounds = ItemBounds.Table[m_Tile];

			m_X = bounds.X;
			m_Y = bounds.Y;
			m_Width = bounds.Width;
			m_Height = bounds.Height;
		}
	}
}