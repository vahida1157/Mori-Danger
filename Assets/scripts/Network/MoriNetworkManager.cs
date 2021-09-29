using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;
using Random = UnityEngine.Random;

namespace Player
{
    public class MoriNetworkManager : MonoBehaviour
    {
        public static string username;
        private static string _hostIP = "127.0.0.1";

        // private void Update()
        // {
        //     if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
        //                 out var networkedClient))
        //     {
        //         var player = networkedClient.PlayerObject.GetComponent<PlayerController>();
        //         if (player)
        //         {
        //             player.MovementControl(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //         }
        //     }
        // }
        private void Start()
        {
            username = "Username" + Random.Range(1, 10);
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                // SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            GUILayout.Label("Username: ");
            username = GUILayout.TextField(username, 40);
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            GUILayout.Label("Host IP: ");
            _hostIP = GUILayout.TextField(_hostIP, 15);
            if (GUILayout.Button("Client"))
            {
                NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = _hostIP;
                NetworkManager.Singleton.StartClient();
            }
            // if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" :
                NetworkManager.Singleton.IsServer ? "Server" : "Client";

            if (GUILayout.Button("Logout"))
            {
                if (mode.Equals("Host")) NetworkManager.Singleton.StopHost();
                else NetworkManager.Singleton.StopClient();
            }
            GUILayout.Label("Username: " + username);
            GUILayout.Label("Transport: " +
                            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        // static void SubmitNewPosition()
        // {
        //     if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        //     {
        //         if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
        //             out var networkedClient))
        //         {
        //             var player = networkedClient.PlayerObject.GetComponent<PlayerNetwork>();
        //             if (player)
        //             {
        //                 player.Move();
        //             }
        //         }
        //     }
        // }
    }
}