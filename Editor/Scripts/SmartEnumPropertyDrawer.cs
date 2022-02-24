using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System;
using Bewildered.Editor;

namespace RedMoon.Smartables
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditorInternal;

    [CustomPropertyDrawer(typeof(SmartEnum), true)]
    public class SmartEnumPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //Begin Property
            EditorGUI.BeginProperty(position, label, property);

            //Get Current Value
            var smartEnum = property.GetValue<SmartEnum>();

            //Get All Other Possible Values
            var enumList = smartEnum.GetValues();

            //Get Current Index
            var i = enumList.IndexOf(enumList.FirstOrDefault(x => x.Equals(smartEnum)));
            i = (i < 0) ? 0 : i;

            //Make Possible Values into List
            string[] options = enumList.Select(x => x.ToString()).ToArray();

            //Do a Popup and Get Selection
            var j = EditorGUI.Popup(position, property.displayName, i, options);

            //Set Value to Current Selection
            property.SetValue(enumList[j]);

            //End Property
            EditorGUI.EndProperty();
        }
    }
#endif
}