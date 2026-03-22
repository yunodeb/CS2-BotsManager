using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Commands;

public class BotsManager : BasePlugin
{
    public override string ModuleName => "BotsManager";
    public override string ModuleVersion => "1.0";
    public override string ModuleAuthor => "ynd";

    private int currentRound = 0;
    private int lastUsedRound = -1;

    public override void Load(bool hotReload)
    {
        AddCommand("bots", "Toggle bots", OnBotsCommand);

        RegisterEventHandler<EventRoundStart>((@event, info) =>
        {
            currentRound++;
            Server.PrintToChatAll($" \x02[BotsManager]\x04 Type !bots to add or remove bots (max 10 players total).");
            return HookResult.Continue;
        });
    }

    private void OnBotsCommand(CCSPlayerController? player, CommandInfo command)
    {
        //How many real players online
        int humanCount = Utilities.GetPlayers().Count(p => p != null && p.IsValid && !p.IsBot);

        //Using !bots one time per round allowed if there is more than one player online
        if (humanCount > 1 && lastUsedRound == currentRound)
        {
            player?.PrintToChat($" \x02[BotsManager]\x04 This command has already been used this round!");
            return;
        }

        int humanT = 0;
        int humanCT = 0;
        int botCount = 0;

        foreach (var p in Utilities.GetPlayers())
        {
            if (p == null || !p.IsValid) continue;

            if (p.IsBot)
            {
                botCount++;
                continue;
            }

            if (p.Team == CsTeam.Terrorist)
                humanT++;
            else if (p.Team == CsTeam.CounterTerrorist)
                humanCT++;
        }

        if (botCount > 0)
        {
            Server.ExecuteCommand("bot_kick");
            Server.PrintToChatAll($" \x02[BotsManager]\x04 All bots have been removed!");
        }
        else
        {
            int totalHumans = humanT + humanCT;
            int botsToAdd = 10 - totalHumans;

            if (botsToAdd <= 0)
            {
                Server.PrintToChatAll($" \x02[BotsManager]\x04 There are already 10 or more players!");
                return;
            }

            for (int i = 0; i < botsToAdd; i++)
            {
                if (humanT <= humanCT)
                {
                    Server.ExecuteCommand("bot_add_t");
                    humanT++;
                }
                else
                {
                    Server.ExecuteCommand("bot_add_ct");
                    humanCT++;
                }
            }

            Server.PrintToChatAll($" \x02[BotsManager]\x04 {botsToAdd} bots have been added!");
        }
        if (humanCount > 1)
            lastUsedRound = currentRound;
    }
}