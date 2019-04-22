using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit;
using Verse;

namespace Toolkit___DailyLootboxes
{
    public static class Lootbox
    {
        public static void OpenLootbox(string username)
        {
            if (Lootbox_Settings.ForceOpenAllLootboxesAtOnce)
            {
                OpenAllLootboxes(username);
                return;
            }

            LootboxComponent lootboxComponent = Current.Game.GetComponent<LootboxComponent>();

            string message = $"@{username} you open a lootbox and discover: ";

            int coinReward = Verse.Rand.Range(Lootbox_Settings.RandomCoinRange.min, Lootbox_Settings.RandomCoinRange.max);

            message += coinReward + " coins";

            Viewer viewer = Viewers.GetViewer(username);

            viewer.GiveViewerCoins(coinReward);

            lootboxComponent.ViewersLootboxes[viewer.username]--;

            Toolkit.client.SendMessage(message, true);
        }

        public static void OpenAllLootboxes(string username)
        {
            string message = $"@{username} you open all your lootboxes and discover: ";

            LootboxComponent lootboxComponent = Current.Game.GetComponent<LootboxComponent>();

            int lootboxQuantity = lootboxComponent.HowManyLootboxesDoesViewerHave(username);

            int coinReward = 0;

            for (int i = 0; i < lootboxQuantity; i++)
            {
                coinReward += Verse.Rand.Range(Lootbox_Settings.RandomCoinRange.min, Lootbox_Settings.RandomCoinRange.max);
            }

            message += coinReward + " coins";

            Viewer viewer = Viewers.GetViewer(username);

            viewer.GiveViewerCoins(coinReward);

            lootboxComponent.ViewersLootboxes[viewer.username] = 0;

            Toolkit.client.SendMessage(message, true);
        }
    }
}
