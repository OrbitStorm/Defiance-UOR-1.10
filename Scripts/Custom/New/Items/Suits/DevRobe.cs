using System;
using Server;

namespace Server.Items
{
	public class DevRobe : BaseSuit
	{
		public override string DefaultName{ get{ return "Developer Robe"; } }

		[Constructable]
		public DevRobe() : base( AccessLevel.Administrator, 1175, 0x204F )
		{
		}

		public DevRobe( Serial serial ) : base( serial )
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

			int version = reader.ReadEncodedInt();
		}
	}
}