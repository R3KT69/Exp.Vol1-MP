using PurrNet;
using TMPro;
using UnityEngine;


public class PlayerProfileNet : NetworkIdentity
{
    public Color color;
    public TMP_Text health_text, name_text;
    public Renderer rend;
    public SyncVar<int> health = new(initialValue: 100);
    
    public NetworkIdentity networkIdentity;

    private void Awake()
    {
        health.onChanged += OnHealthChanged;
        networkIdentity = GetComponent<NetworkIdentity>();
    }

    private void Start()
    {
        if (isOwner)
        {
            if (string.IsNullOrWhiteSpace(Connection_Menu.PlayerName))
            {
                SetPlayerNameServerRpc("Guest");
            } else
            {
                SetPlayerNameServerRpc(Connection_Menu.PlayerName);
            }
            
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        health.onChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int newValue)
    {
        health_text.text = newValue.ToString();
        Debug.Log($"Health : {newValue}");
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        Camera cam = GetComponentInChildren<Camera>();
        if (cam == null) return;

        if (!isOwner)
        {
            cam.gameObject.SetActive(false);
            cam.tag = "Untagged";
        }
        else
        {
            // This is the local player's camera
            cam.gameObject.SetActive(true);
            cam.tag = "MainCamera";
        }
    }


    private void Update()
    {
        /*-----------Client Specific Codes-----------*/
        if (!isOwner) return;
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            color = new Color(Random.value, Random.value, Random.value);
            SetColor(color);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log($"isServer: {isServer}, isOwner: {isOwner}, id: {networkIdentity.localPlayer.Value}");
        }
        

        /*-----------Server Specific Codes-----------
        if (!isServer) return;
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerID clientId = new PlayerID(002, false);
            TargetExample(clientId, "Sent from Server");
        }*/
        
    }


    [ObserversRpc(bufferLast: true)]
    private void SetColor(Color newColor)
    {
        color = newColor;
        rend.material.color = color;
    }

    [ServerRpc]
    public void TakeDamage(int damage)
    {
        health.value -= damage;

        if (health.value <= 0)
        {
            health.value = 0;
            ChangeNameplateColor(Color.red);
            Debug.Log("Has Died");
        }
    }

    [ObserversRpc(bufferLast: true)]
    private void SetPlayerNameServerRpc(string name)
    {
        name_text.text = name;
    }

    [ObserversRpc(bufferLast: true)]
    public void ChangeNameplateColor(Color color)
    {
        name_text.color = color;
    }

/*
    [TargetRpc]
    private void TargetExample(PlayerID target, string message)
    {
       Debug.Log($"[Server] Sent to PlayerID: {target} Message: {message}");
    }
*/
    
}
