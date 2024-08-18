using FluentAssertions;

namespace LordOfTheRings.Tests;
public class ProgramTest
{
    [Fact]
    public void Test_FellowshipOfTheRingService_ExpectedOutput()
    {
        // Arrange
        var fellowshipOfTheRingService = new FellowshipOfTheRingService();

        var characters = new List<Result<Character>>
                         {
                             Character.Create("Frodo", "Hobbit", Weapon.Create("Sting", 30)),
                             Character.Create("Sam", "Hobbit", Weapon.Create("Dagger", 10)),
                             Character.Create("Merry", "Hobbit", Weapon.Create("Short Sword", 24)),
                             Character.Create("Pippin", "Hobbit", Weapon.Create("Bow", 8)),
                             Character.Create("Aragorn", "Human", Weapon.Create("Anduril", 100)),
                             Character.Create("Boromir", "Human", Weapon.Create("Sword", 90)),
                             Character.Create("Legolas", "Elf", Weapon.Create("Bow", 100)),
                             Character.Create("Gimli", "Dwarf", Weapon.Create("Axe", 100)),
                             Character.Create("Gandalf the 🐐", "Wizard", Weapon.Create("Staff", 200)),
                         };

        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);

            // Act
            foreach (Result<Character> characterResult in characters)
            {
                if (characterResult.IsSuccess)
                {
                    var addResult = fellowshipOfTheRingService.AddMember(characterResult.Value);

                    if (!addResult.IsSuccess)
                    {
                        Console.WriteLine($"Failed to add character: {addResult.Error}");
                    }
                }
                else
                {
                    Console.WriteLine($"Character creation failed: {characterResult.Error}");
                }
            }

            Console.WriteLine(fellowshipOfTheRingService.ToString());

            var group1 = new List<string> { "Frodo", "Sam", };
            var group2 = new List<string> { "Merry", "Pippin", "Aragorn", "Boromir", };
            var group3 = new List<string> { "Legolas", "Gimli", "Gandalf the 🐐", };

            fellowshipOfTheRingService.MoveMembersToRegion(group1, "Rivendell").HandleResult();
            fellowshipOfTheRingService.MoveMembersToRegion(group2, "Moria").HandleResult();
            fellowshipOfTheRingService.MoveMembersToRegion(group3, "Lothlorien").HandleResult();

            var group4 = new List<string> { "Frodo", "Sam", };
            fellowshipOfTheRingService.MoveMembersToRegion(group4, "Mordor").HandleResult();
            fellowshipOfTheRingService.MoveMembersToRegion(group4, "Shire").HandleResult();

            fellowshipOfTheRingService.PrintMembersInRegion("Rivendell");
            fellowshipOfTheRingService.PrintMembersInRegion("Moria");
            fellowshipOfTheRingService.PrintMembersInRegion("Lothlorien");
            fellowshipOfTheRingService.PrintMembersInRegion("Mordor");
            fellowshipOfTheRingService.PrintMembersInRegion("Shire");

            fellowshipOfTheRingService.RemoveMember("Frodo").HandleResult();
            fellowshipOfTheRingService.RemoveMember("Sam").HandleResult();

            // Debugging: Print the actual output to see what is different
            string actualOutput = sw.ToString().Replace("\r\n", "\n").Trim();
            Console.WriteLine(actualOutput);

            // Assert
            string expectedOutput =
                "Fellowship of the Ring Members:\n"
                + "Frodo (Hobbit) with Sting in Shire\n"
                + "Sam (Hobbit) with Dagger in Shire\n"
                + "Merry (Hobbit) with Short Sword in Shire\n"
                + "Pippin (Hobbit) with Bow in Shire\n"
                + "Aragorn (Human) with Anduril in Shire\n"
                + "Boromir (Human) with Sword in Shire\n"
                + "Legolas (Elf) with Bow in Shire\n"
                + "Gimli (Dwarf) with Axe in Shire\n"
                + "Gandalf the 🐐 (Wizard) with Staff in Shire\n"
                + "Operation failed: Cannot move Frodo from Mordor to Shire. Reason: There is no coming back from Mordor.\n"
                + "Members in Rivendell:\n"
                + "No members in Moria\n"
                + "No members in Lothlorien\n"
                + "No members in Mordor\n"
                + "Members in Shire:";

            string expectedOutputNormalized = expectedOutput.Replace("\r\n", "\n").Trim();
            actualOutput.Should().Be(expectedOutputNormalized);
        }
    }
}