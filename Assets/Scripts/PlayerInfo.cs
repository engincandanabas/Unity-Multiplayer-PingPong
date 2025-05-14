using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>(writePerm: NetworkVariableWritePermission.Server);

    public void GameManager_OnGameStarted(object sender, EventArgs eventArgs)
    {
        if (this.GetComponent<PlayerController>().IsOwnerPlayer())
        {
            string playerName = PlayerPrefs.GetString("Username", "Player_" + OwnerClientId);
            Debug.Log("Username :" + playerName);

            SetPlayerNameRpc(playerName);
        }
    }
    [Rpc(SendTo.Server)]
    public void SetPlayerNameRpc(string name)
    {
        PlayerName.Value = name;
    }
}
