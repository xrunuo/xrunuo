using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class TunicOfTheGrizzle : BoneChest, ISetItem
	{
		public override int LabelNumber { get { return 1074467; } } // Tunic of the Grizzle

		public override int BasePhysicalResistance { get { return 6; } }
		public override int BaseFireResistance { get { return 10; } }
		public override int BaseColdResistance { get { return 5; } }
		public override int BasePoisonResistance { get { return 7; } }
		public override int BaseEnergyResistance { get { return 10; } }

		[Constructable]
		public TunicOfTheGrizzle()
		{
			Attributes.NightSight = 1;
			Attributes.BonusHits = 5;
			ArmorAttributes.MageArmor = 1;
		}

		public TunicOfTheGrizzle( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( MonstrousInterredGrizzleSet.FullSet( parent as Mobile ) )
					MonstrousInterredGrizzleSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile && Hue == 632 )
			{
				MonstrousInterredGrizzleSet.RemoveBonus( this );
				MonstrousInterredGrizzleSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			MonstrousInterredGrizzleSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			MonstrousInterredGrizzleSet.GetPropertiesSecond( list, this );
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