using System;
using Server;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	public class GargishInitiationLeggings : GargishLeatherLeggings, ISetItem
	{
		public override int LabelNumber { get { return 1116255; } } // Armor of Initiation

		public override int BasePhysicalResistance { get { return 7; } }
		public override int BaseFireResistance { get { return 4; } }
		public override int BaseColdResistance { get { return 4; } }
		public override int BasePoisonResistance { get { return 6; } }
		public override int BaseEnergyResistance { get { return 4; } }

		public override int InitMaxHits { get { return 150; } }
		public override int InitMinHits { get { return 150; } }

		public override bool Brittle { get { return true; } }

		[Constructable]
		public GargishInitiationLeggings()
		{
			Hue = 2101;
			LootType = LootType.Blessed;
		}

		public GargishInitiationLeggings( Serial serial )
			: base( serial )
		{
		}

		public override bool CanEquip( Mobile from )
		{
			Account acct = from.Account as Account;
			TimeSpan totalTime = ( DateTime.UtcNow - acct.Created );
			if ( totalTime >= TimeSpan.FromDays( 30.0 ) )
			{
				from.SendLocalizedMessage( 1116259 ); // This can only be used by accounts less than 1 month old.
				return false;
			}
			else
			{
				return base.CanEquip( from );
			}
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( InitiationSet.FullSet( parent as Mobile ) )
					InitiationSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			this.Hue = 2101;
			if ( parent is Mobile )
				InitiationSet.RemoveBonus( parent as Mobile );
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			InitiationSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			InitiationSet.GetPropertiesSecond( list, this );
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