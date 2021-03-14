#version 430 core

layout (location = 0) out vec4 fragColor;

in vec2 TexCoord;

uniform sampler2D texture0;

void main() {
	fragColor = texture(texture0, TexCoord);
}