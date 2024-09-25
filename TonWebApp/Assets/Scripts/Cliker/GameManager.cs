using TMPro;
using UnityEngine;

namespace Cliker
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int _score;
        [SerializeField] private TextMeshProUGUI _textMeshPro;

        public void ClickButttonHandler()
        {
            _score += 1;
            _textMeshPro.text = $"Score: {_score}";
        }
    }
}