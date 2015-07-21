using System;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items
{
	public enum PigmentsType
	{
		ParagonGold,
		VioletCouragePurple,
		InvulnerabilityBlue,
		LunaWhite,
		DryadGreen,
		ShadowDancerBlack,
		BerserkerRed,
		NoxGreen,
		RumRed,
		FireOrange,

		FadedCoal,
		Coal,
		FadedGold,
		StormBronze,
		Rose,
		MidnightCoal,
		FadedBronze,
		FadedRose,
		DeepRose,

		FreshPlum,
		Silver,
		DeepBrown,
		BurntBrown,
		LightGreen,
		FreshRose,
		PaleBlue,
		NobleGold,
		PaleOrange,
		ChaosBlue
	}

	public class PigmentsOfTokunoMajor : Item, IUsesRemaining
	{
		public override int LabelNumber { get { return 1070933; } } // Pigments of Tokuno

		private PigmentsType m_Type;

		private int m_UsesRemaining;

		[CommandProperty( AccessLevel.GameMaster )]
		public PigmentsType Type { get { return m_Type; } set { m_Type = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining
		{
			get { return m_UsesRemaining; }
			set
			{
				m_UsesRemaining = value;
				InvalidateProperties();
			}
		}

		public bool ShowUsesRemaining
		{
			get { return true; }
			set
			{
			}
		}

		public int GetHue( PigmentsType type )
		{
			int hue = 0;

			switch ( type )
			{
				/* Greater, Neon */
				case PigmentsType.ParagonGold:
					hue = 0x501;
					break;
				case PigmentsType.VioletCouragePurple:
					hue = 0x486;
					break;
				case PigmentsType.InvulnerabilityBlue:
					hue = 0x4F2;
					break;
				case PigmentsType.LunaWhite:
					hue = 0x47E;
					break;
				case PigmentsType.DryadGreen:
					hue = 0x48F;
					break;
				case PigmentsType.ShadowDancerBlack:
					hue = 0x455;
					break;
				case PigmentsType.BerserkerRed:
					hue = 0x21;
					break;
				case PigmentsType.NoxGreen:
					hue = 0x58C;
					break;
				case PigmentsType.RumRed:
					hue = 0x66C;
					break;
				case PigmentsType.FireOrange:
					hue = 0x54F;
					break;

				/* Greater, Metal */
				case PigmentsType.FadedCoal:
					hue = 0x96A;
					break;
				case PigmentsType.Coal:
					hue = 0x96B;
					break;
				case PigmentsType.FadedGold:
					hue = 0x972;
					break;
				case PigmentsType.StormBronze:
					hue = 0x977;
					break;
				case PigmentsType.Rose:
					hue = 0x97C;
					break;
				case PigmentsType.MidnightCoal:
					hue = 0x96C;
					break;
				case PigmentsType.FadedBronze:
					hue = 0x975;
					break;
				case PigmentsType.FadedRose:
					hue = 0x97B;
					break;
				case PigmentsType.DeepRose:
					hue = 0x97E;
					break;

				/* Lesser, Coloured */
				case PigmentsType.FreshPlum:
					hue = 325;
					break;
				case PigmentsType.Silver:
					hue = 1001;
					break;
				case PigmentsType.DeepBrown:
					hue = 1008;
					break;
				case PigmentsType.BurntBrown:
					hue = 1050;
					break;
				case PigmentsType.LightGreen:
					hue = 456;
					break;
				case PigmentsType.FreshRose:
					hue = 1209;
					break;
				case PigmentsType.PaleBlue:
					hue = 591;
					break;
				case PigmentsType.NobleGold:
					hue = 551;
					break;
				case PigmentsType.PaleOrange:
					hue = 46;
					break;
				case PigmentsType.ChaosBlue:
					hue = 5;
					break;
			}

			return hue;
		}

		[Constructable]
		public PigmentsOfTokunoMajor( PigmentsType type, int uses )
			: base( 0xEFF )
		{
			m_Type = type;

			Hue = GetHue( type );

			m_UsesRemaining = uses;

			Weight = 1.0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 1070929 ); // Select the artifact or enhanced magic item to dye.

			from.Target = new DyeTarget( this );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_Type >= PigmentsType.FreshPlum )
				list.Add( 1071431 + (int) m_Type );
			else if ( m_Type >= PigmentsType.FadedCoal )
				list.Add( 1079569 + (int) m_Type );
			else
				list.Add( 1070987 + (int) m_Type );

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public PigmentsOfTokunoMajor( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (int) m_Type );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Type = (PigmentsType) reader.ReadInt();

			m_UsesRemaining = reader.ReadInt();
		}

		private class DyeTarget : Target
		{
			private PigmentsOfTokunoMajor dye;

			public DyeTarget( PigmentsOfTokunoMajor m_dye )
				: base( 8, false, TargetFlags.None )
			{
				dye = m_dye;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				Item item = targeted as Item;

				if ( item == null )
				{
					return;
				}

				if ( !item.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1062334 ); // This item must be in your backpack to be used.
				}
				else if ( item is PigmentsOfTokuno || item is PigmentsOfTokunoMajor || item is CompassionPigment )
				{
					from.SendLocalizedMessage( 1042083 ); // You cannot dye that.
				}
				else if ( item.IsLockedDown )
				{
					from.SendLocalizedMessage( 1070932 ); // You may not dye artifacts and enhanced magic items which are locked down.
				}
				else if ( !PigmentsOfTokuno.CheckWarn( item ) )
				{
					from.SendLocalizedMessage( 1070930 ); // Can't dye artifacts or enhanced magic items that are being worn.
				}
				else if ( PigmentsOfTokuno.CanHue( item ) )
				{
					item.Hue = dye.GetHue( dye.Type );

					dye.UsesRemaining--;

					if ( dye.UsesRemaining <= 0 )
					{
						dye.Delete();
					}
				}
				else
				{
					from.SendLocalizedMessage( 1070931 ); // You can only dye artifacts and enhanced magic items with this tub.
				}
			}
		}
	}
}
