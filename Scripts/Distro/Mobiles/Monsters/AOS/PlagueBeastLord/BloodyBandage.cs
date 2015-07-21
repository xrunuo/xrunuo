using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
	[FlipableAttribute( 0xE20, 0xE22 )]
	public class BloodyBandage : Item
	{

		[Constructable]
		public BloodyBandage()
			: this( 1 )
		{
		}

		[Constructable]
		public BloodyBandage( int amount )
			: base( 0xE20 )
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public BloodyBandage( Serial serial )
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

			/*int version = */reader.ReadInt();
		}

		//This section can be uncommented if you want washable bandages.
		/*		public override void OnDoubleClick( Mobile from )
				{
					if ( from.InRange( GetWorldLocation(), 1 ) )
					{
						from.RevealingAction();

						from.SendMessage( "Where do you want to wash the bloody bandages?"  );

						from.Target = new InternalTarget( this );
					}
					else
					{
						from.SendMessage( "You are too far away" );
					}
				}

				private class InternalTarget : Target
				{
					private static int[] m_WaterTiles = new int[]
					{
						0x00A8, 0x00AB,
						0x0136, 0x0137,
						0x5797, 0x579C,
						0x746E, 0x7485,
						0x7490, 0x74AB,
						0x74B5, 0x75D5,
						0x0B41, 0x0B42,
						0x0B43, 0x0B44
					};

					private BloodyBandage m_Bandage;

					public InternalTarget( BloodyBandage bandage ) : base( 1, false, TargetFlags.Beneficial )
					{
						m_Bandage = bandage;
					}

					protected override void OnTarget( Mobile from, object targeted )
					{
						if ( m_Bandage.Deleted )
							return;
						if (targeted is StaticTarget)
						{
							StaticTarget obj = (StaticTarget)targeted;
							 if ((obj.ItemID > 6025 && obj.ItemID < 6077) || (obj.ItemID > 13420 && obj.ItemID < 13529))
							 {
								 if ( from.InRange( m_Bandage.GetWorldLocation(), 2 ) )
								 {
									 int amount = m_Bandage.Amount;
									 m_Bandage.Consume( amount );
									 from.AddToBackpack( new Bandage( amount ) );
								 }
								 else
								 {
									 from.SendMessage( "You are too far away" );
								 }
							 }
							 else
							 {
								 from.SendMessage ( "You must target water to clean the bandages");
							 }
						 }
					 }
					 private bool checkIsWater( object o, Mobile from )
					 {
						 if( o is IWaterSource )
						 {
							 return true;
						 }
						 else
							 return false;
					 }
				 }*/
	}
}
