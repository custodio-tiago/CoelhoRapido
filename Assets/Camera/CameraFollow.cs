using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public Vector3 offset;   // Offset da câmera em relação ao jogador

    void Update()
    {
        if (player != null)
        {
            // Atualiza a posição da câmera para seguir o jogador
            transform.position = player.position + offset;
        }
    }
}
