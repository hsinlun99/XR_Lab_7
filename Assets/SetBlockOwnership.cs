using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class SetBlockOwnership : NetworkBehaviour
{
    private XRBaseInteractable jengaInteractable;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jengaInteractable = GetComponent<XRBaseInteractable>();
        jengaInteractable.hoverEntered.AddListener(ChangeOwnership);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeOwnership(HoverEnterEventArgs args)
    {
        RequestOwnershipChangeServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipChangeServerRpc(ulong clientId)
    {
        NetworkObject.ChangeOwnership(clientId);
    }

}
