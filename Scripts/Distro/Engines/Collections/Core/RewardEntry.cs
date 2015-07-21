using System;

namespace Server.Engines.Collections
{
	public delegate bool RewardChecker( Mobile m );
	public delegate void OnRewardChosen( Mobile m );

	public interface IRewardEntry
	{
		Type Type { get; }
		object[] Args { get; }
		string Name { get; }
		int Cliloc { get; }
		int RewardTitle { get; }
		int[] Hues { get; }
		int Hue { get; }

		int Tile { get; }
		int X { get; }
		int Y { get; }
		int Width { get; }
		int Height { get; }

		int GetPrice( Mobile m );
		bool IsEligibleBy( Mobile m );
		void OnRewardChosen( Mobile m );
	}

	public class RewardEntry : IRewardEntry
	{
		private readonly Type m_Type;
		private readonly object[] m_Args;
		private readonly string m_Name;
		private readonly int m_Price, m_Cliloc, m_RewardTitle;
		private readonly int[] m_Hues;
		private readonly int m_Hue;

		private readonly int m_Tile;
		private readonly int m_X, m_Y, m_Width, m_Height;

		private readonly OnRewardChosen m_OnRewardChosen;

		public Type Type { get { return m_Type; } }
		public object[] Args { get { return m_Args; } }
		public string Name { get { return m_Name; } }
		public int Cliloc { get { return m_Cliloc; } }
		public int RewardTitle { get { return m_RewardTitle; } }
		public int[] Hues { get { return m_Hues; } }
		public int Hue { get { return m_Hue; } }

		public int Tile { get { return m_Tile; } }
		public int X { get { return m_X; } }
		public int Y { get { return m_Y; } }
		public int Width { get { return m_Width; } }
		public int Height { get { return m_Height; } }

		public virtual bool IsEligibleBy( Mobile m )
		{
			return true;
		}

		public virtual int GetPrice( Mobile m )
		{
			return m_Price;
		}

		public void OnRewardChosen( Mobile m )
		{
			if ( m_OnRewardChosen != null )
				m_OnRewardChosen( m );
		}

		public RewardEntry( Type type, int price, int cliloc, int tile, int hue )
			: this( type, price, cliloc, null, tile, hue )
		{
		}

		public RewardEntry( Type type, int price, int cliloc, int[] hues, int tile, int hue )
			: this( type, price, cliloc, 0, hues, tile, hue )
		{
		}

		public RewardEntry( Type type, int price, int cliloc, int rewardtitle, int tile, int hue )
			: this( type, price, cliloc, rewardtitle, null, tile, hue )
		{
		}

		public RewardEntry( Type type, int price, int cliloc, int rewardtitle, int[] hues, int tile, int hue )
			: this( type, price, cliloc, rewardtitle, hues, tile, hue, null )
		{
		}

		public RewardEntry( Type type, int price, int cliloc, int rewardtitle, int[] hues, int tile, int hue, OnRewardChosen onRewardChosen )
		{
			m_Type = type;
			m_Price = price;
			m_Cliloc = cliloc;
			m_Hues = hues;
			m_RewardTitle = rewardtitle;
			m_Tile = tile;
			m_Hue = hue;
			m_OnRewardChosen = onRewardChosen;

			Rectangle2D bounds = ItemBounds.Table[m_Tile];

			m_X = bounds.X;
			m_Y = bounds.Y;
			m_Width = bounds.Width;
			m_Height = bounds.Height;
		}

		public RewardEntry( Type type, int price, string name, int tile, int hue )
			: this( type, null, price, name, tile, hue )
		{
		}

		public RewardEntry( Type type, object[] args, int price, string name, int tile, int hue )
		{
			m_Type = type;
			m_Args = args;
			m_Price = price;
			m_Name = name;
			m_Tile = tile;
			m_Hue = hue;
			m_Cliloc = -1;

			Rectangle2D bounds = ItemBounds.Table[m_Tile];

			m_X = bounds.X;
			m_Y = bounds.Y;
			m_Width = bounds.Width;
			m_Height = bounds.Height;
		}
	}

	public class ConditionalRewardEntry : RewardEntry
	{
		private RewardChecker m_Checker;

		public override bool IsEligibleBy( Mobile m )
		{
			return m_Checker( m );
		}

		public ConditionalRewardEntry( Type type, int price, int cliloc, int tile, int hue, RewardChecker checker )
			: this( type, price, cliloc, tile, hue, null, checker )
		{
		}

		public ConditionalRewardEntry( Type type, int price, int cliloc, int tile, int hue, OnRewardChosen onRewardChosen, RewardChecker checker )
			: base( type, price, cliloc, 0, null, tile, hue, onRewardChosen )
		{
			m_Checker = checker;
		}
	}
}