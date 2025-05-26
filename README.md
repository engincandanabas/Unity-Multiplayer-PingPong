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

  <img src="[screenshots/menu.png](https://github.com/user-attachments/assets/e394bdf1-c14f-49bc-9ea6-51fed5a4a9d6)" width="400" alt="Main Menu"/>
  <img src="screenshots/gameplay.png" width="400" alt="Gameplay Screen"/>
</p>

## 🎯 Purpose

This project serves as a learning-friendly base for developers looking to understand and experiment with Unity's Netcode for GameObjects. The code is kept minimal and readable, making it easy to build upon.

---

