#version 430 core

layout (location = 0) out vec4 fragColor;

in vec2 FragPos;

vec4 grid(vec2 pos, float width) {
	vec2 rounded = round(pos);
	vec2 a = pos - rounded;

	vec4 col = vec4(0.2, 0.2, 0.2, 0.75);

	if (abs(a.x) < width || abs(a.y) < width) {
		vec2 roundedMajor = round(pos * 0.1);
		vec2 aMajor = pos * 0.1 - roundedMajor;

		if (abs(aMajor.x) < width * 0.1 || abs(aMajor.y) < width * 0.1) {
			col.xyz = vec3(0.6);
		}

		if (rounded.y == 0.0 && abs(pos.y) < width)
			col.xyz = vec3(1.0, 0.2, 0.2);
		if (rounded.x == 0.0 && abs(pos.x) < width)
			col.xyz = vec3(0.2, 1.0, 0.2);

		return col;
	}

	return vec4(0.0);
}

void main() {
	fragColor = grid(FragPos, 0.075);
}