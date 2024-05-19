using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AsteroidGame : MonoBehaviour
{
    [SerializeField] private Button[] asteroidPrefab;
    [SerializeField] private RectTransform spawnPos;
    int countDestroyed = 0;
    [SerializeField] private Text txtDestroyed;
    [SerializeField] private int countDestoyed;
    float spawnDelay = 2f;
    private void OnEnable()
    {
        PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
        foreach (var asteroid in asteroidPrefab)
        {
            if (asteroid.gameObject.activeSelf)
            {
                asteroid.gameObject.SetActive(false);
            }
        }
        InvokeRepeating("SpawnAsteroid", spawnDelay, spawnDelay);
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
        ResetValue();
    }
    
    void SpawnAsteroid()
    {
        foreach (var asteroid in asteroidPrefab)
        {
            if (!asteroid.gameObject.activeSelf)
            {
                asteroid.transform.localPosition = RandomPosition();
                asteroid.gameObject.SetActive(true);
                asteroid.onClick.RemoveAllListeners();
                asteroid.onClick.AddListener(() => ShootAsteroid(asteroid));
                if (gameObject.activeInHierarchy)
                {
                    StartCoroutine(MoveAsteroid(asteroid));
                }
                break;
            }
        }
    }

    Vector2 RandomPosition()
    {
        float randomX = Random.Range(spawnPos.rect.width * 0.5f, spawnPos.rect.width);
        float randomY = Random.Range(0f, spawnPos.rect.height);
        return new Vector2(randomX, randomY);
    }
    IEnumerator MoveAsteroid(Button asteroid)
    {
        RectTransform asteroidRect = asteroid.GetComponent<RectTransform>();
        Vector2 direction = new Vector2(-1f, Random.Range(-0.5f, 0.5f)).normalized;
        float speed = 300f;

        while (asteroid != null && asteroid.gameObject.activeSelf)
        {
            asteroidRect.anchoredPosition += direction * speed * Time.deltaTime;
            if (asteroidRect.anchoredPosition.x < -1920 || asteroidRect.anchoredPosition.y > 540 || asteroidRect.anchoredPosition.y < -540)
            {
                asteroidRect.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
    public void ShootAsteroid(Button asteroid)
    {
        updateDestroyed();
        asteroid.gameObject.SetActive(false);
    }
    void updateDestroyed()
    {
        countDestroyed++;
        txtDestroyed.text = "Destroyed: " + countDestroyed.ToString();
        if (countDestroyed == countDestoyed)
        {
            Debug.Log("Win");
            InventoryManager.Instance.isWinMiniGame = true;
        }
    }
    void ResetValue()
    {
        updateDestroyed();
    }
}
