using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public Vector3 offset;   // Offset da câmera em relação ao jogador
    public SpriteRenderer background; // Referência ao fundo (SpriteRenderer do fundo)
    public float parallaxEffect = 0.5f; // Efeito de paralaxe para o fundo (ajustável)

    void Start()
    {
        // Verifica se a câmera principal está configurada corretamente
        if (Camera.main == null)
        {
            Debug.LogError("Câmera principal não encontrada. Verifique se a tag da câmera está definida como 'MainCamera'.");
            return;
        }

        // Verifica se o fundo está atribuído
        if (background == null)
        {
            Debug.LogError("O SpriteRenderer do fundo não foi atribuído no Inspector.");
            return;
        }

        // Ajusta o tamanho do fundo para preencher a tela
        AdjustBackgroundSize();
    }

    void Update()
    {
        if (player != null)
        {
            // Atualiza a posição da câmera para seguir o jogador
            transform.position = player.position + offset;

            // Atualiza a posição do fundo com efeito de paralaxe
            if (background != null)
            {
                float newX = player.position.x * parallaxEffect;
                background.transform.position = new Vector3(newX, background.transform.position.y, background.transform.position.z);
            }
        }
    }

    // Função para ajustar o tamanho do fundo para preencher a tela
    void AdjustBackgroundSize()
    {
        if (background != null)
        {
            Camera camera = Camera.main;
            float screenAspect = (float)Screen.width / Screen.height;
            float screenHeight = camera.orthographicSize * 2;
            float screenWidth = screenHeight * screenAspect;

            // Ajusta a escala do fundo para preencher a tela
            background.transform.localScale = new Vector3(screenWidth, screenHeight, 1);
        }
    }
}
