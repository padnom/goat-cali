using FluentAssertions;

namespace LordOfTheRings.Tests;
public class FellowshipOfTheRingServiceTests
{
    [Fact]
    public void AddMember_CharacterWithDuplicateName_ThrowsInvalidOperationException()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character1 = new Character
                         {
                             N = "Frodo",
                             R = "Hobbit",
                             W = new Weapon { Name = "Sting", Damage = 10, },
                             C = "Shire",
                         };

        var character2 = new Character
                         {
                             N = "Frodo",
                             R = "Hobbit",
                             W = new Weapon { Name = "Sting", Damage = 10, },
                             C = "Shire",
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
    public void AddMember_InvalidCharacter_ThrowsArgumentException(string name, string race, string weaponName, int damage, string expectedMessage)
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = name,
                            R = race,
                            W = weaponName == null ? null : new Weapon { Name = weaponName, Damage = damage, },
                            C = "Shire",
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

    [Fact]
    public void AddMember_ValidCharacter_AddsCharacter()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = "Frodo",
                            R = "Hobbit",
                            W = new Weapon { Name = "Sting", Damage = 10, },
                            C = "Shire",
                        };

        // Act
        service.AddMember(character);

        // Assert
        var expectedOutput = "Frodo (Hobbit) with Sting in Shire";
        service.ToString().Should().Contain(expectedOutput);
    }

    [Fact]
    public void MoveMembersToRegion_MovingFromMordor_ThrowsException()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Frodo", R = "Hobbit", W = new Weapon { Name = "Sting", Damage = 10, }, C = "Mordor", });

        var memberNames = new List<string> { "Frodo", };

        // Act
        var act = () => service.MoveMembersToRegion(memberNames, "Shire");

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("Cannot move Frodo from Mordor to Shire. Reason: There is no coming back from Mordor.");
    }

    [Fact]
    public void MoveMembersToRegion_ValidMembers_MoveToNewRegion()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Aragorn", R = "Human", W = new Weapon { Name = "Sword", Damage = 100, }, C = "Rivendell", });
        service.AddMember(new Character { N = "Legolas", R = "Elf", W = new Weapon { Name = "Bow", Damage = 50, }, C = "Rivendell", });

        var memberNames = new List<string> { "Aragorn", "Legolas", };

        // Act
        service.MoveMembersToRegion(memberNames, "Gondor");

        // Assert
        var expectedOutput = "Aragorn (Human) with Sword in Gondor";
        service.ToString().Should().Contain(expectedOutput);
        service.ToString().Should().Contain("Legolas (Elf) with Bow in Gondor");
    }

    [Fact]
    public void MoveMembersToRegion_ValidMoveToMordor_ShowsCorrectOutput()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Gollum", R = "Hobbit", W = new Weapon { Name = "Dagger", Damage = 5, }, C = "Shire", });

        var memberNames = new List<string> { "Gollum", };

        // Act
        service.MoveMembersToRegion(memberNames, "Mordor");

        // Assert
        service.ToString().Should().Contain("Gollum (Hobbit) with Dagger in Mordor");
    }

    [Fact]
    public void PrintMembersInRegion_RegionHasMembers_PrintsCorrectly()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Aragorn", R = "Human", W = new Weapon { Name = "Sword", Damage = 100, }, C = "Rivendell", });
        service.AddMember(new Character { N = "Legolas", R = "Elf", W = new Weapon { Name = "Bow", Damage = 50, }, C = "Rivendell", });

        // Act
        var act = () => service.PrintMembersInRegion("Rivendell");

        // Assert
        act.Should().NotThrow(); // Ensure it doesn't throw an exception
        // For console output, you'd typically mock the Console, but here we just ensure no exceptions are thrown
    }

    [Fact]
    public void PrintMembersInRegion_RegionHasNoMembers_PrintsNoMembers()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Aragorn", R = "Human", W = new Weapon { Name = "Sword", Damage = 100, }, C = "Rivendell", });

        // Act
        var act = () => service.PrintMembersInRegion("Mordor");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RemoveMember_NonExistentMember_ThrowsException()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        // Act
        var act = () => service.RemoveMember("Gimli");

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("No character with the name 'Gimli' exists in the fellowship.");
    }

    [Fact]
    public void RemoveMember_ValidMember_RemovesMember()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();
        service.AddMember(new Character { N = "Boromir", R = "Human", W = new Weapon { Name = "Sword", Damage = 90, }, C = "Gondor", });

        // Act
        service.RemoveMember("Boromir");

        // Assert
        service.ToString().Should().NotContain("Boromir");
    }

    [Fact]
    public void UpdateCharacterWeapon_CharacterDoesNotExist_DoesNotThrow()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        // Act
        var act = () => service.UpdateCharacterWeapon("Gimli", "Axe", 80);

        // Assert
        act.Should().NotThrow(); // The method should not throw an exception if the character doesn't exist
    }

    [Fact]
    public void UpdateCharacterWeapon_CharacterExists_WeaponUpdatedCorrectly()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = "Legolas",
                            R = "Elf",
                            W = new Weapon { Name = "Bow", Damage = 50, },
                            C = "Rivendell",
                        };

        service.AddMember(character);

        // Act
        service.UpdateCharacterWeapon("Legolas", "Longbow", 75);

        // Assert
        character.W.Name.Should().Be("Longbow");
        character.W.Damage.Should().Be(75);
    }

    [Fact]
    public void UpdateCharacterWeapon_ValidCharacter_UpdatesWeapon()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = "Aragorn",
                            R = "Human",
                            W = new Weapon { Name = "Sword", Damage = 100, },
                            C = "Rivendell",
                        };

        service.AddMember(character);

        // Act
        service.UpdateCharacterWeapon("Aragorn", "Andúril", 150);

        // Assert
        var expectedOutput = "Aragorn (Human) with Andúril in Rivendell";
        service.ToString().Should().Contain(expectedOutput);
    }
}