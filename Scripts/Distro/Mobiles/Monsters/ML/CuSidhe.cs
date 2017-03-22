using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a cu sidhe corpse" )]
	public class CuSidhe : BaseMount
	{
		private DateTime m_NextHeal = DateTime.UtcNow;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		public override bool StatLossAfterTame { get { return true; } }
		public override bool CanAngerOnTame { get { return true; } }

		public override int GetDeathSound() { return 0x575; }
		public override int GetAttackSound() { return 0x576; }
		public override int GetIdleSound() { return 0x577; }
		public override int GetAngerSound() { return 0x578; }
		public override int GetHurtSound() { return 0x579; }

		private class HueEntry
		{
			private int m_Weight;
			private int m_Hue;

			public int Weight { get { return m_Weight; } }
			public int Hue { get { return m_Hue; } }

			public HueEntry( int weight, int hue )
			{
				m_Weight = weight;
				m_Hue = hue;
			}
		}

		private static HueEntry[] m_HueEntries = new HueEntry[]
			{
				new HueEntry( 20000, 0    ), // Regular
				new HueEntry(   500, 2418 ), // Bronze
				new HueEntry(   500, 2424 ), // Dull Copper
				new HueEntry(   500, 2426 ), // Agapite
				new HueEntry(   500, 2305 ), // Grey
				new HueEntry(   500, 2220 ), // Slimes #19
				new HueEntry(   500, 1447 ), // Green #49
				new HueEntry(   500, 1319 ), // Blue #18
				new HueEntry(    50, 1154 ), // Ice Blue
				new HueEntry(    50, 1153 ), // White
 				new HueEntry(    50, 1652 ), // Very Red
				new HueEntry(    50, 1201 ), // Pinky
				new HueEntry(    50, 1301 ), // Blue #0
				new HueEntry(    50, 1109 ), // Black
				new HueEntry(     1, 1161 )  // Fire
			};

		private static int RandomHue()
		{
			int random = Utility.Random( 23801 ); // total weight sum
			int sum = 0;

			for ( int i = 0; i < m_HueEntries.Length; i++ )
			{
				HueEntry entry = m_HueEntries[i];

				sum += entry.Weight;

				if ( sum > random )
					return entry.Hue;
			}

			return 0;
		}

		[Constructable]
		public CuSidhe()
			: base( "a cu sidhe", 277, 0x3E91, AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 ) // TODO: Verify
		{
			Hue = RandomHue();

			SetStr( 1200, 1225 ); // All Values are taken from Stratics and will change along with the info taken from stratics/OSI.
			SetDex( 150, 170 );
			SetInt( 250, 285 );

			SetHits( 1010, 1170 );

			SetDamage( 21, 28 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Cold, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 50, 65 );
			SetResistance( ResistanceType.Fire, 30, 45 );
			SetResistance( ResistanceType.Cold, 70, 85 );
			SetResistance( ResistanceType.Poison, 30, 50 );
			SetResistance( ResistanceType.Energy, 70, 85 );

			SetSkill( SkillName.MagicResist, 75, 90.0 );
			SetSkill( SkillName.Tactics, 90, 100 );
			SetSkill( SkillName.Wrestling, 90, 100 );

			SetSkill( SkillName.Healing, 90, 100.0 );

			Fame = 10000;
			Karma = -10000;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 101.1;

			PackSpellweavingScroll();
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( DateTime.UtcNow > m_NextHeal )
			{
				if ( Controlled && ControlMaster != null )
					HealMaster();
				else if ( Hits < HitsMax )
					HealSelf();

				m_NextHeal = DateTime.UtcNow + TimeSpan.FromSeconds( 10.0 );
			}
		}

		private void HealMaster()
		{
			Mobile m = ControlMaster;

			if ( this.InRange( m, 1 ) && m.Alive && m.Hits < ( m.HitsMax / 3 ) )
			{
				if ( BandageContext.GetContext( this ) == null )
					BandageContext.BeginHeal( this, ControlMaster, null );
			}
			
			if ( Hits < HitsMax )
				HealSelf();
		}

		private void HealSelf()
		{
			if ( BandageContext.GetContext( this ) == null )
				BandageContext.BeginHeal( this, this, null );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.FindItemOnLayer( Layer.Shoes ) is PadsOfTheCuSidhe )
			{
				from.SendLocalizedMessage( 1071981 ); // Your boots allow you to mount the Cu Sidhe.
			}
			else if ( from.Race != Race.Elf )
			{
				from.SendLocalizedMessage( 1072203 ); // Only Elves may use this.
				return;
			}

			base.OnDoubleClick( from );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}
		
		public override int TreasureMapLevel { get { return 5; } }		

		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } } // Todo: Correct

		public CuSidhe( Serial serial )
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
}