#version 330

out vec4 outputColor;

in vec2 TexCoord;
uniform sampler2D texture1;
uniform sampler2D texture2;

void
main()
{
    outputColor = mix(texture(texture1, TexCoord), texture(texture2, TexCoord), 0.2);;
}
