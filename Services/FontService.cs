using SixLabors.Fonts;
using System;

namespace veneer.Services
{
    public class FontService
    {
        public Font Font { get; private set; }
        public FontFamily FontFamily { get; private set; }

        public FontService(string fontPath, int fontSize)
        {
            var collection = new FontCollection();
            collection.Add(fontPath);
            if (!collection.TryGet("OpenSansEmoji", out FontFamily family))
            {
                throw new InvalidOperationException("Font families could not be loaded.");
            }

            Font = family.CreateFont(fontSize);
        }
    }
}
