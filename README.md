# 🏓 Unity NGO Multiplayer Ping Pong

This project is a simple **real-time two-player Ping Pong** game built using Unity’s official multiplayer framework: **Netcode for GameObjects (NGO)**. The goal is to demonstrate how to build a basic server-authoritative multiplayer game using Unity's networking tools.

## 🎮 Game Overview

- Players can join as host or client from the main menu.
- The ball is controlled only by the host and its movement is synchronized across clients.
- Paddle movements are handled locally and sent to the server, then synced with the other client.
- The score system is synchronized using network variables.

## 🧠 Technical Details

- Built with a **server-authoritative** architecture.
- Uses Unity's `NetworkManager` for connection flow (host/client selection).
- Synchronization of paddles and ball via `NetworkTransform`.
- Real-time score updates handled with `NetworkVariable`.
- Simple UI displays score and game status.

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

