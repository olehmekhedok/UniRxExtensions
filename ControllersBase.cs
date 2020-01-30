using System.Linq;

public interface IControllersBase<out TI> where TI : IIdentity
{
    IObservableCollection<TI> Collection { get; }
    TI GetBy(int id);
}

public abstract class ControllersBase<TC, TI> : IControllersBase<TI> where TC : TI where TI : IIdentity
{
    protected readonly ReactiveCollection<TC, TI> Collection = new ReactiveCollection<TC, TI>();
    IObservableCollection<TI> IControllersBase<TI>.Collection => Collection;

    TI IControllersBase<TI>.GetBy(int id)
    {
        return GetBy(id);
    }

    public TC GetBy(int id)
    {
        return Collection.AsEnumerable().FirstOrDefault(c => c.Id == id);
    }
}