using UnityEngine;

public class GuvenliAlan : MonoBehaviour
{
    private TutorialManager manager;
    private Renderer render;
    private Material defaultMaterial;

    [Header("Görsel Ayarlar")]
    public Material girenOyuncuMateryali; // Ýçine girince yeþil olsun

    void Start()
    {
        manager = FindFirstObjectByType<TutorialManager>();
        render = GetComponent<Renderer>();
        if (render != null) defaultMaterial = render.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        // VR'da oyuncunun kafasý "MainCamera" etiketine sahiptir
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Oyuncu Güvenli Alana Girdi!");

            // Manager'a haber ver
            if (manager != null) manager.OyuncuGuvenliAlandaDurumu(true);

            // Görseli deðiþtir (Yeþil yap)
            if (render != null && girenOyuncuMateryali != null)
                render.material = girenOyuncuMateryali;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Oyuncu Güvenli Alandan Çýktý!");

            // Manager'a haber ver
            if (manager != null) manager.OyuncuGuvenliAlandaDurumu(false);

            // Rengi eski haline getir
            if (render != null)
                render.material = defaultMaterial;
        }
    }
}