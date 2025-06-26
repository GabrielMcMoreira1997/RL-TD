using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform target; // inimigo a seguir
    private Vector3 offset; // posição acima do inimigo
    public Image fillImage;
    private Canvas canvas;

    public void setOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }

    void Start()
    {

        canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            // Acessa ou define a câmera que o Canvas vai usar para eventos (cliques, raycast, etc)
            canvas.worldCamera = Camera.main;
        }  
    }

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }

    public void SetHealth(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }
}
