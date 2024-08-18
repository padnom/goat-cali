using FluentAssertions;

namespace LordOfTheRings.Tests;
public class FellowshipOfTheRingServiceTests
{
    [Theory]
    [InlineData("Frodo", "Hobbit", "Sting", 10, "Shire")]
    public void AddMember_CharacterWithDuplicateName_ThrowsInvalidOperationException(
        string name,
        string race,
        string weaponName,
        int damage,
        string region)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character1 = new Character
                         {
                             Name = name,
                             Race = race,
                             Weapon = new Weapon { Name = weaponName, Damage = damage, },
                             CurrentRegion = region,
                         };

        var character2 = new Character
                         {
                             Name = name,
                             Race = race,
                             Weapon = new Weapon { Name = weaponName, Damage = damage, },
                             CurrentRegion = region,
                         };

        service.AddMember(character1);

        // Act & Assert
        var act = () => service.AddMember(character2);

        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("A character with the same name already exists in the fellowship.");
    }

    [Theory]
    [InlineData(null, "Hobbit", "Sting", 10, "Character must have a name.")]
    [InlineData("Frodo", null, "Sting", 10, "Character must have a race.")]
    [InlineData("Frodo", "Hobbit", null, 10, "Character must have a weapon.")]
    [InlineData("Frodo", "Hobbit", "", 10, "A weapon must have a name.")]
    [InlineData("Frodo", "Hobbit", "Sting", 0, "A weapon must have a damage level.")]
    public void AddMember_InvalidCharacter_ThrowsArgumentException(
        string name,
        string race,
        string weaponName,
        int damage,
        string expectedMessage)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            Name = name,
                            Race = race,
                            Weapon = weaponName == null ? null : new Weapon { Name = weaponName, Damage = damage, },
                            CurrentRegion = "Shire",
                        };

        // Act & Assert
        var act = () => service.AddMember(character);

        act.Should()
           .Throw<ArgumentException>()
           .WithMessage(expectedMessage);
    }

    [Fact]
    public void AddMember_NullCharacter_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        // Act & Assert
        var act = () => service.AddMember(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("Frodo", "Hobbit", "Sting", 10, "Shire", "Frodo (Hobbit) with Sting in Shire")]
    [InlineData("Aragorn", "Human", "Sword", 100, "Rivendell", "Aragorn (Human) with Sword in Rivendell")]
    public void AddMember_ValidCharacter_AddsCharacter(
        string name,
        string race,
        string weaponName,
        int damage,
        string region,
        string expectedOutput)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            Name = name,
                            Race = race,
                            Weapon = new Weapon { Name = weaponName, Damage = damage, },
                            CurrentRegion = region,
                        };

        // Act
        service.AddMember(character);

        // Assert
        service.ToString().Should().Contain(expectedOutput);
    }

    [Theory]
    [InlineData("Frodo", "Mordor", "Shire", "Cannot move Frodo from Mordor to Shire. Reason: There is no coming back from Mordor.")]
    public void MoveMembersToRegion_MovingFromMordor_ThrowsException(
        string name,
        string fromRegion,
        string toRegion,
        string expectedMessage)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { Name = name, Race = "Hobbit", Weapon = new Weapon { Name = "Sting", Damage = 10, }, CurrentRegion = fromRegion, });

        var memberNames = new List<string> { name, };

        // Act
        var act = () => service.MoveMembersToRegion(memberNames, toRegion);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("Aragorn", "Human", "Sword", 100, "Rivendell", "Gondor", "Aragorn (Human) with Sword in Gondor")]
    [InlineData("Legolas", "Elf", "Bow", 50, "Rivendell", "Gondor", "Legolas (Elf) with Bow in Gondor")]
    public void MoveMembersToRegion_ValidMembers_MoveToNewRegion(
        string name,
        string race,
        string weaponName,
        int damage,
        string fromRegion,
        string toRegion,
        string expectedOutput)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { Name = name, Race = race, Weapon = new Weapon { Name = weaponName, Damage = damage, }, CurrentRegion = fromRegion, });

        var memberNames = new List<string> { name, };

        // Act
        service.MoveMembersToRegion(memberNames, toRegion);

        // Assert
        service.ToString().Should().Contain(expectedOutput);
    }

    [Theory]
    [InlineData("Gollum", "Hobbit", "Dagger", 5, "Shire", "Mordor", "Gollum (Hobbit) with Dagger in Mordor")]
    public void MoveMembersToRegion_ValidMoveToMordor_ShowsCorrectOutput(
        string name,
        string race,
        string weaponName,
        int damage,
        string fromRegion,
        string toRegion,
        string expectedOutput)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { Name = name, Race = race, Weapon = new Weapon { Name = weaponName, Damage = damage, }, CurrentRegion = fromRegion, });

        var memberNames = new List<string> { name, };

        // Act
        service.MoveMembersToRegion(memberNames, toRegion);

        // Assert
        service.ToString().Should().Contain(expectedOutput);
    }

    [Theory]
    [InlineData("Rivendell", true)]
    [InlineData("Mordor", false)]
    public void PrintMembersInRegion_RegionHasMembers_PrintsCorrectly(string region, bool hasMembers)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        service.AddMember(new Character
                          {
                              Name = "Aragorn", Race = "Human", Weapon = new Weapon { Name = "Sword", Damage = 100, },
                              CurrentRegion = "Rivendell",
                          });

        service.AddMember(new Character { Name = "Legolas", Race = "Elf", Weapon = new Weapon { Name = "Bow", Damage = 50, }, CurrentRegion = "Rivendell", });

        // Act
        var act = () => service.PrintMembersInRegion(region);

        // Assert
        act.Should().NotThrow(); // Ensure it doesn't throw an exception
    }

    [Theory]
    [InlineData("Gimli", "No character with the name 'Gimli' exists in the fellowship.")]
    public void RemoveMember_NonExistentMember_ThrowsException(string name, string expectedMessage)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        // Act
        var act = () => service.RemoveMember(name);

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("Boromir", "Human", "Sword", 90, "Gondor")]
    public void RemoveMember_ValidMember_RemovesMember(
        string name,
        string race,
        string weaponName,
        int damage,
        string region)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { Name = name, Race = race, Weapon = new Weapon { Name = weaponName, Damage = damage, }, CurrentRegion = region, });

        // Act
        service.RemoveMember(name);

        // Assert
        service.ToString().Should().NotContain(name);
    }

    [Fact]
    public void UpdateCharacterWeapon_CharacterDoesNotExist_DoesNotThrow()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            Name = "Gimli",
                            Weapon = new Weapon { Name = "Sword", Damage = 1, },
                            Race = "Dwarf",
                        };

        service.AddMember(character);

        // Act
        var act = () => service.UpdateCharacterWeapon("Gimli", "Axe", 80);

        // Assert
        act.Should().NotThrow();
        service.ToString().Should().NotContain("Sword");
    }

    [Theory]
    [InlineData("Legolas", "Longbow", 75)]
    public void UpdateCharacterWeapon_CharacterExists_WeaponUpdatedCorrectly(string name, string newWeaponName, int newDamage)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            Name = name,
                            Race = "Elf",
                            Weapon = new Weapon { Name = "Bow", Damage = 50, },
                            CurrentRegion = "Rivendell",
                        };

        service.AddMember(character);

        // Act
        var act = () => service.UpdateCharacterWeapon(name, newWeaponName, newDamage);

        // Assert
        act.Should().NotThrow();

        var expectedOutput = $"{name} (Elf) with {newWeaponName} in Rivendell";
        service.ToString().Should().Contain(expectedOutput);
    }
}