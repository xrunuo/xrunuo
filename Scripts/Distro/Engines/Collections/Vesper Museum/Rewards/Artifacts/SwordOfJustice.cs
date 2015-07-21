using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SwordOfJustice : VikingSword, ICollectionItem
	{
		public override int LabelNumber { get { return 1073261; } } // Sword of Justice - Museum of Vesper Replica

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 80; } }

		private static SlayerName[] m_SuperSlayers = new SlayerName[]
			{
				SlayerName.Demon,
				SlayerName.Undead,
				SlayerName.Repond,
				SlayerName.Arachnid,
				SlayerName.Reptile
			};

		[Constructable]
		public SwordOfJustice()
		{
			Hue = 1150;
			Slayer = m_SuperSlayers[Utility.Random( m_SuperSlayers.Length )]; ;
			WeaponAttributes.HitLowerAttack = 60;
			Attributes.SpellChanneling = 1;
			Attributes.Luck = 100;
			Attributes.WeaponDamage = 50;
			Resistances.Physical = 20;
		}

		public SwordOfJustice( Serial serial )
			: base( serial )
		{
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