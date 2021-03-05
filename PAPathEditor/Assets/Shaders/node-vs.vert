#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

layout (std430, binding = 0) buffer Nodes {
	vec2 posArr[];
};

void main() {
	gl_Position = vec4(vec3(pos.xy + posArr[gl_InstanceID], pos.z), 1.0) * mvp;
}