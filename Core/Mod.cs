using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Mod
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Image { get; set; }
        public Uri Url { get; set; }
        public string Creator { get; set; }
        public Version Version { get; set; }

        public Version FSMinVersion { get; set; }
        public Version FSMaxVersion { get; set; }


        public string GetUrl()
        {
            return Url.GetComponents(UriComponents.HttpRequestUrl, UriFormat.UriEscaped);
        }

        public bool IsCompatible()
        {
            // get game verion
            Version gameversion = Game.Version;
            return true;
        }

    }
}
