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
    int randomCount = 6;
    [SerializeField] private Image BlurImage;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        PlayerMovement.Instance.LockCursor(true, CursorLockMode.None);
        StartCoroutine(PlaySequence());
    }
    private void OnDisable()
    {
        PlayerMovement.Instance.LockCursor(false, CursorLockMode.Locked);
        playerSequence.Clear();
        result.Clear();
        PreviewImage.color = Color.white;
        BlurImage.gameObject.SetActive(true);
    }
    IEnumerator PlaySequence()
    {
        for (int i = 0; i < randomCount; i++)
        {
            yield return new WaitForSeconds(flashDelay);
            Color randomColor = GetRandomColor();
            PreviewImage.color = randomColor;
            previousColor = randomColor;
            result.Add(randomColor);
        }
        
        BlurImage.gameObject.SetActive(false);
    }
    Color GetRandomColor()
    {
        Color randomColor = sequence[Random.Range(0, sequence.Count)];
        while (randomColor == previousColor)
        {
            randomColor = sequence[Random.Range(0, sequence.Count)];
        }
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
        }
        else
        {
            Debug.Log("Wrong sequence!");
        }
    }
}
