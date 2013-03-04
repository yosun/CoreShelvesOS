using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThePointer : MonoBehaviour {
	
	public GameObject goBackPlane; // cannot go beyond this z
	
	public GameObject goCamera;
	OutlineGlowEffectScript oges;
	CamViews camviews;		
	
	float zPosDrop=-0.5f;
	float zPosActivate=-2f;
	
	public GameObject goIPC;
	IntelPerC ipc;
	
	GameObject lastDropped;
	
	public GameObject goApps;
	Dictionary<string,Vector3> colLastPos = new Dictionary<string, Vector3>();
	
	void Start(){
		oges = goCamera.GetComponent<OutlineGlowEffectScript>();
		ipc = goIPC.GetComponent<IntelPerC>();
		camviews = goCamera.GetComponent<CamViews>();	
		
		foreach(Transform t in goApps.transform){
			colLastPos[t.name] = t.position;	
		}
	}
	
	// we hit an app icon
	void OnTriggerEnter(Collider col){
		if(CheckDoNotTrigger(col.gameObject.name))return;
		
		if(TheShelfMan.curSelected!=null)DeselectCurrent(TheShelfMan.curSelected);
		
		if(lastDropped==col.gameObject){print(col.gameObject.name);return;}
	
		HoverMe(col.gameObject,new Color(1,1,1,1));	
		TheShelfMan.SelectMe(col.gameObject);
		
		SetGrav(false,col);
	}
	
	// we left the app icon
	void OnTriggerExit(Collider col){
		if(CheckDoNotTrigger(col.gameObject.name))return;
		
		SnapBack(col.gameObject);
		DeselectCurrent (col.gameObject);
	}
	
	public void DeselectCurrent(GameObject g){print ("deselecting "+g.name);
		SnapBack(g);
		TheShelfMan.DeselectIfMe(g);
		DeselectGlowAll(g);		
	}
	
	void SnapBack(GameObject g){
		if(colLastPos.ContainsKey(g.name)){
			SetGrav (false,g.collider);
			//if(g.transform.position.z>zPosDrop){
			//	print (colLastPos[g.name]+"snap to");
				iTween.MoveTo (g,colLastPos[g.name],1.37f);	
			//}
			/*else 
				SetGrav(true,g.collider); */
		}else print (g.name + "not found in snapback");
	}
	
	bool CheckDoNotTrigger(string name){
		if(name=="Desktop"||name=="Backpane"||name=="WallSide"||name=="PlaneB")return true;
		else return false;
	}
	
	// we are in an app icon
	void OnTriggerStay(Collider col){print (col.name+"STAY");
		if(CheckDoNotTrigger(col.gameObject.name))return;

		if(TheShelfMan.selectConfirmed&&TheShelfMan.theApps==TheShelfMan.TheApps.None){
			col.transform.position = transform.position;
			float z = col.transform.position.z;//print (z);
			if(z<zPosDrop||Input.GetKeyDown(KeyCode.R)){
				print ("run"+col.name);
				TheShelfMan.RunApp(col.gameObject);
				SnapBack(col.gameObject);
				DeselectGlowAll(col.gameObject);
			}/*else if((z<zPosDrop&&ipc.GetOpenCertain())||Input.GetKeyDown(KeyCode.D)){
				//if(lastDropped==col.gameObject)return;
				print ("drop"+col.name);
				lastDropped = col.gameObject;
				SetGrav(true,col); 
				
			} */
		}
	}
	
	void SetGrav(bool flip,Collider col){
		if(flip){
			col.rigidbody.useGravity=true;
			col.rigidbody.isKinematic=false;
			camviews.TurnDownwards();	
		}else{
			col.rigidbody.useGravity=false;
			col.rigidbody.isKinematic=true;
		}
	}
	
	public void HoverMe(GameObject g,Color col){
		if(g.transform.parent==null)return;
		DeselectGlowAll(g);
		
		OutlineGlowRenderer ren = g.GetComponent<OutlineGlowRenderer>();
		ren.DrawOutline=true;
		ren.OutlineColor=col; oges.OutlineColor=col;
		//print (g.name+ "HM");
	}
	
	public static void DeselectGlowAll(GameObject g){
		if(g.transform.parent==null)return;
		Transform parent = g.transform.parent; 
		foreach(Transform t in parent){
			GameObject go = t.gameObject;
			if(go!=TheShelfMan.curSelected){
				UnGlow(go);
				//print (go.name);
			}
		}		
	}
	
	public static void UnGlow(GameObject go){if(go.name=="PlaneB")return; print ("unglowing "+go.name);
		OutlineGlowRenderer ogr = go.GetComponent<OutlineGlowRenderer>();	
		ogr.DrawOutline=false;		
	}
	
	void Update(){
		Vector3 pos = ipc.GetWorldPosition();
		if(pos.z>goBackPlane.transform.position.z){
			pos = new Vector3(pos.x,pos.y,goBackPlane.transform.position.z);
			print ("hitmaxz");
		}
		transform.position = pos;
		//print (pos);
	}
	
}
