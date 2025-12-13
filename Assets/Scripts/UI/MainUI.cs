using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private CardSpawner m_CardSpawner;
        [SerializeField] private GameObject m_SuccessPanel;
        [SerializeField] private GameObject m_LosePanel;
        [SerializeField] private Button m_HomeButtonGame;
        [SerializeField] private Button m_HomeButtonSuccess;
        [SerializeField] private Button m_HomeButtonFail;
        [SerializeField] private Button m_RetryButtonFail;
        private int _currentRow;
        private int _currentColumn;
        public static event Action HomePressedEvent;

        private void OnEnable()
        {
            MenuUI.LevelSelectedEvent += OnLevelSelected;
            MenuUI.ContinueSelectedEvent += OnContinueSelected;
            ScoreManager.GameWinEvent += OnWin;
            ScoreManager.GameLoseEvent += OnLose;
        }

        private void Start()
        {
            m_HomeButtonGame.onClick.AddListener(() =>
            {
                SetGamePanelState(false);
                HomePressedEvent?.Invoke();
            });
            m_HomeButtonSuccess.onClick.AddListener(() =>
            {
                m_SuccessPanel.SetActive(false);
                SaveManager.ClearSave();
                SetGamePanelState(false);
                HomePressedEvent?.Invoke();
            });
            m_HomeButtonFail.onClick.AddListener(() =>
            {
                m_LosePanel.SetActive(false);
                SaveManager.ClearSave();
                SetGamePanelState(false);
                HomePressedEvent?.Invoke();
            });

            m_RetryButtonFail.onClick.AddListener(() =>
            {
                m_LosePanel.SetActive(false);
                OnLevelSelected(_currentRow, _currentColumn);
            });

        }

        private void OnDisable()
        {
            MenuUI.LevelSelectedEvent -= OnLevelSelected;
            MenuUI.ContinueSelectedEvent -= OnContinueSelected;
            ScoreManager.GameWinEvent -= OnWin;
            ScoreManager.GameLoseEvent -= OnLose;
        }

        private void OnContinueSelected()
        {
            SetGamePanelState(true);
            m_CardSpawner.OnContinuePressed();
        }

        private void OnLevelSelected(int row, int column)
        {
            _currentRow = row;
            _currentColumn = column;
            SetGamePanelState(true);
            SaveManager.ClearSave();
            m_CardSpawner.Spawn(row, column);
        }

        private void SetGamePanelState(bool state)
        {
            m_Canvas.gameObject.SetActive(state);
        }
        
        private void OnWin()
        {
            if (m_SuccessPanel != null)
                m_SuccessPanel.SetActive(true);
        }

        private void OnLose()
        {
            if (m_LosePanel != null)
                m_LosePanel.SetActive(true);
        }
    }
}