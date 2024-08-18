namespace LordOfTheRings;
public class FellowshipOfTheRingService
{
    private readonly List<Character> members = new();

    public void AddMember(Character character)
    {
        if (character == null)
        {
            throw new ArgumentNullException(nameof(character), "Character cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(character.N))
        {
            throw new ArgumentException("Character must have a name.");
        }

        if (string.IsNullOrWhiteSpace(character.R))
        {
            throw new ArgumentException("Character must have a race.");
        }

        if (character.W == null)
        {
            throw new ArgumentException("Character must have a weapon.");
        }

        if (string.IsNullOrWhiteSpace(character.W.Name))
        {
            throw new ArgumentException("A weapon must have a name.");
        }

        if (character.W.Damage <= 0)
        {
            throw new ArgumentException("A weapon must have a damage level.");
        }

        var exists = false;

        foreach (var member in members)
        {
            if (member.N == character.N)
            {
                exists = true;

                break;
            }
        }

        if (exists)
        {
            throw new InvalidOperationException(
                "A character with the same name already exists in the fellowship.");
        }

        members.Add(character);
    }

    public void MoveMembersToRegion(List<string> memberNames, string region)
    {
        foreach (string name in memberNames)
        {
            foreach (var character in members)
            {
                if (character.N == name)
                {
                    if (character.C == "Mordor"
                        && region != "Mordor")
                    {
                        throw new InvalidOperationException(
                            $"Cannot move {character.N} from Mordor to {region}. Reason: There is no coming back from Mordor.");
                    }

                    character.C = region;

                    if (region != "Mordor")
                    {
                        Console.WriteLine($"{character.N} moved to {region}.");
                    }
                    else
                    {
                        Console.WriteLine($"{character.N} moved to {region} 💀.");
                    }
                }
            }
        }
    }

    public void PrintMembersInRegion(string region)
    {
        var charactersInRegion = new List<Character>();

        foreach (var character in members)
        {
            if (character.C == region)
            {
                charactersInRegion.Add(character);
            }
        }

        if (charactersInRegion.Count > 0)
        {
            Console.WriteLine($"Members in {region}:");

            foreach (var character in charactersInRegion)
            {
                Console.WriteLine($"{character.N} ({character.R}) with {character.W.Name}");
            }
        }
        else if (charactersInRegion.Count == 0)
        {
            Console.WriteLine($"No members in {region}");
        }
    }

    public void RemoveMember(string name)
    {
        Character characterToRemove = null;

        foreach (var character in members)
        {
            if (character.N == name)
            {
                characterToRemove = character;

                break;
            }
        }

        if (characterToRemove == null)
        {
            throw new InvalidOperationException($"No character with the name '{name}' exists in the fellowship.");
        }

        members.Remove(characterToRemove);
    }

    public override string ToString()
    {
        var result = "Fellowship of the Ring Members:\n";

        foreach (var member in members)
        {
            result += $"{member.N} ({member.R}) with {member.W.Name} in {member.C}" + "\n";
        }

        return result;
    }

    public void UpdateCharacterWeapon(string name, string newWeapon, int damage)
    {
        foreach (var character in members)
        {
            if (character.N == name)
            {
                character.W = new Weapon
                              {
                                  Name = newWeapon,
                                  Damage = damage,
                              };

                break;
            }
        }
    }
}