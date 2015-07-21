using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
	public class AcidPooper : Item
	{
		public override int LabelNumber { get { return 1095058; } } // Acid Pooper

		[Constructable]
		public AcidPooper()
			: base( 0x2808 )
		{
			Weight = 1.0;
			Stackable = true;

			Hue = 0x3F;
		}

		public AcidPooper( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042664 ); // You must have the object in your backpack to use it.
			else
				from.BeginTarget( 1, false, TargetFlags.None, new TargetCallback( BurnWeb_Callback ) );
		}

		private void BurnWeb_Callback( Mobile from, object targeted )
		{
			NavreysWeb web = targeted as NavreysWeb;

			if ( web != null )
			{
				from.SendLocalizedMessage( 1113240 ); // The acid popper bursts and burns away the webbing.
				from.Frozen = false;

				Effects.SendPacket( from.Location, from.Map, new TargetParticleEffect( this, 0x374A, 1, 10, 0x557, 0, 0x139D, 3, 0 ) );

				from.PlaySound( 0x3E );
				from.PlaySound( 0x22F );

				web.Burn();
				Consume();
			}
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