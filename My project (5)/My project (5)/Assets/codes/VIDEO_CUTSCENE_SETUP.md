# Video Cutscene Setup Instructions

## Overview
This setup allows you to play a video cutscene after Level 1 that automatically redirects to Level 2 when finished.

## Step 1: Prepare Your Video File
1. Export/prepare your video file (recommended formats: `.mp4`, `.mov`, `.webm`)
2. Create a `StreamingAssets` folder in your Unity project:
   - Right-click in the Assets folder → Create → Folder → Name it "StreamingAssets"
3. Place your video file in the StreamingAssets folder
4. Note the exact filename (e.g., `Level1Cutscene.mp4`)

## Step 2: Create the Cutscene Scene
1. In Unity, create a new scene: File → New Scene
2. Save it as `Leve2CutScene` (or your preferred name)
3. In the Hierarchy, create the following:
   - Right-click → UI → Canvas (this will also create an EventSystem)
   - Right-click on Canvas → Video → Video Player

## Step 3: Setup the Video Player
1. Select the VideoPlayer object in the Hierarchy
2. In the Inspector:
   - **Render Mode**: Set to "Camera Far Plane" or "Camera Near Plane"
   - **Camera**: Drag the Main Camera here
   - **Play On Awake**: Uncheck this (the script will handle playback)
   - **Wait For First Frame**: Check this
   - **Source**: Set to "URL"
   
## Step 4: Add the VideoCutsceneController Script
1. Select the VideoPlayer object (or create an empty GameObject)
2. Add Component → Scripts → VideoCutsceneController
3. Configure the script in Inspector:
   - **Video Player**: Drag the VideoPlayer component here
   - **Video File Name**: Enter your video filename (e.g., `Level1Cutscene.mp4`)
   - **Next Scene Name**: Enter `level 2` (or the exact name of your Level 2 scene)
   - **Skip On Input**: Check if you want players to skip with any key press

## Step 5: Optional - Add Skip Prompt UI
1. In the Canvas, create a Text element:
   - Right-click on Canvas → UI → Text - TextMeshPro (or Legacy Text)
2. Position it at the bottom of the screen
3. Set text to "Press any key to skip..."
4. In VideoCutsceneController Inspector:
   - Drag this text object to **Skip Prompt UI**

## Step 6: Optional - Add Fade Transition
1. In the Canvas, create an Image:
   - Right-click on Canvas → UI → Image
2. Set it to cover the full screen (Anchor Presets: stretch/stretch)
3. Set color to black with Alpha = 0
4. Add Component → Canvas Group
5. In VideoCutsceneController Inspector:
   - Drag this Image's CanvasGroup to **Fade Canvas**

## Step 7: Configure Level 1 Portal
1. Open your Level 1 scene
2. Select the Portal object at the end of Level 1
3. In the PortalController component:
   - **Play Cutscene Before Level**: Check this box
   - **Cutscene Scene Name**: Enter `Leve2CutScene` (match your cutscene scene name)
   - **Next Level Name**: This can be left as is (the cutscene will handle the transition)

## Step 8: Add Scenes to Build Settings
1. Go to File → Build Settings
2. Make sure these scenes are in the build (in order):
   - Your main menu/level 0
   - Level 1
   - **Leve2CutScene** (your new cutscene scene)
   - level 2
   - Other levels...
3. The order matters - note the scene indices if using build indices

## Testing
1. Play Level 1 in Unity
2. Complete the level and enter the portal
3. The cutscene should play automatically
4. After the video ends (or if skipped), it should transition to Level 2

## Troubleshooting

### Video doesn't play:
- Check that the video file is in StreamingAssets folder
- Verify the filename matches exactly (case-sensitive)
- Check Unity Console for error messages
- Try a different video format (MP4 is most compatible)

### Scene doesn't transition:
- Verify the scene name "level 2" matches exactly in Build Settings
- Check that the scene is added to Build Settings
- Look for errors in the Unity Console

### Video Player not visible:
- Check that Render Mode is set correctly
- Verify the Camera reference is assigned
- Make sure the Canvas is in Screen Space mode

## Advanced Options

### Using Build Index Instead of Scene Names:
In VideoCutsceneController:
- Leave **Next Scene Name** empty
- Set **Next Scene Index** to the build index number of Level 2

### Disabling Skip Feature:
- Uncheck **Skip On Input** in VideoCutsceneController

### Playing Audio Only:
- You can use the same setup with just an AudioSource instead of VideoPlayer
- Modify the script to use AudioClip instead of VideoPlayer

## Files Created/Modified
- `VideoCutsceneController.cs` - New script for video playback and scene transition
- `PortalController.cs` - Updated to support cutscene redirection
