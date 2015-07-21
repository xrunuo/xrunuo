namespace Server.Mobiles // No info on stratics as of yet
{
	[CorpseName( "a ferret corpse" )]
	public class Ferret : BaseCreature
	{
		[Constructable]
		public Ferret()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a ferret";
			Body = 279;
			BaseSoundID = 0xCC;

			SetStr( 30 );
			SetDex( 15 );
			SetInt( 5 );

			SetHits( 18 );
			SetMana( 0 );

			SetDamage( 1, 4 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 15 );

			SetSkill( SkillName.MagicResist, 5.5 );
			SetSkill( SkillName.Tactics, 5.5 );
			SetSkill( SkillName.Wrestling, 5.5 );

			Fame = 300;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1; // TODO: Verify Controle slots and MinTaming and TReq for transfer
		}

		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } } // TODO: Verify FoodType

		public Ferret( Serial serial )
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
