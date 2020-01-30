using System;
using System.Collections.Generic;
using UniRx;

public delegate void AddCallBack<in T>(T value);
public delegate void RemoveCallBack<in T>(T value);

public interface IObservableCollection<out T>: IEnumerable<T>
{
    IDisposable ObserveAdd(AddCallBack<T> callBack);
    IObservable<int> ObserveCountChanged(bool notifyCurrentCount = false);
    IDisposable ObserveRemove(RemoveCallBack<T> callBack);
    IObservable<Unit> ObserveReset();
}

public class ReactiveCollection<T, TI> : ReactiveCollection<T>, IObservableCollection<TI> where T : TI
{
    IEnumerator<TI> IEnumerable<TI>.GetEnumerator()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }

    public IEnumerable<T> AsEnumerable()
    {
        foreach (var item in Items)
        {
            yield return item;
        }
    }

    IDisposable IObservableCollection<TI>.ObserveRemove(RemoveCallBack<TI> callBack)
    {
        return ObserveRemove().Subscribe(s => callBack?.Invoke(s.Value));
    }

    IDisposable IObservableCollection<TI>.ObserveAdd(AddCallBack<TI> callBack)
    {
        return ObserveAdd().Subscribe(s => callBack?.Invoke(s.Value));
    }

    public void RemoveAll()
    {
        for (var i = Items.Count - 1; i >= 0; i--)
        {
            RemoveItem(i);
        }
    }
}
