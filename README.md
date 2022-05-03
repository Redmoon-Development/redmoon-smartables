# Redmoon Smart Enums

## Description
Provides Smart variations on common usage that allows for ease for modification.

## How to Use (Install)
Follow the Install Instructions for https://github.com/sandolkakos/unity-package-manager-utilities

For Git Versioning and Updates, Install: https://github.com/mob-sakai/UpmGitExtension#usage

Then Just copy the link below and add it to your project via Unity Package Manager: [Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
```
https://github.com/Redmoon-Development/redmoon-smartables.git
```

## How to Use (Work)

Red Moon Smartables allows the creation of SmartEnums inside the Unity Inspector.
These are similar to Enums and are serialized similar to enums with two advantages:
1. You can Attach Properties to the Enum (No more static switch statements)
2. You can define new enums outside of the definition class

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

Special Notes:
- Use DamageType.Fire.Clone() in the constructor if you want to set a default.

## Plans

- Factory for safe create and add.
- Smart-Flag systems.

## License
MIT License

There is no legally binding modifications to the MIT License, but if you are using my stuff, I would appreciate doing one of the following: buy me a beer if you ever meet me at a bar or invite me to potential money-making operations via my Contact Information provided below.

## Contact Me
Contact me at jackel1020@gmail.com.
If you want your message to actually be read, add "[Github-Message]" to your subject line.
