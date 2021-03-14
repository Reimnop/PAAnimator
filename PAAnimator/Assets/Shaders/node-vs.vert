#version 430 core

layout (location = 0) in vec3 pos;

uniform mat4 mvp;

out vec3 color;

struct NodeData {
	mat4 transform;
	bool highlighted;
};

layout (std430, binding = 0) buffer NodesBuffer {
	NodeData data[];
};

void main() {
	NodeData data = data[gl_InstanceID];

	gl_Position = vec4(pos, 1.0) * data.transform * mvp;

	vec3 mulCol = vec3(1.0);
	if (data.highlighted == true)
		mulCol = vec3(0.4, 0.4, 0.4);

	color = vec3(1.0);
	if (gl_InstanceID == 0)
		color = vec3(0.4, 1.0, 1.0);

	color *= mulCol;
}