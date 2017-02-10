using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Xunit;

using ConfigRenamer.Model;
using ConfigRenamer.Services;


namespace ConfigRenamer.Tests.Services
{
    [TestClass]
    public class ConfigRenamerServiceTests
    {
        [Fact]
        public void ParseSearchProvider_NoProviderGiven_ResultsInAll()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var options = new Options();

            var service = new ConfigRenamerService(options);

            var stringToParse = "this contains no provider";

            var expected = SearchProvider.All;
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var actual = service.ParseSearchProvider(stringToParse);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            actual.ShouldBeEquivalentTo(expected, "no search provider name is in the string");
        }

        [Fact]
        public void ParseSearchProvider_LuceneProviderGivenLowerCase_ResultsInLucene()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var options = new Options();

            var service = new ConfigRenamerService(options);

            var stringToParse = "this contains lucene";

            var expected = SearchProvider.Lucene;
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var actual = service.ParseSearchProvider(stringToParse);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            actual.ShouldBeEquivalentTo(expected, "lucene should be chosen when lucene is in the string");
        }

        [Fact]
        public void ParseSearchProvider_LuceneProviderGivenCapitalsCase_ResultsInLucene()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var options = new Options();

            var service = new ConfigRenamerService(options);

            var stringToParse = "this contains LUCENE";

            var expected = SearchProvider.Lucene;
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var actual = service.ParseSearchProvider(stringToParse);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            actual.ShouldBeEquivalentTo(expected, "lucene should be chosen when lucene is in the string");
        }
    }
}
