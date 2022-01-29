using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;

namespace RedMoon.Smartables
{

#if UNITY_EDITOR
    using UnityEditor;
    [CustomPropertyDrawer(typeof(SmartEnum), true)]
    public class SmartEnumPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();

            var smartEnum = (SmartEnum)fieldInfo.GetValue(property.serializedObject.targetObject);
            var enumList = SmartEnum.GetValues(smartEnum);

            var i = enumList.IndexOf(enumList.FirstOrDefault(x => x.Equals(smartEnum)));
            i = (i < 0) ? 0 : i;

            string[] options = enumList.Select(x => x.ToString()).ToArray();


            var j = EditorGUI.Popup(position, property.displayName, i, options);
            SetValue(property, enumList[j]);

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();

        }

        //Thanks Kind Stranger who showed me this code to borrow.
        public static void SetValue(UnityEditor.SerializedProperty property, object val)
        {
            object obj = property.serializedObject.targetObject;

            List<KeyValuePair<FieldInfo, object>> list = new List<KeyValuePair<FieldInfo, object>>();

            FieldInfo field = null;
            foreach (var path in property.propertyPath.Split('.'))
            {
                var type = obj.GetType();
                field = type.GetField(path);
                list.Add(new KeyValuePair<FieldInfo, object>(field, obj));
                obj = field.GetValue(obj);
            }

            // Now set values of all objects, from child to parent
            for (int i = list.Count - 1; i >= 0; --i)
            {
                list[i].Key.SetValue(list[i].Value, val);
                // New 'val' object will be parent of current 'val' object
                val = list[i].Value;
            }
        }

    }
#endif
}