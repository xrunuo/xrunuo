using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48CC, 0x48CD )]
	public class GargishTessen : BaseBashing
	{
		public override int LabelNumber { get { return 1097508; } } // gargish Tessen

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.Feint; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Block; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 10; } }
		public override int MinDamage { get { return 10; } }
		public override int MaxDamage { get { return 12; } }
		public override int Speed { get { return 8; } }

		public override int HitSound { get { return 0x232; } }
		public override int MissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash2H; } }

		[Constructable]
		public GargishTessen()
			: base( 0x48CC )
		{
			Weight = 6.0;
			Layer = Layer.TwoHanded;
		}

		public GargishTessen( Serial serial )
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