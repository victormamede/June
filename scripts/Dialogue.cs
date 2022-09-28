using Godot;

public class Dialogue : Resource
{
    [Export]
    public string Value = "";

    [Export(PropertyHint.Enum, "None,Jiggle")]
    public TextEffect Effect = TextEffect.None;

    public enum TextEffect
    {
        None = 0,
        Jiggle = 1
    }

    public static Dialogue FromString(string value)
    {
        char[] delimiter = { '|' };
        string[] values = value.Split(delimiter, 2);

        if (values.Length == 1) return new Dialogue() { Value = values[0] };

        TextEffect effect = TextEffect.None;
        switch (values[0])
        {
            case "jiggle":
                effect = TextEffect.Jiggle;
                break;
        }

        return new Dialogue()
        {
            Value = values[1],
            Effect = effect
        };
    }
}