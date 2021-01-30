using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KennethDevelops.Util{
    public class OrderedList<T> : IEnumerable<OrderedElement<T>>{
        
        private List<OrderedElement<T>> _list = new List<OrderedElement<T>>();
        public readonly int defaultOrder = 100;


        public OrderedList(){ }

        public OrderedList(int defaultOrder){
            this.defaultOrder = defaultOrder;
        }

        public OrderedList<T> Add(int order, T element){
            return Add(new OrderedElement<T>(order, element));
        }
        
        public OrderedList<T> Add(OrderedElement<T> element){
            _list.Add(element);
            return this;
        }

        public OrderedList<T> Remove(OrderedElement<T> element){
            _list.Remove(element);
            return this;
        }

        public OrderedList<T> Remove(T element){
            for (var i = 0; i < _list.Count; i++){
                if (!_list[i].element.Equals(element)) continue;
                
                _list.RemoveAt(i);
                return this;
            }

            return this;
        }

        public OrderedList<T> RemoveAt(int order){
            for (var i = 0; i < _list.Count; i++){
                if (_list[i].order != order) continue;
                
                _list.RemoveAt(i);
                return this;
            }

            return this;
        }
        
        
        public static OrderedList<T> operator -(OrderedList<T> list, OrderedElement<T> element){
            return list == null ? list = new OrderedList<T>() : list.Remove(element);
        }
        
        public static OrderedList<T> operator -(OrderedList<T> list, T element){
            return list == null ? list = new OrderedList<T>() : list.Remove(element);
        }

        public static OrderedList<T> operator +(OrderedList<T> list, OrderedElement<T> element){
            if (list == null) list = new OrderedList<T>();
            return list.Add(element);
        }
        
        public static OrderedList<T> operator +(OrderedList<T> list, T element){
            if (list == null) list = new OrderedList<T>();
            return list.Add(new OrderedElement<T>(list.defaultOrder, element));
        }
        

        IEnumerator<OrderedElement<T>> IEnumerable<OrderedElement<T>>.GetEnumerator(){
            return _list.OrderBy(n => n.order).GetEnumerator();
        }

        public IEnumerator GetEnumerator(){
            return _list.OrderBy(n => n.order).GetEnumerator();
        }
    }
}