//Plague beast lord
/*
 Many variable names are in Spanish, that can confuse a bit.
 But comments are in English.
*/

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Network;

namespace Server.Mobiles
{

	[CorpseName( "a plague beast lord corpse" )]
	public class PlagueBeastLord : BaseCreature
	{
		//Monster life time, once it is opened.
		public static int TiempoVida = 60;//seconds

		//These are the positions of the 8 organs of the beast
		public static Point3D[] Pos_Organos = new Point3D[]
		   {
	   	     new Point3D(96,265,0),
			 new Point3D(70,180,0),
			 new Point3D(293,265,0),
			 new Point3D(96,95,0),
			 new Point3D(293,100,0),
 			 new Point3D(188,98,0),
			 new Point3D(315,180,0),
 			 new Point3D(188,263,0)
			};

		// There will be 8 organs "randomly" placed
		public static OrganoTypes[] Tipo_Organos = new OrganoTypes[]
			{
			 OrganoTypes.Organo_Maiden,
			 OrganoTypes.Organo_Pulpo,
			 OrganoTypes.Organo_Calamar,
			 OrganoTypes.Organo_Venas,
			 OrganoTypes.Organo_Maiden,
			 OrganoTypes.Organo_Pulpo,
			 OrganoTypes.Organo_Venas,
			 OrganoTypes.Organo_Maiden,
			};

		// Brain type inside each organ
		public static BrainTypes[] Color_Cere = new BrainTypes[]
			{
 			 BrainTypes.Brain_Blue,
 			 BrainTypes.Brain_Green,
 			 BrainTypes.Brain_Purple,
 			 BrainTypes.Brain_Yellow,
 			 BrainTypes.Brain_None,  //That means the organ doesn't have brain inside
 			 BrainTypes.Brain_None,
 			 BrainTypes.Brain_None,
 			 BrainTypes.Brain_None,
			};


		Item central;             //Central organ
		Item Org_Calamar;         //Squid organ. To notify it when a healthy gland is placed.
		bool m_congelado = false;   //Frozen flag
		bool m_abierto = false;     //Open flag
		int m_secsLeft = TiempoVida;//Life Time

		bool[] corazones = new bool[] { false, false, false, false };  //Correctly placed brains

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Congelado
		{
			get { return m_congelado; }
			set { m_congelado = value; }
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public bool Abierto
		{
			get { return m_abierto; }
			set
			{
				m_abierto = value;
				Frozen = value;
			}
		}

		[CommandProperty( AccessLevel.Counselor, AccessLevel.GameMaster )]
		public int SegundosVivo
		{
			get { return m_secsLeft; }
			set { m_secsLeft = value; }
		}


		[CommandProperty( AccessLevel.GameMaster )]
		public override int HitsMax { get { return 1800; } }

		public override Poison PoisonImmune { get { return Poison.Lethal; } }


		[Constructable]
		public PlagueBeastLord()
			: base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 1, 1 )
		{
			Body = 775;
			//All strings of plague beast stuff are placed on Plague_Texts class.
			//This way translate it to your own language is easy.
			Name = Plague_Texts.Line[0] + " lord";
			//1029755
			BaseSoundID = 679;
			SpeechHue = 38;
			EmoteHue = 38;
			Str = 500;
			Dex = 100;
			Int = 30;

			SetSkill( SkillName.Wrestling, 100, 100 );
			SetSkill( SkillName.Tactics, 100, 100 );

			SetHits( 1800, 1800 );
			SetDamage( 20, 25 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 25 );
			SetDamageType( ResistanceType.Fire, 25 );


			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 75, 85 );
			SetResistance( ResistanceType.Energy, 25, 35 );


			Hits = 1800;
			SetFameLevel( 2 );
			SetKarmaLevel( 2 );
			//We change the original backpack to a custom backpack: PlagueBackpack.
			Container pack = Backpack;
			if ( pack != null ) pack.Delete(); //Delete the old Backpack
			pack = new PlagueBackpack();
			AddItem( pack );
		}

		public PlagueBeastLord( Serial serial )
			: base( serial )
		{
		}

		///////////////////////////////////////////////////////////////////
		// CrearItem:
		//  Adds an item to the creature, and place it on a give position p
		///////////////////////////////////////////////////////////////////
		public void CrearItem( Item t, Point3D p )
		{
			Backpack.AddItem( t );
			t.Location = p;
			if ( t is BaseOrgano )
			{
				BaseOrgano vis = t as BaseOrgano;
				vis.CrearVisceras();
			}
		}

		///////////////////////////////////////////////////////////////////
		// Abrir:
		//  Open the creature. Creates the puzzle.
		///////////////////////////////////////////////////////////////////
		public void Abrir( Mobile from )
		{
			if ( m_abierto ) //Already opened
			{
				Backpack.DisplayTo( from );
				return;
			}
			m_abierto = true; //Cannot be opened twice

			//Place decorative stuff. Blood, red receptacles...
			for ( int i = 0; i < 8; i++ )
			{
				CrearItem( new Plague_deco( 2527, Plague_Texts.Line[1], 32, false, true ), new Point3D( Utility.RandomMinMax( 80, 360 ), Utility.RandomMinMax( 100, 270 ), 0 ) );
				CrearItem( new Plague_deco( 7570 + ( i % 5 ), Plague_Texts.Line[25], 0, false, true ), new Point3D( Utility.RandomMinMax( 60, 350 ), Utility.RandomMinMax( 90, 250 ), 0 ) );
				CrearItem( new Plague_deco( 7570 + ( i % 5 ), Plague_Texts.Line[25], 0, false, true ), new Point3D( Utility.RandomMinMax( 80, 360 ), Utility.RandomMinMax( 110, 270 ), 0 ) );
				CrearItem( new Plague_deco( 7434 + i, Plague_Texts.Line[25], 0, false, true ), new Point3D( Utility.RandomMinMax( 80, 360 ), Utility.RandomMinMax( 100, 270 ), 0 ) );
			}

			//We place the organs and brains "randomly"
			int Posicion_ = Utility.RandomMinMax( 0, 7 );
			int Color_ = Utility.RandomMinMax( 0, 7 );
			int Tipo_ = Utility.RandomMinMax( 0, 7 );
			//Place organs and other useful things
			for ( int i = 0; i < 8; i++ )
			{
				//Depending on what kind of organ we create one of the four possibles:
				switch ( Tipo_Organos[( Tipo_ + i ) % 8] )
				{
					case OrganoTypes.Organo_Calamar: Org_Calamar = new Organo_Calamar( Color_Cere[( Color_ + i ) % 8] );
						CrearItem( Org_Calamar, Pos_Organos[( Posicion_ + i ) % 8] );
						break;
					case OrganoTypes.Organo_Maiden: CrearItem( new Organo_Maiden( Color_Cere[( Color_ + i ) % 8] ), Pos_Organos[( Posicion_ + i ) % 8] ); break;
					case OrganoTypes.Organo_Pulpo: CrearItem( new Organo_Pulpo( Color_Cere[( Color_ + i ) % 8] ), Pos_Organos[( Posicion_ + i ) % 8] ); break;
					case OrganoTypes.Organo_Venas: CrearItem( new Organo_Venas( Color_Cere[( Color_ + i ) % 8] ), Pos_Organos[( Posicion_ + i ) % 8] ); break;
				}
			}
			//Now we create the receptacles near the core
			//First, the veins connecting them to the central organ
			CrearItem( new Plague_deco( 6939, Plague_Texts.Line[28], PlagueBrain.BrainColors[1], false, true ), PlagueBrain.Recpt_Connectors[1] );
			CrearItem( new Plague_deco( 6940, Plague_Texts.Line[28], PlagueBrain.BrainColors[2], false, true ), PlagueBrain.Recpt_Connectors[2] );
			CrearItem( new Plague_deco( 6940, Plague_Texts.Line[28], PlagueBrain.BrainColors[3], false, true ), PlagueBrain.Recpt_Connectors[3] );
			CrearItem( new Plague_deco( 6939, Plague_Texts.Line[28], PlagueBrain.BrainColors[4], false, true ), PlagueBrain.Recpt_Connectors[4] );
			//Then, the 4 receptacles.
			for ( int i = 1; i < 5; i++ )
			{
				CrearItem( new Plague_deco( 2527, Plague_Texts.Line[1], PlagueBrain.BrainColors[i], false, true ), PlagueBrain.BrainSlot[i] );
			}

			//Create the Central organ.
			central = new OrganoCentral();
			CrearItem( central, new Point3D( 153, 176, 0 ) );
			//Turn to beast and say something
			from.Direction = from.GetDirectionTo( this );
			from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[2] );
			//After that, its backpack is opened
			Backpack.DisplayTo( from );
		}

		///////////////////////////////////////////////////////////////////
		// Place_Gland:
		//   Check it the healthy gland is correctly placed on the squid organ
		// If yes, use the method CambiaGland on Organo_Calamar
		///////////////////////////////////////////////////////////////////
		public void Place_Gland( Point3D p, Item glandula, Mobile from )
		{
			if ( ( ( Org_Calamar.X - 10 ) <= p.X ) && ( p.X <= ( Org_Calamar.X + 30 ) )
				   && ( ( Org_Calamar.Y - 10 ) <= p.Y ) && ( p.Y <= ( Org_Calamar.Y + 30 ) ) )
			{
				Organo_Calamar OC = Org_Calamar as Organo_Calamar;
				OC.CambiaGland( from, glandula );
			}
		}

		///////////////////////////////////////////////////////////////////
		// Congelar:
		//  Freeze the creature
		///////////////////////////////////////////////////////////////////
		public void Congelar( Mobile from )
		{
			if ( m_congelado ) return; //Can't be re-frozen
			//We freeze the creature
			m_congelado = true;
			Frozen = true;
			Hue = 0x480; //Ice hue
			//Will last 60 seconds
			SegundosVivo = TiempoVida;
			new InternalTimer( this ).Start();
			//Forget the combatant (avoid that the frozen beast attacks somebody)
			FocusMob = null;
			Combatant = null;
			FightMode = FightMode.None;
			//Emote(Plague_Texts.Line[3]);	 Doesn't work?
			//Speech stuff
			if ( from != null ) from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[3] );
		}

		///////////////////////////////////////////////////////////////////
		// Descongelar:
		//  Unfreeze the creature.
		///////////////////////////////////////////////////////////////////
		public void Descongelar( Mobile from )
		{
			FightMode = FightMode.Closest;
			Hue = 0;
			Congelado = false;
			Frozen = false;
			//Emote(Plague_Texts.Line[4]);
			if ( from != null ) from.LocalOverheadMessage( MessageType.Emote, 0x66B, false, Plague_Texts.Line[4] );
		}


		///////////////////////////////////////////////////////////////////
		// ColocarCerebro:
		//	 Used when a brain is moved.
		//  If all brains are correctly placed. The central organ is opened
		///////////////////////////////////////////////////////////////////
		public void ColocarCerebro( int cual, bool colocado )
		{
			corazones[cual] = colocado;
			for ( int i = 0; i < 4; i++ )
				if ( !corazones[i] )
				{ return; } //If any brain isn't correctly placed we exit

			//All brain placed
			OrganoCentral OC = central as OrganoCentral;
			OC.Abrir( this );
		}


		public override void OnDoubleClick( Mobile from )
		{
			if ( m_abierto ) Backpack.DisplayTo( from );
			else base.OnDoubleClick( from );
		}


		public override bool CheckNonlocalDrop( Mobile from, Item item, Item target )
		{
			//Only brains and healthy glands can be drop inside it.
			return ( ( item is PlagueBrain ) || ( item is Plague_gland ) );
		}

		public override bool CheckNonlocalLift( Mobile from, Item item )
		{
			//We always can move things from its backpack
			return ( true );
		}

		//If he is damaged, it unfreezes
		public override void AlterDamageScalarFrom( Mobile caster, ref double scalar )
		{
			if ( ( Congelado ) && ( !Abierto ) ) { Descongelar( caster ); }
			base.AlterDamageScalarFrom( caster, ref scalar );
		}
		//Idem
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			if ( ( Congelado ) && ( !Abierto ) ) { Descongelar( attacker ); }
			base.OnGotMeleeAttack( attacker );
		}


		public override bool OnBeforeDeath()
		{
			if ( m_congelado )
			{
				Summoned = true; //Don't left corpse
			}
			return base.OnBeforeDeath();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( (bool) m_congelado );
			writer.Write( (bool) m_abierto );
			writer.Write( (int) m_secsLeft );
			for ( int i = 0; i < 4; i++ )
			{ writer.Write( (bool) corazones[i] ); }
			writer.Write( (Item) central );
			writer.Write( (Item) Org_Calamar );
		}


		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			/*int version = */
			reader.ReadInt();
			m_congelado = reader.ReadBool();
			Frozen = m_congelado;
			if ( m_congelado ) new InternalTimer( this ).Start();
			m_abierto = reader.ReadBool();
			m_secsLeft = reader.ReadInt();
			for ( int i = 0; i < 4; i++ )
			{ corazones[i] = reader.ReadBool(); }
			central = reader.ReadItem();
			Org_Calamar = reader.ReadItem();
		}

		private class InternalTimer : Timer
		{
			private PlagueBeastLord controlado;

			public InternalTimer( PlagueBeastLord creature )
				: base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				controlado = creature;
			}

			protected override void OnTick() //Each second
			{
				if ( ( controlado != null ) && ( controlado.Alive ) )
				{
					if ( !controlado.Congelado ) Stop();

					controlado.SegundosVivo--;
					//At half time, it warns you.
					if ( controlado.SegundosVivo == ( TiempoVida / 2 ) )
					{
						if ( controlado.Abierto )
							controlado.Say( 1053036 );//The plague beast begins to dissolve
						else controlado.Say( 1053035 );//The plague beast burbles incoherently
					}

					if ( controlado.SegundosVivo < 1 )
					{
						Stop();
						if ( controlado.Abierto )
						{
							controlado.Say( false, Plague_Texts.Line[5] );
							controlado.Kill();
						}
						else { controlado.Descongelar( null ); }

					}
				}
				else Stop();
			}
		}
	}
}
