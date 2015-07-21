using System;
using Server;

namespace Server.Items
{
	public class BookOfCircles : BaseBook
	{
		public override double BookWeight { get { return 0.0; } }

		[Constructable]
		public BookOfCircles()
			: base( 0xFF2, "Book of Circles", "Unknown", 20, false )
		{
			Hue = 0x2F3;

			Pages[0].Lines = new string[]
				{ 
					"All begins with the three",
					"principles:",
					"Control, Passion and",
					"Diligence"
				};
			Pages[1].Lines = new string[]
				{
					"From Control springs",
					"Direction.",
					"",
					"From Passion springs",
					"Feeling.",
					"",
					"From Diligence springs",
					"Persistence."
				};
			Pages[2].Lines = new string[]
				{
					"But these three virtues are",
					"no more important than",
					"the other five: Control",
					"combines with Passion",
					"to give Balance.",
					"Passion combines with",
					"Diligence to yield",
					"Achievement."
				};
			Pages[3].Lines = new string[]
				{
					"And Diligence joins with",
					"Control to provide",
					"Precision.",
					"",
					"The absence of Control,",
					"Passion and Diligence is",
					"Chaos."
				};
			Pages[4].Lines = new string[]
				{
					" Thus the absence of the",
					"principles points toward",
					"the seventh virtue, Order.",
					"",
					"The three principles unify",
					"to form Singularity."
				};
			Pages[5].Lines = new string[]
				{
					"This is the eighth virtue,",
					"but it is also the first,",
					"because within Singularity",
					"can be found all the",
					"principles, and thus all",
					"the virtues."
				};
			Pages[6].Lines = new string[]
				{
					"A circle has no end.",
					"It continues forever,",
					"with all parts equally",
					"important in the success",
					"of the whole."
				};
			Pages[7].Lines = new string[]
				{
					"Our society is the same.",
					"It too continues forever,",
					"with all members (and all",
					"virtues) equal parts of",
					"the unified whole."
				};
		}

		public BookOfCircles( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			/*int version = */
			reader.ReadEncodedInt();
		}
	}
}