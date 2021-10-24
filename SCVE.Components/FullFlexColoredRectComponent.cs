using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Primitives;
using SCVE.Core.Rendering;
using SCVE.Core.Utilities;

namespace SCVE.Components
{
    public class FullFlexColoredRectComponent : Component
    {
        private readonly VertexArray _vertexArray;
        private readonly ShaderProgram _shaderProgram;
        private ColorRgba _colorRgba;

        public FullFlexColoredRectComponent(ColorRgba colorRgba)
        {
            Logger.Construct(nameof(FullFlexColoredRectComponent));
            _colorRgba = colorRgba;
            
            _vertexArray = Application.Instance.Cache.VertexArray.Get("Positive Unit");

            _shaderProgram = Application.Instance.Cache.ShaderProgram.LoadOrCache("FlatColor_MVP_Uniform");
        }

        public override void Render(IRenderer renderer)
        {
            _shaderProgram.SetVector4("u_Color", _colorRgba.R, _colorRgba.G, _colorRgba.B, _colorRgba.A);
            
            _shaderProgram.SetMatrix4("u_Model",
                ModelMatrix
            );
            _shaderProgram.SetMatrix4("u_View",
                Application.Instance.ViewProjectionAccessor.ViewMatrix
            );
            _shaderProgram.SetMatrix4("u_Proj",
                Application.Instance.ViewProjectionAccessor.ProjectionMatrix
            );
            _shaderProgram.Bind();

            renderer.RenderSolid(_vertexArray);

            for (var i = 0; i < Children.Count; i++)
            {
                Children[i].Render(renderer);
            }
        }
    }
}