using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Ultima
{
	public class StringList
	{
		private Dictionary<int, string> table;
		private StringEntry[] entries;
		private string language;

		public StringEntry[] Entries{ get{ return entries; } }
        public Dictionary<int, string> Table { get { return table; } }
		public string Language{ get{ return language; } }

		private static byte[] buffer = new byte[1024];

		public StringList( string language )
		{
			this.language = language;
            table = new Dictionary<int, string>();

			string path = Client.GetFilePath( String.Format( "cliloc.{0}", language ) );

			if ( path == null )
			{
				entries = new StringEntry[0];
				return;
			}

            List<StringEntry> list = new List<StringEntry>();

			using ( BinaryReader bin = new BinaryReader( new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read ), Encoding.Unicode ) )
			{
				bin.ReadInt32();
				bin.ReadInt16();

                while (bin.BaseStream.Position != bin.BaseStream.Length)
				{
					int number = bin.ReadInt32();
					bin.ReadByte();
					int length = bin.ReadInt16();

					if ( length > buffer.Length )
						buffer = new byte[(length + 1023) & ~1023];

					bin.Read( buffer, 0, length );
					string text = Encoding.UTF8.GetString( buffer, 0, length );

					list.Add( new StringEntry( number, text ) );
					table[number] = text;
				}
			}

			entries = (StringEntry[])list.ToArray();
		}
	}
}