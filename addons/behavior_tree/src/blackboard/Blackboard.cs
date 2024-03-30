using Godot;
using Godot.Collections;
using System.Collections.Generic;

public partial class Blackboard : RefCounted
{
    private Dictionary _blackboard = new();
    private BehaviorTreePlayer _behaviorTreePlayer;

    #region signal

    [Signal] public delegate void BlackboardValueChangedEventHandler(Variant key, Variant value, bool removed);

    #endregion

    public Blackboard(BehaviorTreePlayer behaviorTreePlayer)
    {
        _behaviorTreePlayer = behaviorTreePlayer;
    }

    public void SetValue(Variant key, Variant value)
    {
        if (_blackboard.TryGetValue(key, out var variable))
        {
            _blackboard[key] = value;
        }
        else
        {
            _blackboard.Add(key, value);
        }

        EmitSignal(SignalName.BlackboardValueChanged, key, value, false);
    }

    public Variant GetValue(Variant key, Variant defaultValue=default)
    {
        return _blackboard.GetValueOrDefault(key, defaultValue);
    }

    public bool HasValue(Variant key)
    {
        return _blackboard.TryGetValue(key, out var value);
    }

    public bool HasKey(Variant key)
    {
        return _blackboard.ContainsKey(key);
    }

    public void RemoveValue(Variant key)
    {
        if (_blackboard.ContainsKey(key))
        {
            _blackboard[key] = default;
        }
    }

    public void RemoveKey(Variant key)
    {
        if (_blackboard.TryGetValue(key, out var value))
        {
            EmitSignal(SignalName.BlackboardValueChanged, key, value, true);
            _blackboard.Remove(key);
        }
    }
}
