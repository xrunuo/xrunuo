using System;
using Server.Items;

namespace Server.Items
{
	public class PlateHiroSode : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 5; } }
		public override int BaseFireResistance { get { return 3; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 3; } }
		public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 55; } }
		public override int InitMaxHits { get { return 75; } }

		public override int StrengthReq { get { return 75; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		[Constructable]
		public PlateHiroSode()
			: base( 0x2780 )
		{
			Weight = 3.0;
		}

		public PlateHiroSode( Serial serial )
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

		public override bool OnCraft( bool exceptional, bool makersMark, Mobile from, Server.Engines.Craft.CraftSystem craftSystem, Type typeRes, BaseTool tool, Server.Engines.Craft.CraftItem craftItem, int resHue )
		{
			if ( exceptional )
				ArmorAttributes.MageArmor = 1;

			return base.OnCraft( exceptional, makersMark, from, craftSystem, typeRes, tool, craftItem, resHue );
		}
	}
}