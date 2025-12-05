using System.Collections;
using UnityEngine;

public enum GorevTipi
{
    None,
    EsyaSabitleme,
    CantaHazirlama
}

public class TutorialManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public AudioSource sesKaynagi;

    [Header("Bölüm 1: Giriþ ve Tanýtým")]
    public AudioClip ses1_Giris;
    public AudioClip ses2_OrtamTanitim;

    [Header("Bölüm 2: Risk Avý (Sabitleme)")]
    public AudioClip ses3_SabitlemeGorevi;
    public AudioClip ses4_SabitlemeBasarili;

    // --- YENÝ EKLENEN KISIM: SAYAÇ AYARLARI ---
    [Header("Sabitleme Görevi Ayarlarý")]
    public int sabitlemeHedefSayisi = 6; // Kaç tane braket takýlmasý lazým? (Bunu 6 yapacaðýz)
    private int sabitlemeMevcutSayi = 0; // Þu an kaç tane takýldý?

    [Header("Bölüm 3: Çanta Hazýrlýðý")]
    public AudioClip ses5_CantaGorevi;
    public AudioClip ses6_CantaBasarili;

    // Çanta için de birden fazla eþya (su, fener vs.) toplanacaksa burayý kullanabiliriz
    [Header("Çanta Görevi Ayarlarý")]
    public int cantaHedefSayisi = 1;
    private int cantaMevcutSayi = 0;

    [Header("Bölüm 4: Deprem Öncesi ve Aný")]
    public AudioClip ses7_HayatUcgeni;
    public AudioClip ses8_DepremBasliyor;

    [Header("Bölüm 5: Tahliye ve Sonuç")]
    public AudioClip ses9_Tahliye;
    public AudioClip ses10_Bitis;

    [Header("Aktif Görev Durumu")]
    public GorevTipi aktifGorev = GorevTipi.None;

    private bool sabitlemeGoreviTamamlandi = false;
    private bool cantaGoreviTamamlandi = false;
    private bool depremSarsintisiBitti = false;

    void Start()
    {
        StartCoroutine(EgitimAkisi());
    }

    IEnumerator EgitimAkisi()
    {
        // --- ADIM 1: GÝRÝÞ ---
        yield return StartCoroutine(SesCalVeBekle(ses1_Giris));
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(SesCalVeBekle(ses2_OrtamTanitim));
        yield return new WaitForSeconds(1f);

        // --- ADIM 2: EÞYA SABÝTLEME ---
        // Sayacý sýfýrla ki garanti olsun
        sabitlemeMevcutSayi = 0;

        yield return StartCoroutine(SesCalVeBekle(ses3_SabitlemeGorevi));

        aktifGorev = GorevTipi.EsyaSabitleme;

        Debug.Log("Kullanýcýnýn 6 adet eþyayý sabitlemesi bekleniyor...");
        // Görev deðiþkeni TRUE olana kadar burada bekler
        yield return new WaitUntil(() => sabitlemeGoreviTamamlandi);

        aktifGorev = GorevTipi.None;

        yield return StartCoroutine(SesCalVeBekle(ses4_SabitlemeBasarili));
        yield return new WaitForSeconds(1f);

        // --- ADIM 3: ÇANTA HAZIRLAMA ---
        cantaMevcutSayi = 0; // Çanta sayacýný sýfýrla

        yield return StartCoroutine(SesCalVeBekle(ses5_CantaGorevi));

        aktifGorev = GorevTipi.CantaHazirlama;

        Debug.Log("Kullanýcýnýn çantayý hazýrlamasý bekleniyor...");
        yield return new WaitUntil(() => cantaGoreviTamamlandi);

        aktifGorev = GorevTipi.None;

        yield return StartCoroutine(SesCalVeBekle(ses6_CantaBasarili));
        yield return new WaitForSeconds(1f);

        // --- ADIM 4: HAYAT ÜÇGENÝ VE DEPREM ---
        yield return StartCoroutine(SesCalVeBekle(ses7_HayatUcgeni));
        Debug.Log("30 Saniye Hayat Üçgeni Arama Süresi...");
        yield return new WaitForSeconds(30f);

        yield return StartCoroutine(SesCalVeBekle(ses8_DepremBasliyor));

        Debug.Log("Sarsýntýnýn bitmesi bekleniyor...");
        yield return new WaitUntil(() => depremSarsintisiBitti);

        // --- ADIM 5: TAHLÝYE VE SONUÇ ---
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SesCalVeBekle(ses9_Tahliye));
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(SesCalVeBekle(ses10_Bitis));
    }

    IEnumerator SesCalVeBekle(AudioClip klip)
    {
        if (klip != null)
        {
            sesKaynagi.PlayOneShot(klip);
            yield return new WaitForSeconds(klip.length);
        }
    }

    // --- GÜNCELLENEN FONKSÝYON ---
    public void GoreviIlerlet(GorevTipi gelenGorev)
    {
        // Sadece aktif olan göreve ait bir iþlem yapýldýysa say
        if (aktifGorev != gelenGorev) return;

        if (gelenGorev == GorevTipi.EsyaSabitleme)
        {
            sabitlemeMevcutSayi++; // Sayacý 1 artýr
            Debug.Log($"Sabitleme Durumu: {sabitlemeMevcutSayi} / {sabitlemeHedefSayisi}");

            // Hedefe ulaþtýk mý?
            if (sabitlemeMevcutSayi >= sabitlemeHedefSayisi)
            {
                sabitlemeGoreviTamamlandi = true;
            }
        }
        else if (gelenGorev == GorevTipi.CantaHazirlama)
        {
            cantaMevcutSayi++;
            Debug.Log($"Çanta Durumu: {cantaMevcutSayi} / {cantaHedefSayisi}");

            if (cantaMevcutSayi >= cantaHedefSayisi)
            {
                cantaGoreviTamamlandi = true;
            }
        }
    }

    // Unity Event'leri bazen Enum yerine int göndermeyi sever, yedek fonksiyon:
    public void GoreviIlerlet(int gelenGorevIndex)
    {
        GoreviIlerlet((GorevTipi)gelenGorevIndex);
    }

    // Eski fonksiyon ismini kullanan yerler kýrýlmasýn diye bunu da tutabiliriz
    // Ama Inspector'da yenisini seçeceðiz.
    public void GoreviTamamla(GorevTipi t) => GoreviIlerlet(t);

    public void DepremBitti()
    {
        depremSarsintisiBitti = true;
    }
}