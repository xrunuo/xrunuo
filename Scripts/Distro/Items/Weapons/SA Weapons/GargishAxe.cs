using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48B3, 0x48B2 )]
	public class GargishAxe : BaseAxe
	{
		public override int LabelNumber { get { return 1097482; } } // gargish axe

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Dismount; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 14; } }
		public override int MaxDamage { get { return 16; } }
		public override int Speed { get { return 12; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		[Constructable]
		public GargishAxe()
			: base( 0x48B3 )
		{
			Weight = 4.0;
		}

		public GargishAxe( Serial serial )
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
	}
}