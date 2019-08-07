using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;

namespace HJEngine.gfx
{
    class Graphics
    {
        public Shader shader;
        public prim.Size size;
        public gfx.ShaderFactory shaders;


        private Matrix4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            Matrix4 result = Matrix4.Identity;
            result[0, 0] = 2f / (right - left);
            result[1, 1] = 2f / (top - bottom);
            result[2, 2] = -2f / (zFar - zNear);
            result[0, 3] = -(right + left) / (right - left);
            result[1, 3] = -(top + bottom) / (top - bottom);
            result[2, 3] = -(zFar + zNear) / (zFar - zNear);
            return result;
        }

        public Graphics(prim.Size size)
        {
            shaders = new gfx.ShaderFactory();

            Matrix4 model = Matrix4.Identity;
            Matrix4 proj = Ortho(0f, 1f, 0f, 1f, 0f, 1f);

            shaders.GetShader("triangle").SetMatrix4("projection", proj);
            shaders.GetShader("triangle").SetMatrix4("model", model);

            shaders.GetShader("texture").SetMatrix4("projection", proj);
            shaders.GetShader("texture").SetMatrix4("model", model);

            this.size = size;
        }
    }
}
