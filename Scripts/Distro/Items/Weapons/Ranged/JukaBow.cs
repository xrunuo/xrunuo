using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class JukaBow : Bow
	{
		public override int StrengthReq { get { return 80; } }
		public override int DexterityReq { get { return 80; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsModified { get { return ( Hue == 0x453 ); } }

		[Constructable]
		public JukaBow()
		{
			Name = "Juka Bow";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsModified )
			{
				from.SendMessage( "That has already been modified." );
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "This must be in your backpack to modify it." );
			}
			else if ( from.Skills[SkillName.Fletching].Base < 100.0 )
			{
				from.SendMessage( "Only a grandmaster bowcrafter can modify this weapon." );
			}
			else
			{
				from.BeginTarget( 2, false, Targeting.TargetFlags.None, new TargetCallback( OnTargetGears ) );
				from.SendLocalizedMessage( 1010614 ); // Which set of gears will you use?
			}
		}

		public void OnTargetGears( Mobile from, object targ )
		{
			Gears g = targ as Gears;

			if ( g == null || !g.IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1010623 ); // You must use gears.
			}
			else if ( IsModified )
			{
				from.SendMessage( "That has already been modified." );
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage( "This must be in your backpack to modify it." );
			}
			else if ( from.Skills[SkillName.Fletching].Base < 100.0 )
			{
				from.SendMessage( "Only a grandmaster bowcrafter can modify this weapon." );
			}
			else
			{
				g.Consume();

				Hue = 0x453;

				Slayer = BaseRunicTool.GetRandomSlayer();

				if ( 0.05 > Utility.RandomDouble() )
					Slayer2 = BaseRunicTool.GetRandomSlayer();

				from.SendMessage( "You modify it." );
			}
		}

		public JukaBow( Serial serial )
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