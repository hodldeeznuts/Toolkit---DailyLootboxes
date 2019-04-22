using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Toolkit___DailyLootboxes
{
    public class ToolkitLootboxes : Mod
    {
        public ToolkitLootboxes(ModContentPack content) : base(content)
        {
            GetSettings<Lootbox_Settings>();
        }

        public override string SettingsCategory() => "Toolkit - Lootboxes";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<Lootbox_Settings>().DoWindowContents(inRect);
        }
    }
}
