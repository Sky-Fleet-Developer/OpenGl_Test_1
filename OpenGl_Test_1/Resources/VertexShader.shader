﻿in vec3 vertexPosition;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
	gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}