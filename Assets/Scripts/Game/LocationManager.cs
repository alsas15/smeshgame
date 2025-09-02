using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocationManager : MonoBehaviour
{
    public SpriteRenderer mapRenderer;
    public AudioSource musicPlayer;

    [System.Serializable]
    public class Location
    {
        public string name;
        public Sprite mapSprite;
        public AudioClip musicClip;
        public GameObject noWalkZones;
    }

    public List<Location> locations;
    public GameObject cafe;

    private string currentLocation;
    private GameObject player;

    void Awake()
    {
        try
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
                Debug.LogWarning("LocationManager: объект с тегом Player не найден!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка в LocationManager.Awake: " + e);
        }
    }

    void Start()
    {
        try
        {
            FitSpriteToScreen(mapRenderer);
            ChangeLocation("forest");

            if (musicPlayer != null && musicPlayer.clip != null)
            {
                musicPlayer.volume = 0.3f;
                musicPlayer.Play();
            }

            if (cafe != null)
                cafe.SetActive(false);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ошибка в LocationManager.Start: " + e);
        }
    }

    public void ChangeLocation(string name)
    {
        Location loc = locations.Find(l => l.name == name);
        if (loc == null)
        {
            Debug.LogWarning("Локация не найдена: " + name);
            return;
        }

        currentLocation = name;

        foreach (var l in locations)
        {
            if (l.noWalkZones != null)
                l.noWalkZones.SetActive(false);
        }

        if (loc.noWalkZones != null)
            loc.noWalkZones.SetActive(true);

        if (mapRenderer != null)
        {
            mapRenderer.sprite = loc.mapSprite;
            FitSpriteToScreen(mapRenderer);
        }

        if (musicPlayer != null)
        {
            musicPlayer.Stop();
            musicPlayer.clip = loc.musicClip;
            musicPlayer.volume = 0.3f;
            musicPlayer.Play();
        }

        if (player != null)
            player.transform.position = Vector3.zero;

        if (cafe != null)
            cafe.SetActive(name == "beach");

        Debug.Log("Перешёл в локацию: " + name);
    }

    private void FitSpriteToScreen(SpriteRenderer renderer)
    {
        if (renderer == null || renderer.sprite == null) return;

        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float spriteHeight = renderer.sprite.bounds.size.y;
        float spriteWidth = renderer.sprite.bounds.size.x;

        float scaleY = screenHeight / spriteHeight;
        float scaleX = screenWidth / spriteWidth;

        renderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        float worldSpriteWidth = spriteWidth * scaleX;
        float extraWidth = worldSpriteWidth - screenWidth;

        renderer.transform.position = new Vector3(-extraWidth / 2f, 0f, 0f);
    }
}
