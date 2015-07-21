using System;

using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "an earth elemental corpse" )]
	public class EnragedEarthElemental : BaseCreature
	{
		public override double DispelDifficulty { get { return 117.5; } }
		public override double DispelFocus { get { return 45.0; } }

		private bool m_IsEnraged;

		[Constructable]
		public EnragedEarthElemental()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Enraged Earth Elemental";
			Body = 14;
			BaseSoundID = 268;

			Hue = 442;

			SetStr( 147, 155 );
			SetDex( 78, 89 );
			SetInt( 94, 110 );

			SetHits( 512, 549 );

			SetDamage( 9, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 45, 55 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Wrestling, 120.0 );

			Fame = 3500;
			Karma = -3500;

			ControlSlots = 2;

			PackItem( new FertileDirt( Utility.RandomMinMax( 1, 4 ) ) );
			PackItem( new IronOre( 3 ) );
			PackItem( new MandrakeRoot() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.Gems );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( !m_IsEnraged && Hits < ( HitsMax / 2 ) && !willKill )
			{
				PlaySound( GetAngerSound() );

				Hue = 1141;

				RawStr = Utility.RandomMinMax( 294, 310 ); // Do not refill hit points
				SetDex( 156, 178 );
				SetInt( 188, 220 );

				m_IsEnraged = true;

				Timer.DelayCall( TimeSpan.Zero, () =>
				{
					PublicOverheadMessage( MessageType.Regular, 0x3B2, 1113587 ); // The creature goes into a frenzied rage!
				} );				
			}
		}

		public override bool BleedImmune { get { return true; } }
		public override int TreasureMapLevel { get { return 1; } }

		public EnragedEarthElemental( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (bool) m_IsEnraged );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_IsEnraged = reader.ReadBool();
		}
	}
}