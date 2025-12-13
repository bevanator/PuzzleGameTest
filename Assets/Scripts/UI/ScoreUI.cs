using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Text m_ScoreText;
        [SerializeField] private Text m_TurnText;
        [SerializeField] private GameObject m_WinPanel;
        [SerializeField] private GameObject m_LosePanel;

        void OnEnable()
        {
            ScoreManager.ScoreChangedEvent += OnScoreChanged;
            ScoreManager.TurnChangedEvent += OnTurnChanged;
            ScoreManager.GameWinEvent += OnWin;
            ScoreManager.GameLoseEvent += OnLose;
        }

        void OnDisable()
        {
            ScoreManager.ScoreChangedEvent -= OnScoreChanged;
            ScoreManager.TurnChangedEvent -= OnTurnChanged;
            ScoreManager.GameWinEvent -= OnWin;
            ScoreManager.GameLoseEvent -= OnLose;
        }

        private void OnScoreChanged(int score)
        {
            if (m_ScoreText != null)
                m_ScoreText.text = score.ToString();
        }

        private void OnTurnChanged(int turns)
        {
            if (m_TurnText != null)
                m_TurnText.text = turns.ToString();
        }

        private void OnWin()
        {
            if (m_WinPanel != null)
                m_WinPanel.SetActive(true);
        }

        private void OnLose()
        {
            if (m_LosePanel != null)
                m_LosePanel.SetActive(true);
        }
    }
}
