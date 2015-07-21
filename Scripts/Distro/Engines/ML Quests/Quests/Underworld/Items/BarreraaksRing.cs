using System;
using Server;

namespace Server.Items
{
	public class BarreraaksRing : GoldRing
	{
		public override int LabelNumber { get { return 1095049; } } // Barreraak’s Old Beat Up Ring

		[Constructable]
		public BarreraaksRing()
		{
			LootType = LootType.Blessed;
		}

		public BarreraaksRing( Serial serial )
			: base( serial )
		{
		}

		public override bool OnEquip( Mobile from )
		{
			if ( !base.OnEquip( from ) )
			{
				return false;
			}
			else if ( from.Mounted )
			{
				from.SendLocalizedMessage( 1010097 ); // You cannot use this while mounted.
				return false;
			}
			else if ( from.Flying )
			{
				from.SendLocalizedMessage( 1113414 ); // You can't use this while flying!
				return false;
			}
			else if ( from.IsBodyMod )
			{
				from.SendLocalizedMessage( 1111896 ); // You may only change forms while in your original body.
				return false;
			}

			return true;
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				Mobile from = parent as Mobile;

				from.BodyMod = 723;
				from.HueMod = 2301;

				from.FixedParticles( 0x3728, 1, 13, 5042, EffectLayer.Waist );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
			{
				Mobile from = parent as Mobile;

				from.BodyMod = 0;
				from.HueMod = -1;

				from.FixedParticles( 0x3728, 1, 13, 5042, EffectLayer.Waist );
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