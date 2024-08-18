using LordOfTheRings;

public class FellowshipOfTheRingService
{
    private readonly List<Character> _members = new();

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

        var duplicateCheck = EnsureUniqueCharacterName(character.Name);

        if (!duplicateCheck.IsSuccess)
        {
            return duplicateCheck;
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

            var character = characterResult.Value;

            var moveResult = character.MoveToRegion(region);

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

        if (charactersInRegion.Any())
        {
            Console.WriteLine($"Members in {region}:");

            foreach (var character in charactersInRegion)
            {
                Console.WriteLine($"{character.Name} ({character.Race}) with {character.Weapon.Name}");
            }
        }
        else
        {
            Console.WriteLine($"No members in {region}");
        }
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
        return "Fellowship of the Ring Members:\n"
               + string.Join("\n", _members.Select(m => $"{m.Name} ({m.Race}) with {m.Weapon.Name} in {m.CurrentRegion}"));
    }

    public Result UpdateCharacterWeapon(string name, string newWeaponName, int damage)
    {
        Result<Character> characterResult = GetCharacterByName(name);

        if (!characterResult.IsSuccess)
        {
            return characterResult;
        }

        var character = characterResult.Value;

        return character.UpdateWeapon(newWeaponName, damage);
    }

    private Result EnsureUniqueCharacterName(string name)
    {
        if (_members.Any(m => m.Name == name))
        {
            return Result.Failure($"A character with the name '{name}' already exists in the fellowship.");
        }

        return Result.Success();
    }

    private Result<Character> GetCharacterByName(string name)
    {
        var character = _members.FirstOrDefault(c => c.Name == name);

        if (character == null)
        {
            return Result<Character>.Failure($"No character with the name '{name}' exists in the fellowship.");
        }

        return Result<Character>.Success(character);
    }
}