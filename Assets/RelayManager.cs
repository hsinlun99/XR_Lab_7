using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using TMPro;


public class RelayManager : MonoBehaviour
{
    [SerializeField] private string joinCode;
    [SerializeField] private TMP_Text joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInput;
    private UnityTransport unityTransport;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        unityTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        SignIn();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void SignIn()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void HostRelay()
    {
        //1. Create an allocation for 2 people 
        int maxConnections = 2;
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        // 2. Create a join code
        joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        //Writing to our text UI so player can see the code
        joinCodeText.text = "Join Code: " + joinCode;
        //3. Configure the UnityTransport and tell it about our Relay server data
        unityTransport.SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
        //StartHost()
        NetworkManager.Singleton.StartHost();
    }

    public async void JoinRelay()
    {
        // 1.take the join code that the user inputs and call oinAllocationAsync
        string code = joinCodeInput.text;
        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(code);
        // 2. Configure the UnityTransport with our allocation
        unityTransport.SetClientRelayData(joinAllocation.RelayServer.IpV4, (ushort)joinAllocation.RelayServer.Port,
           joinAllocation.AllocationIdBytes, joinAllocation.Key, joinAllocation.ConnectionData, joinAllocation.HostConnectionData);
        // 3. StartClient()
        NetworkManager.Singleton.StartClient();
    }


}
