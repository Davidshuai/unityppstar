using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	private static GameController mGamecontroller;
	public Star star;
	public int columnNum = 10;  //列,x
	public int rowNum = 10;     //行,y
	public ArrayList starList;
	public bool collapseVertically = false;
	public bool DestroyOneStar = false;
	public GameObject mScoreText;
	public GameObject mPanelChangeColor = null;
	public GameObject mPanelGameOver = null;

	public bool DestroyNineStar = false;
	public bool DestroyColumnStar = false;
	public bool changestarColor = false;
	public bool exchangeTwoStar = false;
	public bool MultiScore = false;
	public bool StartGame = false;
	private Star waitingChangeColorStar = null;
	public float MULTIPLE = 1.0f;
	private TurnType Turn = TurnType.Bturn;
	public enum TurnType{
		Aturn,
		Bturn
	}
	public enum ColorType{
		NoColor = -1,
		Blue ,
		Green,
		Purple,
		Red,
		Yellow
	}
	public ColorType colorType;
	

	private float score;
	private float Bscore;

	public static GameController getInstance() {
//		if (mGamecontroller == null) {
//			Debug.Log ("new");
//				mGamecontroller = new GameController();           
//			}
		return mGamecontroller;

	}
	
	void Awake(){
		mGamecontroller = this;
	}

	void Start () {
		if(StartGame)
			init ();
	}
	
	void init(){

		starList = new ArrayList ();
		for (int columnIndex=0; columnIndex < columnNum; columnIndex++) //x
		{
			ArrayList temp = new ArrayList ();
			for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) //y
			{ 
				Star NewStar = CreateStar (columnIndex,rowIndex );
				temp.Add (NewStar);
			}
			starList.Add (temp);
		}
		clearButtonState ();
		score = 0;
		Bscore = 0;
		mScoreText.GetComponent<Text>().text = "ScoreA : " + score + "   ScoreB : " + Bscore;
	}
	
	// Update is called once per frame
	void Update () {
		if (StartGame) {
			init ();
			StartGame = false;
		}
	}

	public void buttonRefresh()
	{
		for (int columnIndex=0; columnIndex < columnNum; columnIndex++) //x
		{
			for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) { //y 
				if (GetStar (columnIndex, rowIndex) != null) {
					Star tempstar = GetStar (columnIndex, rowIndex);
					tempstar.changeColor(Random.Range(0, 4));
				}
			}
		}
	}
	

	public void buttonRestartGame ()
	{
		for (int columnIndex=0; columnIndex < columnNum; columnIndex++) //x
		{
			for (int rowIndex = 0; rowIndex < rowNum; rowIndex++) { //y 
				if (GetStar (columnIndex, rowIndex) != null) {
					Star tempstar = GetStar (columnIndex, rowIndex);
					DestroyImmediate (tempstar.gameObject);
				}
			}
		}
		init ();
	}

	public void clearButtonState()
	{
		DestroyOneStar = false;
		DestroyNineStar = false;
		DestroyColumnStar = false;
		changestarColor = false;
		exchangeTwoStar = false;
		waitingChangeColorStar = null;
		colorType = ColorType.NoColor;
		mPanelChangeColor.gameObject.SetActive(false);

	}

	public void ClickAndSelect (Star ChoiceStar)
	{
		int tempdestroystarnum = 0;
		if (collapseVertically)
			return;

		ChangeTurn ();

		if (DestroyOneStar) {
			DestroyImmediate (ChoiceStar.gameObject);
			CalculateScore(1 , Turn);
			DestroyOneStar = false;

		} else if (DestroyNineStar) {
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					Star tempstar = GetStar (ChoiceStar.getColumnIndex() + i, ChoiceStar.getRowIndex() + j);
					if (tempstar != null)
					{
						++tempdestroystarnum;
						DestroyImmediate (tempstar.gameObject);
					}
				}
			}
			CalculateScore(tempdestroystarnum , Turn);
			DestroyNineStar = false;

		} else if (DestroyColumnStar) {

			int columnIndex = ChoiceStar.getColumnIndex();
			for (int rowCount = 0; rowCount < rowNum; rowCount++) {
				if ((GetStar (columnIndex , rowCount)) != null)
				{
					++tempdestroystarnum;
					DestroyImmediate (GetStar (columnIndex, rowCount).gameObject);
				}
			}
			CalculateScore(tempdestroystarnum , Turn);
			DestroyColumnStar = false;

		} else if (changestarColor) {

			if(colorType != ColorType.NoColor)
			{
				ChoiceStar.changeColor ((int)colorType);
				changestarColor = false;
				mPanelChangeColor.gameObject.SetActive(false);
			}

		} else if (exchangeTwoStar) {

			if(waitingChangeColorStar == null)
				waitingChangeColorStar = ChoiceStar;
			else
			{
				int tempstartype = ChoiceStar.mStarType;
				ChoiceStar.changeColor(waitingChangeColorStar.mStarType);
				waitingChangeColorStar.changeColor(tempstartype);
				waitingChangeColorStar = null;
				exchangeTwoStar = false;
			}
		}
		else {
			ArrayList matchesList = new ArrayList ();
			SearchSameStar (matchesList, ChoiceStar.getColumnIndex(), ChoiceStar.getRowIndex());
			tempdestroystarnum = matchesList.Count;
			if (matchesList.Count > 1) {

				for (int i = 0; i < matchesList.Count; ++i) {
					Star star = (Star)matchesList [i];
					DestroyImmediate (star.gameObject);
				}
				CalculateScore(tempdestroystarnum , Turn);
			} else {
				ChangeTurn();
				Debug.Log ("单个不能消除");
			}
		}
		mScoreText.GetComponent<Text>().text = "ScoreA : " + score + "   ScoreB : " + Bscore;
		MoveDown ();
		MoveLeft ();
		if (IsGameOver ()) {
			mPanelGameOver.gameObject.SetActive(true);
			Debug.Log ("GameOver");
		} 


	}
	
	public bool IsGameOver(){
		bool GameOver = true ;
		for (int i = 0; i < columnNum; i++) 
		{
			for (int j = 0; j < rowNum; j++) 
			{
				int curStar = GetStarType (i, j);  //当前
				int rightStar = GetStarType (i + 1, j); //右
				int leftStar = GetStarType (i - 1, j);  //左 
				int upStar = GetStarType (i, j + 1);   //上
				int downStar = GetStarType (i, j - 1); //下
				if (GetStar (i, j) != null && (curStar == rightStar || curStar == leftStar || curStar == upStar || curStar == downStar)) 
				{
					return GameOver = false ;
					//break ;
				} 
			}
		}
		return GameOver;
	}


	public void MoveDown ()
	{
		collapseVertically = false;
		Debug.Log ("movingdown start");
		for (int i = 0; i < columnNum; i++) {
			ArrayList mList = starList [i] as ArrayList;
			int bottom = 0 ;
			for (int j = 0; j < rowNum; j++) {
				Star starA = (Star)mList [j];

				if (starA != null) {
					if (j == bottom)
						bottom = j + 1;
					else {	
						collapseVertically = true;
						starA.MoveToDown(0 , 0 - (j - bottom));
						mList [bottom] = starA;
						mList [j] = null;
						++bottom;
					}
				}
			}
		}
		Debug.Log("movingdown over");
	}

	void MoveLeft(){
		Debug.Log("movingleft start");
		int left = 0;
		for (int i = 0; i < columnNum ; i++) 
		{
			ArrayList List = starList [i] as ArrayList;

			if (StarNumber (List) != 0 ) 
			{
				if(i == left)
					left = i + 1;
				else{
					ArrayList emptyList = starList [left] as ArrayList;
					int realstarnum = StarNumber(List);
					for (int j = 0; j < realstarnum; j++) 
					{
						//Debug.Log("i="+i+",j="+j+","+StarNumber(List));
						Star tempstar = (Star)List[j];
						tempstar.MoveToLeft(left - i,0);
						emptyList[j] = tempstar;
						List[j] = null;
					}
					++left;	
				}
			}
		}
		Debug.Log("movingleft over");
	}



	public int StarNumber(ArrayList arrayList){    //数组中实际星星个数
		int num = 0;
		for (int i=0; i<arrayList.Count; i++) 
		{
			if ((Star)arrayList [i] != null) 
			{
				num++;
			} 
		}
		return num;
	}

	 void SearchSameStar(ArrayList arraylist, int x, int y)
	{
		if (x < 0 || x > columnNum - 1 || y < 0 || y > rowNum - 1)
			return;
		if (arraylist.Contains(GetStar(x,y)))
		    return;
		int curStar = GetStarType (x, y);
		int rightStar = GetStarType (x + 1, y);
		int leftStar = GetStarType (x - 1, y);
		int upStar = GetStarType (x, y + 1) ;
		int downStar = GetStarType (x, y - 1) ;
		arraylist.Add(GetStar(x,y));
		if (curStar == rightStar) 
		{
			SearchSameStar (arraylist, x + 1, y);
		}
			
		if (curStar == leftStar) 
		{
			SearchSameStar (arraylist, x - 1, y);
		}
			
		if (curStar == upStar) 
		{
			SearchSameStar (arraylist, x, y + 1);
		}
		if (curStar == downStar) 
		{
			SearchSameStar (arraylist, x, y - 1);
		}
	}

	public int GetStarType(int columnIndex,int rowIndex ){    //通过行号和列号，取得所对应位置的star的Type
		Star star = GetStar (columnIndex, rowIndex);
		if (star == null)
			return -1;
		else
			return star.mStarType;
	}	

	public Star GetStar(int columnIndex,int rowIndex ){    //通过行号和列号，取得所对应位置的star

		if (columnIndex < 0 || columnIndex > columnNum - 1) 
		{
			return null;
		} else 
		{
			ArrayList temp = starList [columnIndex] as ArrayList;
			int rownum = temp.Count;
			if (rowIndex < 0 || rowIndex > rownum-1)
			{   
			    return null;
		    }
			else
			{
				Star getstar = temp [rowIndex] as Star;
				return getstar;
			}
		}
	}

	public Star CreateStar(int columnIndex,int rowIndex ){  //生成Star
		Star TheStar = Instantiate (star) as Star;
		TheStar.transform.parent = this.transform;
		TheStar.GetComponent<Star>().RandomCreateStarBg();
		TheStar.GetComponent<Star>().UpdatePosition (columnIndex, rowIndex);
		return TheStar;
	}

	public void CalculateScore(int destroyNum , TurnType turntype)
	{
		switch (turntype) {
		case TurnType.Aturn:
			if (destroyNum < 7)
				score += 5 * destroyNum * destroyNum * MULTIPLE;
			else
				score += 7 * destroyNum * destroyNum * MULTIPLE;
			break;
		case TurnType.Bturn:
			if (destroyNum < 7)
				Bscore += 5 * destroyNum * destroyNum * MULTIPLE;
			else
				Bscore += 7 * destroyNum * destroyNum * MULTIPLE;
			break;

		}
	}

	public void ChangeTurn()
	{
		if (Turn == TurnType.Aturn)
			Turn = TurnType.Bturn;
		else
			Turn = TurnType.Aturn;
	}
	
}



