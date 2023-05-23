#version 150

uniform sampler2D texture;
uniform vec3 lightDirection;
uniform vec3 lightDirection2;
uniform mat4 view_matrix;
uniform float time;
const float PI = 3.14;

in VS_OUT
{
	vec4 pos;
	vec3 nrm;
	vec2 uv;
} fs_in;

out vec4 fragment;

float saturate(float value)
{
	return clamp(value, 0, 1);
}

float FresnelSchlick(float F0, float cosTheta) {
    return F0 + (1 - F0) * pow(1.0 - saturate(cosTheta), 5.0);
}

float cos(vec3 a, vec3 b)
{
	float dotProduct = dot(a, b);
	float lengthProduct = length(a) * length(b);
	return dotProduct / lengthProduct;
}

float angle(vec3 a, vec3 b) {
	return acos(cos(a, b));
}

float GGX_PartialGeometry(float cosThetaN, float alpha) {
    float cosTheta_sqr = saturate(cosThetaN*cosThetaN);
    float tan2 = ( 1 - cosTheta_sqr ) / cosTheta_sqr;
    float GP = 2 / ( 1 + sqrt( 1 + alpha * alpha * tan2 ) );
    return GP;
}

float GGX_Distribution(float cosThetaNH, float alpha) {
    float alpha2 = alpha * alpha;
    float NH_sqr = saturate(cosThetaNH * cosThetaNH);
    float den = NH_sqr * alpha2 + (1.0 - NH_sqr);
    return alpha2 / ( PI * den * den );
}

float lerp(float a, float b, float t)
{
	return (b - a) * t + a;
}

vec3 CookTorrance_GGX(vec3 color, vec3 N, vec3 V, vec3 L)
{
	vec3 H = normalize(V + L);
	float roughness = 0.4;
	float NV = dot(N,V);
	float NL = dot(N,L);
	float HV = dot(H,V);
	float roug_sqr = roughness*roughness;

	float G = 0;
	if(NL > 0)
	{
		G = GGX_PartialGeometry(NV, roug_sqr) * GGX_PartialGeometry(NL, roug_sqr);
	}
	float D = GGX_Distribution(dot(N,H), roug_sqr);
	float F = FresnelSchlick(0.1, HV);

	float specK = max(G*D*F*0.25/(NV+0.001), 0);    
    float diffK = max(saturate(1.0-F), 0);
    return color * (diffK*NL/PI) + vec3(specK,specK,specK);
}

void main(void)
{
	if(!gl_FrontFacing) discard;
	vec3 pos = fs_in.pos.xyz;
	vec3 vp = vec3(view_matrix[3]);
	vec3 v = normalize(vp - pos);
	vec3 albedo = texture2D(texture, fs_in.uv).xyz;
	fragment.xyz = CookTorrance_GGX(albedo, normalize(fs_in.nrm), v, -lightDirection) * 3;
	fragment.xyz += CookTorrance_GGX(albedo, normalize(fs_in.nrm), v, -lightDirection2) * 2;
	fragment.w = 1 + time * 0.0001;
}