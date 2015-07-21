using System;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Mobiles
{
	[CorpseName( "a swamp dragon corpse" )]
	public class SwampDragon : BaseMount
	{
		private bool m_BardingExceptional;
		private Mobile m_BardingCrafter;
		private int m_BardingHP;
		private bool m_HasBarding;
		private CraftResource m_BardingResource;
		private bool m_IsFreshned;
		private static Dictionary<Mobile, List<object>> m_Bonus = new Dictionary<Mobile, List<object>>();

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardingCrafter
		{
			get { return m_BardingCrafter; }
			set { m_BardingCrafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardingExceptional
		{
			get { return m_BardingExceptional; }
			set { m_BardingExceptional = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BardingHP
		{
			get { return m_BardingHP; }
			set { m_BardingHP = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasBarding
		{
			get { return m_HasBarding; }
			set
			{
				m_HasBarding = value;

				if ( m_HasBarding )
				{
					if ( !m_IsFreshned )
						Hue = CraftResources.GetHue( m_BardingResource );

					BodyValue = 0x31F;
					ItemID = 0x3EBE;

					ApplyArmor();
				}
				else
				{
					if ( !m_IsFreshned )
						Hue = 0x851;

					BodyValue = 0x31A;
					ItemID = 0x3EBD;
					RemoveMods( this, m_Bonus[this] );
				}

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsFreshned
		{
			get { return m_IsFreshned; }
			set { m_IsFreshned = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource BardingResource
		{
			get { return m_BardingResource; }
			set
			{
				m_BardingResource = value;

				if ( m_HasBarding && !m_IsFreshned )
					Hue = CraftResources.GetHue( value );

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BardingMaxHP
		{
			get
			{
				CraftResourceInfo info = CraftResources.GetInfo( m_BardingResource );

				int dura = ( info.AttributeInfo.ArmorDurability * 50 + 10000 );

				if ( m_BardingExceptional )
					dura += 2000;

				return dura;
			}
		}

		[Constructable]
		public SwampDragon()
			: this( "a swamp dragon" )
		{
		}

		[Constructable]
		public SwampDragon( string name )
			: base( name, 0x31A, 0x3EBD, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x16A;

			SetStr( 201, 300 );
			SetDex( 66, 85 );
			SetInt( 61, 100 );

			SetHits( 121, 180 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Poison, 25 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 40 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Anatomy, 45.1, 55.0 );
			SetSkill( SkillName.MagicResist, 45.1, 55.0 );
			SetSkill( SkillName.Tactics, 45.1, 55.0 );
			SetSkill( SkillName.Wrestling, 45.1, 55.0 );

			Fame = 2000;
			Karma = -2000;

			Hue = 0x851;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 93.9;
		}

		public override int GetIdleSound()
		{
			return 0x2CE;
		}

		public override int GetDeathSound()
		{
			return 0x2CC;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override int GetAttackSound()
		{
			return 0x2C8;
		}

		public override double GetControlChance( Mobile m )
		{
			return 1.0;
		}

		public override bool AutoDispel { get { return !Controlled; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 20; } }
		public override int Scales { get { return 5; } }
		public override ScaleType ScaleType { get { return ScaleType.Green; } }
		public override bool CanAngerOnTame { get { return true; } }

		public SwampDragon( Serial serial )
			: base( serial )
		{
		}

		private void ApplyArmor()
		{
			List<object> mods = new List<object>();

			CraftResourceInfo info = CraftResources.GetInfo( m_BardingResource );

			int exceptionalbonus = 0;

			if ( m_BardingExceptional )
				exceptionalbonus = 1;

			int phys = ( 5 + ( info != null ? info.AttributeInfo.ArmorPhysicalResist : 0 ) + exceptionalbonus ) * 5;
			int fire = ( 3 + (info != null ? info.AttributeInfo.ArmorFireResist : 0) + exceptionalbonus ) * 5;
			int cold = ( 2 + (info != null ? info.AttributeInfo.ArmorColdResist : 0) + exceptionalbonus ) * 5;
			int pois = ( 3 + (info != null ? info.AttributeInfo.ArmorPoisonResist : 0) + exceptionalbonus ) * 5;
			int ener = ( 2 + (info != null ? info.AttributeInfo.ArmorEnergyResist : 0) + exceptionalbonus ) * 5;

			if ( PhysicalResistance + phys > 90 )
				phys = 90 - PhysicalResistance;
			if ( FireResistance + fire > 90 )
				fire = 90 - FireResistance;
			if ( ColdResistance + cold > 90 )
				cold = 90 - ColdResistance;
			if ( PoisonResistance + pois > 90 )
				pois = 90 - PoisonResistance;
			if ( EnergyResistance + ener > 90 )
				ener = 90 - EnergyResistance;

			mods.Add( new ResistanceMod( ResistanceType.Physical, phys ) );
			mods.Add( new ResistanceMod( ResistanceType.Fire, fire ) );
			mods.Add( new ResistanceMod( ResistanceType.Cold, cold ) );
			mods.Add( new ResistanceMod( ResistanceType.Poison, pois ) );
			mods.Add( new ResistanceMod( ResistanceType.Energy, ener ) );

			ApplyMods( this, mods );

			m_Bonus[this] = mods;

		}

		private static void ApplyMods( Mobile m, List<object> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
			{
				object mod = mods[i];

				m.AddResistanceMod( (ResistanceMod) mod );
			}
		}

		private static void RemoveMods( Mobile m, List<object> mods )
		{
			for ( int i = 0; i < mods.Count; i++ )
			{
				object mod = mods[i];

				m.RemoveResistanceMod( (ResistanceMod) mod );
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_HasBarding && m_BardingExceptional )
			{
				if ( m_BardingCrafter != null )
					list.Add( 1060853, m_BardingCrafter.Name ); // armor exceptionally crafted by ~1_val~
				list.Add( 1115719, m_BardingHP.ToString() ); // armor points: ~1_val~
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version

			writer.Write( (bool) m_IsFreshned );
			writer.Write( (bool) m_BardingExceptional );
			writer.Write( (Mobile) m_BardingCrafter );
			writer.Write( (bool) m_HasBarding );
			writer.Write( (int) m_BardingHP );
			writer.Write( (int) m_BardingResource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						m_IsFreshned = reader.ReadBool();
						goto case 1;
					}
				case 1:
					{
						m_BardingExceptional = reader.ReadBool();
						m_BardingCrafter = reader.ReadMobile();
						m_HasBarding = reader.ReadBool();
						m_BardingHP = reader.ReadInt();
						m_BardingResource = (CraftResource) reader.ReadInt();
						break;
					}
			}

			if ( Hue == 0 && !m_HasBarding )
				Hue = 0x851;

			if ( m_HasBarding )
				ApplyArmor();

			if ( BaseSoundID == -1 )
				BaseSoundID = 0x16A;
		}
	}
}
