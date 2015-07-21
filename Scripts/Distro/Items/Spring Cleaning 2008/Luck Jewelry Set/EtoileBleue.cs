using System;
using Server;

namespace Server.Items
{
	public class EtoileBleue : GoldRing, ISetItem
	{
		public override int LabelNumber { get { return 1080238; } } // Etoile Bleue

		[Constructable]
		public EtoileBleue()
		{
			Attributes.Luck = 150;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 1;

			Hue = 1165;
		}

		public EtoileBleue( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( LuckJewelrySet.FullSet( parent as Mobile ) )
					LuckJewelrySet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
				LuckJewelrySet.RemoveBonus( parent as Mobile );
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			LuckJewelrySet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			LuckJewelrySet.GetPropertiesSecond( list, this );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
				reader.ReadBool();
		}
	}
}