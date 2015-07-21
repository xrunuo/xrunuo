using System;
using Server;

namespace Server.Items
{
	public class MelisandesFermentedWine : BaseExplosionPotion
	{
		public override int LabelNumber { get { return 1072114; } } // Melisande's Fermented Wine

		public override int MinDamage { get { return 5; } }
		public override int MaxDamage { get { return 10; } }

		[Constructable]
		public MelisandesFermentedWine()
			: base( PotionEffect.ExplosionLesser )
		{
			ItemID = 0x99B;
			Hue = Utility.RandomList( 465, 11, 1162 );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1074502 ); // It looks explosive.
		}

		public MelisandesFermentedWine( Serial serial )
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