using System.Numerics;

namespace OpenGl_Test_1.Serialization
{
    public class MeshSerializer
    {
        public static Mesh ReadObj(string path)
        {
            if (File.Exists(path))
            {
                Mesh mesh = new Mesh();
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    List<Vector3> verticesSource = new List<Vector3>();
                    List<Vector3> normalsSource = new List<Vector3>();
                    List<Vector2> uvsSource = new List<Vector2>();
                    List<string> trianglesSource = new List<string>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');

                        switch (data[0])
                        {
                            case "o":
                                break;
                            case "v":
                                verticesSource.Add(ReadVector3(data));
                                break;
                            case "vn":
                                normalsSource.Add(ReadVector3(data));
                                break;
                            case "vt":
                                uvsSource.Add(ReadVector2(data));
                                break;
                            case "f":
                                trianglesSource.Add(data[1]);
                                trianglesSource.Add(data[2]);
                                trianglesSource.Add(data[3]);
                                break;
                            default:
                                break;
                        }
                    }

                    List<Vector3> vertices = new List<Vector3>();
                    List<Vector3> normals = new List<Vector3>();
                    List<Vector2> uvs = new List<Vector2>();
                    List<int> triangles = new List<int>();

                    char splitSymbol = '/' ;

                    for (int i = 0; i < trianglesSource.Count; i++)
                    {
                        string triangle = trianglesSource[i];
                        int[] split = triangle.Split(splitSymbol).Select(x => int.Parse(x)-1).ToArray();
                        vertices.Add(verticesSource[split[0]]);
                        uvs.Add(uvsSource[split[1]]);
                        normals.Add(normalsSource[split[2]]);
                        triangles.Add(i);
                    }

                    mesh.SetVertices(vertices.ToArray());
                    mesh.SetNormals(normals.ToArray());
                    mesh.SetUvs(uvs.ToArray());
                    mesh.SetTriangles(triangles.ToArray());
                }
                return mesh;
            }
            else
            {
                Console.WriteLine("File not found!");
                return null;
            }
        }

        private static Vector2 ReadVector2(string[] data)
        {
            return new Vector2(float.Parse(data[1]), float.Parse(data[2]));
        }

        private static Vector3 ReadVector3(string[] data)
        {
            return new Vector3(float.Parse(data[1]), float.Parse(data[2]), float.Parse(data[3]));
        }
    }
}
