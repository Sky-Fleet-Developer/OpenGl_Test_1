#version 150

in vec3 vert_position;
in vec3 vert_normal;
in vec2 vert_uv;

out VS_OUT
{
	vec4 pos;
	vec3 nrm;
	vec2 uv;
} vs_out;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;
uniform vec3 lightDirection;
uniform vec2 time;

void main(void)
{
	vs_out.nrm = normalize((model_matrix * vec4(vert_normal, 0)).xyz);
	vs_out.uv = vert_uv;
	vs_out.pos = model_matrix * vec4(vert_position, 1);
	gl_Position = projection_matrix * view_matrix * vs_out.pos;
}