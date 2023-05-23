using OpenGL;
using OpenGl_Test_1.Serialization;
using System.Globalization;
using System.Numerics;
using Tao.FreeGlut;

namespace OpenGl_Test_1
{
    class Program
    {
        private static int width = 1200, height = 720;
        private static ShaderAsset vertexShader = new ShaderAsset("Resources/VertexShader.shader");
        private static ShaderAsset fragmentShader = new ShaderAsset("Resources/FragmentShader.shader");
        private static string meshPath = "Resources/Cube.obj";
        private static string texturePath = "Resources/crate.jpg";
        private static ShaderProgram program;
        private static Mesh mesh;
        private static Texture texture;
        private static Matrix4 triagleModelMatrix = Matrix4.CreateTranslation(new Vector3(0, -0.5f, 0));
        private static float degToRad = 3.14f / 180f;
        private static double startTime;
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("OpenGl Test");
            Glut.glutCloseFunc(OnClose);

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(OnDisplay);

            LoadShaders();

            Shader vertShader = new Shader(vertexShader.text, ShaderType.VertexShader);
            Shader fragShader = new Shader(fragmentShader.text, ShaderType.FragmentShader);
            program = new ShaderProgram(vertShader, fragShader);
            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, -10), Vector3.Zero, new Vector3(0, 1, 0)));
            try
            {
                program["lightDirection"].SetValue(new Vector3(-1, -1, 0.7f).Normalize());
                program["lightDirection2"].SetValue(new Vector3(1, 1, -.2f).Normalize());
            }catch(Exception e) { }

            LoadMeshes();
            LoadTextures();
            Gl.Viewport(0, 0, width, height);
            Gl.UseProgram(program);
            Gl.BindTexture(texture);
            mesh.SetBuffers(program);
            startTime = DateTime.Now.TimeOfDay.TotalSeconds;

            UpdateLoop(0.016f);

            Glut.glutMainLoop();
        }

        private async static void UpdateLoop(float deltaTime)
        {
            int dtInMs = (int)(deltaTime * 1000);
            while (true)
            {
                await Task.Delay(dtInMs);
                triagleModelMatrix = Matrix4.CreateRotationX(10 * deltaTime * degToRad) * Matrix4.CreateRotationY(20 * deltaTime * degToRad) * triagleModelMatrix;
            }
        }
        
        private static void LoadMeshes()
        {
            mesh = MeshSerializer.ReadObj(meshPath);
        }

        private static void LoadTextures()
        {
            texture = new Texture(texturePath);
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
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            program["model_matrix"].SetValue(triagleModelMatrix);
            program["time"].SetValue((float)(DateTime.Now.TimeOfDay.TotalSeconds - startTime));
            mesh.DrawGl(program);

            Glut.glutSwapBuffers();
        }

        private static void OnClose()
        {
            mesh.Dispose();
            texture.Dispose();
            program.DisposeChildren = true;
            program.Dispose();
        }
    }
}