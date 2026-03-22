# CS2-BotsManager
Manage bots in chat on your CS2 server, add or kick bots using !bots.
Restriction: command is allowed one time per round if there is more than one player online.
!bots in chat = kick all bots, if there isnt a bot on !bots add x bots to have 5on5.
In your gamemode_x_x.cfg add these cvar :
bot_quota 10
bot_quota_mode fill
