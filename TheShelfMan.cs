using UnityEngine;
using System.Collections;

public class TheShelfMan : MonoBehaviour {
	
	public GameObject goTheApps;
	
	public enum TheApps{
		None,
		Earth,
		Memo,
		Maps,
		WebBrowser,
		Gorilla
	}
	public static TheApps theApps = TheApps.None;
	
	public static GameObject curSelected; bool isTouching=false; public static bool selectConfirmed=false; // only used for mobile ios android
	
	Placer placer;
	Rearranger rearranger;
	
	public GameObject goPointer; public GameObject goFacePointer;
	ThePointer tp;
	
	IntelPerC ipc;
	
	public GameObject gotemp;
	
	float timeLastOpened=0f;
	
	public void ActivatePointer(bool flip){
		goPointer.SetActiveRecursively(flip);	
	}
	public void ActivateFacePointer(bool flip){
		goFacePointer.SetActiveRecursively(flip);	
	}	
	
	
	public static void SelectMe(GameObject g){
		curSelected = g;	
	}
	
	public static void DeselectIfMe(GameObject g){
		if(curSelected==g){
			print(g.name+" deselected selectConfirmed="+selectConfirmed);
			
			curSelected=null;
			selectConfirmed=false;
		}
	}
	
	void Awake(){
		placer = GetComponent<Placer>();
		rearranger = GetComponent<Rearranger>();
	}
	
	void Start(){
		placer.PlaceOnShelf(GoManips.ChildrenToArray(goTheApps));
		tp = goPointer.GetComponent<ThePointer>();
		
		ipc = GetComponent<IntelPerC>();
	}
	
	/*void OnGUI(){
		if(GUI.Button (new Rect(10,10,50,50),"test ra"))
			rearranger.RearrangeObj(gotemp);
	}*/
	
	void Update(){
#if UNITY_IPHONE 
		// raycast hit to see if we are touching. curSelected isTouching=true;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast (ray,out hit,1000f)){
			isTouching=true;
			curSelected=hit.transform.gameObject;
		}else{ isTouching=false;curSelected=null;}
#endif 
		
		if(curSelected!=null){
			if(TheInputMode.im == TheInputMode.InputModes.IntelPerC){
				if(ipc.GetClosedCertain () || Input.GetKeyDown(KeyCode.C) || isTouching){
					if((timeLastOpened-Time.timeSinceLevelLoad)<0.5f)
						SelectConfirm (curSelected);	
				}else if(ipc.GetOpenCertainish () || isTouching){
					timeLastOpened=Time.timeSinceLevelLoad;
					
				}
			}
			
			// debug keysets
			if(Input.GetKeyDown (KeyCode.U))
				tp.DeselectCurrent(curSelected);
			
		}

	} 
	
	void SelectConfirm(GameObject g){
		print (g.name+" SC");
		selectConfirmed=true;
		tp.HoverMe(g,new Color(0,1,0,1));
	}
	
	public static void RunApp(GameObject g){print ("Run APp "+g.name);
		if(theApps!=TheApps.None)return;
		GameObject go = g.transform.Find ("_AppHolder").gameObject;
		go.SetActiveRecursively(true);	
		go.transform.parent=GameObject.Find ("GameObject").transform;
		go.transform.position=Vector3.zero;	
		
		ThePointer.UnGlow(g);
		
		if(g.name=="Earth"){
			theApps = TheApps.Earth;
			g.collider.enabled=false;
		}else if(g.name=="Memo"){
			theApps = TheApps.Memo;
			g.collider.enabled=false;
		}else if(g.name=="Maps"){
			theApps = TheApps.Maps;
			g.collider.enabled=false;
		}else if(g.name=="WebBrowser"){
			theApps = TheApps.WebBrowser;
			g.collider.enabled=false;
		}else if(g.name=="Gorilla"){
			theApps = TheApps.Gorilla;
			g.collider.enabled=false;
		}
		// set current scene inactive? 
		
	}
	
	public static void StopApp(GameObject g){print ("Stopping "+g.name);
		Transform t = GameObject.Find ("GameObject").transform.Find ("_AppHolder").transform;
		foreach(Transform tran in t){
			t.gameObject.SetActiveRecursively(false);	
		}
		t.parent = g.transform;
		theApps = TheApps.None;
		g.collider.enabled=true;
		// set app selection scene active?
		
	}	
	
}
