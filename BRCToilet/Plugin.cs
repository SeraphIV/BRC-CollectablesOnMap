using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using BepInEx.Configuration;
using System.IO;
using System.Drawing;

namespace BRCToilet
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class Plugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.SeraphIV.CollectablesOnMap";

        private const string PluginName = "CollectablesOnMap";
        private const string VersionString = "0.0.1";

        public static ManualLogSource Log = null!;

        public static string DisplayCollectableRadarKey = "Display collectable radar";

        public static ConfigEntry<bool> DisplayCollectableRadar;

        public static byte[] GetImage(string filename)
        {
            var assembly = typeof(Plugin).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(filename))
            {
                if (stream == null) return null;
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        private void Awake()
        {
            Log = Logger;

            DisplayCollectableRadar = Config.Bind("General", DisplayCollectableRadarKey, true);

            Application.runInBackground = true;
            SetupHarmony();

            new CollectableManager();
        }

        private void SetupHarmony()
        {
            var Harmony = new Harmony("BRCToilet.Harmony");

            var patches = typeof(Plugin).Assembly.GetTypes()
                                        .Where(m => m.GetCustomAttributes(typeof(HarmonyPatch), false).Length > 0)
                                        .ToArray();

            foreach (var patch in patches)
            {
                Harmony.PatchAll(patch);
            }
        }
    }
}
