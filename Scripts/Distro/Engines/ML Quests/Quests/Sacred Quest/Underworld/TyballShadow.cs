using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an evil shadow corpse" )]
	public class TyballShadow : BaseCreature
	{
		private Item m_Shroud;

		[Constructable]
		public TyballShadow()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Tyball's Shadow";
			Body = 0x190;
			Hue = 0x4001;

			// Guessing from here

			SetStr( 500 );
			SetDex( 150 );
			SetInt( 500 );

			SetHits( 2500 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 70 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Magery, 150.0 );
			SetSkill( SkillName.Meditation, 150.0 );
			SetSkill( SkillName.MagicResist, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );

			Fame = 10000;
			Karma = -10000;

			m_Shroud = new ShroudOfTheCondemned();
			AddItem( m_Shroud );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 0.05 < Utility.RandomDouble() && m_Shroud != null )
				m_Shroud.Delete();
		}

		public override void OnKilledBy( Mobile mob )
		{
			PlayerMobile pm = mob as PlayerMobile;

			if ( pm != null && pm.Backpack != null && !pm.SacredQuest )
			{
				YellowKeyFragment key = pm.Backpack.FindItemByType<YellowKeyFragment>();

				if ( key != null )
					return;

				key = new YellowKeyFragment();

				if ( pm.AddToBackpack( key ) )
				{
					// You have received a glowing key fragment.
					pm.SendLocalizedMessage( 1113701 );
				}
				else
				{
					key.Delete();

					// You attempt to retrieve a key fragment but your bag is too full. The master key vanishes.
					pm.SendLocalizedMessage( 1113702 );
				}
			}
		}

		public override bool ShowFameTitle { get { return false; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public TyballShadow( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version

			writer.Write( m_Shroud );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version > 0 )
				m_Shroud = reader.ReadItem();
		}
	}
}
