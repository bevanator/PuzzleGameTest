using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private CardSpawner m_CardSpawner;
        [SerializeField] private Button m_HomeButton;


        private void Start()
        {
            m_HomeButton.onClick.AddListener(() =>
            {
                SetGamePanelState(false);
            });
        }

        private void OnEnable()
        {
            MenuUI.LevelSelectedEvent += OnLevelSelected;
        }

        private void OnDisable()
        {
            MenuUI.LevelSelectedEvent -= OnLevelSelected;
        }

        private void OnLevelSelected(int row, int column)
        {
            SetGamePanelState(true);
            m_CardSpawner.Spawn(row, column);
        }

        private void SetGamePanelState(bool state)
        {
            m_Canvas.gameObject.SetActive(state);
        }
    }
}