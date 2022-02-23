using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading;

namespace RedMoon.Smartables
{

    [Serializable]
    public abstract class SmartEnum : IEquatable<SmartEnum>
    {
#if UNITY_EDITOR
        public static List<SmartEnum> GetValues(SmartEnum @enum)
        {
            return @enum.GetValues();
        }
        protected abstract List<SmartEnum> GetValues();
#endif
        public bool Equals(SmartEnum other)
        {
            return IsEqual(other);
        }
        protected abstract bool IsEqual(SmartEnum other);
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
            IComparable<SmartEnum<TEnum, TValue>>,
            ISerializationCallbackReceiver
            where TEnum : SmartEnum<TEnum, TValue>, new()
            where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private static readonly Dictionary<TValue, SmartEnum<TEnum, TValue>> _smartEnums = new Dictionary<TValue, SmartEnum<TEnum, TValue>>();
        public static List<SmartEnum<TEnum, TValue>> SmartEnums => _smartEnums.Values.ToList();

        [SerializeField]
        private string _name;
        [SerializeField]
        private TValue _value;
        public string Name => _name;
        public TValue Value => _value;

        protected SmartEnum() { }
        protected SmartEnum(string name, TValue value)
        {
            _name = name;
            _value = value;
            _smartEnums.Add(value, this);
        }

        public virtual int CompareTo(SmartEnum<TEnum, TValue> other) => _value.CompareTo(other._value);
        protected override bool IsEqual(SmartEnum other)
        {
            if (other is SmartEnum<TEnum, TValue> other2) return Equals(other2);
            return false;
        }
        public bool Equals(SmartEnum<TEnum, TValue> other)
        {
            if (System.Object.ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return _value.Equals(other._value);
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
       
        public static bool operator ==(SmartEnum<TEnum, TValue> lhs, SmartEnum<TEnum, TValue> rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(SmartEnum<TEnum, TValue> lhs, SmartEnum<TEnum, TValue> rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// After Deserialization, load in variables from a proxy
        /// </summary>
        /// <param name="other"></param>
        public abstract void Copy(TEnum source);
        public virtual void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            if (_smartEnums.ContainsKey(Value))
            {
                var copy = _smartEnums[Value];
                this._name = copy.Name;
                Copy((TEnum)copy);
            }
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
}