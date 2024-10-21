using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{

    private Lobby hostLobby;
    private Lobby joinedLobby;

    private float heartBeatTimerDefault = 15;
    private float heartBeatTimer = 15;

    private float lobbyUpdateTimerDefault = 1.25f;
    private float lobbyUpdateTimer = 1.25f;

    private string playerName;

    private QueryResponse queryResponse;

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        // Can make this sign in to steam or something instead of anonymous
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = $"KillerStash{Random.Range(0, 1000)}";

        Debug.Log(playerName);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby == null) return;

        lobbyUpdateTimer -= Time.deltaTime;

        if (lobbyUpdateTimer < 0)
        {
            lobbyUpdateTimer = lobbyUpdateTimerDefault;

            joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (hostLobby == null) return;

        heartBeatTimer -= Time.deltaTime;

        if (heartBeatTimer < 0)
        {
            heartBeatTimer = heartBeatTimerDefault;

            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    [Command]
    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "DeathMatch", DataObject.IndexOptions.S1) },
                    { "Map", new DataObject(DataObject.VisibilityOptions.Public, "Plain", DataObject.IndexOptions.S2) }
                }
            };

            hostLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            joinedLobby = hostLobby;

            PrintPlayers(hostLobby);

            Debug.Log("Created Lobby: " + hostLobby.Name + ", " + hostLobby.MaxPlayers + ", " + hostLobby.Id + ", " + hostLobby.LobbyCode);
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Count = 25,
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    new QueryFilter(QueryFilter.FieldOptions.S1, "DeathMatch", QueryFilter.OpOptions.EQ) // Look for gamemode deathmatch, since "GameMode" is using FieldOptions S1
                },
                Order = new List<QueryOrder>
                {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };


            queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log($"{lobby.Name}: {lobby.MaxPlayers}");
            }
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer(),
            };

            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);

            Debug.Log($"Joined Lobby: {lobbyCode}");

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void QuickJoinLobby()
    {
        try
        {
            QuickJoinLobbyOptions quickJoinLobbyOptions = new QuickJoinLobbyOptions
            {
                Player = GetPlayer(),
            };

            joinedLobby = await Lobbies.Instance.QuickJoinLobbyAsync(quickJoinLobbyOptions);

            Debug.Log($"Joined Lobby: {joinedLobby.LobbyCode}");

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            Debug.Log($"{playerName} left");
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void KickPlayer(string playerName)
    {
        try
        {
            List<Player> players = joinedLobby.Players;
            Player player = players.Find(p => { return p.Data["PlayerName"].Value == playerName; });

            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, player.Id);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log($"Players in Lobby {lobby.Name}, Gamemode: {lobby.Data["GameMode"].Value}, Map: {lobby.Data["Map"].Value}");

        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + ": " + player.Data["PlayerName"].Value);
        }
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
        };
    }

    [Command]
    private async void UpdateDataValue(string dataKey, string dataValue)
    {
       try
        {
            Dictionary<string, DataObject> data = hostLobby.Data;

            data[dataKey] = new DataObject(DataObject.VisibilityOptions.Public, dataValue);

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = data
            });

            PrintPlayers(hostLobby);
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void UpdatePlayerValue(string playerKey, string playerValue)
    {
        try
        {
            if (playerKey == "PlayerName") playerName = playerValue;

            joinedLobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    { playerKey, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerValue) }
                }
            });
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private void MigrateLobbyHost()
    {
        try
        {
            List<Player> players = joinedLobby.Players;

            string playerName = players[Random.Range(1, players.Count)].Data["PlayerName"].Value;

            MigrateLobbyHost(playerName);
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void MigrateLobbyHost(string playerName)
    {
        try
        {
            List<Player> players = joinedLobby.Players;
            Player player = players.Find(p => { return p.Data["PlayerName"].Value == playerName; });

            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = player.Id
            });

            PrintPlayers(hostLobby);
            Debug.Log($"{playerName} is host");
        }

        catch (LobbyServiceException e) { Debug.Log(e); }
    }

    [Command]
    private async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e) { Debug.Log(e); }
    }
}
