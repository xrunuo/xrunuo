using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.Collections;
using Server.Factions;

namespace Server.Items
{
	public class PigmentsOfTokuno : Item, IUsesRemaining
	{
		public override int LabelNumber { get { return 1070933; } } // Pigments of Tokuno

		private int m_UsesRemaining;

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

		[Constructable]
		public PigmentsOfTokuno()
			: base( 0xEFF )
		{
			m_UsesRemaining = 10;

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

			list.Add( 1060584, m_UsesRemaining.ToString() ); // uses remaining: ~1_val~
		}

		public PigmentsOfTokuno( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.Write( (int) m_UsesRemaining );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_UsesRemaining = reader.ReadInt();
		}

		public static bool CanHue( Item item )
		{
			if ( item.IsNamed )
				return true;

			if ( item is PigmentsOfTokuno || item is PigmentsOfTokunoMajor )
				return false;

			if ( item is IFactionArtifact )
				return true;

			if ( item is ElvenGlasses )
				return true;

			if ( item is LuckyNecklace )
				return true;

			if ( item is BaseWeapon && ( ( (BaseWeapon) item ).ArtifactRarity > 0 || !CraftResources.IsStandard( ( (BaseWeapon) item ).Resource ) ) )
				return true;

			if ( item is BaseArmor && ( ( (BaseArmor) item ).ArtifactRarity > 0 || !CraftResources.IsStandard( ( (BaseArmor) item ).Resource ) ) )
				return true;

			if ( item is BaseClothing && ( (BaseClothing) item ).ArtifactRarity > 0 )
				return true;

			if ( item is BaseJewel && ( (BaseJewel) item ).ArtifactRarity > 0 )
				return true;

			if ( item is StealableArtifact || item is StealableContainerArtifact || item is StealableLightArtifact || item is StealableLongswordArtifact || item is StealablePlateGlovesArtifact || item is StealableWarHammerArtifact || item is StealableExecutionersAxeArtifact || item is StealableFoodArtifact )
				return true;

			if ( item is SamuraiHelm || item is LeggingsOfEmbers || item is HolySword || item is ShaminoCrossbow )
				return true;

			#region Heritage Items
			if ( item is DupresShield || item is OssianGrimoire || item is QuiverOfInfinity )
				return true;
			#endregion

			#region Champion Artifacts
			if ( item is AcidProofRobe || item is CrownOfTalKeesh || item is Calm || item is FangOfRactus || item is GladiatorsCollar || item is OrcChieftainHelm
				|| item is Pacify || item is Quell || item is ShroudOfDeciet || item is Subdue || item is ANecromancerShroud || item is BraveKnightOfTheBritannia
				|| item is CaptainJohnsHat || item is DetectiveBoots || item is DjinnisRing || item is EmbroideredOakLeafCloak || item is GuantletsOfAnger || item is LieutenantOfTheBritannianRoyalGuard
				|| item is OblivionsNeedle || item is RoyalGuardSurvivalKnife || item is SamaritanRobe || item is TheMostKnowledgePerson || item is TheRobeOfBritanniaAri )
				return true;
			#endregion

			for ( int i = 0; i < Paragon.Artifacts.Length; i++ )
			{
				Type type = Paragon.Artifacts[i];

				if ( type == item.GetType() )
					return true;
			}

			if ( item is ScrappersCompendium )
				return true;

			for ( int i = 0; i < NamedMiniBosses.Artifacts.Length; i++ )
			{
				Type type = NamedMiniBosses.Artifacts[i];

				if ( type == item.GetType() )
					return true;
			}

			for ( int i = 0; i < Leviathan.Artifacts.Length; i++ )
			{
				Type type = Leviathan.Artifacts[i];

				if ( type == item.GetType() )
					return true;
			}


			for ( int i = 0; i < TreasureMapChest.m_Artifacts.Length; i++ )
			{
				Type type = TreasureMapChest.m_Artifacts[i];

				if ( type == item.GetType() )
					return true;
			}

			for ( int i = 0; i < TokunoTreasures.MinorArtifacts.Length; i++ )
			{
				Type type = TokunoTreasures.MinorArtifacts[i];

				if ( type == item.GetType() )
					return true;
			}

			for ( int i = 0; i < TokunoTreasures.MajorArtifacts.Length; i++ )
			{
				Type type = TokunoTreasures.MajorArtifacts[i];

				if ( type == item.GetType() )
					return true;
			}

			if ( CraftableArtifacts.IsCraftableArtifact( item ) )
				return true;

			if ( item is CrimsonCincture || item is DreadsRevenge )
				return true;

			for ( int i = 0; i < MoonglowZooCollection.RewardList.Length; i++ )
			{
				RewardEntry entry = (RewardEntry) MoonglowZooCollection.RewardList[i];

				if ( entry.Type == item.GetType() )
					return true;
			}

			for ( int i = 0; i < VesperMuseumCollection.RewardList.Length; i++ )
			{
				RewardEntry entry = (RewardEntry) VesperMuseumCollection.RewardList[i];

				if ( entry.Type == item.GetType() )
					return true;
			}

			for ( int i = 0; i < BritainLibraryAnimalTrainerCollection.RewardList.Length; i++ )
			{
				RewardEntry entry = (RewardEntry) BritainLibraryAnimalTrainerCollection.RewardList[i];

				if ( entry.Type == item.GetType() )
					return true;
			}

			if ( item is DecorativeCarpet )
				return true;

			if ( SAArtifacts.IsSAArtifact( item ) )
				return true;

			if ( item is NocturneEarrings )
				return true;

			return false;
		}

		public static bool CheckWarn( Item item )
		{
			if ( item is BaseWeapon && ( (BaseWeapon) item ).HitPoints < ( (BaseWeapon) item ).MaxHitPoints )
			{
				return false;
			}

			if ( item is BaseArmor && ( (BaseArmor) item ).HitPoints < ( (BaseArmor) item ).MaxHitPoints )
			{
				return false;
			}

			if ( item is BaseClothing && ( (BaseClothing) item ).HitPoints < ( (BaseClothing) item ).MaxHitPoints )
			{
				return false;
			}

			return true;
		}

		private class DyeTarget : Target
		{
			private PigmentsOfTokuno dye;

			public DyeTarget( PigmentsOfTokuno m_dye )
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
					item.Hue = 0;

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
