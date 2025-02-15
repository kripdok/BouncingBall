using TMPro;
using UnityEngine;

public class UIExample : MonoBehaviour
{
    [SerializeField] private CustomRigidbody rigidbody;
    [SerializeField] private TMP_Text _position;
    [SerializeField] private TMP_Text _vilocity;

    // Update is called once per frame
    void Update()
    {
        _position.text = rigidbody.transform.position.ToString();
        _vilocity.text = rigidbody.TestVelocity.ToString();
    }
}
