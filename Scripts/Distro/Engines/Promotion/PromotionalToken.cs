using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;

using Server.Engines.Promotion;

namespace Server.Items
{
	public enum PromotionalType
	{
		SoulStone,
		SoulStoneFragment,
		CharacterTransfer,
		SeventhAnniversary,
		EighthAnniversary,
		AdvancedCharacter,
		PersonalAttendant,
		ShadowItems,
		CrystalItems,
		BlueSoulstone,
		BrokenFurniture,
		HeritageItems,
		EvilHomeDecoration,
		None
	}

	public class PromotionalToken : Item
	{
		public override int LabelNumber { get { return 1070997; } } // A promotional token

		private PromotionalType m_Type;

		[CommandProperty( AccessLevel.GameMaster )]
		public PromotionalType Type
		{
			get { return m_Type; }
			set
			{
				m_Type = value;

				if ( m_Type == PromotionalType.CrystalItems )
					ItemID = 0x3678;
				else if ( m_Type == PromotionalType.ShadowItems )
					ItemID = 0x3679;
			}
		}

		[Constructable]
		public PromotionalToken( PromotionalType type )
			: base( 0x2AAA )
		{
			Type = type;

			Light = LightType.Circle300;

			LootType = LootType.Blessed;

			Weight = 1.0;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			string args = null;

			switch ( m_Type )
			{
				case PromotionalType.SoulStone:
					args = "Soulstone";
					break;
				case PromotionalType.BlueSoulstone:
					args = "Blue Soulstone";
					break;
				case PromotionalType.SoulStoneFragment:
					args = "Soulstone Fragment";
					break;
				case PromotionalType.CharacterTransfer:
					args = "Character Transfer";
					break;
				case PromotionalType.SeventhAnniversary:
					args = "Anniversary Item";
					break;
				case PromotionalType.EighthAnniversary:
					args = "Anniversary Item";
					break;
				case PromotionalType.AdvancedCharacter:
					args = "Advanced Character";
					break;
				case PromotionalType.ShadowItems:
					args = "Shadow Items";
					break;
				case PromotionalType.CrystalItems:
					args = "Crystal Items";
					break;
				case PromotionalType.PersonalAttendant:
					args = "Personal Attendant";
					break;
				case PromotionalType.BrokenFurniture:
					args = "Broken Furniture";
					break;
				case PromotionalType.HeritageItems:
					args = "Heritage Items";
					break;
				case PromotionalType.EvilHomeDecoration:
					args = "Evil Home D�cor Collection";
					break;
			}

			list.Add( 1070998, args ); // Use this to redeem your ~1_PROMO~
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !this.IsChildOf( from.Backpack ) )
				return;

			from.CloseGump( typeof( PromotionalTokenGump ) );
			from.CloseGump( typeof( AnniversaryRewardGump ) );
			from.CloseGump( typeof( SeventhAniversaryGump ) );
			from.CloseGump( typeof( EighthAnniversaryGump ) );
			from.CloseGump( typeof( BrokenFurnitureGump ) );
			from.CloseGump( typeof( HeritageItemsGump ) );
			from.CloseGump( typeof( EvilHomeDecorationGump ) );

			if ( Type == PromotionalType.AdvancedCharacter )
			{
				PlayerMobile pm = from as PlayerMobile;

				if ( pm.ACState == AdvancedCharacterState.InUse )
				{
					pm.SendLocalizedMessage( 1073815 ); // You are already choosing an advanced character template.
					return;
				}

				if ( from.SkillsTotal > 2000 )
				{
					from.SendGump( new AdvancedCharacterWarningGump( this ) );
					return;
				}
			}


			if ( Type == PromotionalType.CrystalItems )
				from.SendGump( new CrystalSetConfirmGump( this ) );
			else if ( Type == PromotionalType.ShadowItems )
				from.SendGump( new ShadowSetConfirmGump( this ) );
			else if ( Type == PromotionalType.BrokenFurniture )
				from.SendGump( new BrokenFurnitureConfirmGump( this ) );
			else if ( Type == PromotionalType.HeritageItems )
				from.SendGump( new HeritageItemsConfirmGump( this ) );
			else if ( Type == PromotionalType.EvilHomeDecoration )
				from.SendGump( new EvilHomeDecorationConfirmGump( this ) );
			//else if ( Type == PromotionalType.PersonalAttendant )
			//	from.SendGump( new PersonalAttendantConfirmGump( this ) );
			else
				from.SendGump( new PromotionalTokenGump( this, from ) );
		}

		public override bool OnDragLift( Mobile from )
		{
			from.CloseGump( typeof( AnniversaryRewardGump ) );
			from.CloseGump( typeof( PromotionalTokenGump ) );
			from.CloseGump( typeof( SeventhAniversaryGump ) );
			from.CloseGump( typeof( EighthAnniversaryGump ) );
			from.CloseGump( typeof( BrokenFurnitureGump ) );
			from.CloseGump( typeof( HeritageItemsGump ) );
			from.CloseGump( typeof( EvilHomeDecorationGump ) );

			return base.OnDragLift( from );
		}

		public PromotionalToken( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (int) m_Type );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Type = (PromotionalType) reader.ReadInt();
		}
	}

	public class PromotionalTokenGump : Gump
	{
		public override int TypeID { get { return 0x2335; } }

		private Mobile from;
		private PromotionalToken token;

		public PromotionalTokenGump( PromotionalToken t, Mobile m )
			: base( 10, 10 )
		{
			token = t;
			from = m;

			AddPage( 0 );
			AddBackground( 0, 0, 240, 135, 0x2422 );
			AddHtmlLocalized( 15, 15, 210, 75, 1070972, 0x0, true, false ); // Click "OKAY" to redeem the following promotional item:

			switch ( token.Type )
			{
				case PromotionalType.SoulStone:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1030903, 0x0, false, false ); // <center>Soulstone</center>
						break;
					}
				case PromotionalType.BlueSoulstone:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1030903, 0x0, false, false ); // <center>Soulstone</center>
						break;
					}
				case PromotionalType.SoulStoneFragment:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1070999, 0x0, false, false ); // <center>Soulstone Fragment</center> 
						break;
					}
				case PromotionalType.CharacterTransfer:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1062785, 0x0, false, false ); // <BODY><CENTER>Character Transfer<CENTER></BODY>
						break;
					}
				case PromotionalType.SeventhAnniversary:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1062928, 0x0, false, false ); // <CENTER>UO Seventh Anniversary</CENTER>
						break;
					}
				case PromotionalType.EighthAnniversary:
					{
						AddHtml( 15, 60, 210, 75, @"<CENTER>UO Eighth Anniversary</CENTER>", false, false ); // <CENTER>UO Eighth Anniversary</CENTER>
						break;
					}
				case PromotionalType.AdvancedCharacter:
					{
						AddHtmlLocalized( 15, 60, 210, 75, 1072839, 0x0, false, false ); // <center>Advanced Character</center>
						break;
					}
				case PromotionalType.BrokenFurniture:
					{
						AddHtml( 15, 60, 210, 75, @"<CENTER>Broken Furniture Collection</CENTER>", false, false ); // <CENTER>Broken Furniture Collection</CENTER>
						break;
					}
				case PromotionalType.HeritageItems:
					{
						AddHtml( 15, 60, 210, 75, @"<CENTER>Heritage Items Pack</CENTER>", false, false ); // <CENTER>Heritage Items Pack</CENTER>
						break;
					}
				case PromotionalType.EvilHomeDecoration:
					{
						AddHtml( 15, 60, 210, 75, @"<CENTER>Evil Home D�cor Collection</CENTER>", false, false ); // <CENTER>Evil Home D�cor Collection</CENTER>
						break;
					}
			}

			AddButton( 160, 95, 0xF7, 0xF8, 1, GumpButtonType.Reply, 0 );
			AddButton( 90, 95, 0xF2, 0xF1, 0, GumpButtonType.Reply, 0 );
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( info.ButtonID == 1 )
			{
				BankBox bank = from.BankBox;

				if ( bank == null )
					return;

				Accounting.Account acct = from.Account as Accounting.Account;

				switch ( token.Type )
				{
					case PromotionalType.SoulStone:
						{
							SoulStone ss = new SoulStone( acct.Username );

							bank.DropItem( ss );

							from.SendLocalizedMessage( 1070743 ); // A Soulstone has been created in your bank box!

							break;
						}
					case PromotionalType.BlueSoulstone:
						{
							SoulStone ss = new BlueSoulstone( acct.Username );

							bank.DropItem( ss );

							from.SendMessage( "A Blue Soulstone has been created in your bank box!" );

							break;
						}
					case PromotionalType.SoulStoneFragment:
						{
							int offset = Utility.Random( 0, 8 );

							SoulStoneFragment ssf = new SoulStoneFragment( 0x2AA1 + offset, acct.Username );

							bank.DropItem( ssf );

							from.SendLocalizedMessage( 1070976 ); // A soulstone fragment has been created in your bank box.

							break;
						}
					case PromotionalType.AdvancedCharacter:
						{
							pm.SendGump( new AdvancedCharacterChoiceGump() );

							pm.ACState = AdvancedCharacterState.InUse;

							break;
						}
					case PromotionalType.SeventhAnniversary:
						{
							//Cerramos alg�n posible gump abierto (exploit arreglado)
							if ( !pm.HasGump( typeof( SeventhAniversaryGump ) ) )
								pm.SendGump( new SeventhAniversaryGump( token ) );

							break;
						}
					case PromotionalType.EighthAnniversary:
						{
							//Cerramos alg�n posible gump abierto (exploit arreglado)
							if ( !pm.HasGump( typeof( EighthAnniversaryGump ) ) )
								pm.SendGump( new EighthAnniversaryGump( token ) );

							break;
						}
					case PromotionalType.BrokenFurniture:
						{
							//Cerramos alg�n posible gump abierto (exploit arreglado)
							if ( !pm.HasGump( typeof( BrokenFurnitureGump ) ) )
								pm.SendGump( new BrokenFurnitureConfirmGump( token ) );

							break;
						}
					case PromotionalType.HeritageItems:
						{
							//Cerramos alg�n posible gump abierto (exploit arreglado)
							if ( !pm.HasGump( typeof( HeritageItemsGump ) ) )
								pm.SendGump( new HeritageItemsConfirmGump( token ) );

							break;
						}
					case PromotionalType.EvilHomeDecoration:
						{
							//Cerramos alg�n posible gump abierto (exploit arreglado)
							if ( !pm.HasGump( typeof( EvilHomeDecorationGump ) ) )
								pm.SendGump( new EvilHomeDecorationConfirmGump( token ) );

							break;
						}
					// TODO: character transfer
				}

				if ( token.Type != PromotionalType.SeventhAnniversary &&
					 token.Type != PromotionalType.EighthAnniversary )
					token.Delete();
			}
		}
	}
}
