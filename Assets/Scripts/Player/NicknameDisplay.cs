using UnityEngine;
using TMPro;

public class NicknameDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMesh;  // Для UI-текста

    void Start()
    {
        string nick = PlayerPrefs.GetString("Nickname", "Гость");
        SetNickname(nick);
    }

    public void SetNickname(string nickname)
    {
        if (textMesh != null)
        {
            textMesh.text = nickname;
        }
    }
}
