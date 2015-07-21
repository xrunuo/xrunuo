using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Harvest;
using Server.ContextMenus;

namespace Server.Items
{
	public abstract class BasePoleArm : BaseMeleeWeapon
	{
		public override int HitSound { get { return 0x237; } }
		public override int MissSound { get { return 0x238; } }

		public override SkillName Skill { get { return SkillName.Swords; } }
		public override WeaponType Type { get { return WeaponType.Polearm; } }
		public override WeaponAnimation Animation { get { return WeaponAnimation.Slash2H; } }

		public virtual HarvestSystem HarvestSystem { get { return Lumberjacking.System; } }

		public BasePoleArm( int itemID )
			: base( itemID )
		{
		}

		public BasePoleArm( Serial serial )
			: base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( HarvestSystem == null )
				return;

			if ( IsChildOf( from.Backpack ) || Parent == from )
				HarvestSystem.BeginHarvesting( from, this );
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list )
		{
			base.GetContextMenuEntries( from, list );

			if ( HarvestSystem != null )
				BaseHarvestTool.AddContextMenuEntries( from, this, list, HarvestSystem );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
					{
						reader.ReadBool(); // show uses remaining
						goto case 1;
					}
				case 1:
					{
						reader.ReadInt(); // uses remaining
						goto case 0;
					}
				case 0:
					{
						break;
					}
			}
		}
	}
}
