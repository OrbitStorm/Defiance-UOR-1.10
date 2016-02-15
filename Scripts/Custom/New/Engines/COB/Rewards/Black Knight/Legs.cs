//==============================================//
// Created by Dupre					//
//==============================================//
using System;
using Server;

namespace Server.Items
{
	public class BlackLegs : PlateLegs
	{
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public BlackLegs()
		{
			Name = "Black Knights Armour";
			Hue = 1;
			LootType = LootType.Blessed;
		}
		public override bool OnEquip(Mobile from) 
	      { 
			UnMorph(from);
		         return base.OnEquip(from); 
		}
	      public override void OnRemoved( object parent) 
	      { 
	        if (parent is Mobile) 
	        { 
	         Mobile from = (Mobile)parent; 
		   UnMorph(from);
		  }

	         base.OnRemoved(parent); 
      	}

		public bool UnMorph(Mobile from)
		{

		if (from.FindItemOnLayer(Layer.OuterTorso) == null || from.FindItemOnLayer(Layer.OuterTorso).Name != "a Shroud of Phasing")
			{
			this.Hue = 1;
			}
			else
			{
			}
		return true;
		}
		public BlackLegs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}