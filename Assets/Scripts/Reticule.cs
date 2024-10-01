using UnityEngine;

public class Reticule : MonoBehaviour
{
    public float moveSpeed = 0.1f;

    private Vector3 mousePosition;

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        this.transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
    }
}
