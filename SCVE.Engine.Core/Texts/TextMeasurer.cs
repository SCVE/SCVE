using SCVE.Engine.Core.Utilities;

namespace SCVE.Engine.Core.Texts
{
    public static class TextMeasurer
    {
        /// <summary>
        /// Method measures the provided string with font and it's size. Be aware of the allocation for every call.
        /// </summary>
        public static TextMeasurement MeasureText(ScveFont font, string text, float fontSize)
        {
            float x = 0;
            float y = 0;

            // Y never goes back, so we don't need yMax
            float xMax = 0;

            float destLineHeight = Maths.FontSizeToLineHeight(fontSize);
            float lineHeightRel = destLineHeight / font.LineHeight;

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    y += destLineHeight;
                    x = 0;
                    continue;
                }

                var chunk = font.Atlas.Chunks.ContainsKey(text[i]) ? font.Atlas.Chunks[(int)text[i]] : font.Atlas.Chunks[(int)'?'];

                x += chunk.Advance * lineHeightRel;
                if (x > xMax)
                {
                    xMax = x;
                }
            }

            return new TextMeasurement(xMax, y);
        }
    }
}