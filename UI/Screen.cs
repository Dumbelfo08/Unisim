using System;
using OpenTK;
using OpenTK.Mathematics;
using AshLib;

class Screen{
	public List<Button> buttons{get; private set;}
	
	public bool writing;
	public int selected;
	
	public Screen(params Button[] b){
		buttons = new List<Button>();
		
		buttons.AddRange(b);
	}
	
	public Screen setWriting(){
		writing = true;
		selected = -1;
		return this;
	}
	
	public void draw(Renderer ren, bool doHover){
		Vector2d mouse = ren.cam.mouseLastPos - new Vector2d(ren.width / 2f, ren.height / 2f);
		mouse.Y = -mouse.Y;
		
		foreach(Button b in buttons){
			if(b.active){
				b.draw(ren, mouse);
			}
		}
		
		if(doHover){
			foreach(Button b in buttons){
				if(b.active && b.hasHover && b.box != null && b.box % mouse){
					b.drawHover(ren, mouse);
				}
			}
		}
	}
	
	public bool click(Renderer ren, Vector2d m, bool shift){
		Vector2d mouse = ren.cam.mouseLastPos - new Vector2d(ren.width / 2f, ren.height / 2f);
		mouse.Y = -mouse.Y;
		
		for(int i = buttons.Count - 1; i >= 0; i--){
			Button b = buttons[i];
			if(b.active && b.box != null && b.box % mouse){
				if(writing && b is Field f){
					if(selected > -1 && buttons[selected] is Field p){
						p.selected = false;
					}
					
					f.selected = true;
					selected = i;
				}else{
					if(writing && selected > -1 && buttons[selected] is Field p){
						p.selected = false;
						selected = -1;
					}
					
					if(b.quickAction != null && shift){
						b.quickAction.Invoke();
					}else if(b.action != null){
						b.action.Invoke();
					}
				}
				
				return true;
			}
		}
		
		if(writing && selected > -1 && buttons[selected] is Field e){
			e.selected = false;
			selected = -1;
		}
		return false;
	}
	
	public WritingType tryGet(){
		if(writing && selected > -1 && buttons[selected] is Field f){
			return f.type;
		}
		
		return WritingType.Hex;
	}
	
	public void tryAdd(char c){
		if(writing && selected != -1 && buttons[selected] is Field f){
			f.addChar(c);
		}
	}
	
	public void tryDel(){
		if(writing && selected != -1 && buttons[selected] is Field f){
			f.delChar();
		}
	}
	
	public void updateProj(Renderer ren){
		foreach(Button b in buttons){
			b.updateBox(ren);
		}
	}
}