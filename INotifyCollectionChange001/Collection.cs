using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INotifyCollectionChange007
{
    public class Collection<T> :List<T>,INotifyCollectionChanged
    {
        public Collection() : base() { }
        public Collection(IEnumerable<T> enumerable) : base(enumerable) { }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        //public bool IsReadOnly => ((IList)this).IsReadOnly;
        //public bool IsFixedSize => ((IList)this).IsFixedSize;
        //public bool IsSynchronized => ((IList)this).IsSynchronized;
        //public object SyncRoot => ((IList)this).SyncRoot;
        //IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        
    }
}
