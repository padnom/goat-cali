﻿using Microsoft.AspNetCore.Mvc;
using Xunit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;

namespace Goat.Examples.Tests
{
    public class Guidelines
    {
        [Fact]
        public void ClassShouldNotDependOnAnother() =>
            Classes().That()
                .Are(typeof(SomeExample))
                .Should()
                .NotDependOnAny(typeof(Other))
                .Check();

        [Fact]
        public void AnnotatedClassesShouldResideInAGivenNamespace() =>
            Classes().That()
                .HaveAnyAttributes(typeof(ApiControllerAttribute))
                .Should()
                .ResideInNamespace("Controllers", true)
                .Check();

        [Fact]
        public void InterfacesShouldStartWithI() =>
            Interfaces().Should()
                .HaveName("^I[A-Z].*", useRegularExpressions: true)
                .Because("C# convention...")
                .Check();

        [Fact]
        public void ClassesInDomainCanOnlyAccessClassesInDomainItself() =>
            Classes().That()
                .ResideInNamespace("Domain", true)
                .Should()
                .OnlyDependOnTypesThat()
                .ResideInNamespace("Domain", true)
                .Check();
    }
}