namespace LordOfTheRings;
public sealed class Character
{
    public string CurrentRegion { get; set; } = "Shire";
    public string Name { get; set; }
    public string Race { get; set; }
    public Weapon Weapon { get; set; }
}