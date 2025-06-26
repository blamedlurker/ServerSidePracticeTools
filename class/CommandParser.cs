using UnityEngine;
using System;
using System.Collections.Generic;

namespace ServerSidePracticeTools
{
    public class CommandParser
    {
        /// <summary>
        /// Adds the OnChatCommand method to the chat command event listeners of a given event manager.
        /// </summary>
        /// <param name="eventManager">The EventManager pertinent to the calling context.</param>
        public void AddAsListener(EventManager eventManager)
        {
            Debug.Log($"[{SSPTConstants.MOD_NAME}] Adding CommandParser to command listeners...");
            try
            {
                eventManager.AddEventListener("Event_Server_OnChatCommand", OnChatCommand);
                eventManager.AddEventListener("Event_Server_OnSynchronizeComplete", OnClientJoin);
                Debug.Log($"[{SSPTConstants.MOD_NAME}] CommandParser successfully added to command listeners.");
            }
            catch (Exception e)
            {
                Debug.Log($"[{SSPTConstants.MOD_NAME}] Adding CommandParser failed: {e.Message}");
            }
        }

        /// <summary>
        /// Parses a chat command.
        /// </summary>
        /// <param name="message">The dictionary containing the command information.</param>
        public void OnChatCommand(Dictionary<string, object> message)
        {
            ulong clientId = (ulong)message["clientId"];
            string command = (string)message["command"];
            string[] args = (string[])message["args"];
            Player player = PlayerManager.Instance.GetPlayerByClientId(clientId);
            int n;

            switch (command)
            {
                case "/s":
                case "/spawn":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    Vector3 offset = new Vector3(0, 0.3f, 0);
                    n = 1;
                    if (args.Length > 0) int.TryParse(args[0], out n);

                    if (n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>Please spawn no more than {SSPTConstants.SERVER_PUCK_LIMIT} pucks at one time.</color>", clientId);
                        break;
                    }

                    if (PuckManager.Instance.GetPucks().Count + n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        List<Puck> puckList = PuckManager.Instance.GetPucks();
                        for (int i = 0; i < puckList.Count + n - 50; i++)
                        {
                            PuckManager.Instance.Server_DespawnPuck(puckList[i]);
                        }
                    }

                    for (int i = 1; i <= n; i++)
                    {
                        PuckManager.Instance.Server_SpawnPuck(
                            player.PlayerBody.Stick.BladeHandlePosition + i * offset,
                            new Quaternion(0, 0, 0, 0),
                            player.PlayerBody.Rigidbody.linearVelocity
                            );
                    }
                    if (n > 1) UIChat.Instance.Server_SendSystemChatMessage(
                            $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} spawned {n} pucks."
                        );
                    else UIChat.Instance.Server_SendSystemChatMessage(
                            $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} spawned a puck."
                        );
                    break;


                case "/randp":
                case "/randompucks":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    n = 10;
                    if (args.Length > 0) int.TryParse(args[0], out n);

                    if (n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>Please spawn no more than {SSPTConstants.SERVER_PUCK_LIMIT} pucks at one time.</color>", clientId);
                        break;
                    }

                    if (PuckManager.Instance.GetPucks().Count + n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        List<Puck> puckList = PuckManager.Instance.GetPucks();
                        for (int i = 0; i < puckList.Count + n - 50; i++)
                        {
                            PuckManager.Instance.Server_DespawnPuck(puckList[i]);
                        }
                    }

                    for (int i = 1; i <= n; i++)
                    {
                        PuckManager.Instance.Server_SpawnPuck(
                            new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(-40, 40)),
                            new Quaternion(0, 0, 0, 0),
                            new Vector3(0, 0, 0)
                            );
                    }
                    UIChat.Instance.Server_SendSystemChatMessage(
                    $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} has spawned {n} random pucks."
                );
                    break;


                case "/snz":
                case "/spawnneutralzone":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    n = 10;
                    if (args.Length > 0) int.TryParse(args[0], out n);

                    if (n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>Please spawn no more than {SSPTConstants.SERVER_PUCK_LIMIT} pucks at one time.</color>", clientId);
                        break;
                    }

                    if (PuckManager.Instance.GetPucks().Count + n > SSPTConstants.SERVER_PUCK_LIMIT)
                    {
                        List<Puck> puckList = PuckManager.Instance.GetPucks();
                        for (int i = 0; i < puckList.Count + n - 50; i++)
                        {
                            PuckManager.Instance.Server_DespawnPuck(puckList[i]);
                        }
                    }

                    for (int i = 1; i <= n; i++)
                    {
                        PuckManager.Instance.Server_SpawnPuck(
                            new Vector3(UnityEngine.Random.Range(-20, 20), UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(-12, 12)),
                            new Quaternion(0, 0, 0, 0),
                            new Vector3(0, 0, 0)
                            );
                    }
                    UIChat.Instance.Server_SendSystemChatMessage(
                    $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} has spawned {n} random pucks in the neutral zone."
                );
                    break;


                case "/fp":
                case "/freezepucks":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    foreach (Puck puck in PuckManager.Instance.GetPucks()) puck.Server_Freeze();
                    UIChat.Instance.Server_SendSystemChatMessage(
                        $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} froze all pucks."
                    );
                    break;


                case "/ufp":
                case "/unfreezepucks":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    foreach (Puck puck in PuckManager.Instance.GetPucks()) puck.Server_Unfreeze();
                    UIChat.Instance.Server_SendSystemChatMessage(
                        $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} unfroze all pucks."
                    );
                    break;


                case "/ep":
                case "/emptypucks":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    PuckManager.Instance.Server_DespawnPucks();
                    UIChat.Instance.Server_SendSystemChatMessage(
                        $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} removed all pucks."
                    );
                    break;


                case "/rp":
                case "/resetpucks":
                    if (GameManager.Instance.Phase != GamePhase.Warmup)
                    {
                        UIChat.Instance.Server_SendSystemChatMessage($"<color=red>{command} can only be used during warmup!</color>");
                        break;
                    }

                    PuckManager.Instance.Server_DespawnPucks();
                    PuckManager.Instance.Server_SpawnPucksForPhase(GamePhase.Warmup);
                    UIChat.Instance.Server_SendSystemChatMessage(
                        $"{UIChat.Instance.WrapInTeamColor(player.Team.Value, $"#{player.Number.Value} {player.Username.Value.ToString()}")} reset the pucks."
                    );
                    break;


                case "/l":
                case "/list":
                    string comList = "<b><size=24>SSPT Modded Commands</size></b><br>" +
                                     "<b>/spawn</b> (/s) - <i>Spawns a puck or a number of pucks if followed by an integer</i><br>" +
                                     "<b>/randompucks</b> (/randp) - <i>Spawns a number of pucks around the rink at random positions</i><br>" +
                                     "<b>/freezepucks</b> (/fp) - <i>Freezes all currently spawned pucks in place</i><br>" +
                                     "<b>/unfreezepucks</b> (/ufp) - <i>Unfreezes all currently spawned pucks</i><br>" +
                                     "<b>/emptypucks</b> (/ep) - <i>Removes all currently spawned pucks</i><br>" +
                                     "<b>/resetpucks</b> (/rp) - <i>Resets pucks to default warmup positions</i><br>";
                    UIChat.Instance.Server_SendSystemChatMessage(comList, clientId);
                    break;

            }
        }

        /// <summary>
        /// Sends client information on how to access custom command list.
        /// </summary>
        /// <param name="message">Sync complete information.</param>
        public void OnClientJoin(Dictionary<string, object> message)
        {
            ulong clientId = (ulong)message["clientId"];
            UIChat.Instance.Server_SendSystemChatMessage("This server has the ServerSidePracticeTools mod enabled. Use <b>/list</b> to see all custom commands.", clientId);
        }
    }
}