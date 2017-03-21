using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells.Ninjitsu;

namespace Server.Spells.Spellweaving
{
	public class ArcaneCircleSpell : SpellweavingSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Arcane Circle", "Myrshalee",
				-1, 9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 0.5 ); } }
		public override double RequiredSkill { get { return 0.0; } }
		public override int RequiredMana { get { return 24; } }

		public ArcaneCircleSpell( Mobile caster, Item scroll )
			: base( caster, scroll, m_Info )
		{
		}

		public override bool CheckCast()
		{
			if ( !IsValidLocation( Caster.Location, Caster.Map ) )
			{
				Caster.SendLocalizedMessage( 1072705 ); // You must be standing on an arcane circle, pentagram or abbatoir.
				return false;
			}

			if ( GetArcanists().Count < 2 )
			{
				Caster.SendLocalizedMessage( 1080452 ); // There are not enough spellweavers present to create an Arcane Focus.
				return false;
			}

			return base.CheckCast();
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.FixedParticles( 0x3779, 10, 20, 0x0, EffectLayer.Waist );
				Caster.PlaySound( 0x5C0 );

				List<Mobile> arcanists = GetArcanists();

				TimeSpan duration = TimeSpan.FromHours( Math.Max( 1, (int) ( Caster.Skills.Spellweaving.Value / 24 ) ) );

				int strengthBonus = GetStrengthBonus( arcanists.Count );
				duration += TimeSpan.FromHours( strengthBonus - 1 );

				for ( int i = 0; i < arcanists.Count; i++ )
				{
					Mobile m = arcanists[i];
					GiveArcaneFocus( m, duration, strengthBonus );
				}
			}

			FinishSequence();
		}

		private int GetStrengthBonus( int arcanists )
		{
			/*
			 * - Arcane Circles will now give a bonus to arcane focus
			 * - Arcane Focus will require a minimum of two people to cast
			 * - Casting it in a pentagram or abbatoir will give you a Focus with a level equal to number of casters - 1
			 * - Casting in an Arcane Circle (such as in Heartwood or a crafted Circle in a player house) will give you a 1 level bonus.
			 * - Casting in Prism of Light will give you the Arcane Circle bonus plus an additional bonus, for a maximum possible focus level 6. 
			 */

			int strengthBonus = arcanists;

			if ( !IsArcaneCircle( Caster.Location, Caster.Map ) )
				strengthBonus--;

			if ( strengthBonus > 5 )
				strengthBonus = 5;

			if ( IsPrismOfLight( Caster.Location, Caster.Map ) )
				strengthBonus++;

			return strengthBonus;
		}

		private static bool IsPrismOfLight( Point3D p, Map map )
		{
			return ( map == Map.Trammel || map == Map.Felucca ) && p.X == 6589 && p.Y == 178 && p.Z == 26;
		}

		private static bool IsArcaneCircle( Point3D location, Map map )
		{
			int tile;

			if ( IsValidLocation( location, map, out tile ) )
				return tile == 0x307F;

			return false;
		}

		private static bool IsValidLocation( Point3D location, Map map )
		{
			int tile;

			return IsValidLocation( location, map, out tile );
		}

		private static bool IsValidLocation( Point3D location, Map map, out int tile )
		{
			Tile lt = map.Tiles.GetLandTile( location.X, location.Y ); // Land Tiles            

			if ( IsValidTile( lt.ID ) && Math.Abs( lt.Z - location.Z ) <= 1 )
			{
				tile = lt.ID;
				return true;
			}

			Tile[] tiles = map.Tiles.GetStaticTiles( location.X, location.Y ); // Static Tiles

			for ( int i = 0; i < tiles.Length; ++i )
			{
				Tile t = tiles[i];

				int tand = t.ID & TileData.MaxItemValue;

				if ( Math.Abs( t.Z - location.Z ) > 1 )
					continue;
				else if ( IsValidTile( tand ) )
				{
					tile = tand;
					return true;
				}
			}

			var eable = map.GetItemsInRange( location, 0 );

			foreach ( Item item in eable )
			{
				if ( item == null || Math.Abs( item.Z - location.Z ) > 1 )
				{
					continue;
				}
				else if ( IsValidTile( item.ItemID ) )
				{
					tile = item.ItemID;
					return true;
				}
			}


			tile = -1;
			return false;
		}

		public static bool IsValidTile( int itemID )
		{
			// Per OSI, Center tile only
			// Pentagram center, Abbatoir center, Arcane Circle Center or any of the center tiles on the Veeteran Reward Bloody Pentagram
			return ( itemID == 0xFEA || itemID == 0x1216 || itemID == 0x307F || itemID == 0x1D11 || itemID == 0x1D10 || itemID == 0x1D0F || itemID == 0x1D12 );
		}

		private List<Mobile> GetArcanists()
		{
			List<Mobile> arcanists = new List<Mobile>();

			arcanists.Add( Caster );

			// OSI Verified: Even enemies/combatants count
			foreach ( Mobile m in Caster.GetMobilesInRange( 1 ) ) // Range verified as 1
			{
				if ( m != Caster && Caster.CanBeBeneficial( m, false ) && Math.Abs( Caster.Skills.Spellweaving.Value - m.Skills.Spellweaving.Value ) <= 20 && !( m is Clone ) && m.NetState != null )
					arcanists.Add( m );

				// Everyone gets the Arcane Focus, power capped elsewhere
			}

			return arcanists;
		}

		private void GiveArcaneFocus( Mobile to, TimeSpan duration, int strengthBonus )
		{
			if ( to == null ) // Sanity
				return;

			ArcaneFocus focus = FindArcaneFocus( to );

			if ( focus == null )
			{
				ArcaneFocus f = new ArcaneFocus( duration, strengthBonus );

				if ( to.PlaceInBackpack( f ) )
				{
					f.SendTimeRemainingMessage( to );
					to.SendLocalizedMessage( 1072740 ); // An arcane focus appears in your backpack.
				}
				else
				{
					f.Delete();
				}

			}
			else // OSI renewal rules: the new one will override the old one, always.
			{
				to.SendLocalizedMessage( 1072828 ); // Your arcane focus is renewed.
				focus.LifeSpan = duration;
				focus.CreationTime = DateTime.Now;
				focus.StrengthBonus = strengthBonus;
				focus.InvalidateProperties();
				focus.SendTimeRemainingMessage( to );
			}
		}
	}

	public class ArcaneFocus : TransientItem
	{
		public override int LabelNumber { get { return 1032629; } } // Arcane Focus

		private int m_StrengthBonus;

		[CommandProperty( AccessLevel.GameMaster )]
		public int StrengthBonus
		{
			get { return m_StrengthBonus; }
			set { m_StrengthBonus = value; }
		}

		public ArcaneFocus( TimeSpan lifeSpan, int strengthBonus )
			: base( 0x3155, lifeSpan )
		{
			LootType = LootType.Blessed;
			m_StrengthBonus = strengthBonus;
		}

		public ArcaneFocus( Serial serial )
			: base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			list.Add( 1060485, m_StrengthBonus.ToString() ); // strength bonus ~1_val~
		}

		public override bool NonTransferable { get { return true; } }

		public override void HandleInvalidTransfer( Mobile from )
		{
			from.SendLocalizedMessage( 1073480 ); // Your arcane focus disappears.

			this.Delete();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_StrengthBonus );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_StrengthBonus = reader.ReadInt();
		}
	}
}