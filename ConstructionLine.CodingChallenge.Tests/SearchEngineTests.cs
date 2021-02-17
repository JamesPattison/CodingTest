using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void WhenWeHaveNoShirts_ThenWeShouldExpectNoResults()
        {
            var shirts = new List<Shirt>();

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions();

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts.Count == 0);

            foreach (var sizeCount in results.SizeCounts)
            {
                Assert.That(sizeCount.Count == 0);
            }

            foreach (var colorCount in results.ColorCounts)
            {
                Assert.That(colorCount.Count == 0);
            }
        }

        [Test]
        public void WhenPassingNullSearch_ShouldNotThrow()
        {
            var shirts = new List<Shirt>();

            var searchEngine = new SearchEngine(shirts);

            Assert.DoesNotThrow(() => searchEngine.Search(null));
        }

        [Test]
        public void WhenPassingNullShirts_ShouldNotThrow()
        {
            var searchEngine = new SearchEngine(null);

            Assert.DoesNotThrow(() => searchEngine.Search(null));
        }

        [Test]
        public void WhenRunningMoreThanOneSearch_FirstSearchShouldNotImpactSecond()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.Red },
                Sizes = new List<Size> { Size.Small }
            };

            var results = searchEngine.Search(searchOptions);

            Assert.That(results.Shirts.Count == 1);

            searchOptions = new SearchOptions();

            var secondResults = searchEngine.Search(searchOptions);

            Assert.That(secondResults.Shirts.Count == 3);
        }
    }
}
