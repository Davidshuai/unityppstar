using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	public GameObject mPanel_Init;

	public GameObject mPanel_Playing;
	public GameObject mText_ScoreGot;
	public GameObject mText_DiamondNumber;
	public GameObject mPanel_ChooseColor;
	public GameObject mButton_Restart;

	public void OnButton_RestartGame()
	{
		GameController.getInstance ().mPanelGameOver.gameObject.SetActive (false);
		GameController.getInstance ().buttonRestartGame ();
	}
	public void OnButton_StartGame()
	{
		mPanel_Init.gameObject.SetActive (false);
		GameController.getInstance ().StartGame = true;
		mPanel_Playing.gameObject.SetActive (true);
	}

	public void OnButton_ContinueGame()
	{

	}

	public void OnButton_NewUserGift()
	{

	}

	public void OnButton_SignUp()
	{
		
	}

	public void OnButton_Setting()
	{
		
	}

	public void OnButton_Help()
	{
		
	}

	public void OnButton_Pause()
	{
		
	}

	public void OnButton_BuyDiamond()
	{
		
	}

	public void OnButton_Boom()
	{
		if (!(GameController.getInstance().DestroyNineStar))
		{
			GameController.getInstance().clearButtonState();
			GameController.getInstance().DestroyNineStar = true;
		}
		else
			GameController.getInstance().DestroyNineStar = false;
		
	}

	public void OnButton_Brush()
	{
		if (!(GameController.getInstance().changestarColor))
		{
			GameController.getInstance().clearButtonState();
			GameController.getInstance().changestarColor = true;
			mPanel_ChooseColor.gameObject.SetActive(true);
		}
		else
		{
			GameController.getInstance().changestarColor = false;
			mPanel_ChooseColor.gameObject.SetActive(false);
		}
		
	}

	public void OnButton_Rotate()
	{
		GameController.getInstance ().buttonRefresh ();
	}

	public void OnButton_Exchange()
	{
		if (!(GameController.getInstance().exchangeTwoStar))
		{
			GameController.getInstance().clearButtonState();
			GameController.getInstance().exchangeTwoStar = true;
		}
		else
			GameController.getInstance().exchangeTwoStar = false;
	}

	public void OnButton_Hammer()
	{
		if (!(GameController.getInstance().DestroyOneStar)) 
		{
			GameController.getInstance().clearButtonState();
			GameController.getInstance().DestroyOneStar = true;
		}
		else
			GameController.getInstance().DestroyOneStar = false;
	}

	public void OnButton_BoomColumn()
	{
		if (!(GameController.getInstance().DestroyColumnStar))
		{
			GameController.getInstance().clearButtonState();
			GameController.getInstance().DestroyColumnStar = true;
		}
		else
			GameController.getInstance().DestroyColumnStar = false;
	}

	public void OnButton_ScoreCard()
	{
		if (!(GameController.getInstance().MultiScore)) 
		{
			GameController.getInstance().MultiScore = true;
			GameController.getInstance().MULTIPLE = 1.2f;
		} else{
			GameController.getInstance().MultiScore = false;
			GameController.getInstance().MULTIPLE = 1.0f;
		}
	}

	public void OnButton_ChooseColor(int cType)
	{
		switch (cType) {
		case 0:
			GameController.getInstance().colorType = GameController.ColorType.Blue;
			break;
		case 1:
			GameController.getInstance().colorType = GameController.ColorType.Green;
			break;
		case 2:
			GameController.getInstance().colorType = GameController.ColorType.Purple;
			break;
		case 3:
			GameController.getInstance().colorType = GameController.ColorType.Red;
			break;
		default:
			GameController.getInstance().colorType = GameController.ColorType.Yellow;
			break;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
