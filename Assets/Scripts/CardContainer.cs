using System;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryGame
{
    public class CardContainer : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private int m_Rows = 2;
        [SerializeField] private int m_Columns = 2;
        [SerializeField] private float m_Spacing = 10f;
        [SerializeField] private float m_AspectRatio = 0.7f;

        [Header("Padding")]
        [SerializeField] private float m_PaddingLeft = 0f;
        [SerializeField] private float m_PaddingRight = 0f;
        [SerializeField] private float m_PaddingTop = 0f;
        [SerializeField] private float m_PaddingBottom = 0f;

        [Header("Cards")]
        [SerializeField] private List<RectTransform> m_Cards = new List<RectTransform>();

        private RectTransform _rect;

        void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            UpdateLayout();
        }

        void OnRectTransformDimensionsChange()
        {
            UpdateLayout();
        }

        public void UpdateLayout()
        {
            if (_rect == null) return;
            if (m_Cards.Count == 0) return;

            float w = _rect.rect.width - m_PaddingLeft - m_PaddingRight;
            float h = _rect.rect.height - m_PaddingTop - m_PaddingBottom;

            float cellW = (w - (m_Columns - 1) * m_Spacing) / m_Columns;
            float cellH = (h - (m_Rows - 1) * m_Spacing) / m_Rows;

            float cardW = Mathf.Min(cellW, cellH * m_AspectRatio);
            float cardH = cardW / m_AspectRatio;

            float gridW = cardW * m_Columns + m_Spacing * (m_Columns - 1);
            float gridH = cardH * m_Rows + m_Spacing * (m_Rows - 1);

            float offsetX = m_PaddingLeft + (w - gridW) * 0.5f;
            float offsetY = m_PaddingTop + (h - gridH) * 0.5f;

            int index = 0;

            for (int r = 0; r < m_Rows; r++)
            {
                for (int c = 0; c < m_Columns; c++)
                {
                    if (index >= m_Cards.Count) return;

                    RectTransform card = m_Cards[index];
                    card.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardW);
                    card.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cardH);

                    float x = offsetX + c * (cardW + m_Spacing);
                    float y = offsetY + r * (cardH + m_Spacing);

                    card.anchoredPosition = new Vector2(x, -y);

                    index++;
                }
            }
        }

        public void SetCards(List<RectTransform> cards)
        {
            m_Cards = cards;
            UpdateLayout();
        }

        public void SetGrid(int rows, int columns)
        {
            m_Rows = rows;
            m_Columns = columns;
            UpdateLayout();
        }
    }
}
