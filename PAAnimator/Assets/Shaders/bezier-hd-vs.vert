#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

out vec3 color;

layout (std430, binding = 0) buffer NodesBuffer {
	mat4 transforms[];
};

void main() {
	gl_Position = vec4(pos, 1.0) * transforms[gl_InstanceID] * mvp;

	if (gl_InstanceID == 0)
		color = vec3(0.2, 0.8, 0.8);
	else
		color = vec3(0.6);
}