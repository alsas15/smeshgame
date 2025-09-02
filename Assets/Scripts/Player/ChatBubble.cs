using System.Collections;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    public TMP_Text chatText;   // Сюда перетягиваем Text (TMP) из Canvas
    public float showTime = 3f; // Сколько секунд отображать сообщение

    private Coroutine hideCoroutine;

    public void ShowMessage(string message)
    {
        chatText.text = message;
        chatText.gameObject.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterSeconds());
    }

    private IEnumerator HideAfterSeconds()
    {
        yield return new WaitForSeconds(showTime);
        chatText.gameObject.SetActive(false);
    }
}
