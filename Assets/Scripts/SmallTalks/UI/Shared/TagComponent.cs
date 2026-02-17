using TMPro;
using UnityEngine;

namespace SmallTalks.UI.Shared
{
    public sealed class TagComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text textTag;

        public void Initialize(string text)
        {
            textTag.text = text;
        }
    }
}