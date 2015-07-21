using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a changeling's corpse" )]
	public class Changeling : BaseCreature
	{
		public override bool AlwaysAttackable { get { return true; } }
		public override bool CheckResistancesInItems { get { return false; } }

		private DateTime m_NextBodyChange;
		private Timer m_ResetTimer;

		[Constructable]
		public Changeling()
			: base( AIType.AI_Arcanist, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a changeling";
			Body = 264;

			BaseSoundID = 0x46E;

			SetStr( 28, 119 );
			SetDex( 209, 250 );
			SetInt( 308, 396 );

			SetHits( 201, 205 );

			SetDamage( 9, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.EvalInt, 90, 100 );
			SetSkill( SkillName.Magery, 90, 100 );
			SetSkill( SkillName.Meditation, 90, 100 );
			SetSkill( SkillName.MagicResist, 120, 135 );
			SetSkill( SkillName.Tactics, 10, 20 );
			SetSkill( SkillName.Wrestling, 10, 15 );
			SetSkill( SkillName.Spellweaving, 50, 60 );

			Fame = 7000;
			Karma = -7000;

			PackSpellweavingScroll();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( 0.25 > Utility.RandomDouble() && DateTime.Now > m_NextBodyChange )
				ChangeBody();

			base.OnDamage( amount, from, willKill );
		}

		public void RestoreBody()
		{
			Name = "a changeling";
			Body = 264;
			Hue = 0;

			foreach ( Item item in GetEquippedItems() )
				if ( item != null && !( item is Backpack ) )
					item.Delete();

			if ( m_ResetTimer != null )
				m_ResetTimer.Stop();
		}

		public void ChangeBody()
		{
			try
			{
				ArrayList list = new ArrayList();

				foreach ( Mobile m in Map.GetMobilesInRange( Location, 5 ) )
					if ( m.IsPlayer && m.AccessLevel == AccessLevel.Player )
						list.Add( m );

				if ( list.Count <= 0 )
				{
					if ( Body != 264 )
						RestoreBody();

					return;
				}

				Mobile attacker = (Mobile) list[Utility.Random( list.Count - 1 )];

				if ( attacker == null ) return;

				if ( !attacker.Body.IsHuman )
					return;

				ArrayList list2 = new ArrayList();

				foreach ( Item item in GetEquippedItems() )
					if ( item != null && !( item is Backpack ) )
						list2.Add( item );

				for ( int i = 0; i < list2.Count; i++ )
					( (Item) list2[i] ).Delete();

				list2.Clear();

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

				m_NextBodyChange = DateTime.Now + TimeSpan.FromSeconds( 10.0 );

				if ( m_ResetTimer != null )
					m_ResetTimer.Stop();

				m_ResetTimer = Timer.DelayCall( TimeSpan.FromMinutes( 1.0 ), new TimerCallback( RestoreBody ) );
			}
			catch ( NullReferenceException e )
			{
				Logger.Error( e.ToString() );
			}
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
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.Gems, 2 );
		}

		public Changeling( Serial serial )
			: base( serial )
		{
		}

		public override HideType HideType { get { return HideType.Spined; } } // TODO: How is it on OSI?
		public override int Hides { get { return 5; } }
		public override int Meat { get { return 1; } }

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
