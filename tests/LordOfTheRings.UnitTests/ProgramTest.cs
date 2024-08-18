﻿using FluentAssertions;

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
                        Console.WriteLine($"Failed to add character: {addResult.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Character creation failed: {characterResult.Message}");
                }
            }

            Console.WriteLine(fellowshipOfTheRingService.ToString());

            var group1 = new List<string> { "Frodo", "Sam", };
            var group2 = new List<string> { "Merry", "Pippin", "Aragorn", "Boromir", };
            var group3 = new List<string> { "Legolas", "Gimli", "Gandalf the 🐐", };

            fellowshipOfTheRingService.MoveMembersToRegion(group1, "Rivendell").HandleResult(group1, "Rivendell");
            fellowshipOfTheRingService.MoveMembersToRegion(group2, "Moria").HandleResult(group2, "Moria");
            fellowshipOfTheRingService.MoveMembersToRegion(group3, "Lothlorien").HandleResult(group3, "Lothlorien");

            var group4 = new List<string> { "Frodo", "Sam", };
            fellowshipOfTheRingService.MoveMembersToRegion(group4, "Mordor").HandleResult(group4, "Mordor");
            fellowshipOfTheRingService.MoveMembersToRegion(group4, "Shire").HandleResult(group4, "Shire");

            Console.WriteLine(fellowshipOfTheRingService.GetMembersInRegion("Rivendell"));
            Console.WriteLine(fellowshipOfTheRingService.GetMembersInRegion("Moria"));
            Console.WriteLine(fellowshipOfTheRingService.GetMembersInRegion("Lothlorien"));
            Console.WriteLine(fellowshipOfTheRingService.GetMembersInRegion("Mordor"));
            Console.WriteLine(fellowshipOfTheRingService.GetMembersInRegion("Shire"));

            fellowshipOfTheRingService.RemoveMember("Frodo").HandleResult();
            fellowshipOfTheRingService.RemoveMember("Sam").HandleResult();

            string actualOutput = sw.ToString().Replace("\r\n", "\n").Trim();

            Console.WriteLine("=== Actual Output ===");
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
                + "Frodo moved to Rivendell.\n"
                + "Sam moved to Rivendell.\n"
                + "Merry moved to Moria.\n"
                + "Pippin moved to Moria.\n"
                + "Aragorn moved to Moria.\n"
                + "Boromir moved to Moria.\n"
                + "Legolas moved to Lothlorien.\n"
                + "Gimli moved to Lothlorien.\n"
                + "Gandalf the 🐐 moved to Lothlorien.\n"
                + "Frodo moved to Mordor.\n"
                + "Sam moved to Mordor.\n"
                + "Cannot move Frodo from Mordor to Shire. Reason: There is no coming back from Mordor.\n"
                + "No members in Rivendell\n"
                + "Members in Moria:\n"
                + "Merry (Hobbit) with Short Sword\n"
                + "Pippin (Hobbit) with Bow\n"
                + "Aragorn (Human) with Anduril\n"
                + "Boromir (Human) with Sword\n"
                + "Members in Lothlorien:\n"
                + "Legolas (Elf) with Bow\n"
                + "Gimli (Dwarf) with Axe\n"
                + "Gandalf the 🐐 (Wizard) with Staff\n"
                + "Members in Mordor:\n"
                + "Frodo (Hobbit) with Sting\n"
                + "Sam (Hobbit) with Dagger\n"
                + "No members in Shire";

            string expectedOutputNormalized = expectedOutput.Replace("\r\n", "\n").Trim();
            string actualOutputNormalized = actualOutput.Replace("\r\n", "\n").Trim();
            actualOutputNormalized.Should().Be(expectedOutputNormalized, "the actual output should match the expected output.");
        }
    }
}