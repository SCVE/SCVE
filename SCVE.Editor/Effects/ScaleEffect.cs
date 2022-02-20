using System;
using ImGuiNET;
using SCVE.Editor.Editing;
using SCVE.Editor.Imaging;
using SCVE.Editor.Modules;

namespace SCVE.Editor.Effects
{
    public class ScaleEffect : IEffect
    {
        public event Action Updated;
        public float X { get; set; } = 1;

        public float Y { get; set; } = 1;

        private Clip _clip;

        public ScaleEffect()
        {
        }

        public void AttachToClip(Clip clip)
        {
            _clip = clip;
        }

        public void DeAttachFromClip()
        {
            _clip = null;
        }

        public IImage Apply(EffectApplicationContext effectApplicationContext)
        {
            var srcImageFrame = effectApplicationContext.SourceImageFrame;
            var dstSizeX      = (int)(srcImageFrame.Width * X);
            var dstSizeY      = (int)(srcImageFrame.Height * Y);

            bool isDownscalingX = X < 1;
            bool isDownscalingY = Y < 1;

            var rawBytes = srcImageFrame.ToByteArray();

            if (dstSizeX != srcImageFrame.Width)
            {
                if (isDownscalingX)
                {
                    // going from top left corner
                    for (int i = 0; i < dstSizeX; i++)
                    {
                        for (int j = 0; j < srcImageFrame.Height; j++)
                        {
                            int srcPixelX = (int)(((float)i / dstSizeX) * srcImageFrame.Width);
                            int srcPixelY = j;

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * srcImageFrame.Width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * srcImageFrame.Width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // clear pixels, that are out of bounds
                    for (int i = dstSizeX; i < srcImageFrame.Width; i++)
                    {
                        for (int j = 0; j < srcImageFrame.Height; j++)
                        {
                            int srcPixelOffset = (j * srcImageFrame.Width + i) * 4;

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
                    for (int i = srcImageFrame.Width - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < srcImageFrame.Height; j++)
                        {
                            int srcPixelX = (int)((float)i / dstSizeX * srcImageFrame.Width);
                            int srcPixelY = j;

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * srcImageFrame.Width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * srcImageFrame.Width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // in this case we don't eliminate any pixels, because all of them are overlayed
                }
            }

            if (dstSizeY != srcImageFrame.Height)
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
                            int srcPixelY = (int)((float)j / dstSizeY * srcImageFrame.Height);

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * srcImageFrame.Width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * srcImageFrame.Width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // clear pixels, that are out of bounds
                    for (int j = dstSizeY; j < srcImageFrame.Height; j++)
                    {
                        for (int i = 0; i < srcImageFrame.Width; i++)
                        {
                            int srcPixelOffset = (j * srcImageFrame.Width + i) * 4;

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
                    for (int j = srcImageFrame.Height - 1; j >= 0; j--)
                    {
                        for (int i = 0; i < srcImageFrame.Width; i++)
                        {
                            int srcPixelX = i;
                            int srcPixelY = (int)((float)j / dstSizeY * srcImageFrame.Height);

                            int dstPixelX = i;
                            int dstPixelY = j;

                            int srcPixelOffset = (srcPixelY * srcImageFrame.Width + srcPixelX) * 4;
                            int dstPixelOffset = (dstPixelY * srcImageFrame.Width + dstPixelX) * 4;

                            rawBytes[dstPixelOffset + 0] = rawBytes[srcPixelOffset + 0];
                            rawBytes[dstPixelOffset + 1] = rawBytes[srcPixelOffset + 1];
                            rawBytes[dstPixelOffset + 2] = rawBytes[srcPixelOffset + 2];
                            rawBytes[dstPixelOffset + 3] = rawBytes[srcPixelOffset + 3];
                        }
                    }

                    // in this case we don't eliminate any pixels, because all of them are overlayed
                }
            }

            return srcImageFrame;
        }

        public void OnImGuiRender()
        {
            float x = X;
            if (ImGui.SliderFloat("X", ref x, 0, 5))
            {
                X = x;
                Updated?.Invoke();
            }

            float y = Y;
            if (ImGui.SliderFloat("Y", ref y, 0, 5))
            {
                Y = y;
                Updated?.Invoke();
            }
        }
    }
}