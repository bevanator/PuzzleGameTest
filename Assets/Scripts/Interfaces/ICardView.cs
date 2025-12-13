using System;

namespace MemoryGame
{
    public interface ICardView
    {
        int CardId { get; }
        bool IsRevealed { get; }

        void Reveal();
        void Hide();
        void ResetCard();
        void Disable();
    }
}