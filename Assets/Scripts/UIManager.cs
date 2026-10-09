using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image[] lives;

    public void setScore(int score)
    {
        scoreText.SetText(score.ToString());
    }

    public void SetLives(int livesRemaining)
    {
        switch(livesRemaining)
        {
            case 3:
            lives[0].enabled = true;
            lives[1].enabled = true;
            lives[2].enabled = true;
            break;
            case 2:
            lives[0].enabled = false;
            lives[1].enabled = true;
            lives[2].enabled = true;
            break;
            case 1:
            lives[0].enabled = false;
            lives[1].enabled = false;
            lives[2].enabled = true;
            break;
            case 0:
            lives[0].enabled = false;
            lives[1].enabled = false;
            lives[2].enabled = false;
            break;

        }
    }
}
