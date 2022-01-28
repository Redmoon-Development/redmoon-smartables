using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading;

[Serializable]
public abstract class SmartEnum : IEquatable<SmartEnum>
{
#if UNITY_EDITOR
    public static List<SmartEnum> GetValues(SmartEnum @enum)
    {
        return @enum.GetValues();
    }

    public bool Equals(SmartEnum other)
    {
        return GetEqual(other);
    }

    protected abstract List<SmartEnum> GetValues();
    protected abstract bool GetEqual(SmartEnum other);
#endif
}

[Serializable]
public abstract class SmartEnum<TEnum> : SmartEnum<TEnum, int> where TEnum : SmartEnum<TEnum, int>, new()
{
    protected SmartEnum() { }
    protected SmartEnum(string name, int value) : base(name, value) { }
}

[Serializable]
public abstract class SmartEnum<TEnum, TValue> :
        SmartEnum,
        IEquatable<SmartEnum<TEnum, TValue>>,
        IComparable<SmartEnum<TEnum, TValue>>
        where TEnum : SmartEnum<TEnum, TValue>, new()
        where TValue : IEquatable<TValue>, IComparable<TValue>
{
    private static List<SmartEnum<TEnum, TValue>> _smartEnums = new List<SmartEnum<TEnum, TValue>>();
    public static List<SmartEnum<TEnum, TValue>> SmartEnums => _smartEnums;

    [SerializeField]
    protected string _name;
    [SerializeField]
    protected TValue _value;
    public string Name => _name;
    public TValue Value => _value;

    protected SmartEnum() { }
    protected SmartEnum(string name, TValue value)
    {
        _name = name;
        _value = value;
        _smartEnums.Add(this);
    }
    public TEnum Copy()
    {
        return new TEnum
        {
            _value = _value,
            _name = _name
        };
    }

    public virtual int CompareTo(SmartEnum<TEnum, TValue> other) => _value.CompareTo(other._value);
    
    public bool Equals(SmartEnum<TEnum, TValue> other)
    {
        if (System.Object.ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return _value.Equals(other._value);
    }
    public override string ToString()
    {
        return Name;
    }
#if UNITY_EDITOR
    protected override List<SmartEnum> GetValues()
    {
        Type baseType = typeof(TEnum);
        var c1 = Assembly.GetAssembly(baseType)
                .GetTypes()
                .Where(t => baseType.IsAssignableFrom(t))
                .ToList();
        var c2 = c1.SelectMany(t => t.GetFieldsOfType<TEnum>()).ToList();
        return c2
            .OrderBy(t => t.Value)
            .Cast<SmartEnum>()
            .ToList();
    }
    protected override bool GetEqual(SmartEnum other)
    {
        if (other is SmartEnum<TEnum, TValue> other2) return Equals(other2);
        return false;
    }
#endif
}

#if UNITY_EDITOR
internal static class TypeExtensions
{
    public static List<TFieldType> GetFieldsOfType<TFieldType>(this Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(p => type.IsAssignableFrom(p.FieldType))
            .Select(pi => (TFieldType)pi.GetValue(null))
            .ToList();
    }
}
#endif