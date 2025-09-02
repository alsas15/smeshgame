using UnityEngine;
using Mirror;

public class PlayerNetwork : NetworkBehaviour
{
    [SyncVar]
    public string nickname = "Гость";

    [SyncVar]
    Vector2 syncPosition;

    void Update()
    {
        if (!isLocalPlayer) {
            transform.position = syncPosition;
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(moveX, moveY) * 5f * Time.deltaTime;
        transform.Translate(move);

        // отправим координаты на сервер
        if (move != Vector2.zero) {
            CmdMove(transform.position);
        }
    }

    [Command]
    void CmdMove(Vector2 newPos)
    {
        syncPosition = newPos;
    }
}
