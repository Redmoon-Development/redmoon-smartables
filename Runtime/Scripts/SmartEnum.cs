using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RedMoon.Smartables
{

    [Serializable]
    public abstract class SmartEnum : ICloneable, IEquatable<SmartEnum>
    {
        public abstract List<SmartEnum> GetValues();
        public abstract object Clone();
        public abstract bool Equals(SmartEnum other);
    }

    [Serializable]
    public abstract class SmartEnum<TEnum> : SmartEnum<TEnum, int> where TEnum : SmartEnum<TEnum>
    {
        private string _name;
        public string Name => _name;

        public SmartEnum(string name, int value) : base(value)
        {
            _name = name;
        }

        protected override void Construct(TEnum @enum)
        {
            _name = @enum._name;
            ConstructEnum(@enum);
        }
        protected abstract void ConstructEnum(TEnum @enum);
        public override string ToString()
        {
            return _name;
        }
    }

    [Serializable]
    public abstract class SmartEnum<TEnum, TValue> :
            SmartEnum,
            IEquatable<TEnum>,
            IComparable<TEnum>,
            ISerializationCallbackReceiver
            where TEnum : SmartEnum<TEnum, TValue>
            where TValue : IEquatable<TValue>, IComparable<TValue>
    {
        private static readonly Dictionary<TValue, TEnum> _smartEnums = new Dictionary<TValue, TEnum>();

        [SerializeField]
        private TValue _value;
        public TValue Value => _value;

        protected SmartEnum(TValue value)
        {
            _value = value;
            Construct(_value);
        }
       

        public virtual int CompareTo(TEnum other) => _value.CompareTo(other._value);
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(TEnum other)
        {
            if (System.Object.ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            return _value.Equals(other._value);
        }
        public override bool Equals(SmartEnum other)
        {
            if (other is TEnum @enum) return this.Equals(@enum);
            return false;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public static bool operator ==(SmartEnum<TEnum, TValue> lhs, SmartEnum<TEnum, TValue> rhs)
        {
            return lhs?.Equals(rhs) ?? rhs is null;
        }
        public static bool operator !=(SmartEnum<TEnum, TValue> lhs, SmartEnum<TEnum, TValue> rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static List<TEnum> Values()
        {
#if UNITY_EDITOR
            Type baseType = typeof(TEnum);
            var c1 = Assembly.GetAssembly(baseType)
                    .GetTypes()
                    .Where(t => baseType.IsAssignableFrom(t))
                    .ToList();
            var c2 = c1.SelectMany(t => t.GetFieldsOfType<TEnum>()).ToList();
            return c2
                .OrderBy(t => t.Value)
                .ToList();
#else
                    return _smartEnums.Values.ToList();
#endif
        }
        public override List<SmartEnum> GetValues()
        {
            return Values().Cast<SmartEnum>().ToList();
        }

        public override object Clone()
        {
            if (!this.GetType().IsSerializable)
            {
                throw new ArgumentException("The Type must be Serializable.", "source");
            }

            if (this is null)
            {
                return null;
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                var tenum = (TEnum)formatter.Deserialize(stream);
                tenum.Construct(tenum.Value);
                return tenum;
            }
        }

        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            Construct(_value);
        }
        private void Construct(TValue value)
        {
            if (_smartEnums.TryGetValue(value, out TEnum @enum))
            {
                Construct(@enum);
            }
            else
            {
                _smartEnums.Add(value, (TEnum)this);
                Construct(this);
            }
        }
        private void Construct(SmartEnum<TEnum, TValue> @enum)
        {
            _value = @enum._value;
            Construct((TEnum)@enum);
        }
        protected abstract void Construct(TEnum @enum);
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