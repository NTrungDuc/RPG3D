using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    [SerializeField] private Image PreviewImage;
    [SerializeField] private List<Color> sequence;
    [SerializeField] private List<Color> playerSequence;
    [SerializeField] private List<Color> result;
    private Color previousColor;
    float flashDelay = 1f;
    [SerializeField] private int randomCount = 6;
    [SerializeField] private Image BlurImage;

    private void OnEnable()
    {
        PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
        StartCoroutine(PlaySequence());
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
        ResetValue();
    }
    IEnumerator PlaySequence()
    {
        for (int i = 0; i < randomCount; i++)
        {
            Color randomColor = GetRandomColor();
            PreviewImage.color = randomColor;
            yield return new WaitForSeconds(flashDelay);

            result.Add(randomColor);

            PreviewImage.color = Color.white;
            yield return new WaitForSeconds(flashDelay);

            previousColor = randomColor;
        }
        
        BlurImage.gameObject.SetActive(false);
    }
    Color GetRandomColor()
    {
        Color randomColor = sequence[Random.Range(0, sequence.Count)];
        return randomColor;
    }
    public void OnButtonClick(Image button)
    {
        playerSequence.Add(button.color);
        if (playerSequence.Count == randomCount)
        {
            CheckPlayerSequence();
        }
    }
    void CheckPlayerSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != result[i])
            {
                isCorrect = false;
                break;
            }
        }
        if(isCorrect)
        {
            Debug.Log("Correct sequence!");
            InventoryManager.Instance.isWinMiniGame = true;
        }
        else
        {
            Debug.Log("Wrong sequence!");
            ResetValue();
            StartCoroutine(PlaySequence());
        }
    }
    void ResetValue()
    {
        playerSequence.Clear();
        result.Clear();
        PreviewImage.color = Color.white;
        BlurImage.gameObject.SetActive(true);
    }
}
