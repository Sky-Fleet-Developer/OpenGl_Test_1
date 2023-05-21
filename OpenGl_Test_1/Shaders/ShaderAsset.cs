using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLTest.Shaders
{
    internal class ShaderAsset
    {
        private string path;
        private string text;
        public ShaderAsset(string path)
        {
            this.path = path;
            this.text = string.Empty;
        }

        public void Load()
        {
            try
            {
                text = File.ReadAllText(path);
                if (text.Length > 0)
                {
                    Console.WriteLine($"Shader {path} successfuly loaded");
                }
                else
                {
                    Console.WriteLine($"Shader {path} can't be loaded. File is empty.");
                }
            }
            catch (Exception exceprion)
            {
                Console.WriteLine(exceprion.Message); 
            }

        }

        public static implicit operator string(ShaderAsset asset) { return asset.text; }
    }
}
