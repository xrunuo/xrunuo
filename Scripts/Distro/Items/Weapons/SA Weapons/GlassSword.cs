using System;
using Server;

namespace Server.Items
{
	[FlipableAttribute( 0x090C, 0x4073 )]
	public class GlassSword : BaseSword
	{
		public override Race RequiredRace { get { return Race.Gargoyle; } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MortalStrike; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override int Speed { get { return 11; } }

		public override int InitMinHits { get { return 60; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public GlassSword()
			: base( 0x4073 )
		{
			Weight = 6.0;
		}

		public GlassSword( Serial serial )
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