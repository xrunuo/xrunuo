using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class TerMurVeterinarian : Veterinarian
	{
		public override Race DefaultRace { get { return Race.Gargoyle; } }

		[Constructable]
		public TerMurVeterinarian()
		{
			Title = "the Vet";
		}

		public TerMurVeterinarian( Serial serial )
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