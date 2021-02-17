using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class SearchResults
    {
        public List<Shirt> Shirts { get; set; }
        public List<SizeCount> SizeCounts { get; set; }
        public List<ColorCount> ColorCounts { get; set; }

        public SearchResults()
        {
            Shirts = new List<Shirt>();
            SizeCounts = new List<SizeCount>();
            ColorCounts = new List<ColorCount>();
        }
    }


    public class SizeCount
    {
        public Size Size { get; set; }

        public int Count { get; set; }
    }


    public class ColorCount
    {
        public Color Color { get; set; }

        public int Count { get; set; }
    }
}