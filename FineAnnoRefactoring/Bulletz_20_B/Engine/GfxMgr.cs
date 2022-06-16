using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    static class GfxMgr
    {
        private static Dictionary<string, Texture> textures;

        static GfxMgr()
        {
            textures = new Dictionary<string, Texture>();
        }

        public static Texture AddTexture(string name, string path)
        {
            Texture t;
            if (!textures.ContainsKey(name))
            {
                t = new Texture(path);
                textures.Add(name, t);
                return t;
            }
            return null;
        }

        public static Texture GetTexture(string name)
        {
            Texture t = null;
            if (textures.ContainsKey(name))
            {
                t = textures[name];
            }

            return t;
        }

        public static void ClearAll()
        {
            textures.Clear();
        }
    }
}
