using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Logging;
using Dalamud.Plugin;
using Dalamud.Utility;
using DiademCalculator.Attributes;
using FFXIVClientStructs.FFXIV.Client.Game;
using ImGuiNET;
using ImGuiScene;
using Lumina.Data.Files;

namespace DiademCalculator
{
    public unsafe class Plugin : IDalamudPlugin
    {
        static readonly List<DiademResourceInfo> _minerPreset = new List<DiademResourceInfo>()
        {
            new DiademResourceInfo(29939, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Cloudstone
            new DiademResourceInfo(29940, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Rock Salt
            new DiademResourceInfo(29941, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Spring Water 
            new DiademResourceInfo(29942, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Aurum Regis Sand
            new DiademResourceInfo(29943, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Jade
            new DiademResourceInfo(29946, 10, 0, 4), //Grade 2 Skybuilders' Umbral Flarestone
            new DiademResourceInfo(29947, 10, 0, 4), //Grade 2 Skybuilders' Umbral Levinshard

            new DiademResourceInfo(31311, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Cloudstone
            new DiademResourceInfo(31312, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Basilisk Egg
            new DiademResourceInfo(31313, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Alumen
            new DiademResourceInfo(31314, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Clay
            new DiademResourceInfo(31315, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Granite
            new DiademResourceInfo(31318, 5, 0, 2),  //Grade 3 Skybuilders' Umbral Magma Shard
            new DiademResourceInfo(31319, 5, 0, 2),  //Grade 3 Skybuilders' Umbral Levinite

            new DiademResourceInfo(32007, 5, 1, 0),  //Grade 4 Skybuilders' Iron Ore
            new DiademResourceInfo(32008, 5, 1, 0),  //Grade 4 Skybuilders' Iron Sand
            new DiademResourceInfo(32012, 5, 1, 0),  //Grade 4 Skybuilders' Ore
            new DiademResourceInfo(32013, 5, 1, 0),  //Grade 4 Skybuilders' Rock Salt
            new DiademResourceInfo(32014, 5, 1, 0),  //Grade 4 Skybuilders' Mythrite Sand
            new DiademResourceInfo(32020, 5, 2, 0),  //Grade 4 Skybuilders' Electrum Ore
            new DiademResourceInfo(32021, 5, 2, 0),  //Grade 4 Skybuilders' Alumen
            new DiademResourceInfo(32022, 5, 2, 0),  //Grade 4 Skybuilders' Spring Water
            new DiademResourceInfo(32023, 5, 2, 0),  //Grade 4 Skybuilders' Gold Sand
            new DiademResourceInfo(32024, 5, 2, 0),  //Grade 4 Skybuilders' Ragstone
            new DiademResourceInfo(32030, 5, 3, 13), //Grade 4 Skybuilders' Gold Ore
            new DiademResourceInfo(32031, 5, 3, 13), //Grade 4 Skybuilders' Finest Rock Salt
            new DiademResourceInfo(32032, 5, 3, 13), //Grade 4 Skybuilders' Truespring Water
            new DiademResourceInfo(32033, 5, 3, 13), //Grade 4 Skybuilders' Mineral Sand
            new DiademResourceInfo(32034, 5, 3, 13), //Grade 4 Skybuilders' Bluespirit Ore
            new DiademResourceInfo(32040, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Cloudstone
            new DiademResourceInfo(32041, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Spring Water
            new DiademResourceInfo(32042, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Ice Stalagmite
            new DiademResourceInfo(32043, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Silex
            new DiademResourceInfo(32044, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Prismstone
            new DiademResourceInfo(32047, 5, 5, 15), //Grade 4 Skybuilders' Umbral Flarerock
            new DiademResourceInfo(32048, 5, 5, 15), //Grade 4 Skybuilders' Umbral Levinsand
        };

        static readonly List<DiademResourceInfo> _botanistPreset = new List<DiademResourceInfo>()
        {
            new DiademResourceInfo(29934, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Log
            new DiademResourceInfo(29935, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Hardened Sap
            new DiademResourceInfo(29936, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Wheat
            new DiademResourceInfo(29937, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Cotton Boll
            new DiademResourceInfo(29938, 10, 0, 3), //Grade 2 Artisanal Skybuilders' Dawn Lizard
            new DiademResourceInfo(29944, 10, 0, 4), //Grade 2 Skybuilders' Umbral Galewood Log
            new DiademResourceInfo(29945, 10, 0, 4), //Grade 2 Skybuilders' Umbral Earthcap

            new DiademResourceInfo(31306, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Log
            new DiademResourceInfo(31307, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Amber
            new DiademResourceInfo(31308, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Cotton Boll
            new DiademResourceInfo(31309, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Rice
            new DiademResourceInfo(31310, 5, 0, 2),  //Grade 3 Artisanal Skybuilders' Vine
            new DiademResourceInfo(31316, 5, 0, 2),  //Grade 3 Skybuilders' Umbral Galewood Sap
            new DiademResourceInfo(31317, 5, 0, 2),  //Grade 3 Skybuilders' Umbral Tortoise

            new DiademResourceInfo(32005, 5, 1, 0),  //Grade 4 Skybuilders' Switch
            new DiademResourceInfo(32006, 5, 1, 0),  //Grade 4 Skybuilders' Hemp
            new DiademResourceInfo(32009, 5, 1, 0),  //Grade 4 Skybuilders' Mahogany Log
            new DiademResourceInfo(32010, 5, 1, 0),  //Grade 4 Skybuilders' Sesame
            new DiademResourceInfo(32011, 5, 1, 0),  //Grade 4 Skybuilders' Cotton Boll
            new DiademResourceInfo(32015, 5, 2, 0),  //Grade 4 Skybuilders' Spruce Log
            new DiademResourceInfo(32016, 5, 2, 0),  //Grade 4 Skybuilders' Mistletoe
            new DiademResourceInfo(32017, 5, 2, 0),  //Grade 4 Skybuilders' Toad
            new DiademResourceInfo(32018, 5, 2, 0),  //Grade 4 Skybuilders' Vine
            new DiademResourceInfo(32019, 5, 2, 0),  //Grade 4 Skybuilders' Tea Leaves

            new DiademResourceInfo(32025, 5, 3, 13), //Grade 4 Skybuilders' White Cedar Log
            new DiademResourceInfo(32026, 5, 3, 13), //Grade 4 Skybuilders' Primordial Resin
            new DiademResourceInfo(32027, 5, 3, 13), //Grade 4 Skybuilders' Wheat
            new DiademResourceInfo(32028, 5, 3, 13), //Grade 4 Skybuilders' Gossamer Cotton Boll
            new DiademResourceInfo(32029, 5, 3, 13), //Grade 4 Skybuilders' Tortoise
            new DiademResourceInfo(32035, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Log
            new DiademResourceInfo(32036, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Raspberry
            new DiademResourceInfo(32037, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Caiman
            new DiademResourceInfo(32038, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Cocoon
            new DiademResourceInfo(32039, 5, 3, 13), //Grade 4 Artisanal Skybuilders' Barbgrass
            new DiademResourceInfo(32045, 5, 5, 15), //Grade 4 Skybuilders' Umbral Galewood Branch
            new DiademResourceInfo(32046, 5, 5, 15), //Grade 4 Skybuilders' Umbral Dirtleaf
        };


        static readonly List<DiademResourceInfo> _fisherPreset = new List<DiademResourceInfo>()
        {

        };


        const int Scrip_Icon = 65073;
        const int MIN_Icon = 62116;
        const int BTN_Icon = 62117;
        const int FSH_Icon = 62118;

        public string Name => "Diadem Calculator";


        [PluginService]
        public DalamudPluginInterface PluginInterface { get; init; }

        [PluginService]
        public CommandManager Commands { get; init; }

        [PluginService]
        public ChatGui Chat { get; init; }

        [PluginService]
        public DataManager DataManager { get; private set; }

        private readonly PluginCommandManager<Plugin> commandManager;
        private readonly Configuration config;
        private readonly IconsManager iconsManager;


        private DateTime lastUpdate;
        private int minPoints, btnPoints, fshPoints;
        private int minScrips, btnScrips, fshScrips;


        public Plugin()
        {
            this.config = (Configuration)PluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(PluginInterface);

            PluginInterface.UiBuilder.Draw += this.Draw;

            commandManager = new PluginCommandManager<Plugin>(this, Commands);
            iconsManager = new IconsManager(this);

            iconsManager.LoadIcon(Scrip_Icon);
            iconsManager.LoadIcon(FSH_Icon);
            iconsManager.LoadIcon(BTN_Icon);
            iconsManager.LoadIcon(MIN_Icon);
        }

        [Command("/diadem")]
        public void Settings(string command, string args)
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.commandManager.Dispose();

            PluginInterface.SavePluginConfig(this.config);
            PluginInterface.UiBuilder.Draw -= this.Draw;
        }

        public void Draw()
        {
            DrawStats();
        }

        void DrawStats()
        {
            ImGui.SetNextWindowPos(new Vector2(350, 185), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(100, 175), ImGuiCond.Always);
            ImGui.SetNextWindowBgAlpha(1);

            ImGui.Begin("DiademInspector", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);

            if ((DateTime.Now - lastUpdate).TotalMilliseconds > 50)
            {
                CalculatePoints(_minerPreset, out minPoints, out minScrips);
                CalculatePoints(_botanistPreset, out btnPoints, out btnScrips);
                CalculatePoints(_fisherPreset, out fshPoints, out fshScrips);
                lastUpdate = DateTime.Now;
            }

            DrawIcon(iconsManager.GetIcon(BTN_Icon), new Vector2(5, 5));
            DrawPoints(new Vector2(50, 10), btnPoints);

            DrawIcon(iconsManager.GetIcon(MIN_Icon), new Vector2(5, 45));
            DrawPoints(new Vector2(50, 50), minPoints);

            DrawIcon(iconsManager.GetIcon(FSH_Icon), new Vector2(5, 85));
            DrawPoints(new Vector2(50, 90), fshPoints);

            DrawIcon(iconsManager.GetIcon(Scrip_Icon), new Vector2(5, 135));
            DrawPoints(new Vector2(50, 140), minScrips + btnScrips + fshScrips);

            ImGui.End();
        }

        void DrawIcon(TextureWrap iconTex, Vector2 localPos)
        {
            if (iconTex == null)
                return;

            var windowPos = ImGui.GetWindowPos();
            var size = new Vector2(iconTex.Width, iconTex.Height);

            var drawList = ImGui.GetWindowDrawList();
            drawList.AddImage(iconTex.ImGuiHandle, windowPos + localPos, windowPos + localPos + size);
        }

        void DrawPoints(Vector2 localPos, int count)
        {
            ImGui.SetCursorPos(localPos);
            ImGui.Text(count.ToString());
        }

        void CalculatePoints(List<DiademResourceInfo> preset, out int points, out int scrips)
        {
            points = scrips = 0;

            var manager = InventoryManager.Instance();
            if (manager == null)
                return;

            foreach (var item in preset)
            {
                var count = manager->GetInventoryItemCount(item.Id, false, false, false) / item.Set;
                points += count * item.PointsReward;
                scrips += count * item.ScripsReward;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
