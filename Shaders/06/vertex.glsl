#version 330

in  vec3 vPosition;
in  vec2 aTexCoord;

out vec2 TexCoord;
uniform mat4 modelview;

void
main()
{
    gl_Position = modelview * vec4(vPosition, 1.0);
	TexCoord = vec2(aTexCoord.x, aTexCoord.y);
}