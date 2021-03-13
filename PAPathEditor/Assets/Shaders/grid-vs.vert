#version 430 core

layout (location = 0) in vec2 pos;

uniform mat4 viewInverse;
uniform mat4 projInverse;

out vec2 FragPos;

void main() {
	FragPos = vec2(vec4(pos, 0.0, 1.0) * projInverse * viewInverse);
	gl_Position = vec4(pos, 0.0, 1.0);
}