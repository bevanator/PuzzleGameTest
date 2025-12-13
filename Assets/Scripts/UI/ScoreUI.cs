using System;
using DG.Tweening;
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
        [SerializeField] private TextMeshProUGUI m_ComboText;

        private void Awake()
        {
            m_ComboText.text = "X2 COMBO";
            m_ComboText.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            ScoreManager.ScoreChangedEvent += OnScoreChanged;
            ScoreManager.TurnChangedEvent += OnTurnChanged;
            ScoreManager.ComboChangedEvent += OnComboChanged;
        }

        void OnDisable()
        {
            ScoreManager.ScoreChangedEvent -= OnScoreChanged;
            ScoreManager.TurnChangedEvent -= OnTurnChanged;
            ScoreManager.ComboChangedEvent -= OnComboChanged;
        }

        private void OnComboChanged(int combo)
        {
            m_ComboText.gameObject.SetActive(combo > 1);
            m_ComboText.text = "X" + combo + " COMBO";
            m_ComboText.rectTransform.DOKill();
            m_ComboText.rectTransform.DOPunchScale(Vector3.one * 1.5f, 0.5f);
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
