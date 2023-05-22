in vec3 position;
in vec3 normal;

varying vec3 color;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
	color = normal;// * 0.5 + vec3(0.5, 0.5, 0.5);
	gl_Position = projection_matrix * view_matrix * model_matrix * vec4(position, 1);
}