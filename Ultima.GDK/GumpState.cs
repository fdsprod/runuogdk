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
using Ultima.GDK.Gumps;

namespace Ultima.GDK
{
    public class GumpState
    {
        private BaseGumpCollection stateCollection;

        public BaseGumpCollection StateCollection { get { return stateCollection; } } 
        
        public GumpState(BaseGumpCollection collection)
        {
            stateCollection = new BaseGumpCollection(collection.Count);

            for (int i = 0; i < collection.Count; i++)
            {
                stateCollection.Add(collection[i].Clone());
            }
        }
    }
}