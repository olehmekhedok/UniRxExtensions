using System.Linq;
using UniRx;

public interface IModelsBase<out TI> where TI : IIdentity
{
    IObservableCollection<TI> Collection { get; }
    TI GetBy(int id);
}

public abstract class ModelsBase<TM, TI> : IModelsBase<TI> where TM : TI where TI : IIdentity
{
    public readonly ReactiveCollection<TM, TI> Collection = new ReactiveCollection<TM, TI>();
    IObservableCollection<TI> IModelsBase<TI>.Collection => Collection;

    ReactiveCollection<TM> fdf = new ReactiveCollection<TM>();

    TI IModelsBase<TI>.GetBy(int id)
    {
        return GetBy(id);
    }

    public TM GetBy(int id)
    {
        return Collection.AsEnumerable().FirstOrDefault(c => c.Id == id);
    }
}