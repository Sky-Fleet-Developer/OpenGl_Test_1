in vec3 color;

void main(void)
{
	if(!gl_FrontFacing) discard;
	gl_FragColor = vec4(color, 1);
}