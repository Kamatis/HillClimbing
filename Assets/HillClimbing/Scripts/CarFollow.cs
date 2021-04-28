using UnityEngine;

public class CarFollow : MonoBehaviour
{
    public Transform car;
    public Vector2 offset;

    private void Update()
    {
        transform.position = new Vector3(car.position.x + offset.x, car.position.y + offset.y, -10f);
    }
}
