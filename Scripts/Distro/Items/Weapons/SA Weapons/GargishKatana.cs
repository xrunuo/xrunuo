using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48BB, 0x48BA )]
	public class GargishKatana : BaseSword
	{
		public override int LabelNumber { get { return 1097490; } } // gargish Katana

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ArmorIgnore; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 25; } }
		public override int MinDamage { get { return 11; } }
		public override int MaxDamage { get { return 13; } }
		public override int Speed { get { return 10; } }

		public override int HitSound { get { return 0x23B; } }
		public override int MissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 90; } }

		[Constructable]
		public GargishKatana()
			: base( 0x48BB )
		{
			Weight = 6.0;
		}

		public GargishKatana( Serial serial )
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