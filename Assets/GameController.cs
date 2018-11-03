using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject bip;
    [SerializeField]
    private Shader shader;
	// Use this for initialization
	void Start () {
        List<CombineInstance> combineInstances = new List<CombineInstance> ();
        List<Material> materials = new List<Material> ();
        List<Transform> bones = new List<Transform> ();

        SkinnedMeshRenderer[] skinnedMeshs = player.GetComponentsInChildren<SkinnedMeshRenderer> ();

        Transform[] transforms = player.GetComponentsInChildren<Transform> ();
        foreach(SkinnedMeshRenderer smr in skinnedMeshs) {
            materials.AddRange (smr.sharedMaterials);
            for(int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++) {
                CombineInstance ci = new CombineInstance ();
                ci.mesh = smr.sharedMesh;
                ci.subMeshIndex = sub;
                combineInstances.Add (ci);
            }
            int startBoneIndex = bones.Count;
            foreach(Transform t in smr.bones) {
                foreach(Transform transform in transforms) {
                    if(transform.name != t.name)
                        continue;
                    bones.Add (transform);
                    break;
                }
            }

            int endBoneIndex = bones.Count;
            for(int i = 1; i < smr.sharedMesh.subMeshCount; ++i) {
                for(int j = startBoneIndex; j < endBoneIndex; ++j) {
                    bones.Add (bones[j]);
                }
            }
        }

        GameObject go  = Instantiate (bip);
        SkinnedMeshRenderer r = go.AddComponent<SkinnedMeshRenderer> ();
        r.sharedMesh = new Mesh ();
        r.sharedMesh.CombineMeshes (combineInstances.ToArray () , false , false);
        r.bones = bones.ToArray ();
        r.materials = materials.ToArray ();

        player.SetActive (false);

        foreach (Material material in go.GetComponent<Renderer>().materials) {
            material.shader = shader;
            material.SetFloat ("_HeightVal" , 0);
            material.SetFloat ("_TransparentLen" , 0.5f);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
