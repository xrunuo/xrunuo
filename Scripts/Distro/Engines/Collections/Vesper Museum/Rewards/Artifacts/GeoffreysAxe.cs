using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class GeoffreysAxe : ExecutionersAxe, ICollectionItem
	{
		public override int LabelNumber { get { return 1073263; } } // Geoffrey's Axe - Museum of Vesper Replica

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }

		private static int[] m_SuperSlayers = new int[]
			{
				(int)SlayerName.Demon,
				(int)SlayerName.Undead,
				(int)SlayerName.Repond,
				(int)SlayerName.Arachnid,
				(int)SlayerName.Reptile
			};

		[Constructable]
		public GeoffreysAxe()
		{
			Hue = 33;
			Slayer = (SlayerName) Utility.RandomList( m_SuperSlayers );
			Attributes.BonusStr = 10;
			Attributes.Luck = 150;
			Attributes.AttackChance = 15;
			Attributes.WeaponDamage = 40;
			Resistances.Fire = 10;
		}

		public GeoffreysAxe( Serial serial )
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

			/*int version = */
			reader.ReadInt();
		}
	}
}