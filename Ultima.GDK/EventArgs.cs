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
using Ultima.GDK.Gumps;
using System.Collections.Generic;

namespace Ultima.GDK
{
    public class GumpCollectionEventArgs : EventArgs
    {
        private BaseGump item;

        public BaseGump Item { get { return item; } }

        public GumpCollectionEventArgs(BaseGump item)
        {
            this.item = item;
        }
    }

    public class OpenDocumentEventArgs : EventArgs
    {
        private string gumpName;
        private Project project;

        public string GumpName { get { return gumpName; } }
        public Project Project { get { return project; } }

        public OpenDocumentEventArgs(string gumpName, Project project, DocumentType docType)
        {
            this.gumpName = gumpName;
            this.project = project;
        }
    }

    public class BaseGumpsSelectedEventArgs : EventArgs
    {
        private Gump gump;

        public List<BaseGump> SelectedGumps { get { return gump.GetSelectedGumps(); } }

        public BaseGumpsSelectedEventArgs(Gump gump)
            : base()
        {
            this.gump = gump;
        }
    }

    public class HotKeyEventArgs : EventArgs
    {
        private Gump gump;
        private DesignerFrame designer;
        private KeyData keyData;

        public Gump Gump { get { return gump; } }
        public DesignerFrame Designer { get { return designer; } }
        public KeyData KeyData { get { return keyData; } }

        public HotKeyEventArgs(Gump gump, DesignerFrame designer, KeyData keyData)
        {
            this.gump = gump;
            this.designer = designer;
            this.keyData = keyData;
        }
    }
}
