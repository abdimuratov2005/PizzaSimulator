using UnityEngine;

public class GpuInstancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new();
        
        if (GetComponent<SkinnedMeshRenderer>() != null)
        {
            GetComponent<SkinnedMeshRenderer>().SetPropertyBlock(materialPropertyBlock);
        } else
        {
            GetComponent<MeshRenderer>().SetPropertyBlock(materialPropertyBlock);
        }
    }
}
