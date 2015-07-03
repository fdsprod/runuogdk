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

namespace Ultima.GDK
{
    public class ArgumentProcessor
    {
        const string FileFlag = "-f";

        public static ArgumentProcessor Compile(string[] args)
        {
            ArgumentProcessor argProc = new ArgumentProcessor(args);

            return argProc;
        }

        private List<string> args;
        private string fileToOpen;

        public bool HasArguments 
        {
            get 
            {
                return args.Count != 0; 
            } 
        }

        public bool SandwichSupport 
        { 
            get
            { 
                return (args.Contains("-sandwich") || args.Contains("-s")); 
            }
        }

        public bool Debug
        {
            get
            { 
                return (args.Contains("-debug") || args.Contains("-d"));
            } 
        }

        public bool Trace
        {
            get
            {
                return (args.Contains("-trace") || args.Contains("-t"));
            }
        }

        public bool OpenFile
        {
            get
            {
                return string.IsNullOrEmpty(fileToOpen);
            }
        }

        public string FileToOpen
        {
            get
            {
                return fileToOpen;
            }
        }

        private ArgumentProcessor(string[] args)
        {
            this.args = new List<string>(args.Length);

            for (int i = 0; i < args.Length; i++)
            {
                if(args[i].ToLower() == FileFlag && i + 1 <= args.Length)
                {
                    i++;
                    fileToOpen = args[i];
                }
                else
                {
                    this.args.Add(args[i]);
                }
            }
        }
    }
}
