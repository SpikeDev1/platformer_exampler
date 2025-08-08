namespace Gamemaker.Misc
{
    using UnityEngine;

    public class PartObject : MonoBehaviour
    {
        public string tag;

        public object Owner { get; set; }
        public Transform Transform { get; private set; }

        private void Start()
        {
            Transform = gameObject.GetComponent<Transform>();
        }
    }
}