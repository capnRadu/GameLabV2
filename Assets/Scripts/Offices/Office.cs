using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Office : NetworkBehaviour
{
    public string streetName;
    public int maxEmployeesIncrease;
    public int cost;
    public string owningPlayer = null;

    [ServerRpc(RequireOwnership = false)]
    public void UpdateOfficeOwnershipServerRpc()
    {
        PlayersManager.Instance.UpdateOfficeOwnershipClientRpc();
    }
}
