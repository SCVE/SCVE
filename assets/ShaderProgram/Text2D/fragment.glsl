#version 330 core
			
layout(location = 0) out vec4 color;

in vec2 v_TextureCoordinate;

uniform sampler2D texture0;

uniform vec4 u_Color;

void main()
{
    color = u_Color * texture(texture0, v_TextureCoordinate);
    // color = vec4(1,1,1,1);
}