using System;
using Server.Items;

namespace Server.Items
{
	public class LightPlateJingasa : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 2; } }

		public override int InitMinHits { get { return 55; } }
		public override int InitMaxHits { get { return 60; } }

		public override int StrengthReq { get { return 55; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		[Constructable]
		public LightPlateJingasa()
			: base( 0x2781 )
		{
			Weight = 5.0;
		}

		public LightPlateJingasa( Serial serial )
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