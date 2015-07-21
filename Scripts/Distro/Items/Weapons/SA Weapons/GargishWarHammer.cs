using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48C1, 0x48C0 )]
	public class GargishWarHammer : BaseBashing
	{
		public override int LabelNumber { get { return 1097496; } } // gargish war hammer

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.CrushingBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 95; } }
		public override int MinDamage { get { return 17; } }
		public override int MaxDamage { get { return 18; } }
		public override int Speed { get { return 15; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		public override WeaponAnimation Animation { get { return WeaponAnimation.Bash2H; } }

		[Constructable]
		public GargishWarHammer()
			: base( 0x48C1 )
		{
			Weight = 10.0;
			Layer = Layer.TwoHanded;
		}

		public GargishWarHammer( Serial serial )
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