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
using Ultima.GDK.Gumps;

namespace Ultima.GDK
{
    public class BaseGumpCollection : ICollection<BaseGump>
    {
        internal List<BaseGump> items;

        public event EventHandler<GumpCollectionEventArgs> ItemAdded;
        public event EventHandler<GumpCollectionEventArgs> ItemRemoved;

        internal BaseGumpCollection()
        {
            items = new List<BaseGump>();
        }

        internal BaseGumpCollection(IEnumerable<BaseGump> collection)
        {
            items = new List<BaseGump>(collection);
        }

        internal BaseGumpCollection(int capacity)
        {
            items = new List<BaseGump>(capacity);
        }

        public void Add(BaseGump item)
        {
            items.Add(item);

            OnItemAdded(this, new GumpCollectionEventArgs(item));
        }

        private void OnItemAdded(object sender, GumpCollectionEventArgs args)
        {
            if (ItemAdded != null)
            {
                ItemAdded(sender, args);
            }
        }

        private void OnItemRemoved(object sender, GumpCollectionEventArgs args)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(sender, args);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(BaseGump item)
        {
            return items.Contains(item);
        }

        public void CopyTo(BaseGump[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(BaseGump item)
        {
            return items.Remove(item);
        }

        public IEnumerator<BaseGump> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public BaseGump this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }
    }
}
