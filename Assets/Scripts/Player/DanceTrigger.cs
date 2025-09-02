using UnityEngine;

public class DanceTrigger : MonoBehaviour
{
    [Header("Player Sprites")]
    public GameObject idleSprite;   // Обычный спрайт игрока
    public GameObject danceSprite;  // Спрайт с анимацией

    private Animator animator;

    void Start()
    {
        if (danceSprite != null)
        {
            animator = danceSprite.GetComponent<Animator>();
            danceSprite.SetActive(false); // Скрываем анимацию в начале
        }
    }

    private void StartDance(string animName)
    {
        if (idleSprite != null) idleSprite.SetActive(false);
        if (danceSprite != null)
        {
            danceSprite.SetActive(true);
            animator.Play(animName);
        }
    }

    public void PlayRightArm()
    {
        StartDance("RightArm");
    }

    public void PlayLeftArm()
    {
        StartDance("LeftArm");  // исправил название (не нужно .anim!)
    }

    public void PlayLegs()
    {
        StartDance("Legs1");
    }

    public void PlayTurn()
    {
        StartDance("Turn");
    }

    public void StopDance()
    {
        if (danceSprite != null) danceSprite.SetActive(false);
        if (idleSprite != null) idleSprite.SetActive(true);
    }
}
