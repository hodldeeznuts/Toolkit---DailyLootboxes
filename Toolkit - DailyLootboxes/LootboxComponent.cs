using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TwitchToolkit;
using TwitchToolkit.IRC;
using TwitchToolkit.Store;
using Verse;

namespace Toolkit___DailyLootboxes
{
    public class LootboxComponent : TwitchInterfaceBase
    {
        public LootboxComponent(Game game)
        {

        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 20000 == 0)
            {
                if (today != DateTime.Now)
                {
                    ViewersWhoHaveRecievedLootboxesToday = new List<string>();
                    today = DateTime.Now;
                }
            }
        }

        public override void ParseCommand(IRCMessage msg)
        {
            Viewer viewer = Viewers.GetViewer(msg.User);

            if (IsViewerOwedLootboxesToday(viewer.username.ToLower()))
            {
                AwardViewerDailyLootboxes(viewer.username.ToLower());
            }

            if (msg.Message.StartsWith("!openlootbox") && Commands.AllowCommand(msg.Channel))
            {
                if (HowManyLootboxesDoesViewerHave(viewer.username) > 0)
                {
                    Lootbox.OpenLootbox(viewer.username.ToLower());
                }
                else
                {
                    Toolkit.client.SendMessage($"@{viewer.username} you do not have any lootboxes.", true);
                }
            }

            if (msg.Message.StartsWith("!lootboxes") && Commands.AllowCommand(msg.Channel))
            {
                int amount = HowManyLootboxesDoesViewerHave(viewer.username);

                string pluralBoxes = amount > 1 ? "es" : "";

                string instructions = amount > 1 ? " Use !openlootbox" + (ToolkitSettings.UseSeparateChatRoom ? " in the separate chat room." : "") : "";

                Toolkit.client.SendMessage($"@{viewer.username} you currently have {amount} lootbox{pluralBoxes}.{instructions}");
            }

            if (msg.Message.StartsWith("!givelootbox") && (Viewer.IsModerator(viewer.username) || viewer.username.ToLower() == ToolkitSettings.Channel.ToLower()) )
            {
                string[] command = msg.Message.Split(' ');

                if (command.Length < 2)
                {
                    return;
                }

                int amount = 1;

                if (command.Length > 2)
                {
                    int.TryParse(command[2], out amount);
                }

                string pluralBoxes = amount > 1 ? "es" : "";

                command[1] = command[1].Replace("@", "");

                Viewer giftee = Viewers.GetViewer(command[1]);

                GiveViewerLootbox(giftee.username, amount);

                Toolkit.client.SendMessage($"@{giftee.username} you have received {amount} lootbox{pluralBoxes} from {viewer.username}.", Commands.SendToChatroom(msg.Channel));
            }
        }

        public void WelcomeMessage(string username)
        {
            if (!Lootbox_Settings.ShowWelcomeMessage) return;

            string message = $"@{username} Welcome to {ToolkitSettings.Channel}'s Stream, You currently have {HowManyLootboxesDoesViewerHave(username)} Lootbox(es) to open. Use !openlootbox";

            if (ToolkitSettings.UseSeparateChatRoom)
            {
                message += " in the separate chat room.";
            }

            Toolkit.client.SendMessage(message);
        }

        public void AwardViewerDailyLootboxes(string username)
        {
            ViewersWhoHaveRecievedLootboxesToday.Add(username);
            LogViewerLastSeen(username);
            GiveViewerLootbox(username, Lootbox_Settings.LootboxesPerDay);
            WelcomeMessage(username);
        }

        public void GiveViewerLootbox(string username, int amount = 1)
        {
            if (ViewersLootboxes.ContainsKey(username))
            {
                ViewersLootboxes[username] += amount;
            }
            else
            {
                ViewersLootboxes.Add(username, amount);
            }
        }

        private bool IsViewerOwedLootboxesToday(string username)
        {
            if (ViewersWhoHaveRecievedLootboxesToday.Contains(username))
            {
                return false;
            }

            if (!IsViewerOwedLootboxesLookup(username))
            {
                return false;
            }

            return true;
        }

        private bool IsViewerOwedLootboxesLookup(string username)
        {
            if (IsViewerInLastSeenList(username))
            {
                DateTime lastSeen = ViewerLastSeenAt(username);
                if (lastSeen == DateTime.Today)
                {
                    return false;
                }
            }

            return true;
        }

        public void LogViewerLastSeen(string username)
        {
            if (ViewersLastSeenDate.ContainsKey(username))
            {
                ViewersLastSeenDate[username] = DateTime.Now.ToShortDateString();
            }
            else
            {
                ViewersLastSeenDate.Add(username, DateTime.Now.ToShortDateString());
            }
        }

        public bool IsViewerInLastSeenList(string username)
        {
            return ViewersLastSeenDate.ContainsKey(username);
        }

        private DateTime ViewerLastSeenAt(string username)
        {
            return DateTime.ParseExact(ViewersLastSeenDate[username], "dd/MM/yyyy", CultureInfo.InvariantCulture); ;
        }

        public bool DoesViewerHaveLootboxes(string username)
        {
            if (ViewersLootboxes.ContainsKey(username))
            {
                if (ViewersLootboxes[username] > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public int HowManyLootboxesDoesViewerHave(string username)
        {
            if (ViewersLootboxes.ContainsKey(username))
            {
                return ViewersLootboxes[username];
            }

            return 0;
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref ViewersLastSeenDate, "ViewersLastSeenDate", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewersLootboxes, "ViewersLootboxes", LookMode.Value, LookMode.Value);
        }

        public DateTime today = DateTime.Now;

        public List<string> ViewersWhoHaveRecievedLootboxesToday = new List<string>();

        public Dictionary<string, string> ViewersLastSeenDate = new Dictionary<string, string>();

        public Dictionary<string, int> ViewersLootboxes = new Dictionary<string, int>();
    }
}
