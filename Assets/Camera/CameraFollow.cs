using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;          // Referência ao jogador
    public Vector3 offset;            // Offset da câmera em relação ao jogador
    public SpriteRenderer background; // Referência ao fundo (SpriteRenderer do fundo)
    public float parallaxEffect = 1f; // Efeito de paralaxe para o fundo (ajustável)

    private Camera mainCamera;

    void Start()
    {
        // Verifica se a câmera principal está configurada corretamente
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Câmera principal não encontrada. Verifique se a tag da câmera está definida como 'MainCamera'.");
            return;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Atualiza a posição da câmera
            transform.position = player.position + offset;
        }

        // Verifica se o background está definido e aplica o efeito de paralaxe
        if (background != null)
        {
            // Calcula a nova posição do background com o efeito de paralaxe
            Vector3 backgroundPosition = new Vector3(
                mainCamera.transform.position.x * parallaxEffect,
                mainCamera.transform.position.y * parallaxEffect,
                background.transform.position.z); // Mantém a posição Z do background

            // Atualiza a posição do background
            background.transform.position = backgroundPosition;
        }
    }
}
