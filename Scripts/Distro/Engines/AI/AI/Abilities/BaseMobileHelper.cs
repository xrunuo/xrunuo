using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server
{
	public class BaseMobileHelper
	{
		private static void Undress( ArrayList items )
		{
			foreach ( Item i in items )
			{
				i.Delete();
			}

			items.Clear();
		}

		private static void Dress( Mobile m, ArrayList items )
		{
			foreach ( Item i in items )
			{
				m.EquipItem( i );
			}
		}

		public static void ShowMorphEffect( Mobile m )
		{
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );

			Effects.PlaySound( m, m.Map, 0x201 );
		}

		public static void Return( Mobile m, ArrayList items )
		{
			ShowMorphEffect( m );

			m.BodyMod = 0;
			m.HueMod = -1;
			m.NameMod = null;
			m.Title = "";

			Undress( items );
		}

		public static void Turn( Mobile m, ArrayList items, int bodymod, int bodyhue, string namemod, string title, bool criminal )
		{
			ShowMorphEffect( m );

			m.BodyMod = bodymod;
			m.HueMod = bodyhue;

			if ( namemod != null )
			{
				m.NameMod = namemod;
			}

			if ( title != null )
			{
				m.Title = title;
			}

			m.Criminal = criminal;

			Dress( m, items );
		}

		public static int GetRandomHue()
		{
			switch ( Utility.Random( 6 ) )
			{
				case 0:
					return 0;
				case 1:
					return Utility.RandomBlueHue();
				case 2:
					return Utility.RandomGreenHue();
				case 3:
					return Utility.RandomRedHue();
				case 4:
					return Utility.RandomYellowHue();
				case 5:
					return Utility.RandomNeutralHue();
			}

			return 0;
		}

		public static Item GetRandomHair()
		{
			Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2048 ) );

			hair.Hue = Utility.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;

			return hair;
		}

		public static Item GetRandomBeard( int hue )
		{
			Item beard = new Item( Utility.RandomList( 0x0000, 0x203E, 0x203F, 0x2040, 0x2041, 0x2067, 0x2068, 0x2069 ) );

			beard.Hue = hue;
			beard.Layer = Layer.FacialHair;
			beard.Movable = false;

			return beard;
		}

		public static Item GetRandomShirt()
		{
			Item shirt = null;

			switch ( Utility.Random( 5 ) )
			{
				case 0:
					shirt = new Doublet( GetRandomHue() );
					break;
				case 1:
					shirt = new Surcoat( GetRandomHue() );
					break;
				case 2:
					shirt = new Tunic( GetRandomHue() );
					break;
				case 3:
					shirt = new FancyShirt( GetRandomHue() );
					break;
				case 4:
					shirt = new Shirt( GetRandomHue() );
					break;
			}

			return shirt;
		}

		public static Item GetRandomHat()
		{
			Item hat = null;

			switch ( Utility.Random( 6 ) )
			{
				case 0:
					hat = new SkullCap( GetRandomHue() );
					break;
				case 1:
					hat = new Bandana( GetRandomHue() );
					break;
				case 2:
					hat = new WideBrimHat();
					break;
				case 3:
					hat = new TallStrawHat( Utility.RandomNeutralHue() );
					break;
				case 4:
					hat = new StrawHat( Utility.RandomNeutralHue() );
					break;
				case 5:
					hat = new TricorneHat( Utility.RandomNeutralHue() );
					break;
			}

			return hat;
		}

		public static Item GetRandomPants()
		{
			Item pants = null;

			switch ( Utility.Random( 2 ) )
			{
				case 0:
					pants = new ShortPants( GetRandomHue() );
					break;
				case 1:
					pants = new LongPants( GetRandomHue() );
					break;
			}

			return pants;
		}

		public static Item GetRandomFeet()
		{
			Item feet = null;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					feet = new Boots( Utility.RandomNeutralHue() );
					break;
				case 1:
					feet = new Shoes( Utility.RandomNeutralHue() );
					break;
				case 2:
					feet = new Sandals( Utility.RandomNeutralHue() );
					break;
			}

			return feet;
		}
	}
}