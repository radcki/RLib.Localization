using System;
using System.Globalization;
using FluentAssertions;
using RLib.Localization.Attributes;
using Xunit;

namespace RLib.Localization.Tests
{
    public class LocalizationTests
    {
        [Fact]
        public void ShouldGetLocalizedString_ForSpecifiedCulture()
        {
            // Arrange
            var culture = new CultureInfo(KnownCulture.Polish);

            // Act
            var translated = Localization.For(() => Resources.Yes, culture);

            // Assert
            translated.Should().Be("Tak");
        }

        [Fact]
        public void ShouldGetLocalizedString_ForCurrentCulture()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo(KnownCulture.German);

            // Act
            var translated = Localization.For(() => Resources.Yes);

            // Assert
            translated.Should().Be("Ja");
        }

        [Fact]
        public void ShouldGetLocalizedString_ForEnumResource()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo(KnownCulture.Polish);

            // Act
            var translated = Localization.For(() => eEnumResource.NotFound);

            // Assert
            translated.Should().Be("Zasób nie zosta³ odnaleziony");
        }

        [Fact]
        public void ShouldGetBaseString_ForBaseResourceCulture()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo(KnownCulture.German);

            // Act
            var translated = Localization.For(() => Resources.Yes, new CultureInfo(KnownCulture.English));

            // Assert
            translated.Should().Be(Resources.Yes);
        }


        [Fact]
        public void ShouldGetFormattedString_ForMatchingParameter()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo(KnownCulture.Polish);

            // Act
            var translated = Localization.For(() => Resources.Hello, new {name = "Foo"});

            // Assert
            translated.Should().Be("Czeœæ Foo!");
        }


        [Fact]
        public void ShouldThrow_ForNotMatchingParameters()
        {
            // Arrange
            CultureInfo.CurrentCulture = new CultureInfo(KnownCulture.Polish);
            var action = new Action(() => Localization.For(() => Resources.Hello, new { lastName = "Foo" }));

            // Act

            // Assert
            action.Should().Throw<ArgumentException>();
        }
    }

    [BaseResourceCulture(KnownCulture.English)]
    public class Resources
    {
        [Localized(KnownCulture.Polish, "Tak")]
        [Localized(KnownCulture.German, "Ja")]
        public static string Yes = "Yes";


        [Localized(KnownCulture.Polish, "Czeœæ {name}!")]
        [Localized(KnownCulture.German, "Hallo {name}!")]
        public static string Hello = "Hello {name}!";
    }

    public enum eEnumResource
    {
        [Localized(KnownCulture.Polish, "Zasób nie zosta³ odnaleziony")]
        [Localized(KnownCulture.English, "Resource was not found")]
        NotFound = 404
    }
}
