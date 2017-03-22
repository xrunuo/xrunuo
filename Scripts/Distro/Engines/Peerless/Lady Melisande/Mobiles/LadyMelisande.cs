using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	public class LadyMelisande : BaseCreature
	{
		private ArrayList m_Summons = new ArrayList();

		[Constructable]
		public LadyMelisande()
			: base( AIType.AI_Necromancer, FightMode.Closest, 18, 1, 0.2, 0.4 )
		{
			Name = "Lady Melisande";
			Body = 258;
			BaseSoundID = 0x284;

			SetStr( 1281, 1305 );
			SetDex( 600, 900 );
			SetInt( 1226, 1250 );

			SetHits( 30000 );

			SetDamage( 31, 40 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 55, 70 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.EvalInt, 120.0, 130.0 );
			SetSkill( SkillName.Magery, 120.0, 130.0 );
			SetSkill( SkillName.MagicResist, 140.0, 150.0 );
			SetSkill( SkillName.Tactics, 120.0, 130.0 );
			SetSkill( SkillName.Anatomy, 90.0, 100.0 );
			SetSkill( SkillName.Wrestling, 110.0, 120.0 );
			SetSkill( SkillName.Meditation, 110.0, 110.0 );
			SetSkill( SkillName.Necromancy, 110.0, 120.0 );
			SetSkill( SkillName.SpiritSpeak, 120.0, 130.0 );

			Fame = 25000;
			Karma = -25000;
		}

		public override void OnBeforeSpawn( Point3D location, Map map )
		{
			for ( int i = 0; i < 4; i++ )
			{
				BaseCreature satyr = new EnslavedSatyr();

				satyr.Team = this.Team;
				satyr.MoveToWorld( location, map );

				m_Summons.Add( satyr );
			}

			base.OnBeforeSpawn( location, map );
		}

		public override void OnAfterDelete()
		{
			for ( int n = 0; n < m_Summons.Count; n++ )
			{
				Mobile m = (Mobile) m_Summons[n];

				if ( m != null && !m.Deleted )
					m.Kill();
			}

			base.OnAfterDelete();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
			AddLoot( LootPack.PeerlessIngredients, 8 );
			AddLoot( LootPack.Talismans, Utility.RandomMinMax( 1, 5 ) );
		}

		public override bool Unprovokable { get { return true; } }

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			if ( Utility.RandomBool() )
				c.DropItem( new DiseasedBark( Utility.RandomMinMax( 1, 3 ) ) );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new CrimsonCincture() );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new MelisandesCorrodedHatchet() );

			if ( 20000 > Utility.Random( 100000 ) )
				c.DropItem( new EternallyCorruptTree() );

			if ( 2500 > Utility.Random( 100000 ) )
				c.DropItem( new MelisandeHairDye() );

			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new AlbinoSquirrelImprisonedInCrystal() );

			if ( 5000 > Utility.Random( 100000 ) )
				c.DropItem( new MelisandesFermentedWine() );

			if ( 10000 > Utility.Random( 100000 ) )
				c.DropItem( new HumanFeyLeggings() );

			for ( int i = 0; i < 2; i++ )
			{
				if ( 5000 > Utility.Random( 100000 ) )
					c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}
		}

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && ( ( (BaseCreature) m ).Controlled || ( (BaseCreature) m ).Summoned || ( (BaseCreature) m ).Team != this.Team ) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				this.FixedParticles( 0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head );
				m.FixedParticles( 0x373A, 1, 15, 9913, 67, 7, EffectLayer.Head );
				m.PlaySound( 0x1BB );

				m.SendLocalizedMessage( 1075120 ); // An unholy aura surrounds Lady Melisande as her wounds begin to close.

				int toDrain = Utility.RandomMinMax( 10, 40 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.2 >= Utility.RandomDouble() )
				Say( Utility.RandomMinMax( 1075102, 1075118 ) );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.01 > Utility.RandomDouble() && m_Summons.Count < 4 )
			{
				BaseCreature summon = null;

				if ( Utility.RandomBool() )
					summon = new EnslavedSatyr();
				else
					summon = new InsaneDryad();

				summon.Team = this.Team;
				summon.MoveToWorld( this.Location, this.Map );

				m_Summons.Add( summon );

				Say( 1075119 ); // Awake my children!  I summon thee!
			}

			if ( 0.2 >= Utility.RandomDouble() )
				Say( Utility.RandomList( 1075102, 1075118 ) );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}

		public LadyMelisande( Serial serial )
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