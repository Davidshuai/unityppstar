using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public float mXOffset = -5.0f;  //x方向的偏移
	public float mYOffset = -4.0f;  //y方向的偏移
	public float mMovetime = 1.0f;
	public Sprite[] mStarBgs;  //Star的数组
	public int mStarType;  //Star的类型
	public Sprite mStarBg;
	private int mRowIndex = 0;    //,x
	private int mColumnIndex = 0;       //,y
	private SpriteRenderer mRender;
	private const float STARSIZE = 0.56f;

	// Use this for initialization
	void Awake () {
		mRender = GetComponent<SpriteRenderer>(); 
	}

	public void RandomCreateStarBg(){ //生成随机的星星类型
		//mRender = GetComponent<SpriteRenderer>();
		if (mStarBg != null)
			return;
		mStarType = Random.Range(0, mStarBgs.Length);
		mStarBg = mStarBgs[mStarType];
		mRender.sprite = mStarBgs [mStarType];
		
	}
	
	public void UpdatePosition(int columnIndex,int rowIndex){    //调整星星的位置
		mRowIndex = rowIndex;
		mColumnIndex = columnIndex;
		this.transform.position = new Vector3 ( (mColumnIndex + mXOffset)* STARSIZE ,(mRowIndex  + mYOffset) *STARSIZE, 0);
	}
	public bool MoveToDown (int xDir, int yDir)
	{	
		mRowIndex += yDir;
		mColumnIndex += xDir;
		//Store start position to move from, based on objects current transform position.
		Vector2 start = this.transform.position;
		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 end = start + new Vector2 (xDir * STARSIZE , yDir * STARSIZE);

		GameController.getInstance ().collapseVertically = true;
		iTween.MoveTo(gameObject,(Vector3) end, mMovetime);
		StartCoroutine(WaitAndPrint(mMovetime)); 

		return true;
	}

	public bool MoveToLeft (int xDir, int yDir)
	{	
		mRowIndex += yDir;
		mColumnIndex += xDir;
		//Store start position to move from, based on objects current transform position.
		Vector2 start = this.transform.position;
		// Calculate end position based on the direction parameters passed in when calling Move.
		Vector2 end = start + new Vector2 (xDir * STARSIZE , yDir * STARSIZE);

		Hashtable ht = new Hashtable();
		ht.Add("x", end.x);
		ht.Add("time", mMovetime);
		
		if (GameController.getInstance().collapseVertically) {
			ht.Add("delay",mMovetime);						
		}

		iTween.MoveTo(this.gameObject, ht);
		
		return true;
	}

	public void OnMouseDown(){
		GameController.getInstance().ClickAndSelect(this);
	}

	//定义 WaitAndPrint（）方法  
	IEnumerator WaitAndPrint(float waitTime)  
	{  
		
		
		yield return new WaitForSeconds(waitTime);  
		//等待之后执行的动作  
		GameController.getInstance ().collapseVertically = false;
		
		
	}    
	public void changeColor(int startype){
		mStarType = startype;
		mStarBg = mStarBgs[mStarType];
		mRender.sprite = mStarBgs [startype];

	}

	void Update () {
	}

	public int getColumnIndex(){
		return mColumnIndex;
	}
	public int getRowIndex(){
		return mRowIndex;
	}
}
