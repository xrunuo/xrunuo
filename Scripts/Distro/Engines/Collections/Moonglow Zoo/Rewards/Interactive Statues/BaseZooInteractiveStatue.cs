using System;
using Server;

namespace Server.Items
{
	public abstract class BaseZooInteractiveStatue : MonsterStatuette
	{
		public override bool EnableSound { get { return false; } }

		private static readonly int[] m_Sounds = new int[]
			{
				0xF3, 0xFFFF, 0xDC, 0xBA, 0xB0, 0x82, 0xE7, 0x6E, 0xB4,
				0xDB, 0x76, 0xE5, 0xE1, 0xA6, 0x7B, 0xD0, 0x6D, 0xC2, 0x93,
				0x75, 0xE2, 0x76, 0xC0, 0xB1
			};

		public BaseZooInteractiveStatue( int itemid )
			: base( MonsterStatuetteType.CollectionInteractiveStatue )
		{
			ItemID = itemid;
			LootType = LootType.Regular;
		}

		public BaseZooInteractiveStatue( Serial serial )
			: base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( TurnedOn && IsLockedDown && ( !m.Hidden || m.AccessLevel == AccessLevel.Player ) && this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) )
			{
				this.PublicOverheadMessage( Network.MessageType.Regular, 0, Utility.RandomMinMax( 1073207, 1073218 ) );

				Effects.PlaySound( this.Location, this.Map, m_Sounds[Utility.Random( m_Sounds.Length )] );
			}

			base.OnMovement( m, oldLocation );
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