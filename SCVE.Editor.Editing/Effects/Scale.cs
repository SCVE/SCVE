using SCVE.Editor.Editing.Visitors;

namespace SCVE.Editor.Editing.Effects
{
    public class Scale : EffectBase
    {
        public float X { get; set; } = 1;

        public float Y { get; set; } = 1;

        public Scale()
        {
        }

        protected override void Algorithm(byte[] pixels, int width, int height)
        {
            var dstSizeX = (int) (width * X);
            var dstSizeY = (int) (height * Y);

            bool isDownscalingX = X < 1;
            bool isDownscalingY = Y < 1;

            var rawBytes = pixels;

            if (dstSizeX != width)
            {
                if (isDownscalingX)
                {
                    // going from top left corner
                    for (int i = 0; i < dstSizeX; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            int srcPixelX = (int) (((float) i / dstSizeX) * width);
                            int srcPixelY = j;

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // clear pixels, that are out of bounds
                    for (int i = dstSizeX; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            int srcPixelOffset = (j * width + i) * 4;

                            rawBytes[srcPixelOffset + 0] = 0;
                            rawBytes[srcPixelOffset + 1] = 0;
                            rawBytes[srcPixelOffset + 2] = 0;
                            rawBytes[srcPixelOffset + 3] = 0;
                        }
                    }
                }
                else // upscaling X
                {
                    // going from top right corner
                    // for (int i = dstSizeX - 1; i >= 0; i--)
                    for (int i = width - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            int srcPixelX = (int) ((float) i / dstSizeX * width);
                            int srcPixelY = j;

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // in this case we don't eliminate any pixels, because all of them are overlayed
                }
            }

            if (dstSizeY != height)
            {
                // after resizing X, we go for Y
                if (isDownscalingY)
                {
                    // going from top left corner
                    for (int j = 0; j < dstSizeY; j++)
                    {
                        // here we use dstSizeX because we already changed image X size
                        for (int i = 0; i < dstSizeX; i++)
                        {
                            int srcPixelX = i;
                            int srcPixelY = (int) ((float) j / dstSizeY * height);

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // clear pixels, that are out of bounds
                    for (int j = dstSizeY; j < height; j++)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            int srcPixelOffset = (j * width + i) * 4;

                            rawBytes[srcPixelOffset + 0] = 0;
                            rawBytes[srcPixelOffset + 1] = 0;
                            rawBytes[srcPixelOffset + 2] = 0;
                            rawBytes[srcPixelOffset + 3] = 0;
                        }
                    }
                }
                else // upscaling Y
                {
                    // going from bottom left corner
                    for (int j = height - 1; j >= 0; j--)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            int srcPixelX = i;
                            int srcPixelY = (int) ((float) j / dstSizeY * height);

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // in this case we don't eliminate any pixels, because all of them are overlayed
                }
            }
        }

        public override void AcceptVisitor(IEffectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}