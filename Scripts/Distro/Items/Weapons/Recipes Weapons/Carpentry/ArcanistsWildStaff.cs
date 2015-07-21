using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class ArcanistsWildStaff : WildStaff
	{
		public override int LabelNumber { get { return 1073549; } } // Arcanist's Wild Staff 

		[Constructable]
		public ArcanistsWildStaff()
		{
			Attributes.BonusMana = 3;
			Attributes.WeaponDamage = 3;
		}

		public ArcanistsWildStaff( Serial serial )
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