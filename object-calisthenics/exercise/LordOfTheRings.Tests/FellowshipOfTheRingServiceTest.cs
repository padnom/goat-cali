using FluentAssertions;

namespace LordOfTheRings.Tests;
public class FellowshipOfTheRingServiceTests
{
    [Fact]
    public void AddMember_ShouldAddCharacter_WhenValid()
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
        var act = () => service.AddMember(character);

        // Assert
        act.Should().NotThrow();
        service.ToString().Should().Contain("Frodo (Hobbit) with Sting");
    }

    [Fact]
    public void AddMember_ShouldThrowArgumentException_WhenCharacterNameIsNullOrEmpty()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = "",
                            R = "Elf",
                            W = new Weapon { Name = "Bow", Damage = 50, },
                            C = "Rivendell",
                        };

        // Act
        var act = () => service.AddMember(character);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage("Character must have a name.");
    }

    [Fact]
    public void AddMember_ShouldThrowArgumentNullException_WhenCharacterIsNull()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        // Act
        var act = () => service.AddMember(null);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithMessage("Character cannot be null. (Parameter 'character')");
    }

    [Fact]
    public void MoveMembersToRegion_ShouldMoveCharacterToNewRegion_WhenValid()
    {
        // Arrange
        var service = CreateServiceWithMembers();
        var memberNames = new List<string> { "Aragorn", };

        // Act
        var act = () => service.MoveMembersToRegion(memberNames, "Gondor");

        // Assert
        act.Should().NotThrow();
        service.ToString().Should().Contain("Aragorn (Human) with Sword in Gondor");
    }

    [Fact]
    public void MoveMembersToRegion_ShouldThrowException_WhenMovingCharacterFromMordor()
    {
        // Arrange
        var service = new FellowshipOfTheRingService();

        var character = new Character
                        {
                            N = "Gollum",
                            R = "Hobbit",
                            W = new Weapon { Name = "Dagger", Damage = 5, },
                            C = "Mordor",
                        };

        service.AddMember(character);

        var memberNames = new List<string> { "Gollum", };

        // Act
        var act = () => service.MoveMembersToRegion(memberNames, "Shire");

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("Cannot move Gollum from Mordor to Shire. Reason: There is no coming back from Mordor.");
    }

    [Fact]
    public void PrintMembersInRegion_ShouldPrintMembers_WhenMembersExistInRegion()
    {
        // Arrange
        var service = CreateServiceWithMembers();

        // Act
        var act = () => service.PrintMembersInRegion("Rivendell");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void PrintMembersInRegion_ShouldPrintNoMembers_WhenNoMembersExistInRegion()
    {
        // Arrange
        var service = CreateServiceWithMembers();

        // Act
        var act = () => service.PrintMembersInRegion("Mordor");

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RemoveMember_ShouldRemoveCharacter_WhenCharacterExists()
    {
        // Arrange
        var service = CreateServiceWithMembers();

        // Act
        service.RemoveMember("Legolas");

        // Assert
        service.ToString().Should().NotContain("Legolas (Elf)");
    }

    [Fact]
    public void UpdateCharacterWeapon_ShouldUpdateWeapon_WhenCharacterExists()
    {
        // Arrange
        var service = CreateServiceWithMembers();

        // Act
        service.UpdateCharacterWeapon("Aragorn", "Andúril", 150);

        // Assert
        service.ToString().Should().Contain("Aragorn (Human) with Andúril");
    }

    private FellowshipOfTheRingService CreateServiceWithMembers()
    {
        var service = new FellowshipOfTheRingService();

        service.AddMember(new Character
                          {
                              N = "Aragorn",
                              R = "Human",
                              W = new Weapon { Name = "Sword", Damage = 100, },
                              C = "Rivendell",
                          });

        service.AddMember(new Character
                          {
                              N = "Legolas",
                              R = "Elf",
                              W = new Weapon { Name = "Bow", Damage = 50, },
                              C = "Rivendell",
                          });

        return service;
    }
}