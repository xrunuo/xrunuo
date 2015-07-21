using System;
using Server;

namespace Server.Items
{
	public enum TerMurBookEdition
	{
		RegularEdition,
		CollectorsEdition,
		FirstEdition,
		LimitedEdition
	}

	public abstract class TerMurBook : BaseBook
	{
		public static TerMurBook ConstructRandom()
		{
			try
			{
				TerMurBook book = Activator.CreateInstance( m_Books[Utility.Random( m_Books.Length )] ) as TerMurBook;

				return book;
			}
			catch
			{
				return null;
			}
		}

		private static Type[] m_Books = new Type[]
			{
				typeof( AliceInWonderland ),			typeof( ATreatiseOnTheLoreOfGargoyles ),
				typeof( BaldwinsBigBookOfBaking ),		typeof( CrossbowMarksmanship ),
				typeof( DilzalsAlmanacOfGoodAdvice ),	typeof( HubertsHairRaisingAdventure ),
				typeof( KnightsOfLegendVolumeI ),		typeof( KodeksBenmontas ),
				typeof( KodeksBenommani ),				typeof( KodeksDestermur ),
				typeof( KodeksKir ),					typeof( KodeksRit ),
				typeof( KodeksXen ),					typeof( LogbookOfTheEmpire ),
				typeof( OfDreamsAndVisions ),			typeof( PlansForTheConstructionOfAHotAirBalloon ),
				typeof( PlantLore ),					typeof( SnilwitsBigBookOfBoardgameStrategy ),
				typeof( TangledTales ),					typeof( TheBookOfCircles ),
				typeof( TheBookOfAdministration ),		typeof( TheBookOfProsperity ),
				typeof( TheBookOfFamily ),				typeof( TheBookOfSpirituality ),
				typeof( TheBookOfRitual ),				typeof( TheCavernsOfFreitag ),
				typeof( TheBookOfTheUnderworld ),		typeof( TheFirstAgeOfDarkness ),
				typeof( TheCodexOfInfiniteWisdom ),		typeof( TheLostBookOfMantras ),
				typeof( TheQuestOfTheAvatar ),			typeof( TheSecondAgeOfDarkness ),
				typeof( TheThirdAgeOfDarkness ),		typeof( TheWizardOfOz ),
				typeof( WarriorsOfDestiny ),			typeof( Windwalker ),
				typeof( YeLostArtOfBallooning )
			};

		private TerMurBookEdition m_Edition;

		[Constructable]
		public TerMurBook( string title, string author, int pageCount )
			: base( 0xFF2, title, author, pageCount, false )
		{
			if ( 0.05 > Utility.RandomDouble() )
			{
				switch ( Utility.Random( 3 ) )
				{
					case 0:
						m_Edition = TerMurBookEdition.CollectorsEdition;
						Hue = 1150;
						break;

					case 1:
						m_Edition = TerMurBookEdition.LimitedEdition;
						Hue = 1157;
						break;

					case 2:
						m_Edition = TerMurBookEdition.FirstEdition;
						Hue = 1156;
						break;
				}
			}
			else
			{
				m_Edition = TerMurBookEdition.RegularEdition;
				Hue = Utility.RandomDyedHue();
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			switch ( m_Edition )
			{
				case TerMurBookEdition.CollectorsEdition:
					list.Add( 1113207 ); // [Collector's Edition]
					break;

				case TerMurBookEdition.LimitedEdition:
					list.Add( 1113208 ); // [Limited Edition]
					break;

				case TerMurBookEdition.FirstEdition:
					list.Add( 1113209 ); // [First Edition]
					break;
			}
		}

		public TerMurBook( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version

			writer.Write( (int) m_Edition );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();

			m_Edition = (TerMurBookEdition) reader.ReadInt();
		}
	}

	public class AliceInWonderland : TerMurBook
	{
		[Constructable]
		public AliceInWonderland()
			: base( "Alice In Wonderland", "Lewis Carroll", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"Alice saw a peculiar",
					"white rabbit one day. It",
					"was looking at its pocket",
					"watch and worrying about",
					"how late it was. Alice",
					"chased it down a rabbit",
					"hole, and fell a very",
					"long way. She found"
				};

			Pages[1].Lines = new string[]
				{
					"herself in a strange",
					"land. She went to a tea",
					"party there, with a mad",
					"hatter and a dormouse.",
					"She also met a strange",
					"caterpillar, and a cat",
					"that could vanish, with",
					"its grin disappearing"
				};

			Pages[2].Lines = new string[]
				{
					"last. The queen of hearts",
					"yelled ‘Off with her",
					"head!’ and her guards ran",
					"up to grab Alice – but",
					"then she awakened, and",
					"realized it had all been",
					"a dream."
				};
		}

		public AliceInWonderland( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class ATreatiseOnTheLoreOfGargoyles : TerMurBook
	{
		[Constructable]
		public ATreatiseOnTheLoreOfGargoyles()
			: base( "A Treatise on the Lore of Gargoyles", "Norlick the Elder", 5 )
		{
			Pages[0].Lines = new string[]
				{
					"Though gargoyles are",
					"considered by most to",
					"be mere legend, no",
					"records exist",
					"documenting the origins",
					"of the gargoyle",
					"'statues' that adorn",
					"many castles. Even the"
				};
			Pages[1].Lines = new string[]
				{
					"towering stone",
					"guardians of the Codex",
					"of Ultimate Wisdom have",
					"many of the physical",
					"characteristics of the",
					"'legendary' gargoyle.",
					"Nobody seems to know",
					"where they came from"
				};
			Pages[2].Lines = new string[]
				{
					"either. Despite the",
					"lack of hard evidence,",
					"there have been a fair",
					"number of unconfirmed",
					"reports of sightings of",
					"live gargoyles.It is",
					"the opinion of this",
					"author that daemons are"
				};
			Pages[3].Lines = new string[]
				{
					"a form of gargoyle. As",
					"many reliable",
					"encounters with daemons",
					"have been documented in",
					"various scholarly",
					"works, perhaps this is",
					"the best source of",
					"further information on"
				};
			Pages[4].Lines = new string[]
				{
					"the subject of",
					"gargoyles."
				};
		}

		public ATreatiseOnTheLoreOfGargoyles( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class BaldwinsBigBookOfBaking : TerMurBook
	{
		[Constructable]
		public BaldwinsBigBookOfBaking()
			: base( "Baldwin’s Big Book Of Baking", "Baldwin", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"Though some might scoff",
					"at the idea, the making",
					"of breads, pastries, pies,",
					"and cakes is one of the",
					"highest callings in life.",
					"Study this book carefully,",
					"and someday you may be",
					"prepared to take on this",
				};

			Pages[1].Lines = new string[]
				{
					"awesome responsibility."
				};
		}

		public BaldwinsBigBookOfBaking( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class CrossbowMarksmanship : TerMurBook
	{
		[Constructable]
		public CrossbowMarksmanship()
			: base( "Crossbow Marksmanship", "Iolo Fitzowen", 1 )
		{
			Pages[0].Lines = new string[]
				{
					"There is a zen to",
					"shooting well. Become",
					"one with your crossbow.",
					"Clear your mind of all",
					"thoughts save that of",
					"flying with the bolt to",
					"strike the target, and",
					"you will not miss."
				};
		}

		public CrossbowMarksmanship( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class DilzalsAlmanacOfGoodAdvice : TerMurBook
	{
		[Constructable]
		public DilzalsAlmanacOfGoodAdvice()
			: base( "Dilzal's Almanac of Good Advice", "Dilzal", 1 )
		{
			Pages[0].Lines = new string[]
				{
					"Gambling is the surest",
					"way of getting nothing",
					"for something. Small",
					"deeds done are better",
					"than great deeds",
					"planned. Never play",
					"backgammon with a",
					"centaur."
				};
		}

		public DilzalsAlmanacOfGoodAdvice( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class HubertsHairRaisingAdventure : TerMurBook
	{
		[Constructable]
		public HubertsHairRaisingAdventure()
			: base( "Hubert's Hair Raising Adventure", "Bill Pete", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"Written and illustrated",
					"by Bill Pete Hubert the",
					"Lion was haughty and",
					"vain, And especially",
					"proud of his elegant",
					"mane. But conceit of",
					"this sort is not proper",
					"at all, And Hubert the"
				};

			Pages[1].Lines = new string[]
				{
					"Lion was due for a fall."
				};
		}

		public HubertsHairRaisingAdventure( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KnightsOfLegendVolumeI : TerMurBook
	{
		[Constructable]
		public KnightsOfLegendVolumeI()
			: base( "Knights of Legend, Volume I", "Unknown", 5 )
		{
			Pages[0].Lines = new string[]
				{
					"Once, in the kingdom of",
					"Ashtalarea, there was a",
					"great war. For a time",
					"it seemed the forces of",
					"evil would triumph, but",
					"through the valiant",
					"efforts of Seggallion,",
					"the greatest knight in"
				};
			Pages[1].Lines = new string[]
				{
					"the land, the forces of",
					"the dark lord Pildar",
					"were finally driven",
					"back and defeated. All",
					"was well for a time,",
					"but years later Pildar",
					"had grown powerful",
					"again. From his dark"
				};
			Pages[2].Lines = new string[]
				{
					"tower he wove his evil",
					"schemes, and managed to",
					"capture the Duke, and",
					"the great knight",
					"Seggallion as well.",
					"With his greatest foe",
					"out of the way, he was",
					"prepared once more to"
				};
			Pages[3].Lines = new string[]
				{
					"set out on a campaign",
					"of conquest. In those",
					"troubled times, a party",
					"of great heroes arose.",
					"After many perilous",
					"adventures they finally",
					"won their way to",
					"Seggallion's prison and"
				};
			Pages[4].Lines = new string[]
				{
					"set him free. They then",
					"set forth to discover",
					"what had become of the",
					"Duke..."
				};
		}

		public KnightsOfLegendVolumeI( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksBenmontas : TerMurBook
	{
		[Constructable]
		public KodeksBenmontas()
			: base( "Kodeks Benmontas", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"dur anmurde vastim ui",
					"volde lem monte anvolde",
					"lem. ista ver ew behde.",
					"a ui nes tutim reski",
					"lem an min persa de ui.",
					"zentu ku an uiscar ansa",
					"por. a ni sa zentu ku",
					"an porcar. tu nes per"
				};
			Pages[1].Lines = new string[]
				{
					"te pri si kui re",
					"leinle. er duk anvolde",
					"lem, ew ark lem de via",
					"feltas. a duk lem ku",
					"lentas ew vervid."
				};
		}

		public KodeksBenmontas( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksBenommani : TerMurBook
	{
		[Constructable]
		public KodeksBenommani()
			: base( "Kodeks Benommani", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"vas praetim, vas vidlem",
					"naksatilor kalle kodeks",
					"ante termur. ita rele",
					"vastim benommani.",
					"kodeks terle ante",
					"terort pritas, teresta",
					"re vidle pa lem nesde",
					"vasuis. vidlem terreg,"
				};
			Pages[1].Lines = new string[]
				{
					"monle pa naksatilor,",
					"juksarkle kodeks ku",
					"saeykt grav. sol lem ad",
					"omde vestas trak uis",
					"canle terpor ew leg",
					"kodeks. ante kodeks",
					"skrile pri ben ew ver",
					"res kui kuae. lem nes"
				};
			Pages[2].Lines = new string[]
				{
					"sol terpor kodeks, leg",
					"lem, ew inuislok lemde",
					"monuis aptade. ku",
					"verinde uis ew ankadsa",
					"ski, tu mante est ten",
					"un, or, ew us nesle re",
					"pos apta via. ista est",
					"desintas uide murom, ew"
				};
			Pages[3].Lines = new string[]
				{
					"ita uide zenmur sa per",
					"kodeks uisde ew bende.",
					"ista est kuauis kodeks",
					"est: re mon gargh",
					"zenmur trak ultim",
					"benommani."
				};
		}

		public KodeksBenommani( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksDestermur : TerMurBook
	{
		[Constructable]
		public KodeksDestermur()
			: base( "Kodeks Destermur", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"alt desde terreg ai ali",
					"terreg. ante esta",
					"terreg manite mur aniw",
					"zen. plu tri de ista",
					"zen vid zaw ve uide",
					"anvolde lem. ista",
					"daemon ade pal ew",
					"delsa. kualem lokte"
				};
			Pages[1].Lines = new string[]
				{
					"ista daemon de",
					"destermur sa lok. ew",
					"kerde, lem inte son",
					"esta misve uide liy. a",
					"kua lemmur uiste, an",
					"zen anku vol verde uis.",
					"feluis de lokde daemon",
					"nes ankredle."
				};
		}

		public KodeksDestermur( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksKir : TerMurBook
	{
		[Constructable]
		public KodeksKir()
			: base( "Kodeks Kir", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"tu rete ku qi aksi: un,",
					"or, ew us. de un sal",
					"traktas. de or sal",
					"sent. de us sal",
					"tepertas. a ista qi",
					"bentas an plu mag de",
					"sekde pen: un kuporte",
					"ku or re don mistas. or"
				};
			Pages[1].Lines = new string[]
				{
					"kuporte ku us re don",
					"leintas. ew us int ku",
					"un re don benintas.",
					"anai de un, or, ew us",
					"est anord. ita anai de",
					"aksi vidukte trak semde",
					"bentas, ord. qi aksi",
					"priinte re in pritas."
				};
			Pages[2].Lines = new string[]
				{
					"ista est okde bentas, a",
					"lem mistim pri, kuauis",
					"kuante pritas sa venle",
					"tu aksi, ew ita tu",
					"bentas. kir ten an fin.",
					"lem teinte tutim, ku tu",
					"car parde mag te benfin",
					"de tutas. uide murom"
				};
			Pages[3].Lines = new string[]
				{
					"mis. lem mis teinte",
					"tutim, ku tu carzen, ew",
					"tu bentas, par car de",
					"priinle tutas."
				};
		}

		public KodeksKir( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksRit : TerMurBook
	{
		[Constructable]
		public KodeksRit()
			: base( "Kodeks Rit", "Naxatilor", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"vervid ben kua i,",
					"naksatilor, skri kuo i",
					"porle kodeks uide",
					"terreg ew estade kalle",
					"ante tim benommani: ku",
					"auks lorrelinlem i",
					"beninle vorteks lorrel,",
					"o kua i le vid kodeks"
				};
			Pages[1].Lines = new string[]
				{
					"kuater lem terinit",
					"anporle. i inle vorteks",
					"kuad re inbet grav ok",
					"orblap ew trakpor",
					"vorteks destrak termur.",
					"estatim i perle lorrel",
					"re inuislor kodeks ad",
					"kuad. vorteks tanle"
				};
			Pages[2].Lines = new string[]
				{
					"uide terailem, uislor",
					"inle ailemde, ew kodeks",
					"porle des re perle",
					"bende pa uide zenmur."
				};
		}

		public KodeksRit( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class KodeksXen : TerMurBook
	{
		[Constructable]
		public KodeksXen()
			: base( "Kodeks Xen", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"kuatim betlem grespor",
					"de ov, lem inzenle anku",
					"vol. a vel de inzen lem",
					"sa ski si betlem re",
					"invas est volde au",
					"anvolde lem. anvolde",
					"lem ansa lok, ew anten",
					"skitas de volde lem."
				};
			Pages[1].Lines = new string[]
				{
					"lem nes dukle. volde",
					"lem anmur, a lem",
					"kredonle ku skitas ew",
					"uis de zenmur. lem nes",
					"dukte. sek volde ew",
					"anvolde lem sal de mis",
					"ov, ew sek dete de mis",
					"xen. tu per ve pride"
				};
			Pages[2].Lines = new string[]
				{
					"tutas, re plu ben inten",
					"agratas trak temanitas",
					"ante uide termur."
				};
		}

		public KodeksXen( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class LogbookOfTheEmpire : TerMurBook
	{
		[Constructable]
		public LogbookOfTheEmpire()
			: base( "Logbook of the Empire", "Captain Hawkins", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"This logbook appears to",
					"be the log of a ship",
					"called ‘The Empire.’",
					"The last entry speaks",
					"of the burying of a",
					"great treasure, and of",
					"the growing",
					"discontentment of the"
				};
			Pages[1].Lines = new string[]
				{
					"crew. There’s a hastily",
					"scrawled note at the",
					"end, in different",
					"handwriting, that says",
					"“Captain Hawkins won’t",
					"be makin’ no more log",
					"entries.”"
				};
		}

		public LogbookOfTheEmpire( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class OfDreamsAndVisions : TerMurBook
	{
		[Constructable]
		public OfDreamsAndVisions()
			: base( "Of Dreams and Visions", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"Some say that in our",
					"dreams our astral",
					"selves journey to other",
					"realms of existence.",
					"Others say that imps",
					"and daemons create",
					"dreams to disturb our",
					"sleep. Now let the"
				};
			Pages[1].Lines = new string[]
				{
					"truth be known! Dreams",
					"are messages from the",
					"spirit world. Someday",
					"we will learn to",
					"decipher them, and",
					"benefit greatly thereby."
				};
		}

		public OfDreamsAndVisions( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class PlansForTheConstructionOfAHotAirBalloon : TerMurBook
	{
		[Constructable]
		public PlansForTheConstructionOfAHotAirBalloon()
			: base( "Plans for the Construction of a Hot Air Balloon", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"First you must have a",
					"wicker balloon basket",
					"made, large enough to",
					"carry several",
					"passengers. Then you’ll",
					"need a big iron",
					"cauldron, to hold a",
					"fire to generate the"
				};
			Pages[1].Lines = new string[]
				{
					"hot air. Next you must",
					"have a huge bag sewn",
					"out of silk, to hold",
					"the hot air in. Lastly,",
					"get enough rope to tie",
					"the balloon securely to",
					"the basket. Once you’ve",
					"gathered all of these"
				};
			Pages[2].Lines = new string[]
				{
					"together, use these",
					"plans to assemble them.",
					"When flying your",
					"balloon, you’ll find",
					"that a ship’s anchor",
					"makes the best ballast,",
					"and is also useful for",
					"stopping the balloon"
				};
			Pages[3].Lines = new string[]
				{
					"where and when you wish."
				};
		}

		public PlansForTheConstructionOfAHotAirBalloon( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class PlantLore : TerMurBook
	{
		[Constructable]
		public PlantLore()
			: base( "Plant Lore", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"Mistletoe is easiest to",
					"find in the spring. Cut",
					"the sprigs with your",
					"left hand for greatest",
					"effectiveness. Hibiscus",
					"leaves can be used to",
					"make a tea which is",
					"excellent for sore"
				};
			Pages[1].Lines = new string[]
				{
					"throats. Never step on",
					"a dandelion, for it",
					"will anger any",
					"leprechaun who sees you",
					"do so."
				};
		}

		public PlantLore( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class SnilwitsBigBookOfBoardgameStrategy : TerMurBook
	{
		[Constructable]
		public SnilwitsBigBookOfBoardgameStrategy()
			: base( "Snilwit’s Big Book Of Boardgame Strategy", "Snilwit", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"Chess: Try to control",
					"the middle of the board",
					"with your knights,",
					"bishops, and pawns.",
					"Nine Men's Morris:",
					"Don't let any of your",
					"pieces get trapped in",
					"the corners. Draughts:"
				};
			Pages[1].Lines = new string[]
				{
					"Keep your pieces along",
					"the sides of the board,",
					"where they can't be",
					"captured."
				};
		}

		public SnilwitsBigBookOfBoardgameStrategy( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TangledTales : TerMurBook
	{
		[Constructable]
		public TangledTales()
			: base( "Tangled Tales", "Unknown", 5 )
		{
			Pages[0].Lines = new string[]
				{
					"Once upon a time, in a",
					"strange land far away,",
					"there was a wizard’s",
					"apprentice. His master,",
					"Eldritch, sent him on",
					"three quests, knowing",
					"he would learn his",
					"lessons best with the"
				};
			Pages[1].Lines = new string[]
				{
					"real world as his",
					"classroom. He",
					"encountered many",
					"strange and wonderful",
					"things. Ghouls and",
					"griffins stood in his",
					"way, and ghosts and",
					"giants as well. But he"
				};
			Pages[2].Lines = new string[]
				{
					"also found new friends,",
					"and brave adventurers",
					"to help him. He rode on",
					"a flying carpet, and on",
					"the back of a giant",
					"turtle. Finally, after",
					"exploring an abandoned",
					"mine, a pyramid, and a"
				};
			Pages[3].Lines = new string[]
				{
					"castle in the clouds,",
					"he finished his third",
					"quest. He had learned",
					"so much on his journeys",
					"that his master",
					"rewarded him well, and",
					"declared him a wizard",
					"in his own right. He"
				};
			Pages[4].Lines = new string[]
				{
					"soon opened a school of",
					"his own, and as the",
					"years passed, and",
					"students came and went,",
					"his beard grew long",
					"with years."
				};
		}

		public TangledTales( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfAdministration : TerMurBook
	{
		[Constructable]
		public TheBookOfAdministration()
			: base( "The Book of Administration", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"For countless ages, we",
					"winged ones have led",
					"the wingless ones. This",
					"is right and proper.",
					"But we must always",
					"remember that they are",
					"no less valuables than",
					"we. A body with no head"
				};
			Pages[1].Lines = new string[]
				{
					"cannot move. But",
					"neither can a body with",
					"no legs. All must",
					"function in unity if",
					"anything is to be",
					"achieved. So guide the",
					"wingless ones, and keep",
					"them from paths of"
				};
			Pages[2].Lines = new string[]
				{
					"error. But guide them",
					"with respect."
				};
		}

		public TheBookOfAdministration( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfCircles : TerMurBook
	{
		[Constructable]
		public TheBookOfCircles()
			: base( "The Book of Circles", "Unknown", 6 )
		{
			Pages[0].Lines = new string[]
				{
					"All begins with the",
					"three principles:",
					"Control, Passion and",
					"Diligence. From Control",
					"springs Direction. From",
					"Passion springs",
					"Feeling. From Diligence",
					"springs Persistence."
				};
			Pages[1].Lines = new string[]
				{
					"But these three virtues",
					"are no more important",
					"than the other five:",
					"Control combines with",
					"Passion to give",
					"Balance. Passion",
					"combines with Diligence",
					"to yield Achievement."
				};
			Pages[2].Lines = new string[]
				{
					"And Diligence joins",
					"with Control to provide",
					"Precision. The absence",
					"of Control, Passion and",
					"Diligence is Chaos.",
					"Thus the absence of the",
					"principles points",
					"toward the seventh"
				};
			Pages[3].Lines = new string[]
				{
					"virtue, Order. The",
					"three principles unify",
					"to form Singularity.",
					"This is the eighth",
					"virtue, but it is also",
					"the first, because",
					"within Singularity can",
					"be found all the"
				};
			Pages[4].Lines = new string[]
				{
					"principles, and thus",
					"all the virtues. A",
					"circle has no end. It",
					"continues forever, with",
					"all parts equally",
					"important in the",
					"success of the whole.",
					"Our society is the"
				};
			Pages[5].Lines = new string[]
				{
					"same. It too continues",
					"forever, with all",
					"members (and all",
					"virtues) equal parts of",
					"the unified whole."
				};
		}

		public TheBookOfCircles( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfFamily : TerMurBook
	{
		[Constructable]
		public TheBookOfFamily()
			: base( "The Book of Family", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"When a child hatches",
					"from his egg, he is",
					"born without wings. But",
					"even from birth one can",
					"tell whether a child",
					"will grow up to be a",
					"winged or a wingless",
					"one. The wingless ones"
				};
			Pages[1].Lines = new string[]
				{
					"cannot speak, and lack",
					"the intelligence of the",
					"winged ones. They must",
					"be guided. The winged",
					"ones are few, but they",
					"are entrusted with the",
					"intelligence and wisdom",
					"of the race. They must"
				};
			Pages[2].Lines = new string[]
				{
					"guide. Both winged and",
					"wingless ones spring",
					"from the same eggs, and",
					"both belong to the same",
					"family. All function as",
					"a single whole, to",
					"better maintain the",
					"struggle for survival"
				};
			Pages[3].Lines = new string[]
				{
					"in our world."
				};
		}

		public TheBookOfFamily( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfProsperity : TerMurBook
	{
		[Constructable]
		public TheBookOfProsperity()
			: base( "The Book of Prosperity", "Unknown", 6 )
		{
			Pages[0].Lines = new string[]
				{
					"Long ago, the great",
					"seer Naxatilor summoned",
					"the Codex into the",
					"world. Thus began the",
					"great time of",
					"prosperity. The Codex",
					"was placed within the",
					"Temple of Singularity,"
				};
			Pages[1].Lines = new string[]
				{
					"there to be viewed by",
					"those requiring its",
					"knowledge. The seers of",
					"the land, led by",
					"Naxatilor, protected",
					"the Codex with a",
					"forcefield. Only those",
					"upon sacred quests for"
				};
			Pages[2].Lines = new string[]
				{
					"wisdom are allowed to",
					"reach and read the",
					"Codex. Within the Codex",
					"is written the one",
					"right and true answer",
					"to any problem. One has",
					"but to reach the Codex,",
					"read it, and interpret"
				};
			Pages[3].Lines = new string[]
				{
					"its advise properly.",
					"With perfect wisdom and",
					"infallible knowledge,",
					"all that remains is to",
					"have the control,",
					"passion and diligence",
					"required to follow the",
					"proper course. These"
				};
			Pages[4].Lines = new string[]
				{
					"are the underpinnings",
					"of our society, and so",
					"our race is able to use",
					"the Codex wisely and",
					"well. This is the",
					"reason why the Codex",
					"exists: to lead the",
					"gargoyle race to"
				};
			Pages[5].Lines = new string[]
				{
					"ultimate prosperity."
				};
		}

		public TheBookOfProsperity( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfRitual : TerMurBook
	{
		[Constructable]
		public TheBookOfRitual()
			: base( "The Book of Ritual", "Naxatilor", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"Heed well as I,",
					"Naxatilor, write of how",
					"I brought the Codex to",
					"our land and thereby",
					"ushered in the time of",
					"prosperity: With the",
					"help of the Lensmaker I",
					"crafted the Vortex"
				};
			Pages[1].Lines = new string[]
				{
					"Lens, by which I could",
					"see the Codex where it",
					"originally rested. I",
					"created the Vortex Cube",
					"to focus the power of",
					"the Moonstones and draw",
					"the Vortex down to the",
					"world. Then I used the"
				};
			Pages[2].Lines = new string[]
				{
					"lens to form an image",
					"of the Codex upon the",
					"cube. The Vortex",
					"touched our plane, the",
					"image became solid, and",
					"the Codex was brought",
					"down to be used",
					"properly by our race!"
				};
		}

		public TheBookOfRitual( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfSpirituality : TerMurBook
	{
		[Constructable]
		public TheBookOfSpirituality()
			: base( "The Book Of Spirituality", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"In your travels through",
					"life, remember always",
					"that Spirituality",
					"embodies the sum of all",
					"virtues. Chant the",
					"mantra ‘om’ as you",
					"meditate on",
					"Spirituality, and all"
				};
			Pages[1].Lines = new string[]
				{
					"will become clear to",
					"you."
				};
		}

		public TheBookOfSpirituality( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheBookOfTheUnderworld : TerMurBook
	{
		[Constructable]
		public TheBookOfTheUnderworld()
			: base( "The Book of The Underworld", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"Deep below the land",
					"there is another land.",
					"In that land live many",
					"strange creatures. The",
					"most interesting of",
					"these creatures look",
					"something like our",
					"wingless ones. These"
				};
			Pages[1].Lines = new string[]
				{
					"daemons, however, are",
					"pale and soft. Some say",
					"that these daemons from",
					"the underworld can",
					"speak. And, to be sure,",
					"they make sounds that",
					"are similar to our",
					"language. But as"
				};
			Pages[2].Lines = new string[]
				{
					"everyone knows, no",
					"creature without wings",
					"is truly intelligent.",
					"Fables of talking",
					"daemons must be",
					"discredited."
				};
		}

		public TheBookOfTheUnderworld( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheCavernsOfFreitag : TerMurBook
	{
		[Constructable]
		public TheCavernsOfFreitag()
			: base( "The Caverns of Freitag", "Unknown", 2 )
		{
			Pages[0].Lines = new string[]
				{
					"A great dragon named",
					"Freitag came unto the",
					"Mystic Isles, and there",
					"was much fear and",
					"anguish amongst the",
					"populace. One day the",
					"warrior Gertan set",
					"forth to beard the"
				};
			Pages[1].Lines = new string[]
				{
					"dragon in her lair, a",
					"vast series of caverns",
					"inhabited by strange",
					"creatures. He drove",
					"Freitag away from the",
					"land, and there was",
					"much rejoicing."
				};
		}

		public TheCavernsOfFreitag( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheCodexOfInfiniteWisdom : TerMurBook
	{
		[Constructable]
		public TheCodexOfInfiniteWisdom()
			: base( "The Codex Of Infinite Wisdom", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"‘To return the Codex to",
					"the Vortex, place a",
					"convex lens exactly",
					"midway between the",
					"Codex and the Flame of",
					"Singularity, so that",
					"the light from the",
					"flame converges on the"
				};
			Pages[1].Lines = new string[]
				{
					"Codex. Place a concave",
					"lens between the Codex",
					"and the Flame of",
					"Infinity, so that its",
					"light diverges over the",
					"Codex. Then place the",
					"eight moonstones within",
					"the Vortex Cube. Set it"
				};
			Pages[2].Lines = new string[]
				{
					"on the ground in front",
					"of the Codex, and use",
					"it to return the Codex",
					"from whence it came.’"
				};
		}

		public TheCodexOfInfiniteWisdom( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheFirstAgeOfDarkness : TerMurBook
	{
		[Constructable]
		public TheFirstAgeOfDarkness()
			: base( "The First Age of Darkness", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"Early in the reign of",
					"Lord British there came",
					"unto the land of",
					"Britannia a powerful",
					"wizard named Mondain.",
					"He brang forth many",
					"creatures of great",
					"evil, using the power"
				};
			Pages[1].Lines = new string[]
				{
					"of his magic to control",
					"them. And there was",
					"great suffering",
					"throughout the land. It",
					"was in these days that",
					"the Avatar first came",
					"unto our realm, to",
					"vanquish Mondain and"
				};
			Pages[2].Lines = new string[]
				{
					"liberate our people."
				};
		}

		public TheFirstAgeOfDarkness( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheLostBookOfMantras : TerMurBook
	{
		[Constructable]
		public TheLostBookOfMantras()
			: base( "The Lost Book of Mantras", "Unknown", 1 )
		{
			Pages[0].Lines = new string[]
				{
					"Ahm Kim Rum Bem Mu Dim",
					"Sum Kyo Ra Lox Nid Pey",
					"Beh Un Or Us Cah Biff",
					"Pow Ohm Summ Bang Lis",
					"Zowie Om Cow Frem Ort",
					"Lum Spam Mho Yum Tea",
					"Meow"
				};
		}

		public TheLostBookOfMantras( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheQuestOfTheAvatar : TerMurBook
	{
		[Constructable]
		public TheQuestOfTheAvatar()
			: base( "The Quest of the Avatar", "Unknown", 7 )
		{
			Pages[0].Lines = new string[]
				{
					"In days not long past,",
					"the Council of Wizards",
					"erected shrines to the",
					"eight virtues, that",
					"people throughout the",
					"land might meditate",
					"upon them. It was",
					"through studying the"
				};
			Pages[1].Lines = new string[]
				{
					"eight virtues and the",
					"three underlying",
					"principles that the",
					"great hero, who had",
					"defeated the Triad of",
					"Evil, started down the",
					"path that led to",
					"Avatarhood. For verily"
				};
			Pages[2].Lines = new string[]
				{
					"it is known that the",
					"three principles are",
					"Truth, Love and",
					"Courage; And that from",
					"Truth arises Honesty;",
					"And from Love arises",
					"Compassion; And from",
					"Courage arises Valour;"
				};
			Pages[3].Lines = new string[]
				{
					"And that Truth",
					"comingled with Love",
					"gives rise to Justice;",
					"And Love comingled with",
					"Courage gives rise to",
					"Sacrifice; And Courage",
					"comingled with Truth",
					"gives rise to Honor;"
				};
			Pages[4].Lines = new string[]
				{
					"And Truth, Love and",
					"Courage all united",
					"create Spirituality;",
					"And the absence of all",
					"three principles leads",
					"to the vice of pride,",
					"which leads us to think",
					"of the virtue that is"
				};
			Pages[5].Lines = new string[]
				{
					"its opposite, that of",
					"Humility. And on the",
					"quest to become the",
					"embodiment of the eight",
					"virtues, the Avatar",
					"recovered the Codex of",
					"Ultimate Wisdom, and",
					"discovered that the one"
				};
			Pages[6].Lines = new string[]
				{
					"underlying concept that",
					"leads to the three",
					"principles which lead",
					"to the eight virtues is",
					"Infinity. Ponder this",
					"well, oh seeker!"
				};
		}

		public TheQuestOfTheAvatar( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheSecondAgeOfDarkness : TerMurBook
	{
		[Constructable]
		public TheSecondAgeOfDarkness()
			: base( "The Second Age Of Darkness", "Unknown", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"After Mondain’s",
					"passing, Minax the",
					"enchantress brought",
					"forth terrors to menace",
					"the populace. She had",
					"been apprentice to",
					"Mondain. Perhaps more",
					"than that, if the"
				};
			Pages[1].Lines = new string[]
				{
					"moaning and wailing",
					"that echoed through the",
					"halls outside their",
					"shared bedchamber bore",
					"any meaning. She",
					"established a reign of",
					"death and destruction,",
					"her magic fueled by the"
				};
			Pages[2].Lines = new string[]
				{
					"strength of her",
					"passion. Again it was",
					"only the return of the",
					"Avatar that put an end",
					"to the oppression.",
					"Minax was vanquished,",
					"as Mondain before her."
				};
		}

		public TheSecondAgeOfDarkness( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheThirdAgeOfDarkness : TerMurBook
	{
		[Constructable]
		public TheThirdAgeOfDarkness()
			: base( "The Third Age Of Darkness", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"Once Minax was gone,",
					"peace reigned for",
					"twenty years. And then",
					"the last of Mondain’s",
					"legacy became known.",
					"For he and Minax had",
					"together produced an",
					"unholy creation known"
				};
			Pages[1].Lines = new string[]
				{
					"only as Exodus. For",
					"many years it had",
					"stayed hidden,",
					"patiently gathering",
					"information to heighten",
					"its powers. Finally it",
					"chose to exploit the",
					"results of its"
				};
			Pages[2].Lines = new string[]
				{
					"diligence. Creating a",
					"mighty castle to reside",
					"in, Exodus brought",
					"forth a third plague of",
					"monsters, and nowhere",
					"in Britannia was safe",
					"from its relentless",
					"evil. This was the"
				};
			Pages[3].Lines = new string[]
				{
					"greatest challenge yet",
					"for our hero from",
					"another world; yet once",
					"again the Avatar proved",
					"triumphant. And with",
					"the defeat of Exodus, a",
					"new era of prosperity",
					"began."
				};
		}

		public TheThirdAgeOfDarkness( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class TheWizardOfOz : TerMurBook
	{
		[Constructable]
		public TheWizardOfOz()
			: base( "The Wizard of OZ", "Frank L. Baum", 3 )
		{
			Pages[0].Lines = new string[]
				{
					"A little girl named",
					"Dorothy, from the far",
					"off land of Kansas was",
					"carried to the realm of",
					"Oz by a tornado. And",
					"her little dog, too!",
					"She met three faithful",
					"companions, who vowed"
				};
			Pages[1].Lines = new string[]
				{
					"to help her find a way",
					"home. There was a",
					"scarecrow, who was on a",
					"quest for truth, a man",
					"of tin, who was",
					"questing for love, and",
					"a lion, who quested for",
					"courage. Before their"
				};
			Pages[2].Lines = new string[]
				{
					"quest was done, little",
					"Dorothy slew the wicked",
					"witch, freeing the land",
					"from her evil",
					"influence. Her friends",
					"completed their quests,",
					"and she returned home",
					"to Kansas."
				};
		}

		public TheWizardOfOz( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class WarriorsOfDestiny : TerMurBook
	{
		[Constructable]
		public WarriorsOfDestiny()
			: base( "Warriors Of Destiny", "Unknown", 7 )
		{
			Pages[0].Lines = new string[]
				{
					"After the shining",
					"example set by the",
					"Avatar, Britannia",
					"seemed all set to enter",
					"a golden age of peace",
					"and plenty. And then a",
					"great tragedy took",
					"place. Lord British,"
				};
			Pages[1].Lines = new string[]
				{
					"the beloved ruler who",
					"united the land of",
					"Britannia, and saw it",
					"through some of the",
					"hardest times in its",
					"history, was lost. He",
					"had gone on an",
					"expedition to explore"
				};
			Pages[2].Lines = new string[]
				{
					"the newly discovered",
					"underworld, and never",
					"returned. Many thought",
					"him dead. Lord",
					"Blackthorn took control",
					"in his absence,",
					"declaring martial law",
					"to maintain order. It"
				};
			Pages[3].Lines = new string[]
				{
					"was also around this",
					"time that the three",
					"shadowlords appeared at",
					"Stonegate. They were",
					"powerful, evil",
					"creatures, who could",
					"warp the minds of men",
					"and turn them away from"
				};
			Pages[4].Lines = new string[]
				{
					"the three principles.",
					"‘Tis said they could",
					"draw on ‘the power of",
					"the vortex’ to",
					"transport themselves",
					"instantly anywhere in",
					"the realm. It was a",
					"dark time. The Avatar"
				};
			Pages[5].Lines = new string[]
				{
					"was summoned back to",
					"Britannia, and",
					"ultimately managed to",
					"rescue Lord British",
					"before the collapse of",
					"the Underworld,",
					"restoring him to his",
					"rightful place on the"
				};
			Pages[6].Lines = new string[]
				{
					"throne."
				};
		}

		public WarriorsOfDestiny( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class Windwalker : TerMurBook
	{
		[Constructable]
		public Windwalker()
			: base( "Windwalker", "Unknown", 4 )
		{
			Pages[0].Lines = new string[]
				{
					"In exotic Khantun,",
					"there was a benevolent",
					"Emperor known as Chao",
					"Ti. He ruled wisely and",
					"well until his Warlord,",
					"Zhurongm acquired an",
					"elixir from the evil",
					"alchemist Shen Jang to"
				};
			Pages[1].Lines = new string[]
				{
					"put the Emperor into a",
					"coma. Evil spirits from",
					"the Astral Plane took",
					"over shrines throughout",
					"the land, and it was a",
					"dark time. But a young",
					"student, after",
					"completing his studies"
				};
			Pages[2].Lines = new string[]
				{
					"of martial arts in a",
					"small monastery,",
					"managed to liberate the",
					"shrines, defeat the",
					"warlord, and cure the",
					"Emperor with the Elixir",
					"of Immortality. Long",
					"will his name be"
				};
			Pages[3].Lines = new string[]
				{
					"honored in Khantun."
				};
		}

		public Windwalker( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}

	public class YeLostArtOfBallooning : TerMurBook
	{
		[Constructable]
		public YeLostArtOfBallooning()
			: base( "Ye Lost Art Of Ballooning", "Unknown", 5 )
		{
			Pages[0].Lines = new string[]
				{
					"Back in the days of the",
					"Avatar's Quest, rumors",
					"spread of a strange",
					"sight in the skies. A",
					"tinker from the town of",
					"Minoc, claimed to have",
					"rediscovered the lost",
					"art of ballooning."
				};
			Pages[1].Lines = new string[]
				{
					"According to ancient",
					"lore, this was a",
					"mystical practice that",
					"allowed one to soar",
					"through the air, higher",
					"than the birds",
					"themselves. No records",
					"have been found of the"
				};
			Pages[2].Lines = new string[]
				{
					"rituals used to perform",
					"this feat, but several",
					"farmers from the area",
					"still tell tales of",
					"strange sights. A",
					"strange sphere was",
					"seen, way up in the",
					"sky, and the voices of"
				};
			Pages[3].Lines = new string[]
				{
					"men could be heard",
					"drifting down from it.",
					"The apparition drifted",
					"up into the clouds and",
					"was lost to sight. This",
					"took place years ago,",
					"and naught has been",
					"heard of the mystic art"
				};
			Pages[4].Lines = new string[]
				{
					"of ballooning since",
					"that time."
				};
		}

		public YeLostArtOfBallooning( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}