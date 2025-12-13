using System.Collections.Generic;
using UnityEngine;
using MemoryGame.Data;

namespace MemoryGame
{
    public class CardSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CardContainer m_CardContainer;
        [SerializeField] private CardSlot m_CardPrefab;
        [SerializeField] private CardImageData m_CardImageData;



        private readonly List<RectTransform> _cards = new();
        private readonly Dictionary<int, int> _gridIndexToCardId = new();
        private int _rows = 3;
        private int _columns = 4;
        
        public void Spawn(int rows, int columns)
        {
            Clear();

            int totalCards = rows * columns;
            if (totalCards % 2 != 0) return;

            int pairCount = totalCards / 2;
            List<int> cardIds = GenerateCardIds(pairCount);
            Shuffle(cardIds);

            for (int i = 0; i < cardIds.Count; i++)
            {
                int cardId = cardIds[i];
                Sprite sprite = m_CardImageData.CardImages[cardId];

                CardSlot card = Instantiate(m_CardPrefab, m_CardContainer.transform);
                card.SetCardId(cardId);
                card.SetFrontSprite(sprite);

                RectTransform rt = card.GetComponent<RectTransform>();
                _cards.Add(rt);

                _gridIndexToCardId[i] = cardId;
            }

            m_CardContainer.SetGrid(rows, columns);
            m_CardContainer.SetCards(_cards);
        }

        private List<int> GenerateCardIds(int pairCount)
        {
            List<int> uniqueIds = new();
            List<int> result = new();

            while (uniqueIds.Count < pairCount)
            {
                int id = Random.Range(0, m_CardImageData.CardImages.Count);
                if (!uniqueIds.Contains(id))
                    uniqueIds.Add(id);
            }

            for (int i = 0; i < uniqueIds.Count; i++)
            {
                result.Add(uniqueIds[i]);
                result.Add(uniqueIds[i]);
            }

            return result;
        }

        private void Clear()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i] != null)
                    Destroy(_cards[i].gameObject);
            }

            _cards.Clear();
            _gridIndexToCardId.Clear();
        }

        private void Shuffle(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public IReadOnlyDictionary<int, int> GetGridMap()
        {
            return _gridIndexToCardId;
        }

        public void SetGrid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }
    }
}
