using UnityEngine;
using System.Collections;

public class UIManagerScript : MonoBehaviour {

	public Animator Buygift;
	public Animator Signplan;
	public Animator Helpplan;
	public Animator Settingplan;

	public void StartGame()
	{
		Application.LoadLevel("PPstar");
	}
	
	public void OpenBuygift()
	{
		Buygift.enabled = true;
		Buygift.SetBool("TureBool", false);
	}
	
	public void Closebuygift()
	{
		Buygift.SetBool("TureBool", true);
	}


	public void OpenSignplan()
	{
		Signplan.enabled = true;
		Signplan.SetBool("TureBool", false);
	}
	
	public void CloseSignplan()
	{
		Signplan.SetBool("TureBool", true);
	}

	public void OpenHelpplan()
	{
		Helpplan.enabled = true;
		Helpplan.SetBool("TureBool", false);
	}
	
	public void CloseHelpplan()
	{
		Helpplan.SetBool("TureBool", true);
	}

	public void OpenSettingplan()
	{
		Settingplan.enabled = true;
		Settingplan.SetBool("TureBool", false);
	}
	
	public void CloseSettingplan()
	{
		Settingplan.SetBool("TureBool", true);
	}
}
