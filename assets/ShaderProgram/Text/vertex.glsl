#version 330 core

layout(location = 0) in vec3 a_Position;
layout(location = 1) in vec2 a_TextureCoordinate;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Proj;

out vec3 v_Position;
out vec2 v_TextureCoordinate;

void main()
{
    v_Position = a_Position;
    v_TextureCoordinate = a_TextureCoordinate;
    gl_Position = u_Proj * u_View * u_Model * vec4(a_Position, 1.0);	
}