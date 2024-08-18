# Kata

## First Step Create Unit Test For all services with Copilot

### Step 1

I asked to copilot base on the service to create a unit test for the service.
The goal is to have 100% coverage for the service. Even if coverage is not the goal.

![img.png](img.png)

### Step 2

refactor: tests to use Theory and InlineData attributes

Refactored test methods in FellowshipOfTheRingServiceTests to use
[Theory] and [InlineData] attributes for parameterized testing.
Updated various test methods to accept parameters for character
details, regions, and expected outcomes, enhancing flexibility
and test coverage. Replaced several [Fact] attributes with
[Theory] and [InlineData].

### Step 3

refactor: Character class and update related services

Refactored the `Character` class to use more descriptive property names:

- Changed `N` to `Name`
- Changed `R` to `Race`
- Changed `W` to `Weapon`
- Changed `C` to `CurrentRegion`

Modified `FellowshipOfTheRingService` to:

- Use the new property names.
- Add validation methods for character properties.
- Ensure unique character names when adding members.
- Simplify the `MoveMembersToRegion` method.
- Add a method to print members in a specific region.
- Add a method to update a character's weapon.
- Refactor the `ToString` method to reflect the new property names.

### Step 4

Red Test:

Here What I asked to Gpt4-0

``` 
Instead to throw exception I want to use resultpattern.and move logic inside Character Entitiy.Use TDD.With A red test then a green tests
``` 



