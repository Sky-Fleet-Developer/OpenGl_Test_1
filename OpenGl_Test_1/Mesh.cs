using OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenGl_Test_1
{
    public class Mesh
    {
        public string Name;
        public VBO<Vector3> vertices;
        public VBO<Vector3> normals;
        public VBO<Vector2> uvs;
        public VBO<int> triangles;
        public Mesh()
        {
        }

        public void SetVertices(Vector3[] source)
        {
            vertices = new VBO<Vector3>(source);
        }

        public void SetNormals(Vector3[] source)
        {
            normals = new VBO<Vector3>(source);
        }

        public void SetUvs(Vector2[] source)
        {
            uvs = new VBO<Vector2>(source);
        }

        public void SetTriangles(int[] source)
        {
            triangles = new VBO<int>(source, BufferTarget.ElementArrayBuffer);
        }

        public void DrawGl(ShaderProgram program)
        {

            Gl.DrawElements(BeginMode.Triangles, triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
            //Gl.DrawElements(BeginMode.Lines, triangles.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public void SetBuffers(ShaderProgram program)
        {
            Gl.BindBufferToShaderAttribute(vertices, program, "vert_position");
            Gl.BindBufferToShaderAttribute(normals, program, "vert_normal");
            Gl.BindBufferToShaderAttribute(uvs, program, "vert_uv");
            Gl.BindBuffer(triangles);
        }

        public void Dispose()
        {
            vertices.Dispose();
            normals.Dispose();
            uvs.Dispose();
            triangles.Dispose();
        }
    }
}
