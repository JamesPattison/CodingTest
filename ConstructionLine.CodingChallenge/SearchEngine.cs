using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly Dictionary<Color, IEnumerable<Shirt>> _groupedByColor;
        private readonly Dictionary<Size, IEnumerable<Shirt>> _groupedBySize;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _groupedByColor = new Dictionary<Color, IEnumerable<Shirt>>();
            _groupedBySize = new Dictionary<Size, IEnumerable<Shirt>>();

            if (_shirts == null)
                return;

            foreach (var s in _shirts.GroupBy(x => x.Color))
            {
                _groupedByColor[s.Key] = s.ToList();
            }
            foreach (var s in _shirts.GroupBy(x => x.Size))
            {
                _groupedBySize[s.Key] = s.ToList();
            }
        }

        public SearchResults Search(SearchOptions options)
        {
            if (options == null)
                return new SearchResults();

            IEnumerable<Shirt> shirts = new List<Shirt>(_shirts);
            if (options.Colors != null && options.Colors.Any() && _groupedByColor != null)
            {
                shirts = options.Colors.SelectMany(x => _groupedByColor[x]);
            }

            if (options.Sizes != null && options.Sizes.Any() && _groupedBySize != null)
            {
                shirts = shirts.Intersect(options.Sizes.SelectMany(x => _groupedBySize[x]));
            }

            var shirtList = shirts.ToList();

            return new SearchResults
            {
                ColorCounts = CalculateColorCounts(shirtList).ToList(),
                Shirts = shirtList,
                SizeCounts = CalculateSizeCounts(shirtList).ToList()
            };
        }

        private IEnumerable<ColorCount> CalculateColorCounts(IEnumerable<Shirt> shirts)
        {
            var grouped = shirts.GroupBy(x => x.Color);

            foreach (var color in Color.All)
            {
                var foundColors = grouped.FirstOrDefault(x => x.Key == color);
                var count = new ColorCount { Color = color, Count = foundColors?.Count() ?? 0 };
                yield return count;
            }
        }

        private IEnumerable<SizeCount> CalculateSizeCounts(IEnumerable<Shirt> shirts)
        {
            var grouped = shirts.GroupBy(x => x.Size);

            foreach (var size in Size.All)
            {
                var foundColors = grouped.FirstOrDefault(x => x.Key == size);
                var count = new SizeCount() { Size = size, Count = foundColors?.Count() ?? 0 };
                yield return count;
            }
        }
    }
}