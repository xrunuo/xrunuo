using System;
using System.Collections;
using Server.Items;
using Server.Spells.Ninjitsu;

namespace Server.Mobiles
{
	[CorpseName( "Travesty's corpse" )]
	public class Travesty : BaseBardCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			if ( this.Weapon == null )
				return null;

			BaseWeapon weapon = this.Weapon as BaseWeapon;

			return Utility.RandomBool() ? weapon.PrimaryAbility : weapon.SecondaryAbility;
		}

		public override int SuccessSound { get { return 0x58B; } }
		public override int FailureSound { get { return 0x58C; } }

		public override bool UsesDiscordance { get { return Skills[SkillName.Discordance].Value >= 50.0; } }
		public override bool UsesPeacemaking { get { return Skills[SkillName.Peacemaking].Value >= 50.0; } }
		public override bool UsesProvocation { get { return Skills[SkillName.Provocation].Value >= 50.0; } }

		private DateTime m_NextBodyChange;
		private Timer m_ResetTimer;

		private int m_LastHits;

		public override bool AlwaysAttackable { get { return true; } }

		[Constructable]
		public Travesty()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Travesty";
			Body = 264;

			BaseSoundID = 0x46E;

			SetStr( 900, 950 );
			SetDex( 1000, 1050 );
			SetInt( 900, 950 );

			SetHits( 30000 );

			SetDamage( 24, 30 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 85, 90 );
			SetResistance( ResistanceType.Cold, 80, 85 );
			SetResistance( ResistanceType.Poison, 75, 80 );
			SetResistance( ResistanceType.Energy, 70, 75 );

			SetSkill( SkillName.Wrestling, 105, 110 );
			SetSkill( SkillName.Tactics, 115, 120 );
			SetSkill( SkillName.MagicResist, 115, 120 );
			SetSkill( SkillName.Anatomy, 110, 115 );
			SetSkill( SkillName.Magery, 110, 115 );

			Fame = 30000;
			Karma = -30000;
		}

		public override bool AutoDispel { get { return true; } }
		public override bool Unprovokable { get { return true; } }

		public static void SpawnNinjas()
		{
			SpawnNinjaGroup( new Point3D( 80, 1964, 0 ) );
			SpawnNinjaGroup( new Point3D( 80, 1949, 0 ) );
			SpawnNinjaGroup( new Point3D( 92, 1948, 0 ) );
			SpawnNinjaGroup( new Point3D( 92, 1962, 0 ) );
		}

		public static void SpawnNinjaGroup( Point3D _location )
		{
			BaseCreature ninja = new BlackOrderAssassin();
			ninja.MoveToWorld( _location, Map.Malas );

			ninja = new BlackOrderThief();
			ninja.MoveToWorld( _location, Map.Malas );

			ninja = new BlackOrderMage();
			ninja.MoveToWorld( _location, Map.Malas );
		}

		public override bool CheckResistancesInItems { get { return false; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( m_LastHits >= 2000 && Hits < 2000 )
				SpawnNinjas();

			m_LastHits = Hits;

			if ( Utility.RandomBool() && from != null )
			{
				Clone clone = new Clone( this );
				clone.MoveToWorld( Location, Map );

				FixedParticles( 0x376A, 1, 14, 0x13B5, 0, 0, EffectLayer.Waist );
				PlaySound( 0x511 );

				from.Combatant = clone;

				from.SendLocalizedMessage( 1063141 ); // Your attack has been diverted to a nearby mirror image of your target!
			}

			if ( 0.25 > Utility.RandomDouble() && DateTime.UtcNow > m_NextBodyChange )
				ChangeBody();

			base.OnDamage( amount, from, willKill );
		}

		public void RestoreBody()
		{
			Name = "Travesty";
			Body = 264;
			Hue = 0;

			ArrayList list = new ArrayList();

			foreach ( Item item in GetEquippedItems() )
				if ( item != null && !( item is Backpack ) )
					list.Add( item );

			for ( int i = 0; i < list.Count; i++ )
				( (Item) list[i] ).Delete();

			list.Clear();

			Skills[SkillName.EvalInt].Base = 0;
			Skills[SkillName.Provocation].Base = 0;
			Skills[SkillName.Discordance].Base = 0;
			Skills[SkillName.Musicianship].Base = 0;
			Skills[SkillName.Peacemaking].Base = 0;
			Skills[SkillName.Macing].Base = 0;
			Skills[SkillName.Archery].Base = 0;
			Skills[SkillName.Swords].Base = 0;
			Skills[SkillName.Fencing].Base = 0;
			Skills[SkillName.Spellweaving].Base = 0;
			Skills[SkillName.Necromancy].Base = 0;
			Skills[SkillName.SpiritSpeak].Base = 0;

			if ( m_ResetTimer != null )
				m_ResetTimer.Stop();
		}

		public void ChangeBody()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in Map.GetMobilesInRange( Location, 5 ) )
				if ( m.Player && m.AccessLevel == AccessLevel.Player )
					list.Add( m );

			if ( list.Count <= 0 )
			{
				if ( Body != 264 )
					RestoreBody();

				return;
			}

			Mobile attacker = (Mobile) list[Utility.Random( list.Count - 1 )];

			if ( !attacker.Body.IsHuman )
				return;

			foreach ( Item item in GetEquippedItems() )
				if ( item != null && !( item is Backpack ) )
					item.Delete();

			Name = attacker.Name;
			Hue = attacker.Hue;
			Body = attacker.Body;

			HairItemID = attacker.HairItemID;
			HairHue = attacker.HairHue;

			FacialHairItemID = attacker.FacialHairItemID;
			FacialHairHue = attacker.FacialHairHue;

			try
			{
				DupeItem( Layer.OneHanded, attacker );
				DupeItem( Layer.TwoHanded, attacker );
				DupeItem( Layer.Shoes, attacker );
				DupeItem( Layer.Pants, attacker );
				DupeItem( Layer.Shirt, attacker );
				DupeItem( Layer.Helm, attacker );
				DupeItem( Layer.Gloves, attacker );
				DupeItem( Layer.Ring, attacker );
				DupeItem( Layer.Neck, attacker );
				DupeItem( Layer.Waist, attacker );
				DupeItem( Layer.InnerTorso, attacker );
				DupeItem( Layer.Bracelet, attacker );
				DupeItem( Layer.MiddleTorso, attacker );
				DupeItem( Layer.Earrings, attacker );
				DupeItem( Layer.Arms, attacker );
				DupeItem( Layer.Cloak, attacker );
				DupeItem( Layer.OuterTorso, attacker );
				DupeItem( Layer.OuterLegs, attacker );
				DupeItem( Layer.InnerLegs, attacker );
			}
			catch
			{
			}

			// DefaultAI: AI_Mage
			Skills[SkillName.EvalInt].Base = attacker.Skills[SkillName.EvalInt].Value;

			// Bard (default enabled)
			Skills[SkillName.Musicianship].Base = attacker.Skills[SkillName.Musicianship].Value;
			Skills[SkillName.Provocation].Base = attacker.Skills[SkillName.Provocation].Value;
			Skills[SkillName.Discordance].Base = attacker.Skills[SkillName.Discordance].Value;
			Skills[SkillName.Peacemaking].Base = attacker.Skills[SkillName.Peacemaking].Value;

			// Melee
			Skills[SkillName.Swords].Base = attacker.Skills[SkillName.Swords].Value;
			Skills[SkillName.Macing].Base = attacker.Skills[SkillName.Macing].Value;
			Skills[SkillName.Fencing].Base = attacker.Skills[SkillName.Fencing].Value;

			if ( Skills[SkillName.Swords].Value >= 50.0 || Skills[SkillName.Fencing].Value >= 50.0 || Skills[SkillName.Macing].Value >= 50.0 )
				ChangeAIType( AIType.AI_Melee );

			// Archer
			Skills[SkillName.Archery].Base = attacker.Skills[SkillName.Archery].Value;

			if ( Skills[SkillName.Archery].Value >= 50.0 )
				ChangeAIType( AIType.AI_Archer );

			// Arcanist
			Skills[SkillName.Spellweaving].Base = attacker.Skills[SkillName.Spellweaving].Value;

			if ( Skills[SkillName.Spellweaving].Value >= 50.0 )
				ChangeAIType( AIType.AI_Arcanist );

			// Necromancer
			Skills[SkillName.Necromancy].Base = attacker.Skills[SkillName.Necromancy].Value;
			Skills[SkillName.SpiritSpeak].Base = attacker.Skills[SkillName.SpiritSpeak].Value;

			if ( Skills[SkillName.Necromancy].Value >= 50.0 )
				ChangeAIType( AIType.AI_Necromancer );

			m_NextBodyChange = DateTime.UtcNow + TimeSpan.FromSeconds( 10.0 );

			if ( m_ResetTimer != null )
				m_ResetTimer.Stop();

			m_ResetTimer = Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), new TimerCallback( RestoreBody ) );
		}

		public void DupeItem( Layer layer, Mobile attacker )
		{
			Item item = attacker.FindItemOnLayer( layer );

			if ( item != null )
			{
				Item dupeitem = Activator.CreateInstance( item.GetType() ) as Item;

				if ( dupeitem.LootType != LootType.Blessed )
					dupeitem.LootType = LootType.Newbied;

				dupeitem.Hue = item.Hue;
				dupeitem.Movable = false;

				AddItem( dupeitem );
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.PeerlessIngredients, 8 );
			AddLoot( LootPack.Talismans, Utility.RandomMinMax( 1, 5 ) );
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new CrimsonCincture() );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new MarkOfTravesty() );

			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new TragicRemainsOfTravesty() );

			if ( Utility.RandomBool() )
				c.DropItem( new TravestysCollectionOfShells() );

			if ( Utility.RandomBool() )
				c.DropItem( new TravestysFineTeakwoodTray() );

			if ( 25000 > Utility.Random( 100000 ) )
				c.DropItem( new TravestysSushiPreparation() );

			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new ImprisonedDogStatuette() );

			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new HumanFeyLeggings() );

			for ( int i = 0; i < 3; i++ )
			{
				if ( Utility.RandomBool() )
					c.DropItem( new EyeOfTheTravesty( Utility.RandomMinMax( 1, 3 ) ) );
			}

			for ( int i = 0; i < 2; i++ )
			{
				if ( 5000 > Utility.Random( 100000 ) )
					c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}
		}

		public Travesty( Serial serial )
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
	}
}