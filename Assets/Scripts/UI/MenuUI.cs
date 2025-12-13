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
        }
    }
}