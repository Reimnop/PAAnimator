#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

out vec3 color;

layout (std430, binding = 0) buffer NodesBuffer {
	vec2 poses[];
};

layout (std430, binding = 1) buffer HighlightBuffer {
	uint highlights[];
};

void main() {
	gl_Position = vec4(vec3(pos.xy + poses[gl_InstanceID], pos.z), 1.0) * mvp;

	vec3 mulCol = vec3(1.0);
	if (highlights[gl_InstanceID] == 1)
		mulCol = vec3(0.4, 0.4, 0.4);

	color = vec3(1.0);
	if (gl_InstanceID == 0)
		color = vec3(0.4, 1.0, 1.0);

	color *= mulCol;
}