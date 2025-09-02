using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    public TMP_InputField chatInput;
    public Button sendButton;
    public Transform chatContent;
    public GameObject chatMessagePrefab;
    public ChatBubble playerBubble;

    void Start()
    {
        try
        {
            if (sendButton != null)
                sendButton.onClick.AddListener(SendMessage);
            else
                Debug.LogError("ChatManager: sendButton не назначен!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка в ChatManager.Start: " + e);
        }
    }

    public void SendMessage()
    {
        if (chatInput == null)
        {
            Debug.LogError("ChatManager: chatInput не назначен!");
            return;
        }

        string message = chatInput.text.Trim();
        if (string.IsNullOrEmpty(message)) return;

        AddMessage("Player", message);
        chatInput.text = "";

        if (playerBubble != null)
        {
            playerBubble.ShowMessage(message);
        }
    }

    public void AddMessage(string nick, string message)
    {
        if (chatMessagePrefab == null || chatContent == null)
        {
            Debug.LogError("ChatManager: chatMessagePrefab или chatContent не назначены!");
            return;
        }

        GameObject msg = Instantiate(chatMessagePrefab, chatContent);
        TMP_Text text = msg.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = $"<b>{nick}:</b> {message}";
        }
        else
        {
            Debug.LogError("В ChatMessage не найден компонент TMP_Text!");
        }
    }
}
