using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace HJEngine.gfx
{
    class Shader
    {
        public int vertexShader;
        public int fragmentShader;
        public int handle;
        private Dictionary<string, int> uniforms;

        public Shader(string name)
        {
            string vertexShaderSrc;
            string fragmentShaderSrc;
            uniforms = new Dictionary<string, int>();
            using (StreamReader reader = new StreamReader("shaders/" + name + ".glvs", Encoding.UTF8))
            {
                vertexShaderSrc = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader("shaders/" + name + ".glfs", Encoding.UTF8))
            {
                fragmentShaderSrc = reader.ReadToEnd();
            }

            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSrc);
            CompileShader(vertexShader);

            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSrc);
            CompileShader(fragmentShader);

            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);
            GL.LinkProgram(handle);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);


            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numUniforms);
            for ( var i = 0; i < numUniforms; i++)
            {
                var key  = GL.GetActiveUniform(handle, i, out _, out _);
                var loc = GL.GetUniformLocation(handle, key);
                uniforms.Add(key, loc);
            }

        }

        private int CompileShader(int shader)
        {
            GL.CompileShader(shader);
            string log = GL.GetShaderInfoLog(shader);
            if (log != "")
            {
                System.Console.WriteLine(log);
                return 0;
            }
            return 1;
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(handle);
            if (uniforms.ContainsKey(name))
            {
                GL.UniformMatrix4(uniforms[name], true, ref data);
            }
        }

        public void Run()
        {
            GL.UseProgram(handle);
        }

        ~Shader()
        {
            //GL.DeleteProgram(handle);
        }
    }
}
