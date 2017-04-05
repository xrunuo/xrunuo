using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;
using Server.Items;
using Server.ContextMenus;
using Server.Gumps;

namespace Server.Mobiles
{
	public class Sphynx : BaseCreature
	{
		public static Hashtable m_Table = new Hashtable();

		public static bool UnderEffect( Mobile m )
		{
			return m_Table.Contains( m );
		}

		[Constructable]
		public Sphynx()
			: base( AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Body = 788;
			Name = "The Sphynx";

			SetStr( 1001, 1200 );
			SetDex( 176, 195 );
			SetInt( 301, 400 );

			SetHits( 1001, 1200 );
			SetStam( 176, 195 );
			SetMana( 301, 400 );

			SetDamage( 14, 18 );

			SetDamageType( ResistanceType.Physical, 85 );
			SetDamageType( ResistanceType.Energy, 15 );

			SetResistance( ResistanceType.Physical, 60, 80 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Wrestling, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Magery, 95.5, 100.0 );
			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.EvalInt, 90.1, 100.0 );
			SetSkill( SkillName.Meditation, 95.1, 1200.0 );

			Fame = 15000;

			Karma = 0; // TODO: Verify

			PackGold( 1000, 1200 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive )
				list.Add( new AskAboutFutureEntry( from, this ) );

			base.AddCustomContextEntries( from, list );
		}

		public Sphynx( Serial serial )
			: base( serial )
		{
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

		public class AskAboutFutureEntry : ContextMenuEntry
		{
			private Sphynx m_Sphynx;

			private Mobile m;

			public AskAboutFutureEntry( Mobile from, Sphynx Sphynx )
				: base( 6199, 8 )
			{
				m_Sphynx = Sphynx;

				m = from;

				Enabled = !UnderEffect( from );
			}

			public override void OnClick()
			{
				m.CloseGump<SphynxGump>();

				m.SendGump( new SphynxGump( m, m_Sphynx ) );
			}
		}

		public class SphynxGump : Gump
		{
			public override int TypeID { get { return 0x294; } }

			private static int[] m_Messages = new int[]
				{
					1060886, // Your endurance shall protect you from your enemies blows. 
					1060887, // A smile will be upon your lips, as you gaze into the infernos.
					1060888, // The ice of ages will embrace you, and you will embrace it alike.
					1060889, // Your blood runs pure and strong.
					1060890, // Your flesh shall endure the power of storms.
					1060891, // Seek riches and they will seek you.
					1060892, // The power of alchemy shall thrive within you.
					1060893, // Fate smiles upon you this day.
					1060894, // A keen mind in battle will help you avoid injury.
					1060895, // The flow of the ether is strong within you.
					1060901, // Your wounds in battle shall run deep.
					1060902, // The fires of the abyss shall tear asunder your flesh!
					1060903, // Winter's touch shall be your undoing.
					1060904, // Your veins will freeze with poison's chill.
					1060905, // The wise will seek to avoid the anger of storms.
					1060906, // Your dreams of wealth shall vanish like smoke.
					1060907, // The strength of alchemy will fail you.
					1060908, // Only fools take risks in fate's shadow.
					1060909, // Your lack of focus in battle shall be your undoing.
					1060910  // Your connection with the ether is weak, take heed.
			};

			private Mobile from;

			private Sphynx Sphynx;

			public SphynxGump( Mobile m, Sphynx s )
				: base( 150, 50 )
			{
				from = m;

				Sphynx = s;

				AddPage( 0 );

				Closable = false;

				AddImage( 0, 0, 0xE10 );
				AddImageTiled( 0, 14, 15, 200, 0xE13 );
				AddImageTiled( 380, 14, 14, 200, 0xE15 );

				AddImage( 0, 201, 0xE16 );
				AddImageTiled( 15, 201, 370, 16, 0xE17 );
				AddImageTiled( 15, 0, 370, 16, 0xE11 );

				AddImage( 380, 0, 0xE12 );
				AddImage( 380, 201, 0xE18 );
				AddImageTiled( 15, 15, 365, 190, 0xA40 );

				AddRadio( 30, 140, 0x25FF, 0x2602, false, 1 );
				AddHtmlLocalized( 65, 145, 300, 25, 1060863, 0xFFFFFF, false, false ); // Pay for the reading.

				AddRadio( 30, 175, 0x25FF, 0x2602, true, 0 );
				AddHtmlLocalized( 65, 178, 300, 25, 1060862, 0xFFFFFF, false, false ); // No thanks. I decide my own destiny!

				AddHtmlLocalized( 30, 20, 360, 35, 1060864, 0xFFFFFF, false, false ); // Interested in your fortune, are you?  The ancient Sphynx can read the future for you - for a price of course...

				AddImage( 65, 72, 0x15E5 );
				AddImageTiled( 80, 90, 200, 1, 0x2393 );
				AddImageTiled( 95, 92, 200, 1, 0x23C5 );

				AddLabel( 90, 70, 0x66D, "5000" );

				AddHtmlLocalized( 140, 70, 100, 25, 1023823, 0xFFFFFF, false, false ); // gold coins

				AddButton( 290, 175, 0xF7, 0xF8, 2, GumpButtonType.Reply, 0 );

				AddImageTiled( 15, 14, 365, 1, 0x2393 );
				AddImageTiled( 380, 14, 1, 190, 0x2391 );
				AddImageTiled( 15, 205, 365, 1, 0x2393 );
				AddImageTiled( 15, 14, 1, 190, 0x2391 );
				AddImageTiled( 0, 0, 395, 1, 0x23C5 );
				AddImageTiled( 394, 0, 1, 217, 0x23C3 );
				AddImageTiled( 0, 216, 395, 1, 0x23C5 );
				AddImageTiled( 0, 0, 1, 217, 0x23C3 );

				AddHtmlLocalized( 30, 105, 340, 40, 1060865, 0x1DB2D, false, false ); // Do you accept this offer?  The funds will be withdrawn from your backpack.
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				if ( info.ButtonID == 2 )
				{
					if ( info.IsSwitched( 1 ) )
					{
						if ( from.Backpack.ConsumeTotal( typeof( Gold ), 5000 ) )
						{
							from.SendLocalizedMessage( 1060867 ); // You pay the fee.

							Sphynx.Say( Utility.RandomList( m_Messages ) );

							Sphynx.Say( Utility.RandomList( m_Messages ) );

							Sphynx.m_Table[from] = true;
						}
						else
						{
							from.SendLocalizedMessage( 1061006 ); // You haven't got the coin to make the proper donation to the Sphynx.  Your fortune has not been read.
						}
					}
					else
					{
						from.SendLocalizedMessage( 1061007 ); // You decide against having your fortune told.
					}
				}
			}
		}
	}
}