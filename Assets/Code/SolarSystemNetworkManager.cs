using SpaceShipRun.Characters;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace SpaceShipRun.Main
{
    [Obsolete]
    public sealed class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] private string _playerName;
        [SerializeField][Min(1.0f)] private float asd;

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = GetStartPosition();
            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);

            player.GetComponent<ShipController>().PlayerName = _playerName;
            
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}