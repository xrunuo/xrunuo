using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class LeprechaunCloak : Cloak
	{
		[Constructable]
		public LeprechaunCloak()
		{
			Name = "A Leprechaun Cloak";

			Hue = 0x23D;
		}

		public LeprechaunCloak( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class LeprechaunPants : LongPants
	{
		[Constructable]
		public LeprechaunPants()
		{
			Name = "A pair of Leprechaun Pants";

			Hue = 0x23D;
		}

		public LeprechaunPants( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class LeprechaunBoots : Boots
	{
		[Constructable]
		public LeprechaunBoots()
		{
			Name = "A pair of Leprechaun Boots";

			Hue = 0x1;
		}

		public LeprechaunBoots( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class LeprechaunShirt : Shirt
	{
		[Constructable]
		public LeprechaunShirt()
		{
			Name = "A Leprechaun Shirt";
		}

		public LeprechaunShirt( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class LeprechaunFeatheredHat : FeatheredHat
	{
		[Constructable]
		public LeprechaunFeatheredHat()
		{
			Name = "A Leprechaun hat";

			Hue = 0x23D;
		}

		public LeprechaunFeatheredHat( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}

	public class LeprechaunFloppyHat : FloppyHat
	{
		[Constructable]
		public LeprechaunFloppyHat()
		{
			Name = "A Leprechaun hat";

			Hue = 0x23D;
		}

		public LeprechaunFloppyHat( Serial serial )
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

			/*int version = */reader.ReadInt();
		}
	}
}