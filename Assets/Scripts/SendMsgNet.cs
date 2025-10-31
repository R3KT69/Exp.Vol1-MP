using PurrNet;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SendMsgNet : NetworkIdentity
{
    public GameObject textObject;
    public TMP_InputField inputField;
    public Transform MsgBox;
    private string textToSend;
    public PlayerProfileNet playerProfile;

    void Awake()
    {
        inputField = GameObject.Find("Input").GetComponent<TMP_InputField>();
        MsgBox = GameObject.Find("Msg-Box").transform;
        playerProfile = GetComponent<PlayerProfileNet>();
    }

    void Update()
    {
        if (!isOwner) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            textToSend = $"[{playerProfile.networkIdentity.localPlayer.Value}] {playerProfile.name_text.text}: " + inputField.text;
            SendToAll(textToSend);
            inputField.text = null;
        }
    }

    [ObserversRpc(bufferLast: true)]
    void SendToAll(string message)
    {
        GameObject obj = Instantiate(textObject, MsgBox);
        obj.GetComponent<TextMeshProUGUI>().text = message;
    }
}
