# Unity Multiplayer Ping Pong

## 🎯 Project Overview

This is my **first multiplayer game project**, developed as a learning exercise to get hands-on experience with Unity's multiplayer systems. The game is a simple real-time two-player Ping Pong built using **Unity 6.0** and the **Netcode for GameObjects** framework. The architecture follows a **host-authoritative** model where the host controls the main game logic and synchronizes the state with the client.

## 🧠 How It Works

- Players join the game by selecting either **host** or **client** from the main menu.
- The host manages the ball's movement and game logic.
- Both players control their paddles locally; movements are sent to the host and synchronized with the other player.
- Scores are tracked and synchronized via network variables.
- The game demonstrates core multiplayer concepts such as networked transforms, server-authoritative control, and state synchronization.

## 🛠️ Technologies Used

- Unity 6.0
- Netcode for GameObjects (Unity’s official multiplayer framework)
- C# scripting

## 📸 Screenshots

<p align="center">

  <img src="https://github.com/user-attachments/assets/e394bdf1-c14f-49bc-9ea6-51fed5a4a9d6" width="400" alt="Settings"/>
  <img src="https://github.com/user-attachments/assets/8caa7c52-f2ce-48eb-9f3f-9f577e360126" width="400" alt="Create Lobby"/>
  <img src="https://github.com/user-attachments/assets/b8c135a9-f21c-4a2b-b8de-7b2929e67d54" width="400" alt="Lobby List"/>
  <img src="https://github.com/user-attachments/assets/9e2f9473-4843-4396-b1c8-84cb156c5c6e" width="400" alt="Gameplay1"/>
  <img src="https://github.com/user-attachments/assets/159d476f-562e-4f47-9982-500f95e6223c" width="400" alt="Gameplay2"/>
</p>

## 🎯 Purpose

This project serves as a learning-friendly base for developers looking to understand and experiment with Unity's Netcode for GameObjects. The code is kept minimal and readable, making it easy to build upon.

