// Вставьте сюда финальное содержимое файла ObservableStack.cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
    public class StackOperationsLogger
    {
        private readonly Observer obs = new Observer();
        public void SubscribeOn<T>(ObservableStack<T> stack)
        {
            stack.Add1 += (sender, data) => obs.HandleEvent(data.ToString());
            stack.Remove1 += (sender, data) => obs.HandleEvent(data.ToString());
        }

        public string GetLog()
        {
            return obs.Log.ToString();
        }
    }

    public interface IObserver
    {
        void HandleEvent(string mess);
    }

    public class Observer : IObserver
    {
        public StringBuilder Log = new StringBuilder();

        public void HandleEvent(string mess)
        {
            Log.Append(mess);
        }
    }


    public class ObservableStack<T>
    {
        public event EventHandler<StackEventData<T>> Add1;
        public event EventHandler<StackEventData<T>> Remove1;

        readonly List<T> obss = new List<T>();

        public void Add(T obs)
        {
            obss.Add(obs);
            Add1?.Invoke(this, new StackEventData<T> { IsPushed = true, Value = obs });
        }

        public void Remove(T obs)
        {
            obss.Remove(obs);
            Remove1?.Invoke(this, new StackEventData<T> { IsPushed = false, Value = obs });
        }

        readonly List<T> data = new List<T>();

        public void Push(T obj)
        {
            if(data != null)
            	data.Add(obj);
            Add(obj);
        }

        public T Pop()
        {
            if (data.Count == 0)
                throw new InvalidOperationException();
            var result = data[data.Count - 1];
            Remove(data[data.Count - 1]);
            return result;
        }
    }
}

