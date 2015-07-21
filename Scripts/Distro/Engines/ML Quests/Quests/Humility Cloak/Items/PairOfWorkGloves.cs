using System;
using Server;

namespace Server.Items
{
	public class PairOfWorkGloves : LeatherGloves
	{
		public override int LabelNumber { get { return 1075780; } } // Pair of Work Gloves

		[Constructable]
		public PairOfWorkGloves()
		{
			LootType = LootType.Blessed;
			Hue = 0x33C;
		}

		public override bool NonTransferable { get { return true; } }

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1072351 ); // Quest Item
		}

		public PairOfWorkGloves( Serial serial )
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