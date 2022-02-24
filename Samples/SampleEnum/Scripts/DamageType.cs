using RedMoon.Smartables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageType : SmartEnum<DamageType>
{
    public static readonly DamageType Regular = new DamageType(nameof(Regular), 0);
    public static readonly DamageType Fire = new DamageType(nameof(Fire), 1);
    public static readonly DamageType Ice = new DamageType(nameof(Ice), 2);
    public static readonly DamageType Water = new DamageType(nameof(Water), 3);

    private int strLength;
    public int StrLength => strLength;

    private DamageType(string name, int value) : base(name, value)
    {
        strLength = name.Length;
    }
    protected override void ConstructEnum(DamageType @enum)
    {
        strLength = @enum.strLength;
    }
}
