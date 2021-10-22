#version 330 core
			
layout(location = 0) out vec4 color;

in vec2 v_TextureCoordinate;

uniform sampler2D texture0;

uniform vec2 u_Color;

void main()
{
    color = texture(texture0, v_TextureCoordinate);
}