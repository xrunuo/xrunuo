using System;

namespace Server.Items
{
	[Flipable]
	public class RewardDress : BaseOuterTorso, Engines.VeteranRewards.IRewardItem
	{
		private int m_LabelNumber;
		private bool m_IsRewardItem;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsRewardItem { get { return m_IsRewardItem; } set { m_IsRewardItem = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Number
		{
			get { return m_LabelNumber; }
			set
			{
				m_LabelNumber = value;
				InvalidateProperties();
			}
		}

		public override int LabelNumber
		{
			get
			{
				if ( m_LabelNumber > 0 )
				{
					return m_LabelNumber;
				}

				return base.LabelNumber;
			}
		}

		public override int BasePhysicalResistance { get { return 3; } }

		public override bool Dye( Mobile from, IDyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public override bool CanEquip( Mobile m )
		{
			if ( !base.CanEquip( m ) )
			{
				return false;
			}

			return !m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( m, this, new object[] { Hue, m_LabelNumber } );
		}

		[Constructable]
		public RewardDress()
			: this( 0 )
		{
		}

		[Constructable]
		public RewardDress( int hue )
			: this( hue, 0 )
		{
		}

		[Constructable]
		public RewardDress( int hue, int labelNumber )
			: base( 0x1F01, hue )
		{
			Weight = 2.0;
			LootType = LootType.Blessed;

			m_LabelNumber = labelNumber;
		}

		public RewardDress( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_LabelNumber );
			writer.Write( (bool) m_IsRewardItem );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
					{
						m_LabelNumber = reader.ReadInt();
						m_IsRewardItem = reader.ReadBool();
						break;
					}
			}
		}
	}
}