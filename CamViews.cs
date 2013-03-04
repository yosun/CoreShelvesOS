using UnityEngine;
using System.Collections;

public class CamViews : MonoBehaviour {
	
	 Vector3 eulerLeft=new Vector3(0,-90,0);
	 Vector3 eulerRight=new Vector3(0,90,0);
	 Vector3 eulerCenter=Vector3.zero;
	
	 Vector3 eulerDownwards=new Vector3(30,0,0);
	
	int mydir=0;
	
	bool downwards=false;
	
	public void TurnLeft(){
		if(downwards)
			return;
		
		if(TheShelfMan.curSelected!=null)
			return;		
		
		Vector3 rot = transform.rotation.eulerAngles;
		if(mydir==1){
			iTween.RotateTo (gameObject,eulerCenter,3.14f);
			mydir=0;
		}else {
			iTween.RotateTo (gameObject,eulerLeft,3.14f);
			mydir=-1;
		}
		
	} 
	
	public void TurnRight(){
		if(downwards)
			return;
		
		if(TheShelfMan.curSelected!=null)
			return;
		
		Vector3 rot = transform.rotation.eulerAngles;///print (rot);
		if(mydir==-1){
			iTween.RotateTo (gameObject,eulerCenter,3.14f);
			mydir=0;
		}else{
			iTween.RotateTo (gameObject,eulerRight,3.14f);
			mydir=1;
		}
	} 
	
	public void TurnDownwards(){
		
		if(!downwards){
			downwards=true;
			iTween.RotateTo (gameObject,eulerDownwards,3.14f);
		}
	}
	
	public void TurnUpwards(){
		if(!downwards)
			return;
		downwards=false;
		iTween.RotateTo (gameObject,eulerCenter,3.14f);
	}

	
}
