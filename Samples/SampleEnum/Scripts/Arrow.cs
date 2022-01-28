using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class Arrow : MonoBehaviour
{
    [Serializable]
    public class DamageType : SmartEnum<DamageType>
    {
        public static readonly DamageType Regular = new DamageType(nameof(Regular), 0);
        public static readonly DamageType Fire = new DamageType(nameof(Fire), 1);
        public static readonly DamageType Ice = new DamageType(nameof(Ice), 2);
        public static readonly DamageType Water = new DamageType(nameof(Water), 3);

        public DamageType() : base() { }
        private DamageType(string name, int value) : base(name, value) { }
    }
    public void Start()
    {
        Debug.Log(JsonUtility.ToJson(this));
        Debug.Log(damage.ToString());
        Debug.Log(DamageType.SmartEnums.Select(x=>x.ToString()).Aggregate((x,y) => x + " " + y));
    }

    private void Update()
    {

        Debug.Log(damage.ToString());
    }

    [SerializeField]
    public DamageType damage = DamageType.Fire.Copy();
}
