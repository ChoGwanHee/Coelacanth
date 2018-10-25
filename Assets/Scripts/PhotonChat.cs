using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChat : MonoBehaviour, IChatClientListener
{
    public string[] ChannelsToJoinOnConnect;

    public ChatClient chatClient;

    public Image chatBg;
    public Text chatText;
    public Scrollbar scrollbar;
    public Image scrollbarImg;
    public Image scrollHandle;
    public InputField inputFieldChat;
    public Button sendBtn;

    public string UserName { get; set; }

    private string selectedChannelName;


    private bool followingNewChat = true;
    private bool showMinimal = true;


    private void Start()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.ChatAppID))
        {
            Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
            return;
        }

        UserName = PhotonNetwork.player.NickName;
        Connect();

        Show(false);
    }

    private void Update()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
        }

        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputFieldChat.isFocused)
                return;

            if (showMinimal)
            {
                Show(true);
            }
            else
            {
                OnEnterSend();
            }
        }

    }

    public void Connect()
    {
        this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues(UserName));
        Debug.Log("Connecting as: " + UserName);
    }

    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
            inputFieldChat.readOnly = true;
            inputFieldChat.placeholder.GetComponent<Text>().text = "Disconnected";
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
            inputFieldChat.readOnly = true;
            inputFieldChat.placeholder.GetComponent<Text>().text = "Disconnected";
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            UnityEngine.Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        else
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public void OnDisconnected()
    {
        Debug.LogWarning("Photon Chat Disconnected");
    }

    public void OnConnected()
    {
        Debug.Log("Photon Chat Connected");

        this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, 0);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat State: " + state.ToString());
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.selectedChannelName))
        {
            // update text
            ShowChannel(this.selectedChannelName);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("OnSubscribed: " + string.Join(", ", channels));

        inputFieldChat.readOnly = false;
        inputFieldChat.placeholder.GetComponent<Text>().text = "Enter text...";
        ShowChannel(channels[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnEnterSend()
    {
        if (string.IsNullOrEmpty(inputFieldChat.text))
        {
            inputFieldChat.DeactivateInputField();
            Show(false);
            return;
        }
        SendChatMessage(this.inputFieldChat.text);
        this.inputFieldChat.text = "";
        this.inputFieldChat.ActivateInputField();
    }

    public void OnClickSend()
    {
        if (this.inputFieldChat != null)
        {
            SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }
    
    private void SendChatMessage(string inputLine)
    {
        this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this.selectedChannelName = channelName;
        this.chatText.text = channel.ToStringMessages();

        UpdateChatTextSize();
        if(followingNewChat)
            StartCoroutine(FollowNewChat());
        
        //Debug.Log("ShowChannel: " + this.selectedChannelName);
    }

    private void UpdateChatTextSize()
    {
        Canvas.ForceUpdateCanvases();

        int lineCount = chatText.cachedTextGenerator.lineCount;
        int lineHeight = 25;

        chatText.rectTransform.sizeDelta = new Vector2(0, (lineHeight * lineCount) - lineHeight);
    }

    private IEnumerator FollowNewChat()
    {
        yield return new WaitForEndOfFrame();
        scrollbar.value = 0.0f;
    }

    private void Show(bool isShow)
    {
        chatBg.enabled = isShow;
        inputFieldChat.gameObject.SetActive(isShow);
        sendBtn.gameObject.SetActive(isShow);
        scrollbarImg.enabled = isShow;
        scrollHandle.enabled = isShow;
        showMinimal = !isShow;


        if(isShow)
        {
            inputFieldChat.ActivateInputField();
        }
    }
}
