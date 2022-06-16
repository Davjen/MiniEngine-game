using Aiv.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoFinale
{
    static class AudioMgr
    {
        private static Dictionary<string, AudioClip> clips;

        static AudioMgr()
        {
            clips = new Dictionary<string, AudioClip>();
        }

        public static AudioClip AddClip(string name, string path)
        {
            AudioClip clip = new AudioClip(path);
            if (!clips.ContainsKey(name))
            {

                clips.Add(name, clip);
            }

            return clip;
        }

        public static AudioClip GetClip(string name)
        {
            AudioClip clip = null;
            if (clips.ContainsKey(name))
            {
                clip = clips[name];
            }

            return clip;
        }

        public static void ClearAll()
        {
            clips.Clear();
        }
    }
}
