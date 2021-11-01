# SCVE
Super Cool Video Editor

Written in C#.

Inspired by [Hazel](https://github.com/TheCherno/Hazel).

Based on: 
- [OpenTK](https://github.com/opentk/opentk/), 
- [ImageSharp](https://github.com/SixLabors/ImageSharp/), 
- [SharpFont](https://github.com/SpacialCircumstances/SharpFont) 
- UI Layout is based on [WPF's](https://github.com/dotnet/wpf) layout system. No sources, but the [idea](https://habr.com/ru/post/135990/) explained on _habr.com_.
- likely many more.

## Solution structure
- SCVE.Core contains all core abstractions, needed for the app.
- SCVE.OpenTKBindings contains implementations via OpenTK (in future it is possible to use other backends)
- Playground actually is a running app.

## How it works
The app uses OpenTK and GLFW for window and OpenGL manipulation.

- At first we need to create an ApplicationInit object, which holds all necessary info about how the app should be performing.
- Then we call Init() which actually initializes everything that wants to be initialized
- the application.Run() will spin main thread while termination is not requested.

## Current state
For now the app will display layouted rectangles

![image](https://user-images.githubusercontent.com/44116740/139516757-28043c3b-250e-4f29-9a83-152bb5303f9e.png)
Done: 
- Window handling
- Application scope
- Delta Time
- Rendering Context
- FPS Counter
- ArrayBuffer
- VertexBuffer
- IndexBuffer
- Shader
- Program
- Matrix
- Basic Renderer
- More Uniforms in program-
- MVP matrices to create a model-view space
- Shader Program Caching
- Vertical and Horizontal Layouts
- Text Rendering via font atlas
- Font via atlas
- UI from file
- Font caching
- UI Layout System (from scratch, based on WPF layout system)

In Progress:

## TODO

## Possible backends
- Silk.Net
- glfw-net
- SFML.NET

## Investigations

Viewport is a rectangle in which OpenGL will render objects. 

VAO (Vertex Array) is an object in OpenGL which contains all the necessary data to be rendered. 
VBO (Vertex Buffer) is just an array in OpenGL.

### Matrices

- Model Space defines object relative to the object center.

Model Matrix.
This is a `translation * rotation * scale`
This matrix translates the model into World Space

- World Space defines object relative to world center.

View Matrix.
This is a camera matrix
This matrix translates the model into Camera Space.

- Camera Space defines object relative to camera.

Projection Matrix.

This is a homogenous transformer (mapper to -1 to 1 space)  (ortho or perspective)

This matrix translates the model into Screen Space.

- Screen Space defines object relative to screen.

MVP stands for Model-View-Projection.

This is a matrix that applies to every vertex to be displayed.

Matrix multiplication goes in inverted order, so MVP is combined like this:
`MVP = projection * view * model`.

So each vertex(vec3) must be processed like this:
`gl_Position = MVP * vertex`

When the model updates are seldom, we can extract the model matrix from MVP,
so that we use only View-Projection as a constant, and supply vertex already multiplied by Model Matrix.
But currently I don't do this. It requires more human-hours to understand.
