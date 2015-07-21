using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Engines.Imbuing;

namespace Server.Items
{
	public class BaseShield : BaseArmor
	{
		public override ArmorMaterialType MaterialType { get { return ArmorMaterialType.Plate; } }

		public BaseShield( int itemID )
			: base( itemID )
		{
		}

		public BaseShield( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();
		}

		protected override void AddHeartwoodBonus()
		{
			switch ( Utility.RandomMinMax( 1, 7 ) )
			{
				case 1:
					Attributes.BonusDex += 2;
					break;
				case 2:
					Attributes.BonusStr += 2;
					break;
				case 3:
					ArmorAttributes.SelfRepair += 2;
					break;
				case 4:
					Attributes.SpellChanneling = 1;
					break;
				case 5:
					ColdBonus += 3;
					break;
				case 6:
					PhysicalBonus += 5;
					break;
				case 7:
					Attributes.ReflectPhysical += 5;
					break;
				default:
					break;
			}
		}

		public override bool AllowEquipedCast( Mobile from )
		{
			if ( Attributes.SpellChanneling > 0 )
				return true;

			return base.AllowEquipedCast( from );
		}

		#region Imbuing
		public override ImbuingFlag ImbuingFlags { get { return ImbuingFlag.Shield; } }

		public override int MaxIntensity
		{
			get { return Exceptional ? 450 : 400; }
		}
		#endregion
	}
}
