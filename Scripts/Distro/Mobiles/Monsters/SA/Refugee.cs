using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a gargoyle corpse" )]
	public class Refugee : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public Refugee()
			: base( AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Female = Utility.RandomBool();
			Race = Race.Gargoyle;
			Hue = Race.RandomSkinHue();

			Name = "Refugee";
			Utility.AssignRandomHair( this );

			AddItem( new GargishClothKilt() );
			AddItem( new GargishClothArms() );

			SetStr( 100 );
			SetDex( 25 );
			SetInt( 100 );

			SetHits( 100, 125 );

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10, 20 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Tactics, 20.0 );
			SetSkill( SkillName.Wrestling, 20.0 );
		}

		public Refugee( Serial serial )
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
