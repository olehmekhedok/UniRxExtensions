using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectPool<TView> where TView : MonoBehaviour, IIdentity
{
    private readonly Stack<TView> _pool = new Stack<TView>();
    private readonly List<TView> _alive = new List<TView>();
    private readonly TView _template;
    private readonly Transform _parent;
    private readonly ICreator _creator;

    public GameObjectPool(TView template, Transform parent, ICreator creator)
    {
        _template = template;
        _parent = parent;
        _creator = creator;
    }

    public TView GetAlive(int id)
    {
        return _alive.FirstOrDefault(a => a.Id == id);
    }

    public TView Pull()
    {
        var newView = _pool.Count > 0 ? _pool.Pop() : _creator.Instantiate(_template, _parent);
        _alive.Add(newView);
        return InitItem(newView);
    }

    public void Push(IIdentity value)
    {
        if (value == null)
        {
            Debug.LogError("value can't be null");
            return;
        }

        var alive = _alive.FirstOrDefault(a => a.Id == value.Id);
        Push(alive);
    }

    public void Push(TView value)
    {
        if (value == null)
        {
            Debug.LogError("value can't be null");
            return;
        }

        _alive.RemoveAll(a => a.Id == value.Id);
        _pool.Push(TerminateItem(value));
    }

    private static TView InitItem(TView value)
    {
        value.gameObject.SetActive(true);
        return value;
    }

    private TView TerminateItem(TView value)
    {
        value.gameObject.SetActive(false);
        value.transform.SetParent(_parent);

        (value as IUnSubscribe)?.UnSubscribe();

        return value;
    }
}