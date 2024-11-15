using UnityEngine;
using UnityEngine.EventSystems;





[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour {

	public Color color = Color.white;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	public SpriteRenderer spriteRenderer{
		get{
			if(_spriteRenderer == null){
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
	}
	[SerializeField]
	private float _outlineSize = 7;

	private Material _preMat;

    private void Start()
    {
        //SpriteRenderer  spriteRenderer = GetComponent<SpriteRenderer>();
		//Vector3 oriBound = spriteRenderer.bounds.size;
		//Vector3 newBound = new Vector3 (oriBound.x+5, oriBound.y + 5, oriBound.z + 5);
        //spriteRenderer.bounds = new Bounds(transform.position, newBound);
		//Debug.Log(transform.name + "has been expanded");
    }



    void OnEnable() {
		_preMat = spriteRenderer.sharedMaterial;
		spriteRenderer.sharedMaterial = defaultMaterial;
		_outlineSize = 7;
		UpdateOutline(_outlineSize);
	}

	void OnDisable() {
		spriteRenderer.sharedMaterial = _preMat;
		_outlineSize = 0;
		UpdateOutline(_outlineSize);

	}

	void UpdateOutline(float outline) {
		MaterialPropertyBlock mpb = new MaterialPropertyBlock();
		spriteRenderer.GetPropertyBlock(mpb);
		mpb.SetFloat("_OutlineSize", outline);
		mpb.SetColor("_OutlineColor", color);
		spriteRenderer.SetPropertyBlock(mpb);
	}

	void OnValidate(){
		if(enabled){
			UpdateOutline(_outlineSize);
		}
	}


	private static Material _defaultMaterial = null;
	public static Material defaultMaterial{
		get{
			if(_defaultMaterial == null){
				_defaultMaterial = Resources.Load<Material>("Sprite-Outline");
			}
			return _defaultMaterial;
		}
	}
}