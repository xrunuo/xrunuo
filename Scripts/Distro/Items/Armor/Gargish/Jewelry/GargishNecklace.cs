using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[TypeAlias( "Server.Items.GargishNecklaceArmor" )]
	public class GargishNecklace : BaseArmor
	{
		public override int BasePhysicalResistance { get { return 1; } }
		public override int BaseFireResistance { get { return 2; } }
		public override int BaseColdResistance { get { return 2; } }
		public override int BasePoisonResistance { get { return 2; } }
		public override int BaseEnergyResistance { get { return 3; } }

		public override int InitMinHits { get { return 25; } }
		public override int InitMaxHits { get { return 35; } }

		public override int StrengthReq { get { return 10; } }

		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }
		public override CraftResource DefaultResource { get { return CraftResource.Iron; } }

		public override bool Meditable { get { return true; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		[Constructable]
		public GargishNecklace()
			: base( 0x4210 )
		{
			Layer = Layer.Neck;
			Weight = 1.0;
		}

		public GargishNecklace( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version < 1 )
			{
				int oldHue = Hue;
				Resource = CraftResource.Iron;
				Hue = oldHue;

				ArmorAttributes.MageArmor = 0;
			}
		}

		public override bool OnCraft( bool exceptional, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			Type resourceType = typeRes;

			if ( resourceType == null )
				resourceType = craftItem.Ressources.GetAt( 0 ).ItemType;

			var resource = CraftResources.GetFromType( resourceType );

			Hue = CraftResources.GetHue( resource );

			return base.OnCraft( exceptional, makersMark, from, craftSystem, null, tool, craftItem, resHue );
		}
	}
}
