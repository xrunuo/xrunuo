using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class UzeraanHordeMinion : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public UzeraanHordeMinion()
			: base( AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			Name = "a horde minion";
			Body = 776;

			SetStr( 10 );
			SetDex( 10 );
			SetInt( 10 );

			SetHits( 100 );

			SetDamage( 0 );

			CantWalk = true;

			Direction = Direction.South;
		}

		public UzeraanHordeMinion( Serial serial )
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