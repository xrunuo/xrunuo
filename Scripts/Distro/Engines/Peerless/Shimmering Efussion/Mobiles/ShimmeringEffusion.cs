using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a shimmering effusion corpse" )]
	public class ShimmeringEffusion : BaseCreature
	{
		private ArrayList m_Summons = new ArrayList();

		[Constructable]
		public ShimmeringEffusion()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a shimmering effusion";
			Body = 0x105;

			SetStr( 509, 538 );
			SetDex( 354, 381 );
			SetInt( 1513, 1578 );

			SetHits( 20000 );

			SetDamage( 27, 31 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 75, 76 );
			SetResistance( ResistanceType.Fire, 60, 65 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 76, 80 );
			SetResistance( ResistanceType.Energy, 75, 78 );

			SetSkill( SkillName.Wrestling, 100.2, 101.4 );
			SetSkill( SkillName.Tactics, 105.5, 102.1 );
			SetSkill( SkillName.MagicResist, 150 );
			SetSkill( SkillName.Magery, 150.0 );
			SetSkill( SkillName.EvalInt, 150.0 );
			SetSkill( SkillName.Meditation, 120.0 );
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

			c.DropItem( new CapturedEssence() );
			c.DropItem( new ShimmeringCrystals() );

			if ( Utility.RandomDouble() < 0.05 )
			{
				switch ( Utility.Random( 4 ) )
				{
					case 0:
						c.DropItem( new ShimmeringEffusionStatuette() );
						break;
					case 1:
						c.DropItem( new CorporealBrumeStatuette() );
						break;
					case 2:
						c.DropItem( new MantraEffervescenceStatuette() );
						break;
					case 3:
						c.DropItem( new FetidEssenceStatuette() );
						break;
				}
			}

			if ( Utility.RandomDouble() < 0.05 )
				c.DropItem( new FerretImprisonedInCrystal() );

			if ( Utility.RandomDouble() < 0.025 )
				c.DropItem( new CrystallineRing() );

			if ( Utility.RandomDouble() < 0.025 )
				c.DropItem( new CrimsonCincture() );

			if ( Utility.RandomDouble() < 0.1 )
				c.DropItem( new HumanFeyLeggings() );

			for ( int i = 0; i < 2; i++ )
			{
				if ( 5000 > Utility.Random( 100000 ) )
					c.DropItem( SetItemsHelper.GetRandomSetItem() );
			}
		}

		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 5; } }
		public bool HasFireRing { get { return true; } }
		public double FireRingChance { get { return 0.1; } }

		public override int GetIdleSound() { return 0x1BF; }
		public override int GetAttackSound() { return 0x1C0; }
		public override int GetHurtSound() { return 0x1C1; }
		public override int GetDeathSound() { return 0x1C2; }

		#region Helpers
		public bool CanSpawnHelpers { get { return true; } }
		public int MaxHelpersWaves { get { return 4; } }
		public double SpawnHelpersChance { get { return 0.1; } }

		//TODO: Invocacion de los ayudantes
		//public void SpawnHelpers()
		//{
		//    int amount = 1;

		//    //TODO: Debe coger de algun sitio el numero de personas que han entrado
		//    //if ( Altar != null )
		//    //	amount = Altar.Fighters.Count;

		//    if ( amount > 5 )
		//        amount = 5;

		//    for ( int i = 0; i < amount; i ++ )
		//    {				
		//        switch ( Utility.Random( 2 ) )
		//        {
		//            case 0: SpawnHelper( new MantraEffervescence(), 2 ); break;
		//            case 1: SpawnHelper( new CorporealBrume(), 2 ); break;
		//            case 2: SpawnHelper( new FetidEssence(), 2 ); break;
		//        }				
		//    }
		//}

		public override void OnBeforeSpawn( Point3D location, Map map )
		{
			BaseCreature brume = new CorporealBrume();
			brume.Team = this.Team;
			brume.MoveToWorld( location, map );

			BaseCreature mantra = new MantraEffervescence();
			mantra.Team = this.Team;
			mantra.MoveToWorld( location, map );

			BaseCreature fetid = new FetidEssence();
			fetid.Team = this.Team;
			fetid.MoveToWorld( location, map );

			m_Summons.Add( brume );
			m_Summons.Add( mantra );
			m_Summons.Add( fetid );

			base.OnBeforeSpawn( location, map );
		}
		#endregion

		public ShimmeringEffusion( Serial serial )
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
