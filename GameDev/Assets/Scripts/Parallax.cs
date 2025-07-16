using UnityEngine;

public class Parallax : MonoBehaviour
{

    private float length, startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
 
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void Update()
    {
        float newPos = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + newPos, transform.position.y, transform.position.z);
    }

}
