# ☠️ BoneRipper

<a><img src="ReadMe Support/vlcsnap-2024-01-12-18h09m05s503.png" style="width:100%;"></a>

## 🎲 About Game
<a><img src="ReadMe Support/vlcsnap-2024-01-12-18h10m31s445.png" style="width:49%;"> <img src="ReadMe Support/vlcsnap-2024-01-12-18h10m50s896.png" style="width:49%;"></a>
</br></br>
In this game, the player plays as a bone-ripper who rips enemies and collects the bones.
1. At the start of the game, the player is spawned in a raft moving through the ocean. Once he reaches the nearest proximity to the enemy island, he has to jump to the island, and the level starts.
2. In each level, the player has to move toward enemies, hit them, and kill them.
3. The direction alone can be controlled using the cursor.
4. At the same time the player has to escape the arrows shot by the tower enemies and the tower enemies will be in an unreachable position.
5. The player has to find and move towards the final destination to pass the current level.
6. The bone collected during the rampage is counted towards the high score.

## 🎹 Controls
> 🖱**Left mouse click** -> Player to jump from the raft.<br>
> ↗ **cursor**           -> decides the direction for the player to move.

 ## 💡 Features
 <a><img src="ReadMe Support/vlcsnap-2024-01-12-18h13m09s397.png" style="width:49%;"> <img src="ReadMe Support/vlcsnap-2024-01-12-18h10m07s648.png" style="width:49%;"></a>
 </br></br>
 1. Player can customize their character using the in-game shop system, which will remain saved and can be reset.
 2. Can create a higher record than their previous one,  and also have a reset functionality for high-score.
 3. Easy controls to navigate and play the game.
 4. Shootable enemies can predict the player's movement and shoot ( **! Beware**)

## 🧩 Implementations
1. Created a generic abstract pooling system, which is used to main multiple pools for bones, weapons, arrows, and jump path trailing.
2. Implemented scriptable objects to create data sets for shop items, the player, enemies, and the boat.
3. Used singleton pattern to run different services like GameServices, PlayerServices, EnemyServices, WeaponServices etc.
4. Implemented procedural generation scripts for island creation, so that each level iteration will have a unique place to play.
5. Island mesh created and unwrapped programmatically.
6. Created a shader graph(node-based) for the procedurally generated island, so that a blend of rock and grass textures can be applied.
7. Used Observer pattern to implement less coupled events.
8. Implemented many of the objects that have a lot of logic using the Model-View-Controller pattern.
</br></br>
<a><img src="ReadMe Support/vlcsnap-2024-01-12-18h12m51s434.png" style="width:49%;"> <img src="ReadMe Support/vlcsnap-2024-01-12-18h11m00s454.png" style="width:49%;"></a>
</br>

## ⚙️ Notable Game-Editor Functionalities

> **World Service**
>> 1. Minimum and Maximum radius for the island can be set.
>> 2. Segment count can be altered to create wavy edges for the island.
>> 3. The default number of hideouts on the island can be set.

> **Enemy Service**
>> 1. The default count of enemies can be set.
>> 2. The interval time for enemy spawn can be set.
>> 3. The number of tower enemies who can shoot simultaneously can be altered.

> **Boat Service**
>> 1. The Distance range for the spawning island can be set as required.
>> 2. The distance range to toggle the jump path can be altered.

## ▶️ Gameplay

<div align="left">
      <a href="https://www.youtube.com/watch?v=C0m0EpjuSfI">
         <img src="ReadMe Support/FireShot Capture 003 - (10) BONERIPPER Gameplay - YouTube - www.youtube.com.png" style="width:100%;">
      </a>
</div>
