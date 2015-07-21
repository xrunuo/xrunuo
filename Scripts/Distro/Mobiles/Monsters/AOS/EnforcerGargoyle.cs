using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle enforcers corpse" )]
	public class EnforcerGargoyle : BaseCreature
	{
		[Constructable]
		public EnforcerGargoyle()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 754;
			Name = "a gargoyle enforcer";
			BaseSoundID = 372;

			SetStr( 260, 350 );
			SetDex( 76, 95 );
			SetInt( 101, 125 );

			SetHits( 182, 185 );

			SetSkill( SkillName.Wrestling, 80.1, 90 );
			SetSkill( SkillName.Tactics, 70.1, 80 );
			SetSkill( SkillName.MagicResist, 120.1, 130 );
			SetSkill( SkillName.Magery, 80.1, 90 );
			SetSkill( SkillName.Anatomy, 70.1, 80 );
			SetSkill( SkillName.EvalInt, 70.3, 100 );
			SetSkill( SkillName.Meditation, 70.3, 100 );

			SetDamage( 8, 11 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 15, 25 );

			SetFameLevel( 5 );
			SetKarmaLevel( 5 );

			PackGold( 180, 240 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.MedScrolls, 1 );
		}

		public override void OnMovement( Mobile mobile, Point3D oldLocation )
		{
			if ( Alive )
			{
				DestroyerGargoyle mHeal = null;

				foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
				{
					if ( m is DestroyerGargoyle )
					{
						mHeal = (DestroyerGargoyle) m;
						break;
					}
				}

				if ( mHeal != null && mHeal.Mana >= 11 && ( this.Hits < ( this.HitsMax - 50 ) ) && !( this.Poisoned || Server.Items.MortalStrike.IsWounded( this ) ) && ( mHeal.CanSee( this ) ) )
				{
					int toHeal = (int) ( mHeal.Skills[SkillName.Magery].Value * 0.4 );
					toHeal += Utility.Random( 1, 10 );
					mHeal.Mana -= 11;

					this.Heal( toHeal );

					this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					this.PlaySound( 0x202 );
				}
			}

			base.OnMovement( mobile, oldLocation );
		}

		public override int TreasureMapLevel { get { return 1; } }

		public EnforcerGargoyle( Serial serial )
			: base( serial )
		{
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
