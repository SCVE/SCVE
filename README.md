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
For now the app will display slowly pulsating white screen

Done: 
- Window handling
- Application scope
- Delta Time
- Rendering Context
- Basic Renderer

## Possible backends
- Silk.Net
- glfw-net

## Investigations

Viewport is a rectangle in which OpenGL will render objects. 