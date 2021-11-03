using System.Collections.Generic;
using SCVE.Core.Main;
using SCVE.Core.Texts;
using SCVE.Core.Utilities;

namespace SCVE.Core.Caches
{
    public class FontCache
    {
        /// <summary>
        /// Name - Font dictionary
        /// </summary>
        private Dictionary<string, Dictionary<int, ScveFont>> _cachedFonts;

        public FontCache()
        {
            _cachedFonts = new Dictionary<string, Dictionary<int, ScveFont>>();
        }

        public ScveFont GetOrCache(string fontFileName, int fontSize)
        {
            if (!_cachedFonts.ContainsKey(fontFileName))
            {
                // The cache has no loaded fonts of this name and size
                // So we add an empty dictionary here
                _cachedFonts.Add(fontFileName, new Dictionary<int, ScveFont>());
            }

            var cachedFontsOfName = _cachedFonts[fontFileName];
            if (!cachedFontsOfName.ContainsKey(fontSize))
            {
                var lineHeight = Maths.FontSizeToLineHeight(fontSize);
                var fontLoadData = Engine.Instance.FileLoaders.Font.Load(
                    fontFileName,
                    lineHeight
                );

                cachedFontsOfName[fontSize] = new ScveFont(fontLoadData, lineHeight);
            }

            return cachedFontsOfName[fontSize];
        }
    }
}