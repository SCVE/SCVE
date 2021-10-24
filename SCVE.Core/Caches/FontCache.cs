using System.Collections.Generic;
using SCVE.Core.App;
using SCVE.Core.Texts;

namespace SCVE.Core.Caches
{
    public class FontCache
    {
        /// <summary>
        /// Name - Font dictionary
        /// </summary>
        private Dictionary<string, Dictionary<float, ScveFont>> _cachedFonts;

        public FontCache()
        {
            _cachedFonts = new Dictionary<string, Dictionary<float, ScveFont>>();
        }

        public ScveFont GetOrCache(string fontFileName, float size)
        {
            if (!_cachedFonts.ContainsKey(fontFileName))
            {
                // The cache has no loaded fonts of this name and size
                // So we add an empty dictionary here
                _cachedFonts.Add(fontFileName, new Dictionary<float, ScveFont>());
            }

            var cachedFontsOfName = _cachedFonts[fontFileName];
            if (!cachedFontsOfName.ContainsKey(size))
            {
                var fontLoadData = Application.Instance.FileLoaders.Font.Load(fontFileName, size);

                cachedFontsOfName[size] = new ScveFont(fontLoadData, size);
            }

            return cachedFontsOfName[size];
        }
    }
}