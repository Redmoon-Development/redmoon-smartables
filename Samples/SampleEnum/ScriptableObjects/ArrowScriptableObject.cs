using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RedMoon/Samples/ArrowScriptableObject")]
public class ArrowScriptableObject : ScriptableObject
{
    public DamageType damage = DamageType.Water.Copy();
}
