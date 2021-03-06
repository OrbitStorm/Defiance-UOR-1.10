using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute( 0x1765, 0x1767 )]
	public class UncutCloth : Item, IScissorable, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} piece of cloth" : "{0} pieces of cloth", Amount );
			}
		}

		int ICommodity.DescriptionNumber { get { return LabelNumber; } }

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public UncutCloth() : this( 1 )
		{
		}

		[Constructable]
		public UncutCloth( int amount ) : base( 0x1767 )
		{
			Stackable = true;
			Amount = amount;
			Dyable = true;
		}

		public UncutCloth( Serial serial ) : base( serial )
		{
		}

		public override bool DisplayDyable{ get{ return false; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void OnSingleClick( Mobile from )
		{
			int number = (Amount == 1) ? 1049124 : 1049123;

			from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", Amount.ToString() ) );
		}
		public bool Scissor( Mobile from, Scissors scissors )
		{
			if ( Deleted || !from.CanSee( this ) ) return false;

			base.ScissorHelper( from, new Bandage(), 1 );

			return true;
		}
	}
}