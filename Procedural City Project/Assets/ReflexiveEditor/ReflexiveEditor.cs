using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

// Feature Complete Reflexive Editor to access private members in development without compromising encapsulation
public abstract class ReflexiveEditor : Editor
{
    protected T PullFieldValue<T>(string _fieldName, object _target)
    {
        return ((T) _target.GetType().GetField(_fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_target));
    }

    protected void PushFieldValue<T>(string _fieldName, object _target, T _value)
    {
        _target.GetType().GetField(_fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_target, _value);
    }

    protected void CallFunction(string _functionName, object _target)
    {
        _target.GetType().GetMethod(_functionName).Invoke(_target, null);
    }
}
