using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Ultima.GDK.Gumps;

namespace Ultima.GDK
{
    public class Artwork
    {
        private static Dictionary<KeyValuePair<Hue, int>, Bitmap> cache = new Dictionary<KeyValuePair<Hue,int>,Bitmap>();
        private static Hue EmptyHue = new Hue(0);

        public static Bitmap GetStatic(int index)
        {
            KeyValuePair<Hue, int> key = new KeyValuePair<Hue,int>(EmptyHue, index);

            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            else
            {
                Bitmap bmp = Ultima.Art.GetStatic(index);
                cache.Add(key, bmp);
                return cache[key];
            }
        }

        public static Bitmap GetGump(Hue hue, int index)
        {
            KeyValuePair<Hue, int> key = new KeyValuePair<Hue,int>(EmptyHue, index);

            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            else
            {
                Bitmap bmp = Ultima.Gumps.GetGump(index, hue, false);
                cache.Add(key, bmp);
                return cache[key];
            }
        }

        public static void CleanUp(List<BaseGump> currentGumps)
        {

        }
    }
}
