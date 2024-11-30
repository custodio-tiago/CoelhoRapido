using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishFlag : MonoBehaviour
{
    public GameObject stageCompleteTextPrefab; // Prefab de texto para "STAGE COMPLETED"
    private GameObject stageCompleteTextInstance; // Instância do texto
    private bool playerReachedFinish = false; // Controle para pressionar ENTER

    private void Start()
    {
        // Garantir que o texto de "STAGE COMPLETED" não apareça no início
        if (stageCompleteTextPrefab != null)
        {
            stageCompleteTextInstance = Instantiate(stageCompleteTextPrefab, FindObjectOfType<Canvas>().transform);
            stageCompleteTextInstance.SetActive(false); // Desativa o texto no início
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o Player colidiu com a bandeirinha
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player alcançou a bandeira de fim de fase!");

            // Exibe o texto de "STAGE COMPLETED"
            if (stageCompleteTextPrefab != null)
            {
                stageCompleteTextInstance.SetActive(true); // Torna o texto visível
                stageCompleteTextInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // Centraliza o texto
            }

            playerReachedFinish = true; // Habilita o próximo passo

            // Pausa o jogo
            Time.timeScale = 0f; // Pausa o tempo do jogo
        }
    }

    private void Update()
    {
        // Verifica se o jogador pode prosseguir
        if (playerReachedFinish && Input.GetKeyDown(KeyCode.Return)) // ENTER pressionado
        {
            Debug.Log("Jogador pressionou ENTER. Carregando próxima cena...");
            SceneManager.LoadScene("mapScene"); // Carrega a próxima cena

            // Retoma o tempo do jogo
            Time.timeScale = 1f; // Retoma o tempo do jogo
        }
    }

    private void OnDrawGizmos()
    {
        // Desenha um gizmo para verificar a área do trigger (só para ajudar a depuração)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // Ajuste o tamanho conforme necessário
    }
}
