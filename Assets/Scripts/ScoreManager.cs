using System;
using UnityEngine;

namespace MemoryGame
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Rules")]
        [SerializeField] private int m_MinTurnsToWin = 10;
        [SerializeField] private int m_TotalPairs = 8;

        public static event Action<int> ScoreChangedEvent;
        public static event Action<int> TurnChangedEvent;
        public static event Action GameWinEvent;
        public static event Action GameLoseEvent;

        public int Score { get; private set; }
        public int Turns { get; private set; }
        public int Combo { get; private set; }

        private int _matchedPairs;

        void OnEnable()
        {
            CardMatchManager.MatchedEvent += OnMatched;
            CardMatchManager.MismatchEvent += OnMismatch;
        }

        void OnDisable()
        {
            CardMatchManager.MatchedEvent -= OnMatched;
            CardMatchManager.MismatchEvent -= OnMismatch;
        }

        private void OnMatched()
        {
            Turns++;
            Combo++;

            int points = 1;
            if (Combo % 3 == 0)
                points += 2;

            Score += points;
            _matchedPairs++;

            ScoreChangedEvent?.Invoke(Score);
            TurnChangedEvent?.Invoke(Turns);

            CheckGameEnd();
        }

        private void OnMismatch()
        {
            Turns++;
            Combo = 0;

            TurnChangedEvent?.Invoke(Turns);
            CheckGameEnd();
        }

        private void CheckGameEnd()
        {
            if (Turns > m_MinTurnsToWin)
            {
                GameLoseEvent?.Invoke();
                return;
            }

            if (Turns <= m_MinTurnsToWin)
            {
                if(_matchedPairs == m_TotalPairs) GameWinEvent?.Invoke();
            }
        }

        public void ResetGame()
        {
            Score = 0;
            Turns = 0;
            Combo = 0;
            _matchedPairs = 0;

            ScoreChangedEvent?.Invoke(Score);
            TurnChangedEvent?.Invoke(Turns);
        }
    }
}