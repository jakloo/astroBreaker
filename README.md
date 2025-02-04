# Astro Breaker

In this simple arcade game for Cardboard VR, you destroy debris floating in space to get the best score possible. It is made in Unity for Android phones with a Cardboard headset.

![image](https://github.com/user-attachments/assets/bdc1e188-520a-4c52-81d2-0cbd6ce55e33)

## Controls
- Head (mouse in Unity editor) movements.
- To press a button, tap/click anywhere while looking at the button (indicated by the button changing its colour).

## How to play
- Debris are spawning and moving pseudo-randomly. They have different colours (6 colours total).
- At any given time, there is only one colour of debris you can destroy.
- You destroy a piece of debris by looking straight at it for one second.
- You get more points by destroying debris that is smaller, faster, and/or farther away.
- You have 3 lives, once you lose all of them, the game ends. Get as many points as possible until then to reach a high score!
- To pause the game while playing, look under your feet and click. A pause menu then appears in the space in front of you.

There are two different game modes:

### Space mode
Complete tasks by destroying garbage in outer space!

![image](https://github.com/user-attachments/assets/6bce5d8b-adb9-4fdc-8bb9-f0d686e60ecd)

- Debris spawns anywhere in the space around you (not behind you though, no need to look back!)
- The bottom left corner shows the current colour and number of debris you must find and destroy.
- There is a time limit for completing the task. Every time the timer reaches zero, it starts again and you lose a life.
- The timer refreshes every time you destroy some debris.

### Atmosphere mode
Destroy all the debris before it falls down to Earth!

![image](https://github.com/user-attachments/assets/9f037884-7e7d-487f-b1d6-2fb04732d0b5)

- Debris spawns at the top of the space and falls straight down.
- When debris reaches a certain altitude close to the bottom of the screen, it starts burning. If it burns for 10 seconds, it disappears and you lose a life.
- You manually choose which colour of debris you can destroy. The bottom left corner shows the current colour. Move to the next colour by clicking. Hold the click longer to keep moving down the colour list.

## Upgrades
In both game modes, special upgrades are spawned alongside regular debris.

![image](https://github.com/user-attachments/assets/aeb83041-973a-49ff-9961-8a4b150ca17f)

- Upgrades are colourless, so you can always destroy them.
- There are 3 types of upgrades.
- Looking at an upgrade shows you which type it is.
- Destroying an upgrade instantly grants you a special effect based on the upgrade's type:

### Freeze
- For a certain duration shown in the left bottom corner, all debris stops moving.
- In Space mode, the timer stops.
- In Atmosphere mode, the burning debris 10-second limit timer is stopped.
- No new objects spawn during the duration.

### Double

![image](https://github.com/user-attachments/assets/a03499b2-7981-4431-aaff-7ebf3ad6c642)

- You get up to 3 charges of Double strike, marked under your reticle.
- When you destroy debris and there is at least one other debris of the same colour, anywhere on the map, one Double strike charge is used to destroy that other debris.
- If multiple targets share the same colour, the closest one is destroyed.

### Boom

![image](https://github.com/user-attachments/assets/640509a1-08c5-41f1-b16d-ff26e5631c34)

- Destroys all debris and upgrades in a spherical area around the upgrade.
- This triggers all other upgrades that are destroyed.

