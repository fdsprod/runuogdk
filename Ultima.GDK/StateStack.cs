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
    public class StateStack : Stack<GumpState>
    {

    }

    public class RedoStack : StateStack { }
    public class UndoStack : StateStack { }
}
