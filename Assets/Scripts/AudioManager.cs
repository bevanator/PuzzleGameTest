using System.Collections.Generic;
using UnityEngine;

namespace MemoryGame
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private AudioSource m_Source;

        [Header("Clips")]
        [SerializeField] private AudioClip m_FlipClip;
        [SerializeField] private AudioClip m_MatchClip;
        [SerializeField] private AudioClip m_MismatchClip;
        [SerializeField] private AudioClip m_GameOverClip;

        void OnEnable()
        {
            CardSlot.ClickedEvent += OnCardClicked;
            CardMatchManager.MatchedEvent += OnMatched;
            CardMatchManager.MismatchEvent += OnMismatch;
            ScoreManager.GameLoseEvent += PlayGameOver;
        }

        void OnDisable()
        {
            CardSlot.ClickedEvent -= OnCardClicked;
            CardMatchManager.MatchedEvent -= OnMatched;
            CardMatchManager.MismatchEvent -= OnMismatch;
            ScoreManager.GameLoseEvent -= PlayGameOver;
        }

        private void OnCardClicked(ICardView card)
        {
            Play(m_FlipClip);
        }

        private void OnMatched(List<int> gridPosList)
        {
            Play(m_MatchClip);
        }

        private void OnMismatch()
        {
            Play(m_MismatchClip);
        }

        public void PlayGameOver()
        {
            Play(m_GameOverClip);
        }

        private void Play(AudioClip clip)
        {
            if (clip == null) return;
            m_Source.PlayOneShot(clip);
        }
    }
}