using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace F2DPath
{
    class TextureCache
    {
        private static Dictionary<string, Texture> Textures;

        static TextureCache()
        {
            Textures = new Dictionary<String, Texture>();
            Textures.Add("brown", new Texture("images/brown.png"));
            Textures.Add("green", new Texture("images/green.png"));
            Textures.Add("blue", new Texture("images/blue.png"));
        }
        
        public static Texture Named(String name)
        {
            return Textures[name];
        }

    }
}
