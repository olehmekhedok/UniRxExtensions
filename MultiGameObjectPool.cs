using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiGameObjectPool<TView, TType> 
    where TView : MonoBehaviour, IType<TType>, IIdentity
    where TType : struct
{
    private readonly Dictionary<TType, Stack<TView>> _pool = new Dictionary<TType, Stack<TView>>();
    private readonly List<TView> _alive = new List<TView>();
    private readonly List<TView> _templates = new List<TView>();
    private readonly Transform _parent;
    private readonly ICreator _creator;

    public MultiGameObjectPool(IEnumerable<TView> templates, Transform parent, ICreator creator)
    {
        _templates.AddRange(templates);
        _parent = parent;
        _creator = creator;
    }

    public TView GetAlive(int id)
    {
        return _alive.FirstOrDefault(a => a.Id == id);
    }

    public TView Pull(TType type)
    {
        TView newView;
        if (_pool.TryGetValue(type, out var stack) && stack.Count > 0)
        {
            newView = stack.Pop();
        }
        else
        {
            var template = _templates.FirstOrDefault(t => type.Equals(t.Type));

            if (template == null)
            {
                Debug.LogError("Can't find prefab for value:" + type);
                return default;
            }

            newView = _creator.Instantiate(template, _parent);
        }

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

        if (_pool.TryGetValue(value.Type, out var stack) == false)
        {
            stack = new Stack<TView>();
            _pool.Add(value.Type, stack);
        }

        _alive.RemoveAll(a => a.Id == value.Id);
        stack.Push(TerminateItem(value));
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