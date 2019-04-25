using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Settings;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace Toolkit___DailyLootboxes
{
    public class ToolkitLootboxes : Mod
    {
        public ToolkitLootboxes(ModContentPack content) : base(content)
        {
            GetSettings<Lootbox_Settings>();
            Settings_ToolkitExtensions.RegisterExtension(new ToolkitExtension(this, typeof(LootboxPatchSettings)));
        }

        public override string SettingsCategory() => "Toolkit - Lootboxes";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<Lootbox_Settings>().DoWindowContents(inRect);
        }
    }

    public class LootboxPatchSettings : ToolkitWindow
    {
        public LootboxPatchSettings(Mod mod) : base(mod)
        {
            this.Mod = mod;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Mod.GetSettings<Lootbox_Settings>().DoWindowContents(inRect);
        }
    }
    
}
