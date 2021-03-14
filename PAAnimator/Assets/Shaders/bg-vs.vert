#version 430 core

layout (location = 0) in vec2 pos;
layout (location = 1) in vec2 texCoord;

uniform mat4 mvp;

out vec2 TexCoord;

void main() {
	TexCoord = texCoord;
	gl_Position = vec4(pos, 0.0, 1.0) * mvp;
}