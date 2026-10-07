using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void setScore(int score)
    {
        scoreText.SetText(score.ToString());
    }
}
