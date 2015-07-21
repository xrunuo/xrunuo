using System;
using Server;
using Server.Items;

namespace Server.Engines.Quests
{
	public class MagicalRope : TransientItem
	{
		public override int LabelNumber { get { return 1074338; } } // Magical Rope

		[Constructable]
		public MagicalRope()
			: base( 0x020D, TimeSpan.FromSeconds( 600 ) )
		{
			LootType = LootType.Blessed;
			Weight = 5.0;
		}

		public MagicalRope( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
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