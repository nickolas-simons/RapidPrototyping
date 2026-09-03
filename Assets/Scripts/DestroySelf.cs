using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    // used for animation events so fire and forget objects clean up after themselves
    private void StartDestroy()
    {
        Destroy(gameObject);
    }
}
