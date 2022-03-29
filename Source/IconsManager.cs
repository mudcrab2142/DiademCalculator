using Dalamud.Utility;
using ImGuiScene;
using Lumina.Data.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiademCalculator
{
    public class IconsManager
    {
        private readonly Plugin plugin;
        private readonly Dictionary<int, TextureWrap> cache = new Dictionary<int, TextureWrap>();

        public IconsManager(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void LoadIcon(int id)
        {
            var path = $"ui/icon/{id / 1000 * 1000:000000}/{id:000000}.tex";
            try
            {
                TexFile? iconFile = plugin.DataManager.GetFile<TexFile>(path);
                if (iconFile != null)
                {
                    var icon = plugin.PluginInterface.UiBuilder.LoadImageRaw(iconFile.GetRgbaImageData(), iconFile.Header.Width, iconFile.Header.Height, 4);
                    cache[id] = icon;
                }
            }
            catch(Exception e)
            {
                plugin.Chat.Print(e.ToString());
            }
        }

        public TextureWrap GetIcon(int id)
        {
            if (cache.TryGetValue(id, out var tex))
                return tex;

            return null;
        }
    }
}
