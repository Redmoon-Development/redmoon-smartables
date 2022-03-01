# redmoon-smartables
Provides Smart variations on common usage that allows for ease for modification.

Recommended Packages for Installation and Versioning:
- https://github.com/sandolkakos/unity-package-manager-utilities
- https://github.com/mob-sakai/UpmGitExtension

Example:
```
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
```

This class is a Smart Enum. A SmartEnum is a value that serializes as a value and constructs itself as an Injectible Enum with attached properties at runtime.
Add Serializable Tag to allow for Enum Serialization.
Construct by Providing a Value.
ConstructEnum is used to define a deserialization process to make sure values are proper at runtime. Use it to set-up private properties with the constructor.


Special Notes:
- Use DamageType.Fire.Clone() in the constructor if you want to set a default.
