using System;
using Server;

namespace Server.Items
{
	public abstract class BaseLibraryQuotes : MonsterStatuette
	{
		public virtual int MsgMin { get { return 1073301; } }
		public virtual int MsgMax { get { return 1073309; } }

		private static readonly int[] m_Sounds = new int[]
			{
				0x32F, 0x420, 0x312, 0x41B, 0x30C, 0x310, 0x421, 0x41E,
				0x311, 0x421, 0x30F, 0x441, 0x41F
			};

		public BaseLibraryQuotes()
			: base( MonsterStatuetteType.CollectionInteractiveStatue )
		{
			Hue = 450;
			ItemID = 0xFBD;
			LootType = LootType.Blessed;
		}

		public BaseLibraryQuotes( Serial serial )
			: base( serial )
		{
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( TurnedOn && IsLockedDown && ( !m.Hidden || true/*m.AccessLevel == AccessLevel.Player*/ ) && this.InRange( m, 2 ) && !this.InRange( oldLocation, 2 ) )
			{
				this.PublicOverheadMessage( Network.MessageType.Regular, 0, Utility.RandomMinMax( MsgMin, MsgMax ) );

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