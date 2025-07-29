using UnityEngine;

public class FloatObject : MonoBehaviour
{
    // Amplitude of the floating motion
    public float amplitude = 0.5f;
    // Speed of the floating motion
    public float frequency = 1f;
    // Highlight material to make the object glow
    public Material highlightMaterial;
    // Emission color for pulsing glow
    public Color emissionColor = Color.yellow;
    // Initial position
    private Vector3 startPos;
    // Reference to the renderer
    private Renderer rend;

    void Start()
    {
        startPos = transform.position;
        rend = GetComponent<Renderer>();
        if (highlightMaterial != null && rend != null)
        {
            rend.material = highlightMaterial;
            rend.material.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        // Animate emission for glowing effect
        if (highlightMaterial != null && rend != null)
        {
            float glow = (Mathf.Sin(Time.time * frequency * 2f) + 1f) * 0.5f; // Range 0-1
            Color animatedEmission = emissionColor * glow;
            rend.material.SetColor("_EmissionColor", animatedEmission);
        }
    }
}
