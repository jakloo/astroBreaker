# Astro Breaker

In this simple arcade game for Cardboard VR, you destroy debris floating in space to get the best score possible. It is made in Unity for Android phones with a Cardboard headset.

## Controls
- Head (mouse in Unity editor) movements.
- To press a button, tap/click anywhere while looking at the button (indicated by the button changing its color).

## How to play
- Debris are spawning and moving pseudo-randomly. They have different colors (6 colors total).
- At any given time, there is only one color of debris you can destroy.
- You destroy a piece of debris by looking straight at it for one second.
- You get more points by destroying debris that is smaller, faster, and/or more away.
- You have 3 lives, once you lose all of them, the game ends. Get as many points as possible until then to reach a high score!
- To pause the game while playing, look under your feet and click. A pause menu then appears in the space in front of you.

There are two different game modes:

### Space mode
- Debris spawn anywhere in the space around you (not behind you though, no need to look back!)
- Bottom left corner shows the curent color and number of debris that you must find and destroy.
- There is a time limit to complete the task. Everytime the timer reaches zero, it starts again and you lose a life.
- The timer refreshes everytime you destroy some debris.

### Atmosphere mode
Destroy all the debris before it falls down to Earth!
- Debris spawns at the top of the space and falls straight down.
- When a debris reaches a certain altitude close to the bottom of the screen, it starts burning. If it burns for 10 seconds, it disappears and you lose a life.
- You manually choose which color of debris you can destroy. Bottom left shows the currently chosen color. Move to the next color by clicking. Hold the click longer to keep moving down the color list.

## Upgrades
In both game modes, special upgrades are spawned alongside regular debris.
- Upgrades are colorless - you can always destroy them.
- There are 3 types of upgrades.
- Looking at an upgrade shows you which type it is.
- Destroying an upgrade instantly grants you a special effect based on the upgrade's type:

### Freeze
- For a certain duration shown in the left bottom corner, all debris stop moving.
- In Space mode, the timer stops.
- In Atmosphere mode, the burning debris 10 second limit timer is stopped.
- No new objects spawn during the duration.

### Boom
- Destroys all debrs and upgrades in a spherical area around the upgrade.
- This triggers all other upgrades destroyed.

### Double
- You get up to 3 charges of Double strike, marked under your reticle.
- When you destroy debris and there is at least one other debris of the same color, anywhere on the map, one Double strike charge is used to destroy that other debris.
- If there are multiple targets of the same color, the closest one is destroyed.

