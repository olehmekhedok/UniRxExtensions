using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(IIdentity))]
public class Highlighter : MonoBehaviour
{
    [SerializeField] private string _propertyName = default;
    [SerializeField] private Renderer _renderer = default;

    [Inject] private IMessenger _messenger = default;

    private FloatPropertyBlockSetter _blockSetter = default;
    private IIdentity _identity = default;
    private IDisposable _subscriber = default;
    private bool _highlighting = false;

    private IIdentity Identity => _identity ?? (_identity = GetComponent<IIdentity>());

    private FloatPropertyBlockSetter BlockSetter =>
        _blockSetter ?? (_blockSetter = new FloatPropertyBlockSetter(_renderer, _propertyName));

    private void Start()
    {
        UnSubscriber();
        _subscriber = _messenger.Subscribe<HighlightObject>(OnHighlightObject);
    }

    private void OnEnable()
    {
        UnSubscriber();
        _subscriber = _messenger.Subscribe<HighlightObject>(OnHighlightObject);
    }

    private void OnDisable()
    {
        UnSubscriber();
    }

    private void UnSubscriber()
    {
        _subscriber?.Dispose();
        _subscriber = null;
    }

    private void OnHighlightObject(HighlightObject message)
    {
        if (message.Id == Identity.Id)
        {
            if (_highlighting == false)
            {
                _highlighting = true;
                BlockSetter.SetValue(1);
            }
        }
        else
        {
            if (_highlighting)
            {
                _highlighting = false;
                BlockSetter.SetValue(0);
            }
        }
    }
}
