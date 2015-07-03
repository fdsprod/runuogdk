using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Ultima
{
    public class UnicodeFonts
    {
        private static BinaryReader reader;
        private static FileStream stream;
        private static BinaryReader[] readerArray = new BinaryReader[3];
        private static FileStream[] streamArray = new FileStream[3];
        private static CharInfo[] uniCharCache = new CharInfo[0x3e9];

        public static Bitmap GetCharImage(int font, char c)
        {
            Bitmap charImage;

            if (stream == null)
            {
                Init();
            }

            stream = streamArray[font];
            reader = readerArray[font];

            int index = c;
            stream.Seek((long)(index * 4), SeekOrigin.Begin);
            int num2 = reader.ReadInt32();
            stream.Seek((long)num2, SeekOrigin.Begin);
            CharInfo info = new CharInfo();
            uniCharCache[index] = info;
            info.Kerning = reader.ReadByte();
            info.BaseLine = reader.ReadByte();
            info.Width = reader.ReadByte();
            info.Height = reader.ReadByte();
            int num3 = info.Height - info.BaseLine;
            if ((info.Width + info.Height) != 0)
            {
                charImage = new Bitmap((info.Width + (info.Kerning * 2)) + 2, (info.Height + info.BaseLine) + 2, PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(charImage);
                graphics.Clear(Color.Red);
                graphics.Dispose();
                int height = info.Height - 1;
                for (int i = 0; i <= height; i++)
                {
                    int width = info.Width - 1;
                    for (int k = 0; k <= width; k++)
                    {
                        byte num6 = 0;
                        int num5 = k % 8;
                        if (num5 == 0)
                        {
                            num6 = reader.ReadByte();
                        }
                        if ((((byte)(num6 >> ((7 - num5) & 7))) & 1) == 1)
                        {
                            charImage.SetPixel((k + info.Kerning) + 1, (i + info.BaseLine) + 1, Color.LightGray);
                        }
                    }
                }
                int num11 = charImage.Width - 1;
                for (int j = 0; j <= num11; j++)
                {
                    int num10 = charImage.Height - 1;
                    for (int m = 0; m <= num10; m++)
                    {
                        if (charImage.GetPixel(j, m).ToArgb() == -65536)
                        {
                            bool flag = false;
                            if ((j < (charImage.Width - 1)) && (charImage.GetPixel(j + 1, m).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (j > 0)) && (charImage.GetPixel(j - 1, m).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (m < (charImage.Height - 1))) && (charImage.GetPixel(j, m + 1).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                                flag = true;
                            }
                            if ((!flag && (m > 0)) && (charImage.GetPixel(j, m - 1).ToArgb() == -2894893))
                            {
                                charImage.SetPixel(j, m, Color.Black);
                            }
                        }
                    }
                }
            }
            else
            {
                charImage = new Bitmap(3, 1, PixelFormat.Format32bppArgb);
                charImage.MakeTransparent();
            }
            charImage.MakeTransparent(Color.Red);
            info.Cache = charImage;
            return charImage;
        }

        public static Bitmap GetStringImage(int Font, string Text)
        {
            int height = 0;
            int num2 = 0;
            Bitmap[] bitmapArray = new Bitmap[(Text.Length - 1) + 1];
            int num9 = Text.Length;
            for (int i = 0; i < num9; i++)
            {
                bitmapArray[i] = GetCharImage(Font, Text[i]);
                num2 += bitmapArray[i].Width;
                if (bitmapArray[i].Height > height)
                {
                    height = bitmapArray[i].Height;
                }
            }
            Bitmap image = new Bitmap(num2, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(image);
            int num8 = Text.Length;
            int num3 = 0;
            for (int j = 0; j < num8; j++)
            {
                graphics.DrawImage(bitmapArray[j], num3, 0);
                num3 += bitmapArray[j].Width;
            }
            int num7 = Text.Length;
            for (int k = 0; k < num7; k++)
            {
                bitmapArray[k].Dispose();
            }
            graphics.Dispose();
            return image;
        }

        public static void Init()
        {
            streamArray[0] = new FileStream(Client.GetFilePath("unifont.mul"), FileMode.Open, FileAccess.Read);
            streamArray[1] = new FileStream(Client.GetFilePath("unifont1.mul"), FileMode.Open, FileAccess.Read);
            streamArray[2] = new FileStream(Client.GetFilePath("unifont2.mul"), FileMode.Open, FileAccess.Read);
            readerArray[0] = new BinaryReader(streamArray[0]);
            readerArray[1] = new BinaryReader(streamArray[1]);
            readerArray[2] = new BinaryReader(streamArray[2]);
        }

        private class CharInfo
        {
            public int BaseLine;
            public Bitmap Cache;
            public int Height;
            public int Kerning;
            public long Offset;
            public int Width;

            public override string ToString()
            {
                string str = "";
                return str;
            }
        }

        private class FontSet
        {
            public UnicodeFonts.CharInfo[] cinfo = new UnicodeFonts.CharInfo[0x10001];
        }
    }

 

}
