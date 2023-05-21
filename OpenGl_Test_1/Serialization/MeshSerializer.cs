using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] data = line.Split(' ');

                        switch (data[0])
                        {
                            case "o":
                                break;
                            case "v":
                                break;
                            case "vn":
                                break;
                            case "vt":
                                break;
                            case "f":
                                break;
                            default:
                                break;
                        }
                    }
                }
                return mesh;
            }
            else
            {
                Console.WriteLine("File not found!");
                return null;
            }
        }
    }
}
