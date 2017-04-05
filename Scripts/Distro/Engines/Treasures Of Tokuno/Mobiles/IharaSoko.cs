using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Mobiles
{
	public class IharaSoko : BaseVendor
	{
		protected ArrayList m_SBInfos = new ArrayList();

		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override bool IsActiveVendor { get { return false; } }
		public override bool IsInvulnerable { get { return true; } }
		public override bool DisallowAllMoves { get { return true; } }
		public override bool ClickTitle { get { return false; } }
		public override bool CanTeach { get { return false; } }

		public bool CheckItem( Item item )
		{
			if ( item is Bleach )
				return false;

			if ( item is ChestOfHeirlooms )
			{
				ChestOfHeirlooms coh = item as ChestOfHeirlooms;

				if ( !coh.Locked )
					return false;
			}

			return true;
		}

		public ArrayList FindMinorArtifacts( Mobile m )
		{
			ArrayList list = new ArrayList();

			for ( int i = 0; i < TokunoTreasures.MinorArtifacts.Length; i++ )
			{
				Type type = TokunoTreasures.MinorArtifacts[i];

				Item[] items = m.Backpack.FindItemsByType( type );

				for ( int j = 0; j < items.Length; j++ )
				{
					if ( items[j] != null )
					{
						if ( CheckItem( items[j] ) )
							list.Add( items[j] );
					}
				}
			}

			// Added for lesser pigments of tokuno colored
			foreach ( PigmentsOfTokunoMajor pigments in m.Backpack.FindItemsByType<PigmentsOfTokunoMajor>() )
			{
				if ( pigments.Type >= PigmentsType.FreshPlum )
					list.Add( pigments );
			}

			return list;
		}

		public override void InitSBInfo()
		{
		}

		[Constructable]
		public IharaSoko()
			: base( "the Imperial Minister of Trade" )
		{
		}

		public IharaSoko( Serial serial )
			: base( serial )
		{
		}

		public override void InitBody()
		{
			InitStats( 100, 100, 25 );

			Hue = 0x8403;
			Body = 0x190;

			Female = false;

			Name = "Ihara Soko";
		}

		public override void InitOutfit()
		{
			AddItem( new SamuraiTabi( 0x711 ) );
			AddItem( new Kamishimo( 0x483 ) );

			LightPlateJingasa jingasa = new LightPlateJingasa();
			jingasa.Hue = 0x711;
			AddItem( jingasa );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( m.Alive && m is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile) m;

				if ( pm.Z < 47 )
					return;

				if ( this.InRange( pm, 3 ) && !this.InRange( oldLocation, 3 ) )
				{
					ArrayList list = FindMinorArtifacts( pm );

					if ( pm.ToTItemsTurnedIn == 10 )
					{
						pm.CloseGump<ChooseRewardGump>();
						pm.CloseGump<ChoosePigmentGump>();

						pm.SendGump( new ChooseRewardGump( pm, this ) );

						return;
					}

					if ( list.Count == 0 )
					{
						Say( 1071013 ); // Bring me 10 of the lost treasures of Tokuno and I will reward you with a valuable item.
						return;
					}

					pm.SendGump( new ChooseMinorArtifactGump( pm, this, list ) );
				}
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