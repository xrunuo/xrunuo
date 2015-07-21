using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
	public enum QuestTier
	{
		ThepemTier1,
		ThepemTier2,

		ZosilemTier1,
		ZosilemTier2,
		ZosilemTier3,

		PercolemTier1,
		PercolemTier2,
		PercolemTier3
	}

	public class QuestTierInfo
	{
		private static QuestTierInfo[] m_Table = new QuestTierInfo[]
			{
				new QuestTierInfo( TimeSpan.FromMinutes( 30.0 ) ),							 /* Thepem Tier 1 */
				new QuestTierInfo( TimeSpan.FromHours( 2.0 ), 5, QuestTier.ThepemTier1 ),	 /* Thepem Tier 2 */
				
				new QuestTierInfo( TimeSpan.FromMinutes( 30.0 ), 5, QuestTier.ThepemTier2 ), /* Zosilem Tier 1 */
				new QuestTierInfo( TimeSpan.FromHours( 2.0 ), 10, QuestTier.ZosilemTier1 ),	 /* Zosilem Tier 2 */
				new QuestTierInfo( TimeSpan.FromDays( 1.0 ), 20, QuestTier.ZosilemTier2 ),	 /* Zosilem Tier 3 */

				new QuestTierInfo( TimeSpan.FromMinutes( 30.0 ) ),							 /* Percolem Tier 1 */
				new QuestTierInfo( TimeSpan.FromHours( 2.0 ), 5, QuestTier.PercolemTier1 ),	 /* Percolem Tier 2 */
				new QuestTierInfo( TimeSpan.FromDays( 1.0 ), 10, QuestTier.PercolemTier2 ),	 /* Percolem Tier 3 */
			};

		public static QuestTierInfo GetQuestTierInfo( QuestTier tier )
		{
			return m_Table[(int) tier];
		}

		private TimeSpan m_Cooldown;
		private int m_RequiredAmount;
		private QuestTier m_RequiredTier;

		public TimeSpan Cooldown { get { return m_Cooldown; } }
		public int RequiredAmount { get { return m_RequiredAmount; } }
		public QuestTier RequiredTier { get { return m_RequiredTier; } }

		public QuestTierInfo( TimeSpan cooldown )
			: this( cooldown, 0, QuestTier.ThepemTier1 )
		{
		}

		public QuestTierInfo( TimeSpan cooldown, int requiredAmount, QuestTier requiredTier )
		{
			m_Cooldown = cooldown;
			m_RequiredAmount = requiredAmount;
			m_RequiredTier = requiredTier;
		}
	}

	[PropertyObject]
	public class TieredQuestInfo
	{
		private static readonly int TierAmount = Enum.GetValues( typeof( QuestTier ) ).Length;

		private int[] m_Values;

		public int GetValue( QuestTier tier )
		{
			return GetValue( (int) tier );
		}

		public int GetValue( int index )
		{
			if ( index < 0 || index >= m_Values.Length )
				return 0;

			return m_Values[index];
		}

		public void SetValue( QuestTier tier, int value )
		{
			SetValue( (int) tier, value );
		}

		public void SetValue( int index, int value )
		{
			if ( index < 0 || index >= m_Values.Length )
				return;

			m_Values[index] = value;
		}

		public void OnCompleted( QuestTier tier )
		{
			OnCompleted( (int) tier );
		}

		public void OnCompleted( int index )
		{
			if ( index < 0 || index >= m_Values.Length )
				return;

			m_Values[index]++;
		}

		public override string ToString()
		{
			return "...";
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ThepemTier1 { get { return GetValue( QuestTier.ThepemTier1 ); } set { SetValue( QuestTier.ThepemTier1, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ThepemTier2 { get { return GetValue( QuestTier.ThepemTier2 ); } set { SetValue( QuestTier.ThepemTier2, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ZosilemTier1 { get { return GetValue( QuestTier.ZosilemTier1 ); } set { SetValue( QuestTier.ZosilemTier1, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ZosilemTier2 { get { return GetValue( QuestTier.ZosilemTier2 ); } set { SetValue( QuestTier.ZosilemTier2, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int ZosilemTier3 { get { return GetValue( QuestTier.ZosilemTier3 ); } set { SetValue( QuestTier.ZosilemTier3, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PercolemTier1 { get { return GetValue( QuestTier.PercolemTier1 ); } set { SetValue( QuestTier.PercolemTier1, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PercolemTier2 { get { return GetValue( QuestTier.PercolemTier2 ); } set { SetValue( QuestTier.PercolemTier2, value ); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int PercolemTier3 { get { return GetValue( QuestTier.PercolemTier3 ); } set { SetValue( QuestTier.PercolemTier3, value ); } }

		public TieredQuestInfo()
		{
			m_Values = new int[TierAmount];
		}

		public TieredQuestInfo( GenericReader reader )
		{
			int version = reader.ReadEncodedInt();

			switch ( version )
			{
				case 0:
					{
						int length = reader.ReadEncodedInt();
						m_Values = new int[length];

						for ( int i = 0; i < length; i++ )
							m_Values[i] = reader.ReadEncodedInt();

						if ( m_Values.Length != TierAmount )
						{
							int[] oldValues = m_Values;
							m_Values = new int[TierAmount];

							for ( int i = 0; i < m_Values.Length && i < oldValues.Length; i++ )
								m_Values[i] = oldValues[i];
						}

						break;
					}
			}
		}

		public static void Serialize( GenericWriter writer, TieredQuestInfo info )
		{
			writer.WriteEncodedInt( (int) 0 ); // version

			int length = info.m_Values.Length;
			writer.WriteEncodedInt( length );

			for ( int i = 0; i < length; i++ )
				writer.WriteEncodedInt( info.m_Values[i] );
		}
	}

	public abstract class TieredQuest : BaseQuest
	{
		public abstract QuestTier Tier { get; }

		public override bool CanOffer()
		{
			QuestTierInfo info = QuestTierInfo.GetQuestTierInfo( Tier );

			return Owner.TieredQuestInfo.GetValue( info.RequiredTier ) >= info.RequiredAmount;
		}

		public override TimeSpan RestartDelay
		{
			get { return QuestTierInfo.GetQuestTierInfo( Tier ).Cooldown; }
		}

		public override void OnCompleted()
		{
			base.OnCompleted();

			Owner.TieredQuestInfo.OnCompleted( Tier );
		}
	}
}