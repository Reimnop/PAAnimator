#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

out vec3 color;

layout (std430, binding = 0) buffer MatsBuffer {
	mat4 mvps[];
};

void main() {
	gl_Position = vec4(pos, 1.0) * mvps[gl_InstanceID] * mvp;
}