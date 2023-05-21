using OpenGL;
using Tao.FreeGlut;

namespace OpenGl_Test_1
{
    class Program
    {
        private static int width = 1200, height = 720;
        private static ShaderAsset vertexShader = new ShaderAsset("Resources/VertexShader.shader");
        private static ShaderAsset fragmentShader = new ShaderAsset("Resources/FragmentShader.shader");
        private static ShaderProgram program;
        private static VBO<Vector3> triangleVerts;
        private static VBO<Vector3> triangleColors;
        private static VBO<int> triangleElements;
        private static Matrix4 triagleModelMatrix = Matrix4.CreateTranslation(new Vector3(-1.5f, 0, 0));
        private static float degToRad = 3.14f / 180f;

        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("OpenGl Test");
            Glut.glutCloseFunc(OnClose);

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);

            LoadShaders();

            program = new ShaderProgram(vertexShader, fragmentShader);
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));

            LoadMeshes();

            UpdateLoop(0.016f);

            Glut.glutMainLoop();
        }

        private async static void UpdateLoop(float deltaTime)
        {
            int dtInMs = (int)(deltaTime * 1000);
            while (true)
            {
                await Task.Delay(dtInMs);
                triagleModelMatrix = Matrix4.CreateRotationY(10 * deltaTime * degToRad) * triagleModelMatrix;
            }
        }
        
        private static void LoadMeshes()
        {
            triangleVerts = new VBO<Vector3>(new Vector3[] { new Vector3(0, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0) });
            triangleElements = new VBO<int>(new int[] { 0, 1, 2 }, BufferTarget.ElementArrayBuffer);
            float dark = 0.3f;
            triangleColors = new VBO<Vector3>(new Vector3[] { new Vector3(1, dark, dark), new Vector3(dark, 1, dark), new Vector3(dark, dark, 1) });
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

            program["model_matrix"].SetValue(triagleModelMatrix);

            uint vertexPositionIndex = (uint)Gl.GetAttribLocation(program.ProgramID, "vertexPosition");
            Gl.EnableVertexAttribArray(vertexPositionIndex);
            Gl.BindBuffer(triangleVerts);
            Gl.VertexAttribPointer(vertexPositionIndex, triangleVerts.Size, triangleVerts.PointerType, true, sizeof(float) * 3, IntPtr.Zero);
            Gl.BindBufferToShaderAttribute(triangleColors, program, "vertexColor");
            Gl.BindBuffer(triangleElements);

            Gl.DrawElements(BeginMode.Triangles, triangleElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);

            Glut.glutSwapBuffers();
        }

        private static void OnClose()
        {
            triangleVerts.Dispose();
            triangleElements.Dispose();
            triangleColors.Dispose();
            program.DisposeChildren = true;
            program.Dispose();
        }
    }
}