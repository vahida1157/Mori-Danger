using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class PlayerNametag : NetworkBehaviour
    {
        [SerializeField] public Text playerUsername;

        public NetworkVariableString networkUsername = new NetworkVariableString(new NetworkVariableSettings()
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        });

        void Update()
        {
            if (IsLocalPlayer)
            {
                if (IsServer)
                {
                    networkUsername.Value = MoriNetworkManager.username;
                }
                else
                {
                    SetNametagServerRpc(MoriNetworkManager.username);
                }
            }

            playerUsername.text = networkUsername.Value;
        }

        [ServerRpc]
        private void SetNametagServerRpc(string username)
        {
            networkUsername.Value = username;
        }
    }
}