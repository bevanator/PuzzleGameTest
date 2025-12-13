using System;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryGame
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private Button m_Button2X2;
        [SerializeField] private Button m_Button2X3;
        [SerializeField] private Button m_Button5X6;

        public static event Action<int, int> LevelSelectedEvent;
        public static event Action ContinueSelectedEvent;

        private void OnEnable()
        {
            MainUI.HomePressedEvent += OnHomePressed;
        }

        private void OnDisable()
        {
            MainUI.HomePressedEvent -= OnHomePressed;
        }

        private void OnHomePressed()
        {
            m_ContinueButton.gameObject.SetActive(SaveManager.TryGetState(out _, out _, out _));
        }

        private void Start()
        {
            m_Button2X2.onClick.AddListener(() =>
            {
                LevelSelectedEvent?.Invoke(2, 2);
            });
            
            m_Button2X3.onClick.AddListener(() =>
            {
                LevelSelectedEvent?.Invoke(2, 3);
            });
            
            m_Button5X6.onClick.AddListener(() =>
            {
                LevelSelectedEvent?.Invoke(5, 6);
            });
            
            m_ContinueButton.gameObject.SetActive(SaveManager.TryGetState(out _, out _, out _));
            
            m_ContinueButton.onClick.AddListener(() =>
            {
                ContinueSelectedEvent?.Invoke();
            });
        }
    }
}