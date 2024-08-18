namespace LordOfTheRings;
public sealed class FellowshipOfTheRingService
{
    private readonly List<Character> _members = [];

    public Result AddMember(Character character)
    {
        if (character == null)
        {
            return Result.Failure("Character cannot be null.");
        }

        var validationResult = character.Validate();

        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        if (_members.Any(m => m.Name == character.Name))
        {
            return Result.Failure($"A character with the name '{character.Name}' already exists in the fellowship.");
        }

        _members.Add(character);

        return Result.Success();
    }

    public Result MoveMembersToRegion(List<string> memberNames, string region)
    {
        foreach (string name in memberNames)
        {
            Result<Character> characterResult = GetCharacterByName(name);

            if (!characterResult.IsSuccess)
            {
                return characterResult;
            }

            var moveResult = characterResult.Value.MoveToRegion(region);

            if (!moveResult.IsSuccess)
            {
                return moveResult;
            }
        }

        return Result.Success();
    }

    public void PrintMembersInRegion(string region)
    {
        var charactersInRegion = _members.Where(c => c.CurrentRegion == region).ToList();

        if (charactersInRegion.Count == 0)
        {
            Console.WriteLine($"No members in {region}");

            return;
        }

        Console.WriteLine($"Members in {region}:");

        charactersInRegion.ForEach(character =>
                                       Console.WriteLine($"{character.Name} ({character.Race}) with {character.Weapon.Name}")
        );
    }

    public Result RemoveMember(string name)
    {
        Result<Character> characterResult = GetCharacterByName(name);

        if (!characterResult.IsSuccess)
        {
            return characterResult;
        }

        _members.Remove(characterResult.Value);

        return Result.Success();
    }

    public override string ToString()
    {
        return "Fellowship of the Ring Members:\n" + string.Join("\n", _members.Select(m => $"{m.Name} ({m.Race}) with {m.Weapon.Name} in {m.CurrentRegion}"));
    }

    public Result UpdateCharacterWeapon(string name, string newWeaponName, int damage)
    {
        Result<Character> characterResult = GetCharacterByName(name);

        if (!characterResult.IsSuccess)
        {
            return characterResult;
        }

        return characterResult.Value.UpdateWeapon(newWeaponName, damage);
    }

    private Result<Character> GetCharacterByName(string name)
    {
        var character = _members.Find(c => c.Name == name);

        return character != null
            ? Result<Character>.Success(character)
            : Result<Character>.Failure($"No character with the name '{name}' exists in the fellowship.");
    }
}