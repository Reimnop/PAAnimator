#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

layout (std430, binding = 0) buffer NodesBuffer {
	mat4 transforms[];
};

void main() {
	gl_Position = vec4(pos, 1.0) * transforms[gl_InstanceID] * mvp;
}