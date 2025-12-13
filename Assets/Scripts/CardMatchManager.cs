using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace MemoryGame
{
    public class CardMatchManager : MonoBehaviour
    {
        [Header("Rules")]
        [SerializeField] private int m_MaxOpenCards = 2;
        [SerializeField] private float m_MismatchHideDelay = 0.5f;

        private readonly List<ICardView> _openCards = new List<ICardView>();
        public static event Action<List<int>> MatchedEvent;
        public static event Action MismatchEvent;


        void OnEnable()
        {
            CardSlot.ClickedEvent += OnCardClicked;
            MainUI.HomePressedEvent += OnHomePressed;
        }

        void OnDisable()
        {
            CardSlot.ClickedEvent -= OnCardClicked;
            MainUI.HomePressedEvent -= OnHomePressed;
        }

        private void OnHomePressed()
        {
            HideOpenCards(_openCards);
            _openCards.Clear();
            DOTween.KillAll();
        }

        private void OnCardClicked(ICardView card)
        {
            if (card.IsRevealed) return;

            card.Reveal();
            _openCards.Add(card);

            if (_openCards.Count == m_MaxOpenCards)
            {
                var cardsToCompare = _openCards.ToList();
                EvaluateOpenCards(cardsToCompare);
                _openCards.Clear();
            }
        }

        private void EvaluateOpenCards(List<ICardView> cardsToCompare)
        {
            if (cardsToCompare.Count < 2) return;

            if (cardsToCompare[0].CardId == cardsToCompare[1].CardId)
            {
                DOVirtual.DelayedCall(m_MismatchHideDelay*2, () =>
                {
                    int gridindex0 = ((CardSlot)cardsToCompare[0]).GridIndex;
                    int gridindex1 = ((CardSlot)cardsToCompare[1]).GridIndex;
                    MatchedEvent?.Invoke(new List<int>{gridindex0, gridindex1});
                    foreach (ICardView card in cardsToCompare) 
                        card.Disable();
                });
                
            }
            else
            {
                DOVirtual.DelayedCall(m_MismatchHideDelay, () =>
                {
                    MismatchEvent?.Invoke();
                    HideOpenCards(cardsToCompare);
                });
            }
        }

        private void HideOpenCards(List<ICardView> cardsToCompare)
        {
            foreach (var card in cardsToCompare)
                card.Hide();
        }
        
    }
}
