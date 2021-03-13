#version 430 core

layout (location = 0) out vec4 fragColor;

in vec2 FragPos;

vec4 grid(vec2 pos, float width) {
	vec2 rounded = round(pos);
	vec2 a = pos - rounded;

	vec4 col = vec4(0.2, 0.2, 0.2, 0.8);

	if (abs(a.x) < width || abs(a.y) < width) {
		if (rounded.y == 0.0 && abs(pos.y) < width)
			col.x = 1.0;
		if (rounded.x == 0.0 && abs(pos.x) < width)
			col.y = 1.0;

		return col;
	}

	return vec4(0.0);
}

void main() {
	fragColor = grid(FragPos, 0.075);
}