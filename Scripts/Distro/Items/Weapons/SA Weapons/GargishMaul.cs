using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48C3, 0x48C2 )]
	public class GargishMaul : BaseBashing
	{
		public override int LabelNumber { get { return 1097498; } } // gargish maul

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 45; } }
		public override int MinDamage { get { return 14; } }
		public override int MaxDamage { get { return 16; } }
		public override int Speed { get { return 14; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		[Constructable]
		public GargishMaul()
			: base( 0x48C3 )
		{
			Weight = 10.0;
		}

		public GargishMaul( Serial serial )
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