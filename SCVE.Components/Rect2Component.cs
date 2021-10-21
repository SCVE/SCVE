using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class Rect2Component : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly Program _program;
        private ColorRgba _colorRgba;

        public Rect2Component(ColorRgba colorRgba)
        {
            Logger.Construct(nameof(Rect2Component));
            _colorRgba = colorRgba;
            
            _vertexArray = Application.Instance.VertexArrayCache.Get("Positive Unit");

            _program = Application.Instance.ShaderProgramCache.LoadOrCache("FlatColor_MVP_Uniform");
        }

        public override void Render(IRenderer renderer)
        {
            _program.SetVector4("u_Color", _colorRgba.R, _colorRgba.G, _colorRgba.B, _colorRgba.A);
            
            _program.SetMatrix4("u_Model",
                ModelMatrix
            );
            _program.SetMatrix4("u_View",
                Application.Instance.ViewProjectionAccessor.ViewMatrix
            );
            _program.SetMatrix4("u_Proj",
                Application.Instance.ViewProjectionAccessor.ProjectionMatrix
            );
            _program.Bind();

            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}