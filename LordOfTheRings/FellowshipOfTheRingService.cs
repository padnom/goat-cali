using LordOfTheRings;

public class FellowshipOfTheRingService
{
    private readonly List<Character> _members = new();

    public void AddMember(Character character)
    {
        ValidateCharacter(character);
        EnsureUniqueCharacterName(character.Name);

        _members.Add(character);
    }

    public void MoveMembersToRegion(List<string> memberNames, string region)
    {
        foreach (string name in memberNames)
        {
            var character = GetCharacterByName(name);
            ValidateMoveToRegion(character, region);
            character.CurrentRegion = region;
            PrintMoveMessage(character, region);
        }
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

    public void RemoveMember(string name)
    {
        var character = GetCharacterByName(name);
        _members.Remove(character);
    }

    public override string ToString()
    {
        return "Fellowship of the Ring Members:\n"
               + string.Join("\n",
                             _members.Select(m =>
                                                 $"{m.Name} ({m.Race}) with {m.Weapon.Name} in {m.CurrentRegion}"));
    }

    public void UpdateCharacterWeapon(string name, string newWeaponName, int damage)
    {
        var character = GetCharacterByName(name);

        character.Weapon = new Weapon
                           {
                               Name = newWeaponName,
                               Damage = damage,
                           };
    }

    private void EnsureUniqueCharacterName(string name)
    {
        if (_members.Any(m => m.Name == name))
        {
            throw new InvalidOperationException($"A character with the name '{name}' already exists in the fellowship.");
        }
    }

    private Character GetCharacterByName(string name)
    {
        var character = _members.FirstOrDefault(c => c.Name == name);

        if (character == null)
        {
            throw new InvalidOperationException($"No character with the name '{name}' exists in the fellowship.");
        }

        return character;
    }

    private void PrintMoveMessage(Character character, string region)
    {
        if (region == "Mordor")
        {
            Console.WriteLine($"{character.Name} moved to {region} 💀.");
        }
        else
        {
            Console.WriteLine($"{character.Name} moved to {region}.");
        }
    }

    private void ValidateCharacter(Character character)
    {
        if (character == null)
        {
            throw new ArgumentNullException(nameof(character), "Character cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(character.Name))
        {
            throw new ArgumentException("Character must have a name.");
        }

        if (string.IsNullOrWhiteSpace(character.Race))
        {
            throw new ArgumentException("Character must have a race.");
        }

        if (character.Weapon == null)
        {
            throw new ArgumentException("Character must have a weapon.");
        }

        if (string.IsNullOrWhiteSpace(character.Weapon.Name))
        {
            throw new ArgumentException("A weapon must have a name.");
        }

        if (character.Weapon.Damage <= 0)
        {
            throw new ArgumentException("A weapon must have a damage level.");
        }
    }

    private void ValidateMoveToRegion(Character character, string region)
    {
        if (character.CurrentRegion == "Mordor"
            && region != "Mordor")
        {
            throw new InvalidOperationException(
                $"Cannot move {character.Name} from Mordor to {region}. Reason: There is no coming back from Mordor.");
        }
    }
}