using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame
{
    public class ScoreUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_TurnText;


        void OnEnable()
        {
            ScoreManager.ScoreChangedEvent += OnScoreChanged;
            ScoreManager.TurnChangedEvent += OnTurnChanged;
        }

        void OnDisable()
        {
            ScoreManager.ScoreChangedEvent -= OnScoreChanged;
            ScoreManager.TurnChangedEvent -= OnTurnChanged;
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

    }
}
