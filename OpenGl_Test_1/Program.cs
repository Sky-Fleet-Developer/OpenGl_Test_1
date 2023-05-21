using OpenGL;
using OpenGLTest.Shaders;
using System;
using Tao.FreeGlut;

namespace OpenGLTest
{
    class Program
    {
        private static int width = 1200, height = 720;
        private static ShaderAsset vertexShader = new ShaderAsset("Resources/VertexShader.shader");
        private static ShaderAsset fragmentShader = new ShaderAsset("Resources/FragmentShader.shader");
        private static ShaderProgram program;
        private static VBO<Vector3> triangleVerts, squareVerts;
        private static VBO<int> triangleElements, squareElements;
        private static Matrix4 modelMatrix = Matrix4.CreateTranslation(new Vector3(-1.5f, 0, 0));

        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("OpenGl Test");

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);

            LoadShaders();

            program = new ShaderProgram(vertexShader, fragmentShader);
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));

            LoadMeshes();

            Glut.glutMainLoop(); 
        }
        
        private static void LoadMeshes()
        {
            triangleVerts = new VBO<Vector3>(new Vector3[] { new Vector3(0, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0) });
            squareVerts = new VBO<Vector3>(new Vector3[] { new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0) });
            triangleElements = new VBO<int>(new int[] { 0, 1, 2 }, BufferTarget.ElementArrayBuffer);
            squareElements = new VBO<int>(new int[] { 0, 1, 2, 3 }, BufferTarget.ElementArrayBuffer);
        }

        private static void LoadShaders()
        {
            vertexShader.Load();
            fragmentShader.Load();
        }

        private static void OnDisplay()
        {
        }

        private static void OnRenderFrame()
        {
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            program.Use();

            program["model_matrix"].SetValue(modelMatrix);

            uint vertexPositionIndex = (uint)Gl.GetAttribLocation(program.ProgramID, "vertexPosition");
            Gl.EnableVertexAttribArray(vertexPositionIndex);
            Gl.BindBuffer(triangleVerts);
            Gl.VertexAttribPointer(vertexPositionIndex, triangleVerts.Size, triangleVerts.PointerType, true, sizeof(float) * 3, IntPtr.Zero);
            Gl.BindBuffer(triangleElements);

            Gl.DrawElements(BeginMode.Triangles, triangleElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Glut.glutSwapBuffers();
        }
    }
}