using System;
using System.Collections.Generic;
using Server;
using Server.Spells;
using Server.Factions;

namespace Server.Items
{
	public class FactionStrongoldRune : Item
	{
		public static readonly TimeSpan Cooldown = TimeSpan.FromHours( 2.0 );

		public override int LabelNumber { get { return 1094700; } } // Faction Stronghold Rune

		[Constructable]
		public FactionStrongoldRune()
			: base( 0x1F14 )
		{
			Hue = 0x482;
			Weight = 1.0;
		}

		public FactionStrongoldRune( Serial serial )
			: base( serial )
		{
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );

			list.Add( 1041350 ); // faction item
		}

		private static Dictionary<Mobile, Timer> m_CooldownTable = new Dictionary<Mobile, Timer>();

		public override void OnDoubleClick( Mobile from )
		{
			Faction faction = Faction.Find( from );

			if ( !IsChildOf( from.Backpack ) )
			{
				// That is not in your backpack.
				from.SendLocalizedMessage( 1042593 );
			}
			else if ( faction == null )
			{
				// You may not use this unless you are a faction member!
				from.SendLocalizedMessage( 1010376, null, 0x25 );
			}
			else if ( from.Criminal )
			{
				// Thou'rt a criminal and cannot escape so easily.
				from.SendLocalizedMessage( 1005561, null, 0x22 );
			}
			else if ( SpellHelper.CheckCombat( from ) )
			{
				// Wouldst thou flee during the heat of battle??
				from.SendLocalizedMessage( 1005564, null, 0x22 );
			}
			else if ( from.Region.IsPartOf( typeof( Regions.Jail ) ) )
			{
				// You'll need a better jailbreak plan then that!
				from.SendLocalizedMessage( 1041530 );
			}
			else if ( m_CooldownTable.ContainsKey( from ) )
			{
				Timer cooldownTimer = m_CooldownTable[from];

				// You must wait ~1_seconds~ seconds before you can use this item.
				from.SendLocalizedMessage( 1079263, ( cooldownTimer.Next - DateTime.UtcNow ).Seconds.ToString() );
			}
			else
			{
				Point3D dest = faction.Definition.Stronghold.FactionStone;
				dest.X--;
				dest.Y++;

				from.PlaySound( 0x1FC );
				from.MoveToWorld( dest, Faction.Facet );
				from.PlaySound( 0x1FC );

				Delete();
				from.SendLocalizedMessage( 1094706 ); // Your faction stronghold rune has disappeared.

				m_CooldownTable[from] = Timer.DelayCall( Cooldown, new TimerCallback( delegate { m_CooldownTable.Remove( from ); } ) );
			}
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