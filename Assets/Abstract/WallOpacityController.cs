using UnityEngine;

public class WallOpacityController : MonoBehaviour
{
    public float duration = 10f;
    public KeyCode key = KeyCode.Z;

    private RaycastHit hit;
    private Material originalMaterial;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    originalMaterial = renderer.material;

                    Material transparentMaterial = new Material(originalMaterial);
                    transparentMaterial.SetFloat("_Mode", 2); // Set rendering mode to transparent
                    transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    transparentMaterial.SetInt("_ZWrite", 0);
                    Color color = originalMaterial.color;
                    transparentMaterial.color = color;

                    renderer.material = transparentMaterial;

                    Invoke("ResetOpacity", duration);
                }
            }
        }
    }

    void ResetOpacity()
    {
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
    }
}