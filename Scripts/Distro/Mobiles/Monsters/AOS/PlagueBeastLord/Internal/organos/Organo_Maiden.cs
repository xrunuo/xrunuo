using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
	public class Organo_Maiden : BaseOrgano
	{



		public Organo_Maiden( BrainTypes cerebro )
			: base( 4685, Plague_Texts.Line[15], 0x487/*66B*/, cerebro )
		{
		}

		public Organo_Maiden( Serial serial )
			: base( serial )
		{
		}

		public override void CrearVisceras()
		{
			Mybrain = new PlagueBrain( HoldBrain );
			PonerSangre( Mybrain, 20, 30 );
			PlagueBrain Cere = Mybrain as PlagueBrain;
			Cere.CrearVisceras( 4652, -4, -5 );
		}


		public override void Abrir( Mobile from )
		{
		}

		public override bool IsAccessibleTo( Mobile check )
		{
			return true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			PlagueBeastLord m = RootParent as PlagueBeastLord;
			if ( m == null ) return false;

			from.RevealingAction();
			from.Direction = from.GetDirectionTo( m );
			if ( Abierto )
			{
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[21] );
			}
			else
			{
				ItemID = 4682;
				base.Abrir( from );
				from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[22] );
				from.PlaySound( 0x2AC );
			}
			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */reader.ReadInt();
		}
	}
}
