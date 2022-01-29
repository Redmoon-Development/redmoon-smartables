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
        [SerializeField]
        public DamageType damage = DamageType.Fire.Copy();

        public void Start()
        {
            Debug.Log(JsonUtility.ToJson(this));
            Debug.Log(damage.ToString());
            Debug.Log(DamageType.SmartEnums.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y));
        }

        private void Update()
        {
            Debug.Log(damage.ToString());
        }
    }