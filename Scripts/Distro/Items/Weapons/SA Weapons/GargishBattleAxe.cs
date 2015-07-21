using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x48B1, 0x48B0 )]
	public class GargishBattleAxe : BaseAxe
	{
		public override int LabelNumber { get { return 1097480; } } // gargish battle axe

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }

		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override int StrengthReq { get { return 35; } }
		public override int MinDamage { get { return 15; } }
		public override int MaxDamage { get { return 17; } }
		public override int Speed { get { return 14; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		[Constructable]
		public GargishBattleAxe()
			: base( 0x48B1 )
		{
			Weight = 4.0;
			Layer = Layer.TwoHanded;
		}

		public GargishBattleAxe( Serial serial )
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