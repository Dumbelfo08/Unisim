#version 330 core

layout (location = 0) in vec2 aPos;

out vec2 FragPos;

uniform mat4 view;
uniform mat4 projection;

uniform vec2 pos;
uniform float rad;

void main()
{
	FragPos = aPos;
	gl_Position = projection * view * vec4(pos + rad * aPos, 0.0, 1.0); //The position
}