using LordOfTheRings;

var fellowship = new FellowshipOfTheRingService();

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

foreach (Result<Character> characterResult in characters)
{
    if (characterResult.IsSuccess)
    {
        var addResult = fellowship.AddMember(characterResult.Value);

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

Console.WriteLine(fellowship.ToString());

var group1 = new List<string> { "Frodo", "Sam", };
var group2 = new List<string> { "Merry", "Pippin", "Aragorn", "Boromir", };
var group3 = new List<string> { "Legolas", "Gimli", "Gandalf the 🐐", };

fellowship.MoveMembersToRegion(group1, "Rivendell").HandleResult();
fellowship.MoveMembersToRegion(group2, "Moria").HandleResult();
fellowship.MoveMembersToRegion(group3, "Lothlorien").HandleResult();

var group4 = new List<string> { "Frodo", "Sam", };
fellowship.MoveMembersToRegion(group4, "Mordor").HandleResult();
fellowship.MoveMembersToRegion(group4, "Shire").HandleResult();

fellowship.PrintMembersInRegion("Rivendell");
fellowship.PrintMembersInRegion("Moria");
fellowship.PrintMembersInRegion("Lothlorien");
fellowship.PrintMembersInRegion("Mordor");
fellowship.PrintMembersInRegion("Shire");

fellowship.RemoveMember("Frodo").HandleResult();
fellowship.RemoveMember("Sam").HandleResult();