using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class BlackthornsKryss : Kryss, ICollectionItem
	{
		public override int LabelNumber { get { return 1073260; } } // Blackthorn's Kryss - Museum of Vesper Replica

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
		public BlackthornsKryss()
		{
			Hue = 1509;
			Attributes.WeaponSpeed = 25;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.UseBestSkill = 1;
			Slayer = (SlayerName) Utility.RandomList( m_SuperSlayers );
			WeaponAttributes.HitLeechHits = 30;
		}

		public BlackthornsKryss( Serial serial )
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