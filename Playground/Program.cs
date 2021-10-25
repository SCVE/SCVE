using System;
using SCVE.Components;
using SCVE.Core;
using SCVE.Core.App;
using SCVE.Core.Caches;
using SCVE.Core.Entities;
using SCVE.Core.Rendering;
using SCVE.ImageSharpBindings;
using SCVE.Null;
using SCVE.OpenTKBindings;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var applicationInit = new ApplicationInitNull();
            applicationInit.Renderer = new OpenGLRenderer();
            applicationInit.DeltaTimeProvider = new GlfwDeltaTimeProvider();
            applicationInit.Window = new GlfwWindow(new WindowProps("Super Cool Video Editor"));
            applicationInit.Input = new GlfwInput();
            applicationInit.RenderEntitiesCreator = new OpenGLRenderEntitiesCreator();
            applicationInit.FileLoaders = new PlaygroundFileLoaders();
            applicationInit.FontAtlasGenerator = new SharpFontImageSharpFontAtlasGenerator();

            var application = Application.Init(applicationInit);

            application.ViewProjectionAccessor.SetFromWindow();

            var positiveUnitVertexArray = CreatePositiveUnitVertexArray(application);
            var vertexCacheInitiator = new VertexCacheInitiator()
                .With("Positive Unit", positiveUnitVertexArray);
            vertexCacheInitiator.Init(application.Cache.VertexArray);

            // application.ViewProjectionAccessor.SetView(ScveMatrix4X4.Identity.Set(2, 3, -1));

            var rootComponent = new TextViaAtlasComponent();
            application.RootComponent = rootComponent;

            // var verticalLayoutEvenSpaceComponent = new VerticalLayoutEvenSpaceComponent();
            // application.RootComponent = verticalLayoutEvenSpaceComponent;
            //
            // application.RootComponent.SetPositionAndSize(0, 0, application.MainWindow.Width, application.MainWindow.Height);
            //
            // var horizontalLayout = new HorizontalLayoutEvenSpaceComponent();
            // application.RootComponent.AddChild(horizontalLayout);
            // horizontalLayout.AddChild(new FullFlexColoredRectComponent(new ColorRgba(1, 0, 0, 1)));
            // horizontalLayout.AddChild(new FullFlexColoredRectComponent(new ColorRgba(0, 0.5f, 0, 1)));
            // horizontalLayout.AddChild(new FullFlexColoredRectComponent(new ColorRgba(0, 0, 1, 1)));
            // application.RootComponent.AddChild(new FullFlexColoredRectComponent(new ColorRgba(0, 1, 0, 1)));
            // application.RootComponent.AddChild(new FullFlexColoredRectComponent(new ColorRgba(0, 0, 1, 1)));

            application.Run();

            Console.WriteLine("Exiting");
            // Profiler.Invokations.Print();
        }

        private static VertexArray CreatePositiveUnitVertexArray(Application application)
        {
            VertexArray vertexArray = application.RenderEntitiesCreator.CreateVertexArray();
            var rectGeometry = GeometryGenerator.GeneratePositiveUnitSquare();

            var buffer = Application.Instance.RenderEntitiesCreator.CreateVertexBuffer(rectGeometry.Vertices);

            buffer.Layout = new VertexBufferLayout(new()
            {
                new(VertexBufferElementType.Float3, "a_Position")
            });
            vertexArray.AddVertexBuffer(buffer);

            var indexBuffer = Application.Instance.RenderEntitiesCreator.CreateIndexBuffer(rectGeometry.Indices);

            vertexArray.SetIndexBuffer(indexBuffer);
            return vertexArray;
        }
    }
}