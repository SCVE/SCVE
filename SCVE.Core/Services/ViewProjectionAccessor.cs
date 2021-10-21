using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Utilities;

namespace SCVE.Core.Services
{
    public class ViewProjectionAccessor
    {
        public ScveMatrix4X4 ViewMatrix { get; private set; } = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ProjectionMatrix { get; private set; } = ScveMatrix4X4.Identity;
        public ScveMatrix4X4 ViewProjectionMatrix { get; private set; } = ScveMatrix4X4.Identity;

        public ViewProjectionAccessor()
        {
            Application.Instance.Input.WindowSizeChanged += InputOnWindowSizeChanged;
        }

        public void SetFromWindow()
        {
            SetProjection(
                ScveMatrix4X4.CreateOrthographicOffCenter(
                    0,
                    Application.Instance.MainWindow.Width,
                    Application.Instance.MainWindow.Height,
                    0,
                    -1,
                    1
                )
            );
        }

        private void InputOnWindowSizeChanged(int width, int height)
        {
            Logger.Warn($"Window Size Changed: {width}:{height}");

            // SetFromWindow();

            // SetProjection(ScveMatrix4X4.CreateOrthographicOffCenter(-width / 2f, width / 2f, -height / 2f, height / 2f, -1, 1));
            SetProjection(ScveMatrix4X4.CreateOrthographicOffCenter(
                    0,
                    width,
                    height,
                    0,
                    -1,
                    1
                )
            );
        }

        public void SetProjection(ScveMatrix4X4 projection)
        {
            // Logger.Warn($"Setting projection to \n{projection}");
            ProjectionMatrix = projection;

            ViewProjectionMatrix.MakeIdentity().Multiply(projection).Multiply(ViewMatrix);
        }

        public void SetView(ScveMatrix4X4 view)
        {
            ViewMatrix = view;

            ViewProjectionMatrix.MakeIdentity().Multiply(ProjectionMatrix).Multiply(view);
        }
    }
}