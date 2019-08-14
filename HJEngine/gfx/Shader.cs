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

    class ShaderFactory
    {
        private Dictionary<string, Shader> shaders;

        public ShaderFactory()
        {
            shaders = new Dictionary<string, Shader>();
            shaders.Add("texture", new gfx.Shader("texture"));
            shaders.Add("triangle", new gfx.Shader("triangle"));
        }

        public Shader GetShader(string name)
        {
            if (shaders.ContainsKey(name))
                return shaders[name];
            else
                return null;
        }

        public void Use(string name)
        {
            shaders[name].Use();
        }
    }

    class Shader
    {
        public int vertexShader;
        public int fragmentShader;
        public int handle;
        public string name;
        private Dictionary<string, int> uniforms;
        private Dictionary<string, int> attributes;

        public Shader(string name)
        {
            string vertexShaderSrc;
            string fragmentShaderSrc;
            uniforms = new Dictionary<string, int>();
            attributes = new Dictionary<string, int>();
            this.name = name;
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
            GL.GetProgram(handle, GetProgramParameterName.ActiveAttributes, out var numAttributes);

            for ( var i = 0; i < numAttributes; i++)
            {
                var key = GL.GetActiveAttrib(handle, i, out _, out _);
                var loc = GL.GetAttribLocation(handle, key);
                attributes.Add(key, loc);
            }

            for ( var i = 0; i < numUniforms; i++)
            {
                var key  = GL.GetActiveUniform(handle, i, out _, out _);
                var loc = GL.GetUniformLocation(handle, key);
                uniforms.Add(key, loc);
            }
        }

        public int GetAttributeLocation(string name)
        {
            return GL.GetAttribLocation(handle, name);
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
            else
                Console.WriteLine(name + " does not exist");
        }

        public void SetFloat(string name, float value)
        {
            GL.UseProgram(handle);
            if (uniforms.ContainsKey(name))
            {
                GL.Uniform1(uniforms[name], value);
            }
            else
                Console.WriteLine(name + " does not exist");
        }

        public void SetVec2(string name, Vector2 data)
        {
            GL.UseProgram(handle);
            if (uniforms.ContainsKey(name))
            {
                GL.Uniform2(uniforms[name], ref data);
            }
            else
                Console.WriteLine(name + " does not exist");
        }

        public void SetVec4(string name, Vector4 data)
        {
            GL.UseProgram(handle);
            if (uniforms.ContainsKey(name))
            {
                GL.Uniform4(uniforms[name], ref data);
            }
            else
                Console.WriteLine(name + " does not exist");
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation(handle, name);
            GL.Uniform1(location, value);
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }
        
        public void Stop()
        {
            GL.UseProgram(0);
        }

        ~Shader()
        {
            //GL.DeleteProgram(handle);
        }
    }
}
