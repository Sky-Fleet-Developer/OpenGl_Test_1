#version 130

in vec3 vert_position;
in vec3 vert_normal;
in vec2 vert_uv;

out vec3 nrm;
out vec2 uv;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
	nrm = normalize((model_matrix * vec4(vert_normal, 0)).xyz);
	uv = vert_uv;
	gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vert_position, 1);
}