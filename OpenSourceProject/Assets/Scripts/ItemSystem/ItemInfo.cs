using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "ItemInfo", menuName = "ItemInfo", order = 0)]
    public class ItemInfo : ScriptableObject
    {
        [SerializeField]
        private string itemName;
        /// <summary>
        /// 아이템의 이름입니다.
        /// </summary>
        public string ItemName
        {
            get { return itemName; }
        }

        [SerializeField]
        private string itemDescription;
        /// <summary>
        /// 아이템의 설명입니다.
        /// </summary>
        public string ItemDescription
        {
            get { return itemDescription; }
        }

        [SerializeField]
        private Sprite itemSprite;
        /// <summary>
        /// 아이템의 이미지입니다.
        /// </summary>
        public Sprite ItemSprite
        {
            get { return itemSprite; }
        }

        [SerializeField]
        private int itemID;
        /// <summary>
        /// 아이템의 ID입니다.
        /// ID를 통해 아이템의 효과가 결정됩니다.
        /// </summary>
        public int ItemID
        {
            get { return itemID; }
        }

    }
}