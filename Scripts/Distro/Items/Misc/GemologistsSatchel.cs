using System;
using Server;

namespace Server.Items
{
	public class GemologistsSatchel : Bag
	{
		public override int LabelNumber { get { return 1113378; } } // Gemologist's Satchel

		private static Type[] m_LesserGems = new Type[]
			{
				typeof( Amber ),		typeof( Amethyst ),
				typeof( Citrine ),		typeof( Diamond ),
				typeof( Emerald ),		typeof( Ruby ),
				typeof( Sapphire ),		typeof( StarSapphire ),
				typeof( Tourmaline )
			};

		private static Type[] m_GreaterGems = new Type[]
			{
				typeof( BlueDiamond ),	typeof( BrilliantAmber ),
				typeof( DarkSapphire ),	typeof( EcruCitrine ),
				typeof( FireRuby ),		typeof( PerfectEmerald ),
				typeof( Turquoise ),	typeof( WhitePearl )
			};

		[Constructable]
		public GemologistsSatchel()
		{
			Hue = 1177;

			DropGems( m_LesserGems, 4 );
			DropGems( m_GreaterGems, 2 );
		}

		private void DropGems( Type[] types, int amount )
		{
			for ( int i = 0; i < amount; i++ )
			{
				Item item = (Item) Activator.CreateInstance( types[Utility.Random( types.Length )] );
				DropItem( item );
			}
		}

		public GemologistsSatchel( Serial serial )
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