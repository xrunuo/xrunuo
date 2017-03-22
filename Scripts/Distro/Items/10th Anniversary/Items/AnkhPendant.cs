using System;
using System.Collections.Generic;

using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Events;

namespace Server.Items
{
	public class AnkhPendant : BaseNecklace
	{
		public static void Initialize()
		{
			foreach ( VirtueBonusEntry entry in m_Entries )
			{
				GenerateRegion( entry, Map.Trammel );
				GenerateRegion( entry, Map.Felucca );
			}
		}

		private static void GenerateRegion( VirtueBonusEntry entry, Map map )
		{
			Region region = new VirtueBonusRegion( entry, map );
			region.Register();
		}

		public override int LabelNumber { get { return 1079525; } } // Ankh Pendant

		[Constructable]
		public AnkhPendant()
			: base( 0x3BB5 )
		{
			Weight = 1.0;
			Hue = Utility.RandomBool() ? 0x96D : 0;
		}

		private static Dictionary<Mobile, VirtueBonusEntry> m_Bonuses = new Dictionary<Mobile, VirtueBonusEntry>();

		private void OnMantraSpoken( Mobile from, VirtueBonusEntry bonusEntry )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( pm == null )
				return;

			if ( m_Bonuses.ContainsKey( pm ) )
			{
				var currentBonusEntry = m_Bonuses[pm];

				// You already feel ~1_VIRTUE~ from your earlier contemplation of the virtues.
				pm.SendLocalizedMessage( 1079544, string.Format( "#{0}", currentBonusEntry.Cliloc ) );
			}
			else if ( pm.NextAnkhPendantUse > DateTime.UtcNow )
			{
				TimeSpan delta = pm.NextAnkhPendantUse - DateTime.UtcNow;

				if ( delta < TimeSpan.FromHours( 1.0 ) )
				{
					// You feel as if you should contemplate what you've learned for another ~1_TIME~ minutes.
					pm.SendLocalizedMessage( 1079568, ( (int) delta.TotalMinutes ).ToString() );
				}
				else
				{
					// You feel as if you should contemplate what you've learned for another ~1_TIME~ hours.
					pm.SendLocalizedMessage( 1079566, ( (int) delta.TotalHours ).ToString() );
				}
			}
			else
			{
				List<AttributeMod> mods = ComputeBonuses( pm, bonusEntry );

				foreach ( AttributeMod mod in mods )
					pm.AddAttributeMod( mod );

				m_Bonuses.Add( pm, bonusEntry );
				pm.NextAnkhPendantUse = DateTime.UtcNow + TimeSpan.FromDays( 1.0 );

				// Contemplating at the shrine has left you feeling more ~1_VIRTUE~.
				pm.SendLocalizedMessage( 1079546, string.Format( "#{0}", bonusEntry.Cliloc ) );

				Timer.DelayCall( TimeSpan.FromHours( 1.0 ),
					delegate
					{
						foreach ( AttributeMod mod in mods )
							pm.RemoveAttributeMod( mod );

						m_Bonuses.Remove( pm );

						pm.SendLocalizedMessage( 1079553 ); // The effects of meditating at the shrine have worn off.
					} );
			}
		}

		private static List<AttributeMod> ComputeBonuses( Mobile m, VirtueBonusEntry entry )
		{
			List<AttributeMod> mods = new List<AttributeMod>();

			switch ( entry.Virtue )
			{
				case VirtueName.Honesty:
					{
						mods.Add( new AttributeMod( AosAttribute.RegenMana, 2 ) );
						break;
					}
				case VirtueName.Compassion:
					{
						mods.Add( new AttributeMod( AosAttribute.RegenHits, 2 ) );
						break;
					}
				case VirtueName.Valor:
					{
						mods.Add( new AttributeMod( AosAttribute.RegenStam, 2 ) );
						break;
					}
				case VirtueName.Justice:
					{
						bool bump = 0.5 > Utility.RandomDouble();
						bool which = Utility.RandomBool();

						mods.Add( new AttributeMod( AosAttribute.RegenMana, bump && which ? 2 : 1 ) );
						mods.Add( new AttributeMod( AosAttribute.RegenHits, bump && !which ? 2 : 1 ) );

						break;
					}
				case VirtueName.Sacrifice:
					{
						bool bump = 0.5 > Utility.RandomDouble();
						bool which = Utility.RandomBool();

						mods.Add( new AttributeMod( AosAttribute.RegenHits, bump && which ? 2 : 1 ) );
						mods.Add( new AttributeMod( AosAttribute.RegenStam, bump && !which ? 2 : 1 ) );

						break;
					}
				case VirtueName.Honor:
					{
						bool bump = 0.5 > Utility.RandomDouble();
						bool which = Utility.RandomBool();

						mods.Add( new AttributeMod( AosAttribute.RegenMana, bump && which ? 2 : 1 ) );
						mods.Add( new AttributeMod( AosAttribute.RegenStam, bump && !which ? 2 : 1 ) );

						break;
					}
				case VirtueName.Spirituality:
					{
						mods.Add( new AttributeMod( AosAttribute.RegenHits, 0.25 > Utility.RandomDouble() ? 2 : 1 ) );
						mods.Add( new AttributeMod( AosAttribute.RegenMana, 0.25 > Utility.RandomDouble() ? 2 : 1 ) );
						mods.Add( new AttributeMod( AosAttribute.RegenStam, 0.25 > Utility.RandomDouble() ? 2 : 1 ) );

						break;
					}
				case VirtueName.Humility:
					{
						var attribute = Utility.RandomList( AosAttribute.RegenHits, AosAttribute.RegenMana, AosAttribute.RegenStam );
						mods.Add( new AttributeMod( attribute, 3 ) );

						break;
					}
			}

			return mods;
		}

		private static VirtueBonusEntry[] m_Entries = new VirtueBonusEntry[]
			{
				new VirtueBonusEntry( VirtueName.Honesty, "Ahm", new Rectangle2D( 4208, 561, 5, 5 ), 1079539 ),
				new VirtueBonusEntry( VirtueName.Compassion, "Mu", new Rectangle2D( 1855, 872, 5, 5 ), 1079535 ),
				new VirtueBonusEntry( VirtueName.Valor, "Ra", new Rectangle2D( 2488, 3928, 5, 5 ), 1079543 ),
				new VirtueBonusEntry( VirtueName.Justice, "Beh", new Rectangle2D( 1298, 629, 5, 5 ), 1079536 ),
				new VirtueBonusEntry( VirtueName.Sacrifice, "Cah", new Rectangle2D( 3352, 287, 5, 5 ), 1079538 ),
				new VirtueBonusEntry( VirtueName.Honor, "Summ", new Rectangle2D( 1722, 3525, 5, 5 ), 1079540 ),
				new VirtueBonusEntry( VirtueName.Spirituality, "Om", new Rectangle2D( 1592, 2487, 5, 5 ), 1079542 ),
				new VirtueBonusEntry( VirtueName.Humility, "Lum", new Rectangle2D( 4271, 3694, 5, 5 ), 1079541 ),
			};

		private class VirtueBonusEntry
		{
			public VirtueName Virtue { get; private set; }
			public string Mantra { get; private set; }
			public Rectangle2D Area { get; private set; }
			public int Cliloc { get; private set; }

			public VirtueBonusEntry( VirtueName virtue, string mantra, Rectangle2D location, int cliloc )
			{
				Virtue = virtue;
				Mantra = mantra;
				Area = location;
				Cliloc = cliloc;
			}
		}

		private class VirtueBonusRegion : Region
		{
			private VirtueBonusEntry m_BonusEntry;

			public VirtueBonusRegion( VirtueBonusEntry bonusEntry, Map map )
				: base( null, map, null, bonusEntry.Area )
			{
				m_BonusEntry = bonusEntry;
			}

			public override void OnSpeech( SpeechEventArgs args )
			{
				base.OnSpeech( args );

				if ( Insensitive.Equals( args.Speech, m_BonusEntry.Mantra ) )
				{
					AnkhPendant pendant = args.Mobile.FindItemOnLayer( Layer.Neck ) as AnkhPendant;

					if ( pendant != null )
						pendant.OnMantraSpoken( args.Mobile, m_BonusEntry );
				}
			}
		}

		public AnkhPendant( Serial serial )
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
