using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class ServantOfSemidar : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public ServantOfSemidar()
			: base( AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			Name = "a servant of semidar";
			Body = 38;

			SetStr( 10 );
			SetDex( 10 );
			SetInt( 10 );

			SetHits( 100 );

			SetDamage( 0 );

			CantWalk = true;

			Direction = Direction.East;
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1005494 ); // enslaved
		}

		public ServantOfSemidar( Serial serial )
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