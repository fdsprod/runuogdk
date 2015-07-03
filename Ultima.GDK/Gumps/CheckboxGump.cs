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

namespace Ultima.GDK.Gumps
{
	public class CheckboxGump : RadioGump
	{
		public CheckboxGump() 
			: base(211, 210) { }

        public override BaseGump Clone()
        {
            CheckboxGump b = new CheckboxGump();

            return base.Clone(b);
        }

		//public void AddCheck( int x, int y, int inactiveID, int activeID, bool initialState, int switchID )

		public override string ToString()
		{
			return String.Format("\t\tAddCheck( {0}, {1}, {2}, {3}, {4}, {5} );", Location.X, Location.Y, NormalID, CheckedID, CheckState == CheckState, Value);
		}
	}
}
