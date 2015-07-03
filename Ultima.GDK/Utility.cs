/***************************************************************************
 *
 * $Author: Jeff Boulanger
 * 
 * This work is protected by the Creative Commons Attribution-Noncommercial-No 
 * Derivative Works 3.0 Unported License.  All information regarding this 
 * license can be found at http://creativecommons.org/licenses/by-nc-nd/3.0/
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Ultima;
using Ultima.GDK;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;

namespace Ultima.GDK
{
    public class Utility
    {
        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int key);

		private static StringList stringList;
        private static Font font;
        private static Random rand = new Random();

        public static Random Rand { get { return rand; } } 

        public static Font Font
        {
            get
            {
                if (font == null)
                {
                    font = new Font("Times New Roman", 12f);
                }

                return font;
            }
        }

        public static void LogException(Exception e)
        {
            try
            {
                string timeStamp = GetTimeStamp();
                string fileName = String.Format("crashlog_{0}.log", timeStamp);

                string root = System.IO.Directory.GetCurrentDirectory();
                string filePath = Path.Combine(root, fileName);

                StringBuilder sb = new StringBuilder();
                Version ver = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                sb.AppendLine("RunUO:GDK Crash Report");
                sb.AppendLine("===================");
                sb.AppendLine();
                sb.AppendFormat("Version:\t\t{0}.{1}, Build {2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
                sb.AppendLine();
                sb.AppendFormat("Operating System: \t{0}", Environment.OSVersion);
                sb.AppendLine();
                sb.AppendFormat(".NET Framework: \t{0}", Environment.Version);
                sb.AppendLine();
                sb.AppendFormat("Processor Count: \t{0}", Environment.ProcessorCount);
                sb.AppendLine();
                sb.AppendFormat("Memory Usage: \t\t{0:0,0}KB", (Environment.WorkingSet / 1024));
                sb.AppendLine();
                sb.AppendFormat("Time: \t\t\t{0}", DateTime.Now);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Stack Trace:");
                sb.AppendLine(e.ToString() + "");
                sb.AppendLine();
                sb.AppendLine();

                using (StreamWriter op = new StreamWriter(filePath, true))
                {
                    op.Write(sb.ToString());
                    op.Close();
                }
            }
            catch { }
        }

        private static string GetTimeStamp()
        {
            DateTime now = DateTime.Now;

            return String.Format("{0:00}{1:00}{2:0000}",
                    now.Day,
                    now.Month,
                    now.Year
                );
        }

        public static Color Convert555ToARGB(short hue)
        {
            int red = (((short)(hue >> 10)) & 0x1f) * 8;
            int green = (((short)(hue >> 5)) & 0x1f) * 8;
            int blue = (hue & 0x1f) * 8;
            return Color.FromArgb(red, green, blue);
        }

        public static Rectangle BuildRectangle( Point one, Point two )
        {
            return new Rectangle( Math.Min( one.X, two.X ), 
                Math.Min( one.Y, two.Y ), 
                Math.Abs( one.X - two.X ), 
                Math.Abs( one.Y - two.Y ) );
        }

		public static string GetCliloc(int index)
		{
			if( stringList == null )
				stringList = new StringList("ENU");

			string cliloc = string.Empty;

			if( stringList.Table.ContainsKey(index) )
				cliloc = (string)stringList.Table[index];

			return cliloc;
		}

        private static Color emptyColor = Color.FromArgb(0, 0, 0, 0);
        public static Color EmptyColor { get { return emptyColor; } }

        private static bool mouseHitSuccess;
        public static bool MouseHitSuccess { get { return mouseHitSuccess; } set { mouseHitSuccess = value; } }

        public static byte[] ConvertCharToByteArray(char[] chars)
        {
            byte[] data = new byte[chars.Length];

            for (int i = 0; i < chars.Length; i++)
            {
                data[i] = (byte)chars[i];
            }

            return data;
        }

 		public static bool IsKeyDown( Keys k )
 		{
 			return ( GetAsyncKeyState( (int)k ) & 0xFF00 ) != 0;
 		}

        public static int GetInt32(string intString, int defaultValue)
        {
            try
            {
                return XmlConvert.ToInt32(intString);
            }
            catch (Exception e)
            {
                try
                {
                    return System.Convert.ToInt32(intString);
                }
                catch (Exception err)
                {
                    return defaultValue;
                }
            }
        }

        public static string GetText(XmlElement node, string defaultValue)
        {
            if (node == null)
                return defaultValue;

            return node.InnerText == string.Empty ? defaultValue : node.InnerText;
        }

        public static bool GetBool(string boolString, bool defaultValue)
        {
            try
            {
                return XmlConvert.ToBoolean(boolString);
            }
            catch (Exception e)
            {
                try
                {
                    return System.Convert.ToBoolean(boolString);
                }
                catch (Exception err)
                {
                    return defaultValue;
                }
            }
        }

        public static int GetAttributeInt32(XmlElement node, string attributeName, int defaultValue)
        {
            XmlAttribute attr = node.Attributes[attributeName];
            string intString = attr.Value;

            try
            {
                return XmlConvert.ToInt32(intString);
            }
            catch (Exception e)
            {
                try
                {
                    return System.Convert.ToInt32(intString);
                }
                catch (Exception err)
                {
                    return defaultValue;
                }
            }
        }

        public static bool GetAttributeBoolean(XmlElement node, string attributeName, bool defaultValue)
        {
            XmlAttribute attr = node.Attributes[attributeName];
            string intString = attr.Value;

            try
            {
                return XmlConvert.ToBoolean(intString);
            }
            catch (Exception e)
            {
                try
                {
                    return System.Convert.ToBoolean(intString);
                }
                catch (Exception err)
                {
                    return defaultValue;
                }
            }
        }

        public static string GetAttributeString(XmlElement node, string attributeName)
        {
            XmlAttribute attr = node.Attributes[attributeName];

            if (attr == null)
            {
                return string.Empty;
            }

            return attr.Value;
        }

        public static Keys GetAttributeKey(XmlElement node, string attributeName)
        {
            XmlAttribute attr = node.Attributes[attributeName];
            string keyString = attr.Value;

            if (Enum.IsDefined(typeof(Keys), keyString))
            {
                Keys key = (Keys)Enum.Parse(typeof(Keys), keyString, true);
                return key;
            }
            else
            {
                try
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), keyString, true);
                    return key;
                }
                catch { }
            }

            return Keys.None;
        }
    }
}
