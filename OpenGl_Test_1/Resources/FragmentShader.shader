#version 130

uniform sampler2D texture;
uniform vec3 light_direction;

in vec3 nrm;
in vec2 uv;

out vec4 fragment;

void main(void)
{
	if(!gl_FrontFacing) discard;
	float diffuse = max(dot(nrm, light_direction), 0);
	float ambient = 0.3;
	fragment = texture2D(texture, uv);
	fragment.xyz *= max(ambient, diffuse);
}