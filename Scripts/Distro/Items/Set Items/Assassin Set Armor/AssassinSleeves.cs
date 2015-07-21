using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class AssassinSleeves : LeatherArms, ISetItem
	{
		public override int LabelNumber { get { return 1074304; } } // Assassin Armor

		public override int BasePhysicalResistance { get { return 9; } }
		public override int BaseFireResistance { get { return 6; } }
		public override int BaseColdResistance { get { return 3; } }
		public override int BasePoisonResistance { get { return 8; } }
		public override int BaseEnergyResistance { get { return 4; } }

		[Constructable]
		public AssassinSleeves()
		{
			Attributes.BonusStam = 2;
			Attributes.WeaponSpeed = 5;
		}

		public AssassinSleeves( Serial serial )
			: base( serial )
		{
		}

		public override void OnAdded( object parent )
		{
			base.OnAdded( parent );

			if ( parent is Mobile )
			{
				if ( AssassinSet.FullSet( parent as Mobile ) )
					AssassinSet.ApplyBonus( parent as Mobile );
			}
		}

		public override void OnRemoved( object parent )
		{
			base.OnRemoved( parent );

			if ( parent is Mobile )
			{
				AssassinSet.RemoveBonus( this );
				AssassinSet.RemoveBonus( parent as Mobile );
			}
		}

		public override void GetSetArmorPropertiesFirst( ObjectPropertyList list )
		{
			AssassinSet.GetPropertiesFirst( list, this );
		}

		public override void GetSetArmorPropertiesSecond( ObjectPropertyList list )
		{
			AssassinSet.GetPropertiesSecond( list, this );
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