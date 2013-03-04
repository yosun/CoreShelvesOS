using UnityEngine;
using System.Collections;

public class ThePointerFace : MonoBehaviour {

	public GameObject goTheApps;
	
	public GameObject goIPC;
	IntelPerC ipc;
	
	public GameObject goCam;
	CamViews camviews;	
	
	float timeElapsed=0f;
	
	void Start(){
		ipc = goIPC.GetComponent<IntelPerC>();
		camviews = goCam.GetComponent<CamViews>();
	}	
	
	void Update(){
		timeElapsed+=Time.deltaTime;
		
		Vector3 pos = new Vector3(ipc.GetFaceRectCenter().x*-0.0333f+8f,ipc.GetFaceRectCenter().y*-0.023f+5f,0);
		transform.position = pos;
		 if(Input.GetKeyDown (KeyCode.LeftArrow)){
				camviews.TurnLeft ();	
		 }else if(Input.GetKeyDown (KeyCode.RightArrow)){
				camviews.TurnRight ();		
		 }else if(Input.GetKeyDown (KeyCode.DownArrow)){
				camviews.TurnDownwards();	
		 }else if(Input.GetKeyDown (KeyCode.UpArrow)){
				camviews.TurnUpwards();	
		 } 
	}
	
	void OnTriggerEnter(Collider col){
		if(timeElapsed<3.14f||ipc.GetFaceRectCenter ()==Vector2.zero)return;
		
		string name = col.name;
		if(name == "Backpane-SideRight"){
			camviews.TurnRight ();
		}else if(name == "SideplaneLeft"){
			camviews.TurnLeft ();
		}else if(name == "Backpane-DesktopCollider"){
			camviews.TurnDownwards();	
			if(TheShelfMan.TheApps.Earth == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Earth").gameObject);	
			}else if(TheShelfMan.TheApps.Memo == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Memo").gameObject);	
			}else if(TheShelfMan.TheApps.WebBrowser == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("WebBrowser").gameObject);	
			}else if(TheShelfMan.TheApps.Gorilla == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Gorilla").gameObject);	
			}			
		}else if(name == "Backpane-ShelfCollider"){
			camviews.TurnUpwards();
			if(TheShelfMan.TheApps.Earth == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Earth").gameObject);	
			}else if(TheShelfMan.TheApps.Memo == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Memo").gameObject);	
			}else if(TheShelfMan.TheApps.Maps == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Maps").gameObject);	
			}else if(TheShelfMan.TheApps.WebBrowser == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("WebBrowser").gameObject);	
			}else if(TheShelfMan.TheApps.Gorilla == TheShelfMan.theApps){
				TheShelfMan.StopApp(goTheApps.transform.Find("Gorilla").gameObject);	
			}
		}
	}
	
}
