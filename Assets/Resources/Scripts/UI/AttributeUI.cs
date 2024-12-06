using TMPro;

using UnityEngine;

namespace UI
{
    public class AttributeUI : MonoBehaviour
    {
        [SerializeField] TMP_Text value;

        public string Value { get => value.text; set => this.value.text = value; }

        public Color Color { get => value.color; set => this.value.color = value; }
    }
}