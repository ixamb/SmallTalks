using UnityEngine;

namespace SmallTalks.UI.Tools
{
    public sealed class HorizontalViewNavElement : MonoBehaviour
    {
        [SerializeField] private int index;
        
        public int Index => index;
    }
}