using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a chaos dragoon elite corpse" )]
	public class ChaosDragoonElite : BaseCreature
	{
		private static int m_MinTime = 4;
		private static int m_MaxTime = 8;

		private DateTime m_NextAbilityTime;

		[Constructable]
		public ChaosDragoonElite()
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			Name = "a chaos dragoon elite";
			Body = 0x190;
			Hue = Utility.RandomSkinHue();

			SetStr( 276, 350 );
			SetDex( 66, 90 );
			SetInt( 126, 150 );

			SetDamage( 26, 28 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 15, 25 );
			SetResistance( ResistanceType.Cold, 50 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.1, 110.0 );
			SetSkill( SkillName.Anatomy, 80.1, 100.0 );
			SetSkill( SkillName.EvalInt, 85.1, 100.0 );
			SetSkill( SkillName.Magery, 85.1, 100.0 );

			SetSkill( SkillName.Fencing, 85.1, 100.0 );

			Fame = 8000;
			Karma = -8000;

			ChaosDragoon.InitOutfit( this );

			CraftResource res = CraftResource.None;

			switch ( Utility.Random( 9 ) )
			{
				case 0:
					res = CraftResource.DullCopper;
					break;
				case 1:
					res = CraftResource.ShadowIron;
					break;
				case 2:
					res = CraftResource.Copper;
					break;
				case 3:
					res = CraftResource.Bronze;
					break;
				case 4:
					res = CraftResource.Gold;
					break;
				case 5:
					res = CraftResource.Agapite;
					break;
				case 6:
					res = CraftResource.Verite;
					break;
				case 7:
					res = CraftResource.Valorite;
					break;
				case 8:
					res = CraftResource.None;
					break;
			}

			SwampDragon mount = new SwampDragon();

			if ( res != CraftResource.None )
			{
				mount.HasBarding = true;
				mount.BardingResource = res;
				mount.BardingHP = mount.BardingMaxHP;
			}

			mount.Rider = this;

			AddItem( new Gold( 400, 500 ) );
			AddItem( new Shirt( Utility.RandomDyedHue() ) );
			PackMagicItems( 1, 3 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Gems, 1 );
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool HasBreath { get { return true; } }

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( mount != null )
			{
				mount.Rider = null;
			}

			if ( mount is Mobile )
			{
				( (Mobile) mount ).Kill();
			}

			return base.OnBeforeDeath();
		}

		public ChaosDragoonElite( Serial serial )
			: base( serial )
		{
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( DateTime.UtcNow >= m_NextAbilityTime )
			{
				ChaosDragoon.StompAttack( defender );

				m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( Utility.RandomMinMax( m_MinTime, m_MaxTime ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */reader.ReadInt();
		}
	}
}