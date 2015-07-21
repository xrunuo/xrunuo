using System;
using Server;

namespace Server.Items
{
	public class ChannelersDefender : GlassSword
	{
		public override int LabelNumber { get { return 1113518; } } // Channeler's Defender

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		public override bool CanImbue { get { return false; } }

		[Constructable]
		public ChannelersDefender()
		{
			Hue = 195;

			WeaponAttributes.HitLowerAttack = 60;
			Attributes.SpellChanneling = 1;
			Attributes.CastSpeed = 1;
			Attributes.AttackChance = 5;
			Attributes.DefendChance = 10;
			Attributes.CastRecovery = 1;
			Attributes.WeaponSpeed = 20;
			Attributes.LowerManaCost = 5;
		}

		public ChannelersDefender( Serial serial )
			: base( serial )
		{
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			phys = fire = pois = cold = 0;
			nrgy = 100;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}