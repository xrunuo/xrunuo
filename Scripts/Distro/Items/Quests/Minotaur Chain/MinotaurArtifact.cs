using System;
using Server;
using Server.Items;

namespace Server.Engines.MLQuests
{
	public class MinotaurArtifact : Item
	{
		public override int LabelNumber { get { return 1074826; } } // Minotaur Artifact

		[Constructable]
		public MinotaurArtifact()
		{
			LootType = LootType.Blessed;
			Hue = 256;

			switch ( Utility.RandomMinMax( 1, 3 ) )
			{
				default:
				case 1: ItemID = 0x9ED; Weight = 30.0; break;
				case 2: ItemID = 0xB46; Weight = 5.0; break;
				case 3: ItemID = 0xB48; Weight = 5.0; break;
			}
		}

		public MinotaurArtifact( Serial serial )
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
