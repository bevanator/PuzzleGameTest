using System.Collections.Generic;
using UnityEngine;

namespace MemoryGame.Data
{
    [CreateAssetMenu(menuName = "Card Image Data")]
    public class CardImageData : ScriptableObject
    {
        public List<Sprite> CardImages = new();
    }
}