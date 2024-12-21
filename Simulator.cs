using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using OpenTK;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Common.Input;
using StbImageSharp;
using AshLib;

//Auto generated by Tebas project manager
class Simulator : GameWindow{
	
	Renderer ren;
	
	KeyBind fullscreen = new KeyBind(Keys.F11, false);
	KeyBind screnshot = new KeyBind(Keys.F2, false);
	
	KeyBind debugMode = new KeyBind(Keys.LeftAlt, false);
	KeyBind escape = new KeyBind(Keys.Escape, Keys.LeftShift, false);
	
	KeyBind showForces = new KeyBind(Keys.F3, false);
	KeyBind showPoints = new KeyBind(Keys.F6, false);
	KeyBind showBoxes = new KeyBind(Keys.F4, false);
	KeyBind showClouds = new KeyBind(Keys.F5, false);
	
	KeyBind nextParticle = new KeyBind(Keys.Tab, Keys.LeftShift, false);
	
	KeyBind tick = new KeyBind(Keys.F, false);
	KeyBind pause = new KeyBind(Keys.Space, false);
	
	KeyBind tickRateUp = new KeyBind(Keys.KeyPadAdd, false);
	KeyBind tickRateDown = new KeyBind(Keys.KeyPadSubtract, false);
	
	KeyBind debugInfo = new KeyBind(Keys.I, false);
	
	KeyBind moveUp = new KeyBind(Keys.W, true);
	KeyBind moveDown = new KeyBind(Keys.S, true);
	KeyBind moveLeft = new KeyBind(Keys.A, true);
	KeyBind moveRight = new KeyBind(Keys.D, true);
	
	KeyBind k1 = new KeyBind(Keys.D1, false);
	KeyBind k2 = new KeyBind(Keys.D2, false);
	KeyBind k3 = new KeyBind(Keys.D3, false);
	KeyBind k4 = new KeyBind(Keys.D4, false);
	KeyBind k5 = new KeyBind(Keys.D5, false);
	KeyBind k6 = new KeyBind(Keys.D6, false);
	KeyBind k7 = new KeyBind(Keys.D7, false);
	KeyBind k8 = new KeyBind(Keys.D8, false);
	KeyBind k9 = new KeyBind(Keys.D9, false);
	KeyBind k0 = new KeyBind(Keys.D0, false);
	KeyBind ka = new KeyBind(Keys.A, false);
	KeyBind kb = new KeyBind(Keys.B, false);
	KeyBind kc = new KeyBind(Keys.C, false);
	KeyBind kd = new KeyBind(Keys.D, false);
	KeyBind ke = new KeyBind(Keys.E, false);
	KeyBind kf = new KeyBind(Keys.F, false);
	
	KeyBind del = new KeyBind(Keys.Backspace, false);
	KeyBind min = new KeyBind(Keys.Slash, false);
	KeyBind point = new KeyBind(Keys.Period, false);
	
	bool isFullscreened;
	
	public static DeltaHelper dh;
	
	Simulation sim;
	
	Particle ghost;
	
	static void Main(string[] args){
		using(Simulator sim = new Simulator()){
			sim.Run();
		}
	}
	
	Simulator() : base(GameWindowSettings.Default, NativeWindowSettings.Default){
		CenterWindow(new Vector2i(640, 480));
		Title = "Unisim";
		
		VSync = VSyncMode.On;
	}
	
	void initialize(){
		setIcon();
		
		DrawBufferController dbc = new DrawBufferController();
		
		sim = new Simulation(Examples.RPF, 30f, dbc);
		
		ren = new Renderer(this, sim, null, dbc);
		
		sim.tick();
		
		dh = new DeltaHelper();
		dh.Start();
	}
	
	void onResize(int x, int y){
		GL.Viewport(0, 0, x, y);
		if(this.ren != null){
			ren.updateSize(x, y);
		}
	}
	
	void closeCurrent(){
		ren.current = null;
	}
	
	void setSolarSystem(){
		if(sim.isRunning){
			return;
		}
		
		sim.reset(Examples.solntse);
		ren.cam.setFollow(null);
	}
	
	void setRPF(){
		if(sim.isRunning){
			return;
		}
		
		sim.reset(Examples.RPF);
		ren.cam.setFollow(null);
	}
	
	void setCustomRPF(){
		uint m;
		if(!uint.TryParse(((Field)ren.current.buttons[1]).text, out m)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse min num");
			return;
		}
		
		uint x;
		if(!uint.TryParse(((Field)ren.current.buttons[2]).text, out x)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse max num");
			return;
		}
		
		if(m > x){
			((Text)ren.current.buttons[12]).setText("Min cant be bigger than max");
			return;
		}
		
		float s;
		if(!float.TryParse(((Field)ren.current.buttons[3]).text, out s)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse size");
			return;
		}
		
		float v;
		if(!float.TryParse(((Field)ren.current.buttons[4]).text, out v)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse velocity");
			return;
		}
		
		uint b;
		if(!uint.TryParse(((Field)ren.current.buttons[5]).text, out b)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse basitrons");
			return;
		}
		
		uint mw;
		if(!uint.TryParse(((Field)ren.current.buttons[6]).text, out mw)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse marks");
			return;
		}
		
		uint t;
		if(!uint.TryParse(((Field)ren.current.buttons[7]).text, out t)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse tarks");
			return;
		}
		
		uint c;
		if(!uint.TryParse(((Field)ren.current.buttons[8]).text, out c)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse charks");
			return;
		}
		
		uint p;
		if(!uint.TryParse(((Field)ren.current.buttons[9]).text, out p)){
			((Text)ren.current.buttons[12]).setText("Couldnt parse pharks");
			return;
		}
		
		sim.reset(Examples.RPFparams((int) m, (int) x, s, v, (int) b, (int) mw, (int) t, (int) c, (int) p));
	}
	
	void addParticle(){		
		Color3 c;
		try{
			c = new Color3(((Field)ren.current.buttons[1]).text);
		}catch(Exception){
			((Text)ren.current.buttons[8]).setText("Couldnt parse color");
			return;
		}
		
		float mass;
		if(!float.TryParse(((Field)ren.current.buttons[2]).text, out mass)){
			((Text)ren.current.buttons[8]).setText("Couldnt parse mass");
			return;
		}
		
		if(mass <= 0f){
			((Text)ren.current.buttons[8]).setText("Mass must be positive");
			return;
		}
		
		float radius;
		if(!float.TryParse(((Field)ren.current.buttons[3]).text, out radius)){
			((Text)ren.current.buttons[8]).setText("Couldnt parse radius");
			return;
		}
		
		if(radius <= 0f){
			((Text)ren.current.buttons[8]).setText("Radius must be positive");
			return;
		}
		
		float charge;
		if(!float.TryParse(((Field)ren.current.buttons[4]).text, out charge)){
			((Text)ren.current.buttons[8]).setText("Couldnt parse charge");
			return;
		}
		
		float weak;
		if(!float.TryParse(((Field)ren.current.buttons[5]).text, out weak)){
			((Text)ren.current.buttons[8]).setText("Couldnt parse weak");
			return;
		}
		
		ghost = new Particle(radius, mass, charge, weak, c);
		ren.ghost = ghost;
		ren.current = null;
	}
	
	void customRPF(){
		ren.current = new Screen(ren, new Text(ren, "Custom RPF", 0, 1, 0f, 20f),
										new Field(ren, "Min number of particles:", "100", 0, 0, 180f, 5f * ren.fSeparation, 100f),
										new Field(ren, "Max number of particles:", "500", 0, 0, 180f, 4f * ren.fSeparation, 100f),
										new Field(ren, "Size:", "100", 0, 0, 180f, 3f * ren.fSeparation, 100f).setDescription("Size of the square"),
										new Field(ren, "Velocity:", "0.5", 0, 0, 180f, 2f * ren.fSeparation, 100f),
										new Field(ren, "Weight of basitrons:", "30", 0, 0, 180f, 1f * ren.fSeparation, 100f),
										new Field(ren, "Weight of marks:", "30", 0, 0, 180f, 0f, 100f),
										new Field(ren, "Weight of tarks:", "30", 0, 0, 180f, -1f * ren.fSeparation, 100f),
										new Field(ren, "Weight of charks:", "5", 0, 0, 180f, -2f * ren.fSeparation, 100f),
										new Field(ren, "Weight of pharks:", "5", 0, 0, 180f, -3f * ren.fSeparation, 100f),
										new TextButton(ren, "Done", 0, -1, 0f, 2f * ren.separation, 300f, new Color3("557755")).setAction(setCustomRPF),
										new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent),
										new Text(ren, "", 0, 1, 0f, 60f)).setWriting();
	}
	
	public void addParticleScreen(){
		ren.current = new Screen(ren, new Text(ren, "Add particle", 0, 1, 0f, 20f),
										new Field(ren, "Color:", "#CCCCCC", 0, 0, 80f, 2f * ren.fSeparation, 100f),
										new Field(ren, "Mass:", "1", 0, 0, 80f, ren.fSeparation, 100f),
										new Field(ren, "Radius:", "1", 0, 0, 80f, 0f, 100f),
										new Field(ren, "Charge:", "0", 0, 0, 80f, -1f * ren.fSeparation, 100f).setDescription("Electrical charge"),
										new Field(ren, "Weak charge:", "0", 0, 0, 80f, -2f * ren.fSeparation, 100f).setDescription("Weak, long range weak force"),
										new TextButton(ren, "Done", 0, -1, 0f, 2f * ren.separation, 300f, new Color3("557755")).setAction(addParticle),
										new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent),
										new Text(ren, "", 0, 1, 0f, 60f)).setWriting();
	}
	
	void setRanSS(){
		if(sim.isRunning){
			return;
		}
		
		sim.reset(PlanetSystem.Random);
		ren.cam.setFollow(null);
	}
	
	void setSolara(){
		if(sim.isRunning){
			return;
		}
		
		sim.reset(Examples.solara);
		ren.cam.setFollow(null);
	}
	
	void setKyra(){
		if(sim.isRunning){
			return;
		}
		
		sim.reset(Examples.kyra);
		ren.cam.setFollow(null);
	}
	
	void github(){
		Process.Start(new ProcessStartInfo("https://github.com/Dumbelfo08"){UseShellExecute = true});
	}
	
	void desmos(){
		Process.Start(new ProcessStartInfo("https://www.desmos.com/calculator/8bq31utqb4"){UseShellExecute = true});
	}
	
	void youtube(){
		Process.Start(new ProcessStartInfo("https://www.youtube.com/watch?v=LXCAxlIyGBQ&t"){UseShellExecute = true});
	}
	
	void newScene(){
		ren.current = new Screen(ren, new Text(ren, "New Simulation", 0, 1, 0f, 20f),
										new TextButton(ren, "Solar System", 0, 0, 0f, 3f * ren.separation, 300f, new Color3("555577")).setAction(setSolarSystem),
										new TextButton(ren, "Solara System", 0, 0, 0f, 2f * ren.separation, 300f, new Color3("555577")).setDescription("Fictional Solar System").setAction(setSolara),
										new TextButton(ren, "Kyra System", 0, 0, 0f, 1f * ren.separation, 300f, new Color3("555577")).setDescription("Fictional Solar System").setAction(setKyra),
										new TextButton(ren, "Random solar system", 0, 0, 0f, 0f, 300f, new Color3("555577")).setDescription("Could be unstable!").setAction(setRanSS),
										new TextButton(ren, "RPF", 0, 0, 0f, -1f * ren.separation, 300f, new Color3("555577")).setDescription("Field randomly populated with elemental particles").setAction(setRPF),
										new ImageBackButton(ren, "pencil", 0, 0, 175f, -1f * ren.separation, ren.textSize.Y + 10f, ren.textSize.Y + 10f, ren.textColor, new Color3("555577")).setDescription("Edit").setAction(customRPF),
										new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent));
	}
	
	void info(){
		ren.current = new Screen(ren, new Text(ren, "Info", 0, 1, 0f, 20f),
										new Text(ren, "Unisim, created by Dumbelfo", 0, 1, 10f, 3f * ren.textSize.Y),
										new Text(ren, "Version 1.0.0", 0, 1, 0f, 4f * ren.textSize.Y),
										new Text(ren, "Particle simulator, aiming to simulate both planet systems", -1, 1, 10f, 6f * ren.textSize.Y),
										new Text(ren, "and elemental particles", -1, 1, 10f, 7f * ren.textSize.Y),
										new TextButton(ren, "GitHub", 0, -1, -200f, 3f * ren.separation, 190f, new Color3("555577")).setAction(github),
										new TextButton(ren, "Desmos", 0, -1, 0f, 3f * ren.separation, 190f, new Color3("555577")).setDescription("Graph showing forces between two particles").setAction(desmos),
										new TextButton(ren, "Youtube", 0, -1, 200f, 3f * ren.separation, 190f, new Color3("555577")).setAction(youtube),
										new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent));
	}
	
	void help(){
		ren.current = new Screen(ren, new Text(ren, "You can pause the simulation with the space bar or clicking the icon", -1, 1, 10f, 0f),
										new Text(ren, "You can follow a particle by clicking on it or using Tab", -1, 1, 10f, ren.textSize.Y),
										new Text(ren, "You can click the camera icon or press Shift+Tab to stop following", -1, 1, 10f, 2f * ren.textSize.Y),
										new Text(ren, "You can move the camera with WASD", -1, 1, 10f, 3f * ren.textSize.Y),
										new Text(ren, "You can access advanced with Alt", -1, 1, 10f, 4f * ren.textSize.Y),
										new Text(ren, "You can toggle fullscreen with F11", -1, 1, 10f, 5f * ren.textSize.Y),
										new Text(ren, "You can take a screenshot with F2", -1, 1, 10f, 6f * ren.textSize.Y),
										new Text(ren, "You can advance 1 tick with F", -1, 1, 10f, 7f * ren.textSize.Y),
										new Text(ren, "You can change simulation speed with numpad + and - or with icons", -1, 1, 10f, 8f * ren.textSize.Y),
										new Text(ren, "You can see velocites(yellow) and forces(green) with F3", -1, 1, 10f, 9f * ren.textSize.Y),
										new Text(ren, "You can see bounding boxes and collision points with F4", -1, 1, 10f, 10f * ren.textSize.Y),
										new Text(ren, "You can toggle background clouds with F5", -1, 1, 10f, 11f * ren.textSize.Y),
										new Text(ren, "You can toggle particle points with F6", -1, 1, 10f, 12f * ren.textSize.Y),
										new Text(ren, "You add particles with the icon, and then choose position and velocity with right click", -1, 1, 10f, 13f * ren.textSize.Y),
										new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent));
	}
	
	void handleKeyboardInput(){
		// check to see if the window is focused
		if(!IsFocused){
			return;
		}
		
		switch(escape.isActiveMod(KeyboardState)){
			case 1:
			//Close();
			if(ren.current != null){
				ren.current = null;
			}else{
				if(sim.isRunning){
					ren.pause();
				}
				ren.current = new Screen(ren, new Text(ren, "Pause Menu", 0, 1, 0f, 20f),
											new TextButton(ren, "Close", 0, -1, 0f, 1f * ren.separation, 300f, new Color3("775555")).setAction(closeCurrent),
											new TextButton(ren, "New Simulation", 0, 0, 0f, 0f, 300f, new Color3("555577")).setAction(newScene),
											new TextButton(ren, "Help", 0, 0, 0f, -1f * ren.separation, 300f, new Color3("555577")).setAction(help),
											new TextButton(ren, "Info", 0, 0, 0f, -2f * ren.separation, 300f, new Color3("555577")).setAction(info),
											new TextButton(ren, "Quit", 0, 0, 0f, 1f * ren.separation, 300f, new Color3("557755")).setAction(Close));
			}
			break;
			
			case 2:
			Close();
			break;
		}
		
		if(screnshot.isActive(KeyboardState)){
			captureScreenshot();
			ren.setCornerInfo("Saved screenshot");
		}
		
		if(fullscreen.isActive(KeyboardState)){
			toggleFullscreen();
		}
		
		if(ren.current != null){
			if(ren.current.writing){
				if(del.isActive(KeyboardState)){
					ren.current.tryDel();
				}else if(point.isActive(KeyboardState)){
					ren.current.tryAdd('.');
				}else if(min.isActive(KeyboardState)){
					ren.current.tryAdd('-');
				}else if(k0.isActive(KeyboardState)){
					ren.current.tryAdd('0');
				}else if(k1.isActive(KeyboardState)){
					ren.current.tryAdd('1');
				}else if(k2.isActive(KeyboardState)){
					ren.current.tryAdd('2');
				}else if(k3.isActive(KeyboardState)){
					ren.current.tryAdd('3');
				}else if(k4.isActive(KeyboardState)){
					ren.current.tryAdd('4');
				}else if(k5.isActive(KeyboardState)){
					ren.current.tryAdd('5');
				}else if(k6.isActive(KeyboardState)){
					ren.current.tryAdd('6');
				}else if(k7.isActive(KeyboardState)){
					ren.current.tryAdd('7');
				}else if(k8.isActive(KeyboardState)){
					ren.current.tryAdd('8');
				}else if(k9.isActive(KeyboardState)){
					ren.current.tryAdd('9');
				}else if(ka.isActive(KeyboardState)){
					ren.current.tryAdd('A');
				}else if(kb.isActive(KeyboardState)){
					ren.current.tryAdd('B');
				}else if(kc.isActive(KeyboardState)){
					ren.current.tryAdd('C');
				}else if(kd.isActive(KeyboardState)){
					ren.current.tryAdd('D');
				}else if(ke.isActive(KeyboardState)){
					ren.current.tryAdd('E');
				}else if(kf.isActive(KeyboardState)){
					ren.current.tryAdd('F');
				}
			}
			return;
		}
		
		if(debugMode.isActive(KeyboardState)){
			ren.toggleDebugMode();
		}
		
		if(showForces.isActive(KeyboardState)){
			ren.modes[3].toggleActivation();
			ren.modes[4].toggleActivation();
			sim.tryGenerate();
			
			if(ren.modes[3].active){
				ren.setCornerInfo("Forces enabled");
			}else{
				ren.setCornerInfo("Forces disabled");
			}
		}
		
		if(showPoints.isActive(KeyboardState)){
			ren.modes[2].toggleActivation();
			
			if(ren.modes[2].active){
				ren.setCornerInfo("Points enabled");
			}else{
				ren.setCornerInfo("Points disabled");
			}
		}
		
		if(showBoxes.isActive(KeyboardState)){
			ren.modes[5].toggleActivation();
			ren.modes[6].toggleActivation();
			sim.tryGenerate();
			
			if(ren.modes[5].active){
				ren.setCornerInfo("Bounding boxes enabled");
			}else{
				ren.setCornerInfo("Bounding boxes disabled");
			}
		}
		
		if(showClouds.isActive(KeyboardState)){
			ren.modes[0].toggleActivation();
			sim.tryGenerate();
			
			if(ren.modes[0].active){
				ren.setCornerInfo("Clouds enabled");
			}else{
				ren.setCornerInfo("Clouds disabled");
			}
		}
		
		if(debugInfo.isActive(KeyboardState)){
			Console.WriteLine(sim.tt.meanInfo());
			/* File.WriteAllText("log.tlog", Simulation.tl.getLog());
			
			string notepadPlusPlusPath = @"C:\Program Files\Notepad++\notepad++.exe"; // Adjust this path if necessary
			
			// Use Process.Start to open the file with Notepad++
			Process.Start(new ProcessStartInfo
			{
				FileName = notepadPlusPlusPath,
				Arguments = "\"log.tlog\"", // Quote the file path to handle spaces
				UseShellExecute = false // Ensures the process uses the provided executable
			}); */
		}
		
		switch(nextParticle.isActiveMod(KeyboardState)){
			case 1:
				Particle p = sim.getNextParticle(ren.cam.follow);
				ren.cam.setFollow(p);
				break;
			case 2:
				ren.cam.setFollow(null);
				break;
		}
		
		if(tickRateUp.isActive(KeyboardState)){
			sim.targetTPS *= 1.3f;
			ren.setCornerInfo(sim.targetTPS.ToString("F0"));
		}
		
		if(tickRateDown.isActive(KeyboardState)){
			sim.targetTPS /= 1.3f;
			ren.setCornerInfo(sim.targetTPS.ToString("F0"));
		}
		
		if(tick.isActive(KeyboardState)){
			if(!sim.isRunning){
				sim.tick();
				ren.setCornerInfo("Tick advanced");
			}
		}
		
		if(pause.isActive(KeyboardState)){
			ren.pause();
		}
		
		if(moveUp.isActive(KeyboardState)){
			ren.cam.moveUp((float) dh.deltaTime);
		}
		
		if(moveDown.isActive(KeyboardState)){
			ren.cam.moveDown((float) dh.deltaTime);
		}
		
		if(moveLeft.isActive(KeyboardState)){
			ren.cam.moveLeft((float) dh.deltaTime);
		}
		
		if(moveRight.isActive(KeyboardState)){
			ren.cam.moveRight((float) dh.deltaTime);
		}
		
		ren.cam.endFrame();
	}
	
	void toggleFullscreen(){
		if(!isFullscreened){
			MonitorInfo mi = Monitors.GetMonitorFromWindow(this);
			WindowState = WindowState.Fullscreen;
			this.CurrentMonitor = mi.Handle;
			isFullscreened = true;
			VSync = VSyncMode.On;
		} else {
			WindowState = WindowState.Normal;
			isFullscreened = false;
			VSync = VSyncMode.On;
		}
	}
	
	public static void checkErrors(){
		OpenTK.Graphics.OpenGL.ErrorCode errorCode = GL.GetError();
        while (errorCode != OpenTK.Graphics.OpenGL.ErrorCode.NoError)
        {
            Console.WriteLine($"OpenGL Error: {errorCode}");
            errorCode = GL.GetError();
        }
	}
	
	void captureScreenshot(){
		int width = ren.width;  // Or use your window's width
		int height = ren.height; // Or use your window's height
		
		// Create a byte array to hold the pixel data
		byte[] pixels = new byte[width * height * 3]; // RGBA (4 bytes per pixel)
		
		// Read pixels from OpenGL frame buffer
		GL.ReadBuffer(ReadBufferMode.Front);
		GL.ReadPixels(0, 0, width, height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, pixels);
		
		// Create a new byte array to hold the RGB data (ignore alpha channel)
		byte[] rgbPixels = new byte[width * height * 3]; // RGB (3 bytes per pixel)
		
		// Copy only RGB values (ignore alpha channel)
		for (int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				rgbPixels[((height - i - 1) * width + j) * 3] = pixels[(i * width + j) *3];      // Red
				rgbPixels[((height - i - 1) * width + j) * 3 + 1] = pixels[(i * width + j) * 3 + 1];  // Green
				rgbPixels[((height - i - 1) * width + j) * 3 + 2] = pixels[(i * width + j) * 3 + 2];  // Blue
			}
		}
		
		// Create a Bitmap and write the RGB pixel data into it
		using (Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
		{
			BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height),
											ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			System.Runtime.InteropServices.Marshal.Copy(rgbPixels, 0, data.Scan0, rgbPixels.Length);
			bitmap.UnlockBits(data);
	
			// Save the image (or copy to clipboard if needed)
			bitmap.Save("screenshot.png", ImageFormat.Png);
		}
	}
	
	void setIcon(){
		byte[] imageBytes = AssemblyFiles.get("res.icon.png");
		
		//Generate the image and put it as icon
		ImageResult image = ImageResult.FromMemory(imageBytes, ColorComponents.RedGreenBlueAlpha);
		if (image == null || image.Data == null){
			return;
		}
		
		OpenTK.Windowing.Common.Input.Image i = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, image.Data);
		WindowIcon w = new WindowIcon(i);
		
		this.Icon = w;
	}
	
	protected override void OnLoad(){		
		initialize();
		base.OnLoad();
	}
	
	protected override void OnResize(ResizeEventArgs args){
		onResize(args.Width, args.Height);
		base.OnResize(args);
	}
	
	protected override void OnUpdateFrame(FrameEventArgs args){
		handleKeyboardInput();
		base.OnUpdateFrame(args);
	}
	
	protected override void OnRenderFrame(FrameEventArgs args){
		ren.draw();
		Context.SwapBuffers();
		checkErrors();
		base.OnRenderFrame(args);
		dh.Frame();
		//dh.Target(144f);
	}
	
	protected override void OnMouseWheel(MouseWheelEventArgs args){
		if(ren.current != null){
			return;
		}
		
		ren.cam.scroll(args.OffsetY);
        
		base.OnMouseWheel(args);
    }
	
	protected override void OnMouseMove(MouseMoveEventArgs e){
        ren.cam.mouse(e.X, e.Y);
		base.OnMouseMove(e);
    }
	
	protected override void OnMouseDown(MouseButtonEventArgs e){
        if(e.Button == MouseButton.Left){
			Screen s;
			if(ren.current == null){
				if(!ren.main.click(ren.cam.mouseLastPos)){
					Particle p = sim.getParticleCursor(ren.cam.mouseWorldPos);
					if(p != null){
						ren.cam.setFollow(p);
					}
				}
			}else{
				ren.current.click(ren.cam.mouseLastPos);
			}
        }else if(e.Button == MouseButton.Right){
			if(ghost != null && !ren.ghostLocked){
				ghost.translate(ren.cam.mouseWorldPos);
				ren.ghostLocked = true;
			}
		}
		
		base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e){
        if(e.Button == MouseButton.Right){
			if(ghost != null && ren.ghostLocked){
				ghost.addVelocity(ghost.position - ren.cam.mouseWorldPos);
				sim.addParticle(ghost);
				ghost = null;
				ren.ghost = null;
				ren.ghostLocked = false;
				sim.tryGenerate();
			}
        }
		
		base.OnMouseUp(e);
    }
}