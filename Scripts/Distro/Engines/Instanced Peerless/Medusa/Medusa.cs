using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Engines.Loyalty;

namespace Server.Mobiles
{
	[CorpseName( "Medusa's corpse" )]
	public class Medusa : BaseCreature, ICarvable
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		private const int InitialStatueAmount = 12;

		private List<BaseCreature> m_Statues;
		private DateTime m_NextCarve;
		private int m_ScalesLeft = 15;

		[Constructable]
		public Medusa()
			: base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.175, 0.350 )
		{
			Name = "Medusa";
			Body = 728;

			SetStr( 1378, 1397 );
			SetDex( 129, 143 );
			SetInt( 575, 671 );

			SetHits( 50000 );

			SetDamage( 21, 28 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Energy, 20 );

			SetResistance( ResistanceType.Physical, 55, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 55, 60 );
			SetResistance( ResistanceType.Poison, 85, 90 );
			SetResistance( ResistanceType.Energy, 65, 70 );

			SetSkill( SkillName.Anatomy, 110.0, 120.0 );
			SetSkill( SkillName.MagicResist, 120.0, 130.0 );
			SetSkill( SkillName.Tactics, 120.0, 130.0 );
			SetSkill( SkillName.Wrestling, 115.0, 125.0 );
			SetSkill( SkillName.Magery, 115.0, 125.0 );
			SetSkill( SkillName.EvalInt, 100.0, 130.0 );
			SetSkill( SkillName.Meditation, 100.0, 110.0 );

			Fame = 22000;
			Karma = -22000;

			Bow bow = new Bow();
			bow.Attributes.SpellChanneling = 1;
			bow.Attributes.CastSpeed = 1;
			bow.LootType = LootType.Blessed;
			AddItem( bow );

			PackItem( new Arrow( Utility.RandomMinMax( 125, 175 ) ) );

			m_Statues = new List<BaseCreature>();
		}

		public override int GetAttackSound() { return 0x612; }
		public override int GetDeathSound() { return 0x613; }
		public override int GetHurtSound() { return 0x614; }
		public override int GetIdleSound() { return 0x615; }

		#region Carve Scales
		public void Carve( Mobile from, Item item )
		{
			if ( m_ScalesLeft > 0 )
			{
				if ( DateTime.UtcNow < m_NextCarve )
				{
					// The creature is still recovering from the previous harvest. Try again in a few seconds.
					from.SendLocalizedMessage( 1112677 );
				}
				else
				{
					from.RevealingAction();

					if ( 0.2 > Utility.RandomDouble() )
					{
						int amount = Math.Min( m_ScalesLeft, Utility.RandomMinMax( 2, 3 ) );

						m_ScalesLeft -= amount;

						Item scales = new GreaterMedusaScales( amount );

						if ( from.PlaceInBackpack( scales ) )
						{
							// You harvest magical resources from the creature and place it in your bag.
							from.SendLocalizedMessage( 1112676 );
						}
						else
						{
							scales.MoveToWorld( from.Location, from.Map );
						}

						m_NextCarve = DateTime.UtcNow + TimeSpan.FromMinutes( 1.0 );
					}
					else
					{
						from.SendLocalizedMessage( 1112675, "", 33 ); // Your attempt fails and angers the creature!!

						PlaySound( GetHurtSound() );

						Combatant = from;
					}
				}
			}
			else
			{
				from.SendLocalizedMessage( 1112674 ); // There's nothing left to harvest from this creature.
			}
		}
		#endregion

		#region Statues
		private static Type[] m_StatueTypes = new Type[]
			{
				typeof( OphidianArchmage ),		typeof( OphidianWarrior ),
				typeof( WailingBanshee ),		typeof( OgreLord ),
				typeof( Dragon ),				typeof( UndeadGargoyle )
			};

		private void AssignRandomAnimation( BaseCreature bc )
		{
			int animation, frames;

			switch ( Utility.Random( 6 ) )
			{
				default:
				case 0: animation = 4; frames = 0; break;
				case 1: animation = 16; frames = 2; break;
				case 2: animation = 33; frames = 1; break;
				case 3: animation = 17; frames = 4; break;
				case 4: animation = 31; frames = 5; break;
				case 5: animation = 6; frames = 1; break;
			}

			bc.StatueAnimation = animation;
			bc.StatueFrames = frames;
		}

		private BaseCreature CreateStatue()
		{
			try
			{
				BaseCreature bc = (BaseCreature) Activator.CreateInstance( m_StatueTypes[Utility.Random( m_StatueTypes.Length )] );

				bc.Frozen = true;
				bc.Petrified = true;
				bc.Blessed = true;
				bc.HueMod = 2401;
				bc.Direction = (Direction) Utility.Random( 8 );

				AssignRandomAnimation( bc );

				return bc;
			}
			catch
			{
				return null;
			}
		}

		public override void OnBeforeSpawn( Point3D location, Map map )
		{
			base.OnBeforeSpawn( location, map );

			for ( int i = 0; i < InitialStatueAmount; i++ )
			{
				BaseCreature statue = CreateStatue();

				if ( statue != null )
				{
					Point3D loc = map.GetSpawnPosition( location, 40 );

					statue.MoveToWorld( loc, map );
					statue.OnBeforeSpawn( loc, map );

					m_Statues.Add( statue );
				}
			}
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			for ( int i = 0; i < m_Statues.Count; i++ )
			{
				BaseCreature bc = m_Statues[i];

				if ( !bc.Deleted )
					bc.Delete();
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( 0.1 > Utility.RandomDouble() )
			{
				if ( m_Statues.Count > 0 )
				{
					BaseCreature bc = m_Statues[0];

					m_Statues.RemoveAt( 0 );

					if ( bc != null && !bc.Deleted )
					{
						PublicOverheadMessage( MessageType.Regular, 33, 1112767 ); // Medusa releases one of the petrified creatures!!

						bc.Petrified = false;
						bc.Frozen = false;
						bc.Blessed = false;
						bc.HueMod = -1;
					}
				}
			}
		}
		#endregion

		#region Replicas
		private EvilReplica BuildReplica( Mobile m )
		{
			if ( m is BaseCreature )
				return new PetReplica( m as BaseCreature );
			else
				return new PlayerReplica( m );
		}

		private void CreateReplica( Mobile m )
		{
			EvilReplica replica = BuildReplica( m );

			replica.OnBeforeSpawn( m.Location, m.Map );
			replica.MoveToWorld( m.Location, m.Map );

			if ( m is BaseCreature )
			{
				BaseCreature pet = m as BaseCreature;

				Mobile master = pet.Summoned ? pet.SummonMaster : pet.ControlMaster;

				if ( master != null )
					master.SendLocalizedMessage( 1113285, "", 42 ); // Beware! A statue of your pet has been created!
			}

			Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerCallback( replica.Unpetrify ) );
		}
		#endregion

		#region Petrification
		private bool CheckGorgonCharges( Mobile m )
		{
			foreach ( var gc in m.GetEquippedItems().OfType<IGorgonCharges>() )
			{
				if ( gc.GorgonCharges > 0 )
				{
					bool resisted = false;
					double chance = 0.0;

					switch ( gc.GorgonQuality )
					{
						case GorgonQuality.Normal:
							chance = 0.6; break;
						case GorgonQuality.Exceptional:
							chance = 0.8; break;
						case GorgonQuality.Invulnerable:
							chance = 1.0; break;
					}

					if ( chance > Utility.RandomDouble() )
					{
						m.SendLocalizedMessage( 1112599 ); // Your Gorgon Lens deflect Medusa's petrifying gaze!
						resisted = true;
					}
					else
					{
						m.SendLocalizedMessage( 1112621 ); // Your lenses fail to deflect Medusa's gaze!!
					}

					gc.GorgonCharges--;

					if ( gc.GorgonCharges == 0 )
						m.SendLocalizedMessage( 1112600 ); // Your lenses crumble. You are no longer protected from Medusa's gaze!

					return resisted;
				}
			}

			return false;
		}

		private void RemovePetrification( Mobile m )
		{
			if ( m is BaseCreature )
			{
				BaseCreature bc = m as BaseCreature;

				bc.Petrified = false;
				bc.Frozen = false;
				bc.Blessed = false;
				bc.HueMod = -1;
			}
			else
			{
				m.SolidHueOverride = -1;
				m.Frozen = false;

				m.SendLocalizedMessage( 502382 ); // You can move!
			}

			if ( 0.6 > Utility.RandomDouble() )
				CreateReplica( m );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.3 > Utility.RandomDouble() )
			{
				if ( !CheckGorgonCharges( attacker ) )
				{
					if ( attacker is BaseCreature )
					{
						BaseCreature pet = attacker as BaseCreature;

						Mobile master = pet.Summoned ? pet.SummonMaster : pet.ControlMaster;

						if ( master != null )
							master.SendLocalizedMessage( 1113281, "", 42 ); // Your pet has been petrified!

						pet.Petrified = true;
						pet.Frozen = true;
						pet.Blessed = true;
						pet.HueMod = 0x2E1;

						AssignRandomAnimation( pet );
					}
					else
					{
						attacker.SolidHueOverride = 0x961;
						attacker.Frozen = true;

						attacker.SendLocalizedMessage( 1112768, "", 33 ); // You have been turned to stone!!!
					}

					Timer.DelayCall<Mobile>( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback<Mobile>( RemovePetrification ), attacker );
				}
			}

			if ( this.InRange( attacker, 2 ) && 0.6 > Utility.RandomDouble() )
			{
				attacker.SendLocalizedMessage( 1112368 ); // You have been poisoned by Medusa's snake-like hair!

				attacker.ApplyPoison( this, Poison.Greater );

				Effects.SendPacket( attacker.Location, attacker.Map, new TargetParticleEffect( attacker, 0x374A, 10, 15, 0, 0, 0x139D, 3, 0 ) );
				Effects.PlaySound( attacker.Location, attacker.Map, 0x574 );
			}
		}
		#endregion

		#region Lethal Arrow
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.5 > Utility.RandomDouble() )
			{
				defender.SendLocalizedMessage( 1112369 ); // You have been poisoned by Medusa's lethal arrow!

				defender.ApplyPoison( this, Utility.RandomBool() ? Poison.Deadly : Poison.Lethal );

				Effects.SendPacket( defender.Location, defender.Map, new TargetParticleEffect( defender, 0x36CB, 1, 18, 0x43, 5, 0x26B7, 3, 0 ) );
				Effects.PlaySound( defender.Location, defender.Map, 0xDD );
			}
		}
		#endregion

		#region Loot
		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 3 );
		}

		private static Type[] UniqueArtifacts = new Type[]
			{
				typeof( Slither ),			typeof( IronwoodCompositeBow )
			};

		private static Type[] SharedArtifacts = new Type[]
			{
				typeof( DemonBridleRing ),	typeof( StoneDragonsTooth ),
				typeof( PetrifiedSnake ),	typeof( StormCaller ),
				typeof( SummonersKilt ),	typeof( Venom )
			};
		private static Type[] DecorativeItems = new Type[]
		{
			typeof( MedusaFloorTileDeed ),	typeof( MedusaStatue )
		};

		private static Item CreateArtifact( Type[] types )
		{
			try { return (Item) Activator.CreateInstance( types[Utility.Random( types.Length )] ); }
			catch { }

			return null;
		}

		protected override void OnAfterDeath( Container c )
		{
			base.OnAfterDeath( c );

			c.DropItem( new MedusaBlood() );

			double random = Utility.RandomDouble();

			Item artifact = null;

			if ( 0.025 > random ) // 2.5% of getting a unique artifact
				artifact = CreateArtifact( UniqueArtifacts );
			else if ( 0.20 > random ) // 17.5% of getting a shared artifact
				artifact = CreateArtifact( SharedArtifacts );
			else if ( 0.25 > random ) // 5% of getting a decorative item
				artifact = CreateArtifact( DecorativeItems );

			if ( artifact != null )
			{
				Mobile m = MonsterHelper.GetTopAttacker( this );

				if ( m != null )
					MonsterHelper.GiveArtifactTo( m, artifact );
				else
					artifact.Delete();
			}
		}

		public override void OnCarve( Mobile from, Corpse corpse, bool butcher )
		{
			base.OnCarve( from, corpse, butcher );

			if ( 0.2 > Utility.RandomDouble() )
				corpse.DropItem( new LesserMedusaScales( 5 ) );
		}
		#endregion

		public override bool CanFlee { get { return false; } }

		public override LoyaltyGroup LoyaltyGroupEnemy { get { return LoyaltyGroup.GargoyleQueen; } }
		public override int LoyaltyPointsAward { get { return 150; } }

		public Medusa( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );

			writer.WriteMobileList( m_Statues );
			writer.Write( (int) m_ScalesLeft );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadInt();

			m_Statues = reader.ReadStrongMobileList<BaseCreature>();
			m_ScalesLeft = reader.ReadInt();
		}
	}

	#region Replicas
	public abstract class EvilReplica : BaseCreature
	{
		public override bool DeleteCorpseOnDeath { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CheckResistancesInItems { get { return false; } }

		public EvilReplica( Mobile orig )
			: base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = String.Format( NameFormat, orig.Name );
			Body = orig.Body;
			Hue = orig.Hue;

			SetStr( orig.Str );
			SetDex( orig.Dex );
			SetInt( orig.Int );

			SetHits( orig.HitsMax );
			SetStam( orig.StamMax );
			SetMana( orig.ManaMax );

			SetResistance( ResistanceType.Physical, orig.PhysicalResistance );
			SetResistance( ResistanceType.Fire, orig.FireResistance );
			SetResistance( ResistanceType.Cold, orig.ColdResistance );
			SetResistance( ResistanceType.Poison, orig.PoisonResistance );
			SetResistance( ResistanceType.Energy, orig.EnergyResistance );

			for ( int i = 0; i < orig.Skills.Length; i++ )
			{
				Skill skill = orig.Skills[i];

				SetSkill( skill.SkillName, skill.NonRacialValue );
			}

			Petrified = true;
			Frozen = true;

			SolidHueOverride = PetrifiedHue;
			HueMod = PetrifiedHue;
		}

		public abstract int PetrifiedHue { get; }
		public abstract string NameFormat { get; }

		public virtual void Unpetrify()
		{
			Petrified = false;
			Frozen = false;
			Blessed = false;

			SolidHueOverride = -1;
			HueMod = -1;
		}

		public EvilReplica( Serial serial )
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

			Delete();
		}
	}

	public class PetReplica : EvilReplica
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return m_Original.GetWeaponAbility();
		}

		public override int PetrifiedHue { get { return 0x2E1; } }
		public override string NameFormat { get { return "{0} (evil)"; } }

		private BaseCreature m_Original;

		public PetReplica( BaseCreature orig )
			: base( orig )
		{
			m_Original = orig;

			Blessed = true;

			BaseSoundID = orig.BaseSoundID;

			SetHits( orig.HitsMax / 2 );

			SetDamage( orig.DamageMin, orig.DamageMax );

			SetDamageType( ResistanceType.Physical, orig.PhysicalDamage );
			SetDamageType( ResistanceType.Fire, orig.FireDamage );
			SetDamageType( ResistanceType.Cold, orig.ColdDamage );
			SetDamageType( ResistanceType.Poison, orig.PoisonDamage );
			SetDamageType( ResistanceType.Energy, orig.EnergyDamage );
		}

		public override void OnBeforeSpawn( Point3D location, Map m )
		{
			base.OnBeforeSpawn( location, m );

			StatueAnimation = m_Original.StatueAnimation;
			StatueFrames = m_Original.StatueFrames;
			Direction = m_Original.Direction;
		}

		public PetReplica( Serial serial )
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

			Delete();
		}
	}

	public class PlayerReplica : EvilReplica
	{
		public override WeaponAbility GetWeaponAbility()
		{
			if ( this.Weapon == null )
				return null;

			BaseWeapon weapon = this.Weapon as BaseWeapon;

			return Utility.RandomBool() ? weapon.PrimaryAbility : weapon.SecondaryAbility;
		}

		public override int PetrifiedHue { get { return 0x961; } }
		public override string NameFormat { get { return "{0} the Evil Twin"; } }

		public override Mobile ConstantFocus { get { return m_Original; } }
		public override bool BardImmune { get { return true; } }

		private Mobile m_Original;
		private DateTime m_ExpireTime;

		public PlayerReplica( Mobile orig )
			: base( orig )
		{
			m_Original = orig;
			m_ExpireTime = DateTime.UtcNow + TimeSpan.FromMinutes( 2.0 );

			HairItemID = orig.HairItemID;
			HairHue = orig.HairHue;
			FacialHairItemID = orig.FacialHairItemID;
			FacialHairHue = orig.FacialHairHue;

			SetDamage( 1, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.DetectHidden, 100.0 );

			SwitchAI();
			CopyEquipment( orig );
		}

		public override void OnThink()
		{
			if ( Petrified )
				return;

			if ( !m_Original.Alive || m_Original.IsDeadBondedPet || DateTime.UtcNow > m_ExpireTime )
			{
				Kill();
				return;
			}
			else if ( Map != m_Original.Map || !this.InRange( m_Original, 15 ) )
			{
				Map fromMap = Map;
				Point3D from = Location;

				Map toMap = m_Original.Map;
				Point3D to = m_Original.Location;

				if ( toMap != null )
				{
					for ( int i = 0; i < 5; ++i )
					{
						Point3D loc = new Point3D( to.X - 4 + Utility.Random( 9 ), to.Y - 4 + Utility.Random( 9 ), to.Z );

						if ( toMap.CanSpawnMobile( loc ) )
						{
							to = loc;
							break;
						}
						else
						{
							loc.Z = toMap.GetAverageZ( loc.X, loc.Y );

							if ( toMap.CanSpawnMobile( loc ) )
							{
								to = loc;
								break;
							}
						}
					}
				}

				Map = toMap;
				Location = to;

				ProcessDelta();

				Effects.SendLocationParticles( EffectItem.Create( from, fromMap, EffectItem.DefaultDuration ), 0x3728, 1, 13, 37, 7, 5023, 0 );
				FixedParticles( 0x3728, 1, 13, 5023, 37, 7, EffectLayer.Waist );

				PlaySound( 0x37D );
			}

			if ( m_Original.Hidden && this.InRange( m_Original, 3 ) && DateTime.UtcNow >= this.NextSkillTime && UseSkill( SkillName.DetectHidden ) )
			{
				Target targ = this.Target;

				if ( targ != null )
					targ.Invoke( this, this );
			}

			Combatant = m_Original;
			FocusMob = m_Original;

			if ( AIObject != null )
				AIObject.Action = ActionType.Combat;

			base.OnThink();
		}

		public override bool OnBeforeDeath()
		{
			Effects.PlaySound( Location, Map, 0x10B );
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, TimeSpan.FromSeconds( 10.0 ) ), 0x37CC, 1, 50, 2101, 7, 9909, 0 );

			Delete();

			return false;
		}

		public override void Unpetrify()
		{
			if ( !m_Original.Alive )
			{
				Delete();
				return;
			}

			base.Unpetrify();

			FixedParticles( 0x376A, 1, 14, 0x13B5, EffectLayer.Waist );
			PlaySound( 0x511 );
		}

		private double GetSkill( SkillName name )
		{
			return Skills[name].Value;
		}

		private void SwitchAI()
		{
			AIType ai;

			if ( GetSkill( SkillName.Necromancy ) > 50.0 )
				ai = AIType.AI_Necromancer;
			else if ( GetSkill( SkillName.Mysticism ) > 50.0 )
				ai = AIType.AI_Mystic;
			else if ( GetSkill( SkillName.Spellweaving ) > 50.0 )
				ai = AIType.AI_Arcanist;
			else if ( GetSkill( SkillName.Magery ) > 50.0 )
				ai = AIType.AI_Mage;
			else if ( GetSkill( SkillName.Archery ) > 50.0 )
				ai = AIType.AI_Archer;
			else
				ai = AIType.AI_Melee;

			ChangeAIType( ai );
		}

		private void CopyEquipment( Mobile orig )
		{
			try
			{
				DupeItem( Layer.OneHanded, orig );
				DupeItem( Layer.TwoHanded, orig );
				DupeItem( Layer.Shoes, orig );
				DupeItem( Layer.Pants, orig );
				DupeItem( Layer.Shirt, orig );
				DupeItem( Layer.Helm, orig );
				DupeItem( Layer.Gloves, orig );
				DupeItem( Layer.Ring, orig );
				DupeItem( Layer.Neck, orig );
				DupeItem( Layer.Waist, orig );
				DupeItem( Layer.InnerTorso, orig );
				DupeItem( Layer.Bracelet, orig );
				DupeItem( Layer.MiddleTorso, orig );
				DupeItem( Layer.Earrings, orig );
				DupeItem( Layer.Arms, orig );
				DupeItem( Layer.Cloak, orig );
				DupeItem( Layer.OuterTorso, orig );
				DupeItem( Layer.OuterLegs, orig );
				DupeItem( Layer.InnerLegs, orig );
			}
			catch
			{
			}
		}

		private void DupeItem( Layer layer, Mobile attacker )
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

		public PlayerReplica( Serial serial )
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

			Delete();
		}
	}
	#endregion
}
