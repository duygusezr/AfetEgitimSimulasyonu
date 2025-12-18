using UnityEngine;

public class WaterRiseController : MonoBehaviour
{
    [Header("Water Settings")]
    public float riseSpeed = 0.5f;       // saniyede ne kadar yükseliyor
    public float maxHeight = 3f;          // oda dolmuþ sayýlacaðý yükseklik

    [Header("State")]
    public bool waterActive = false;      // görevler baþarýsýz olunca true

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        if (!waterActive) return;

        float newY = transform.localScale.y + riseSpeed * Time.deltaTime;
        newY = Mathf.Clamp(newY, startScale.y, maxHeight);

        transform.localScale = new Vector3(
            startScale.x,
            newY,
            startScale.z
        );

        transform.position = new Vector3(
            transform.position.x,
            newY / 2f,
            transform.position.z
        );
    }
}
