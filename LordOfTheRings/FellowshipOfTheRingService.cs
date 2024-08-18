namespace LordOfTheRings;
public sealed class FellowshipOfTheRingService
{
    private readonly Fellowship _fellowship = new();

    public Result AddMember(Character character) => _fellowship.AddMember(character);

    public Result MoveMembersToRegion(List<string> memberNames, string region) => _fellowship.MoveMembersToRegion(memberNames, region);

    public void PrintMembersInRegion(string region)
    {
        List<Character> charactersInRegion = _fellowship.GetMembersInRegion(region);

        if (!charactersInRegion.Count.Equals(0))
        {
            Console.WriteLine($"No members in {region}");

            return;
        }

        Console.WriteLine($"Members in {region}:");

        charactersInRegion.ForEach(character =>
                                       Console.WriteLine($"{character.Name} ({character.Race}) with {character.Weapon.Name}"));
    }

    public Result RemoveMember(string name) => _fellowship.RemoveMember(name);

    public override string ToString() => "Fellowship of the Ring Members:\n" + _fellowship;

    public Result UpdateCharacterWeapon(string name, string newWeaponName, int damage)
    {
        Result<Character> characterResult = _fellowship.GetCharacterByName(name);

        return !characterResult.IsSuccess ? characterResult : characterResult.Value.UpdateWeapon(newWeaponName, damage);
    }
}