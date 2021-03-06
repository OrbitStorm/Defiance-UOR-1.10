using System;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x204F, 0x204F )]
	public class TRLDress : BaseOuterTorso
	{
		[Constructable]
		public TRLDress() : base( 0x204F )
		{
			Weight = 5.0;
			Name = "Staff Robe";
			Layer = Layer.OuterTorso;
			Hue = 3;
		}

		public override void OnDoubleClick( Mobile m )
		{
			if( Parent != m )
			{
				m.SendMessage( "You must be wearing the robe to use it!" );
			}
			else
			{
				if ( ItemID == 0x204F || ItemID == 0x204F )
				{
					m.SendMessage( "YOU ARE NOW PRIVATE." );
					m.PlaySound( 0x57 );
					ItemID = 0x1F03;
					m.NameMod = null;
					m.RemoveItem(this);
					m.EquipItem(this);
					if( m.Kills >= Mobile.MurderCount)
					{
						m.Criminal = true;
					}
					if( m.GuildTitle != null)
					{
						m.DisplayGuildTitle = true;
					}
				}
				else if ( ItemID == 0x204F || ItemID == 0x1F03 )
				{
					m.SendMessage( "YOU ARE NOW PUBLIC STAFF." );
					m.PlaySound( 0x57 );
					ItemID = 0x204F;
					m.NameMod = "Trial Defiance";
					m.DisplayGuildTitle = true;
					m.Criminal = false;
					m.RemoveItem(this);
					m.EquipItem(this);
				}
			}
		}

		public override bool OnEquip( Mobile from )
		{
			if ( ItemID == 0x204F )
			{
				from.NameMod = "Trial Defiance";
				from.DisplayGuildTitle = true;
				from.Criminal = false;
			}
			return base.OnEquip(from);
		}

		public override void OnRemoved( Object o )
		{
			if( o is Mobile )
			{
				((Mobile)o).NameMod = null;
			}
			if( o is Mobile && ((Mobile)o).Kills >= Mobile.MurderCount)
			{
				((Mobile)o).Criminal = true;
			}
			if( o is Mobile && ((Mobile)o).GuildTitle != null )
			{
				((Mobile)o).DisplayGuildTitle = true;
			}
			base.OnRemoved( o );
		}

		public TRLDress( Serial serial ) : base( serial )
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
			int version = reader.ReadInt();
		}

	}
}