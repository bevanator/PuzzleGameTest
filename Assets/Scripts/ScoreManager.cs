using System;
using System.Collections.Generic;
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
        public static event Action<int> ComboChangedEvent;
        public static event Action GameWinEvent;
        public static event Action GameLoseEvent;

        private int _score;
        private int _turns;
        private int _combo;

        private int _matchedPairs;

        void Start()
        {
            CardMatchManager.MatchedEvent += OnMatched;
            CardMatchManager.MismatchEvent += OnMismatch;
            CardSpawner.GridSpawnEvent += OnGridInit;
        }

        void OnDisable()
        {
            CardMatchManager.MatchedEvent -= OnMatched;
            CardMatchManager.MismatchEvent -= OnMismatch;
            CardSpawner.GridSpawnEvent -= OnGridInit;
        }

        private void OnGridInit(int row, int column)
        {
            m_TotalPairs = (row * column) / 2;
            if (SaveManager.TryGetState(out _score, out _turns)) 
                LoadGameState();
            else ResetGameState();
            
        }

        private void OnMatched(List<int> gridPosList)
        {
            _turns++;
            _combo++;
            if(_combo >= 2) ComboChangedEvent?.Invoke(_combo);

            int points = 1;
            if (_combo % 3 == 0)
                points += 2;

            _score += points;
            _matchedPairs++;

            ScoreChangedEvent?.Invoke(_score);
            TurnChangedEvent?.Invoke(_turns);

            CheckGameEnd();
            SaveManager.SaveGameState(_score, _turns);
        }

        private void OnMismatch()
        {
            _turns++;
            _combo = 0;
            ComboChangedEvent?.Invoke(_combo);
            TurnChangedEvent?.Invoke(_turns);
            CheckGameEnd();
            SaveManager.SaveGameState(_score, _turns);
        }

        private void CheckGameEnd()
        {
            if (_turns <= m_MinTurnsToWin)
            {
                if (_matchedPairs == m_TotalPairs)
                {
                    GameWinEvent?.Invoke();
                }
            }

            if (_turns > m_MinTurnsToWin)
            {
                {
                    GameLoseEvent?.Invoke();
                }
            }
        }

        private void LoadGameState()
        { 
            ScoreChangedEvent?.Invoke(_score);
            TurnChangedEvent?.Invoke(_turns);
        }


        private void ResetGameState()
        {
            _score = 0;
            _turns = 0;
            _combo = 0;
            _matchedPairs = 0;

            SaveManager.ResetState();
            
            ScoreChangedEvent?.Invoke(_score);
            TurnChangedEvent?.Invoke(_turns);
            ComboChangedEvent?.Invoke(_combo);
        }
    }
}