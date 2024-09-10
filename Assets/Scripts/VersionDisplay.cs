using UnityEngine;
using UnityEngine.UI;
public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private Text versionText;

    void Start()
    {
        versionText.text = Application.version;
    }
}
