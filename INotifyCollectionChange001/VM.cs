using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace INotifyCollectionChange007
{
    public class VM : Notifiable
    {

        Collection<Item> items;
        public ICollectionView Items { get; set; }

        public Command Add { get; set; }
        public Command Remove { get; set; }
        public Command Edit { get; set; }
        public Command Replace { get; set; }
        public Command Move { get; set; }
        public Command Clear { get; set; }
        public Command Hide { get; set; }


        int count;

        public VM()
        {
            items = new Collection<Item>();
            Items =new CollectionViewSource()
            {
                Source=items,
                IsLiveFilteringRequested=true,
                LiveFilteringProperties = {nameof(Item.IsInvisible)}

            }.View;
            Items.Filter = (o) => !(o as Item).IsInvisible;
            Add = new Command(add, (o) => true);
            Remove = new Command(remove, (o) => true);
            Edit = new Command(edit, (o) => true);
            Replace = new Command(replace, (o) => true);
            Move = new Command(move, (o) => true);
            Clear = new Command(clear, (o) => true);
            Hide = new Command(clear, (o) => true);
        }

        void add(object o)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    items.Add(new Item()
                    {
                        Id = ++count,
                        Name = "Item No." + count
                    });
                }
            }).Wait();
            items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            
            //List<Item> newItems = null;
            //Task.Run(() =>
            //{
            //    index = items.Count;
            //    for (int i = 0; i < 3; i++)
            //    {
            //        items.Add ( new Item()
            //        {
            //            Id = ++count,
            //            Name = "Item No." + count
            //        });
            //    }
            //    newItems = items.GetRange(index, items.Count - index);
               
            //}).Wait();
            //items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.GetRange(index, items.Count-index)));

                                             //Создание по 3 элемента
        //    for (int i = 0; i < 3; i++)
        //    {
        //        newItems.Add(new Item()
        //        {
        //            Id = ++count,
        //            Name = "Item No." + count
        //        });
        //    }
        //    if (items.Count == 0) items.AddRange(newItems);
        //    else items.InsertRange(items.Count, newItems);
        //}).Wait();
        //items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems));
        }

        void remove(object o)
        {
            var selectedItems = new List<object>(o as IEnumerable<object>);
           
            Task.Run(() =>
            {
                foreach (var item in selectedItems)
                {
                    
                    items.Remove(item as Item);
                    
                }

            }).Wait();

            items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void edit(object o)
        {
            Item oldItem = null;
            Task.Run(() =>
            {
                oldItem = o as Item;
                oldItem.Name = "Edited" + oldItem.Id;

            }).Wait();
            oldItem.OnPropertyChanged(nameof(oldItem.Name));
           
        }
        void replace(object o)
        {
            var selectedItems = new List<object>(o as IEnumerable<object>);
          

            Task.Run(() =>
            {
                foreach (var item in selectedItems)
                {
                    var old = item as Item;
                    //var index = Items.IndexOf(old);
                    var @new= new Item()
                    {
                        Id = old.Id,
                        Name = "Replaced" + old.Id
                    };
                    items[items.IndexOf(old)] = @new; 
                    //App.Current.Dispatcher.Invoke(() => Items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, @new, old,index)));
                }
                
            }).Wait();
            //Items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));

            //2-й способ
            //var selectedItems = o as IEnumerable<object>;
            //var newItems = new List<Item>();

            //Task.Run(() =>
            //{
            //    foreach (var item in selectedItems)
            //    {
            //        var old = item as Item;
            //        var @new = new Item()
            //        {
            //            Id = old.Id,
            //            Name = "Replaced" + old.Id
            //        };
            //        items[items.IndexOf(old)] = @new;
            //        newItems.Add(@new);
            //    }

            //}).Wait();
            //items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems, selectedItems));
        }

        void move(object o)
        {
            Item item = null;
            int oldIndex, newIndex;
            oldIndex= newIndex = 0;


            Task.Run(() =>
            {
                item = o as Item;
                oldIndex = items.IndexOf(item);
                newIndex = 0;
                items.Insert(newIndex, item);
            }).Wait();
            items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move,item, newIndex, oldIndex));
        }

        void clear(object o)
        {
            Task.Run(items.Clear).Wait();
            items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        void hide(object o)
        {
            var item = o as Item;
            //var index = items.IndexOf(item);
            item.IsInvisible = true;
            item.OnPropertyChanged(nameof(item.IsInvisible));
            //items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, index, index));
            //items.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, item, index));

        }

    }

    public class Item:Notifiable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInvisible { get; set; }
    }
}
