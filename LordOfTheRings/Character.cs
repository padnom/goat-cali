using LordOfTheRings;

public sealed class Character
{
    public string CurrentRegion { get; private set; }
    public string Name { get; }
    public string Race { get; }
    public Weapon Weapon { get; private set; }

    private Character(string name, string race, Weapon weapon, string currentRegion = "Shire")
    {
        Name = name;
        Race = race;
        Weapon = weapon;
        CurrentRegion = currentRegion;
    }

    public static Result<Character> Create(string name, string race, Result<Weapon> weapon, string currentRegion = "Shire")
    {
        if (!weapon.IsSuccess)
        {
            return Result<Character>.Failure(weapon.Error);
        }

        var character = new Character(name, race, weapon.Value, currentRegion);
        var validationResult = character.Validate();

        if (!validationResult.IsSuccess)
        {
            return Result<Character>.Failure(validationResult.Error);
        }

        return Result<Character>.Success(character);
    }

    public Result MoveToRegion(string newRegion)
    {
        if (CurrentRegion == "Mordor"
            && newRegion != "Mordor")
        {
            return Result.Failure($"Cannot move {Name} from Mordor to {newRegion}. Reason: There is no coming back from Mordor.");
        }

        CurrentRegion = newRegion;

        return Result.Success();
    }

    public override string ToString() => $"{Name} ({Race}) with {Weapon.Name} in {CurrentRegion}";

    public Result UpdateWeapon(string newWeaponName, int damage)
    {
        Result<Weapon> weaponUpdateResult = Weapon.Create(newWeaponName, damage);

        if (!weaponUpdateResult.IsSuccess)
        {
            return Result.Failure(weaponUpdateResult.Error);
        }

        Weapon = weaponUpdateResult.Value;

        return Result.Success();
    }

    public Result Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return Result.Failure("Character name must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(Race))
        {
            return Result.Failure("Character race must not be empty.");
        }

        return Result.Success();
    }
}