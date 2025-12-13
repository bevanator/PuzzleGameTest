using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace MemoryGame
{
    public class CardSlot : MonoBehaviour, IPointerClickHandler, ICardView
    {
        [Header("Card")]
        [SerializeField] private int m_CardId;
        [SerializeField] private Image m_Image;
        [SerializeField] private Sprite m_BackSprite;
        [SerializeField] private Sprite m_FrontSprite;
        [SerializeField] private float m_FlipDuration = 0.15f;
        [SerializeField] private float m_ScaleDownFactor = 0.9f;

        public static event Action<ICardView> ClickedEvent;

        public int CardId => m_CardId;
        public bool IsRevealed { get; private set; }

        private Sequence _sequence;
        public int GridIndex { get; private set; }

        public void SetGridIndex(int index)
        {
            GridIndex = index;
        }


        private void Awake()
        {
            m_Image.sprite = m_BackSprite;
            m_Image.rectTransform.localScale = Vector3.one;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_sequence != null && _sequence.IsPlaying()) return;
            ClickedEvent?.Invoke(this);
        }

        public void Reveal()
        {
            if (IsRevealed) return;
            PlayFlip(m_FrontSprite, true);
        }

        public void Hide()
        {
            if (!IsRevealed) return;
            PlayFlip(m_BackSprite, false);
        }

        private void PlayFlip(Sprite targetSprite, bool revealed)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            IsRevealed = revealed;
            _sequence.Append(m_Image.transform.DOScale(m_ScaleDownFactor, m_FlipDuration));
            _sequence.Append(m_Image.transform.DOScale(1f, m_FlipDuration));
            _sequence.Append(m_Image.transform.DOScaleX(0f, m_FlipDuration))
                     .AppendCallback(() => m_Image.sprite = targetSprite);
            _sequence.Append(m_Image.transform.DOScaleX(1f, m_FlipDuration));

        }

        public void SetFrontSprite(Sprite sprite)
        {
            m_FrontSprite = sprite;
            if (IsRevealed) m_Image.sprite = m_FrontSprite;
        }

        public void ResetCard()
        {
            _sequence?.Kill();
            IsRevealed = false;
            m_Image.transform.localScale = Vector3.one;
            m_Image.sprite = m_BackSprite;
        }

        public void Disable()
        {
            m_Image.transform.DOScale(0f, m_FlipDuration)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        public void SetCardId(int cardId)
        {
            m_CardId = cardId;
        }
    }
}
