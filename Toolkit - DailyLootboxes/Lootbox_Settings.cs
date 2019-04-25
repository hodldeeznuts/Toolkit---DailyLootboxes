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
    public class Lootbox_Settings : ModSettings
    {
        public static IntRange RandomCoinRange = new IntRange(250, 750);

        public static int LootboxesPerDay = 1;

        public static bool ShowWelcomeMessage = true;

        public static bool ForceOpenAllLootboxesAtOnce = false;

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

            ls.Begin(inRect);

            ls.ColumnWidth = inRect.width / 2;

            ls.CheckboxLabeled("Show welcome message?", ref ShowWelcomeMessage);

            ls.Gap();

            ls.Label("Amount of coins given per lootbox range");
            ls.IntRange(ref RandomCoinRange, 1, 10000);

            ls.Gap();

            string perDayBuffer = LootboxesPerDay.ToString();
            ls.TextFieldNumericLabeled("Lootboxes per day:", ref LootboxesPerDay, ref perDayBuffer, 0, 20);

            ls.Gap();

            ls.CheckboxLabeled("Force Viewers to Open all Lootboxes at Once", ref ForceOpenAllLootboxesAtOnce);

            ls.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref RandomCoinRange, "RandomCoinRange", new IntRange(250, 750));
            Scribe_Values.Look(ref LootboxesPerDay, "LootboxesPerDay", 1);
            Scribe_Values.Look(ref ShowWelcomeMessage, "ShowWelcomeMessage", true);
            Scribe_Values.Look(ref ForceOpenAllLootboxesAtOnce, "ForceOpenAllLootboxesAtOnce", false);
        }
    }
}
