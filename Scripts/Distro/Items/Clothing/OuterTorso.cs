using System;

namespace Server.Items
{
	public abstract class BaseOuterTorso : BaseClothing
	{
		public BaseOuterTorso( int itemID )
			: this( itemID, 0 )
		{
		}

		public BaseOuterTorso( int itemID, int hue )
			: base( itemID, Layer.OuterTorso, hue )
		{
		}

		public BaseOuterTorso( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[Flipable( 0x230E, 0x230D )]
	public class GildedDress : BaseOuterTorso
	{
		[Constructable]
		public GildedDress()
			: this( 0 )
		{
		}

		[Constructable]
		public GildedDress( int hue )
			: base( 0x230E, hue )
		{
			Weight = 3.0;
		}

		public GildedDress( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[Flipable( 0x1F00, 0x1EFF )]
	public class FancyDress : BaseOuterTorso
	{
		[Constructable]
		public FancyDress()
			: this( 0 )
		{
		}

		[Constructable]
		public FancyDress( int hue )
			: base( 0x1F00, hue )
		{
			Weight = 3.0;
		}

		public FancyDress( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	public class DeathRobe : Robe
	{
		public override bool DisplayLootType { get { return false; } }

		[Constructable]
		public DeathRobe()
		{
			LootType = LootType.Newbied;
			Hue = 2301;
		}

		new public bool Scissor( Mobile from, Scissors scissors )
		{
			from.SendLocalizedMessage( 502440 ); // Scissors can not be used on that to produce anything.
			return false;
		}

		public DeathRobe( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[Flipable]
	public class RewardRobe : BaseOuterTorso, Engines.VeteranRewards.IRewardItem
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
					return m_LabelNumber;

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
				return false;

			return !m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( m, this, new object[] { Hue, m_LabelNumber } );
		}

		[Constructable]
		public RewardRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public RewardRobe( int hue )
			: this( hue, 0 )
		{
		}

		[Constructable]
		public RewardRobe( int hue, int labelNumber )
			: base( 0x1F03, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;

			m_LabelNumber = labelNumber;
		}

		public RewardRobe( Serial serial )
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

	public class RewardGargishRobe : BaseOuterTorso, Engines.VeteranRewards.IRewardItem
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

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
					return m_LabelNumber;

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
				return false;

			return !m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( m, this, new object[] { Hue, m_LabelNumber } );
		}

		[Constructable]
		public RewardGargishRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public RewardGargishRobe( int hue )
			: this( hue, 0 )
		{
		}

		[Constructable]
		public RewardGargishRobe( int hue, int labelNumber )
			: base( 0x4000, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;

			m_LabelNumber = labelNumber;
		}

		public RewardGargishRobe( Serial serial )
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

	public class RewardGargishFancyRobe : BaseOuterTorso, Engines.VeteranRewards.IRewardItem
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

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
					return m_LabelNumber;

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
				return false;

			return !m_IsRewardItem || Engines.VeteranRewards.RewardSystem.CheckIsUsableBy( m, this, new object[] { Hue, m_LabelNumber } );
		}

		[Constructable]
		public RewardGargishFancyRobe()
			: this( 0 )
		{
		}

		[Constructable]
		public RewardGargishFancyRobe( int hue )
			: this( hue, 0 )
		{
		}

		[Constructable]
		public RewardGargishFancyRobe( int hue, int labelNumber )
			: base( 0x4002, hue )
		{
			Weight = 3.0;
			LootType = LootType.Blessed;

			m_LabelNumber = labelNumber;
		}

		public RewardGargishFancyRobe( Serial serial )
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

	[Flipable]
	public class Robe : BaseOuterTorso, IArcaneEquip
	{
		#region Arcane Impl
		private int m_MaxArcaneCharges, m_CurArcaneCharges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxArcaneCharges
		{
			get { return m_MaxArcaneCharges; }
			set
			{
				m_MaxArcaneCharges = value;
				InvalidateProperties();
				Update();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int CurArcaneCharges
		{
			get { return m_CurArcaneCharges; }
			set
			{
				m_CurArcaneCharges = value;
				InvalidateProperties();
				Update();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsArcane { get { return ( m_MaxArcaneCharges > 0 && m_CurArcaneCharges >= 0 ); } }

		public void Update()
		{
			if ( IsArcane )
			{
				ItemID = 0x26AE;
			}
			else if ( ItemID == 0x26AE )
			{
				ItemID = 0x1F04;
			}

			if ( IsArcane && CurArcaneCharges == 0 )
			{
				Hue = 0;
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( IsArcane )
			{
				list.Add( 1061837, "{0}\t{1}", m_CurArcaneCharges, m_MaxArcaneCharges ); // arcane charges: ~1_val~ / ~2_val~
			}
		}

		public void Flip()
		{
			if ( ItemID == 0x1F03 )
			{
				ItemID = 0x1F04;
			}
			else if ( ItemID == 0x1F04 )
			{
				ItemID = 0x1F03;
			}
		}
		#endregion

		public override bool WearableByGargoyles { get { return true; } }

		[Constructable]
		public Robe()
			: this( 0 )
		{
		}

		[Constructable]
		public Robe( int hue )
			: base( 0x1F03, hue )
		{
			Weight = 3.0;
		}

		public Robe( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			if ( IsArcane )
			{
				writer.Write( true );
				writer.Write( (int) m_CurArcaneCharges );
				writer.Write( (int) m_MaxArcaneCharges );
			}
			else
			{
				writer.Write( false );
			}
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
					{
						if ( reader.ReadBool() )
						{
							m_CurArcaneCharges = reader.ReadInt();
							m_MaxArcaneCharges = reader.ReadInt();

							if ( Hue == 2118 )
							{
								Hue = ArcaneGem.DefaultArcaneHue;
							}
						}

						break;
					}
			}
		}
	}

	[Flipable( 0x2684, 0x2683 )]
	public class HoodedShroudOfShadows : BaseOuterTorso
	{
		[Constructable]
		public HoodedShroudOfShadows()
			: this( 0x455 )
		{
		}

		[Constructable]
		public HoodedShroudOfShadows( int hue )
			: base( 0x2684, hue )
		{
			LootType = LootType.Blessed;
			Weight = 3.0;
		}

		public override bool Dye( Mobile from, IDyeTub sender )
		{
			from.SendLocalizedMessage( sender.FailMessage );
			return false;
		}

		public HoodedShroudOfShadows( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}
	}

	[Flipable( 0x1f01, 0x1f02 )]
	public class PlainDress : BaseOuterTorso
	{
		[Constructable]
		public PlainDress()
			: this( 0 )
		{
		}

		[Constructable]
		public PlainDress( int hue )
			: base( 0x1F01, hue )
		{
			Weight = 2.0;
		}

		public PlainDress( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			if ( Weight == 3.0 )
			{
				Weight = 2.0;
			}
		}
	}
}
