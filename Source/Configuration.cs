using Dalamud.Configuration;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace DiademCalculator
{
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; }

        public float BackgroundAlpha = 1;
        public bool ShowOutsideFirmamentAndDiadem;
        public bool LockWindow;

        // Add any other properties or methods here.
        [JsonIgnore] private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.pluginInterface.SavePluginConfig(this);
        }
    }
}
