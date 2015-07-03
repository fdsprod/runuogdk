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
using System.Reflection;
using Ultima.GDK.Gumps;
using System.Xml;
using System.IO;

namespace Ultima.GDK
{
    public class Project
    {
        private List<Gump> gumps;
        private string filename;
        private string name;

        public string Name { get { return name; } set { name = value; } }
        public List<Gump> Gumps { get { return gumps; } set { gumps = value; } }

        public bool HasModifiedItems
        {
            get
            {
                for (int i = 0; i < Gumps.Count; i++)
                {
                    if (Gumps[i].IsOpen && Gumps[i].Modified)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public List<Gump> OpenGumps
        {
            get
            {
                List<Gump> gumps = new List<Gump>();

                for (int i = 0; i < Gumps.Count; i++)
                {
                    if (Gumps[i].IsOpen)
                    {
                        gumps.Add(Gumps[i]);
                    }
                }

                return gumps;
            }
        }
        
        public List<Gump> ModifiedGumps
        {
            get
            {
                List<Gump> gumps = new List<Gump>();

                for (int i = 0; i < Gumps.Count; i++)
                {
                    if (Gumps[i].IsOpen && Gumps[i].Modified)
                    {
                        gumps.Add(Gumps[i]);
                    }
                }

                return gumps;
            }
        }

        public Project(string name, string filename)
        {
            this.filename = filename;
            this.name = name;

            gumps = new List<Gump>();
        }

        internal Project()
        {

        }

        public static Project Load(string filename)
        {
            return new Project();
        }
    }
}
