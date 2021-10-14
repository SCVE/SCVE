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
For now the app will display colored rectangle

![image](https://user-images.githubusercontent.com/44116740/137251769-ec6802f0-4337-4f71-894f-0a3f544b907c.png)

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

In Progress: 
- Basic Renderer

## Possible backends
- Silk.Net
- glfw-net

## Investigations

Viewport is a rectangle in which OpenGL will render objects. 

VAO (Vertex Array) is an object in OpenGL which contains all the necessary data to be rendered. 
VBO (Vertex Buffer) is just an array in OpenGL.