## 殭屍射擊遊戲核心腳本展示（Unity + C#）

本專案為 Unity 開發的第一人稱殭屍射擊遊戲，展示我在C#程式設計、遊戲架構邏輯、AI行為與角色控制方面的實作能力。

此Repo提取專案中最具代表性的腳本，方便展示與審閱，不含場景、素材與非必要檔案。

---

## 遊戲玩法簡介

- 玩家可自由移動與射擊，使用射線偵測攻擊敵人
- 敵人具備 AI 尋路、追擊與巡邏邏輯
- 實作血量系統、子彈管理與武器控制
- 整體系統模組化，方便未來擴充與維護

---

## 核心程式模組說明

| 腳本名稱 | 功能描述 |
|----------|----------|
| "FPScontrol.cs" | 玩家第一人稱視角移動控制，搭配 `InputControl.cs` 處理輸入 |
| "InputControl.cs" | 負責統一處理鍵盤滑鼠輸入 |
| "Bullet.cs" | 子彈射出與碰撞偵測處理 |
| "ProjectGun.cs" | 玩家槍械操作，包含開火、裝彈、動畫播放等 |
| "Health.cs" | 提供血量控制、傷害處理與死亡判斷 |
| "Fighter.cs" | 敵人基礎攻擊邏輯，使用泛化處理打擊邏輯 |
| "AIController.cs" | 敵人行為核心，整合移動、追擊與攻擊邏輯 |
| "AIMover.cs" | 敵人尋路行走控制（基於 NavMesh） |
| "PatorlPath.cs" | 敵人巡邏路徑系統，搭配 AI Controller 進行路線移動 |
| "Gamemanager.cs" | 控制全局流程與遊戲狀態管理（暫停、結束等） |

---

## 使用技術

- Unity Engine 2022+
- C# 語言
- Unity NavMesh AI 尋路
- Raycast 射線偵測

---

## 使用方式

本 Repo 為程式展示用途，如需實際測試請將腳本整合至 Unity 專案中使用。

---

## 作者資訊

陳彥均｜Unity 遊戲開發學員  
熱愛遊戲開發與敘事設計，擅長以程式實作角色控制與系統邏輯，未來期望加入遊戲團隊進一步拓展實力！

---

## Demo

![遊戲畫面](Image/GamePlay1.png)
![遊戲畫面2](Image/GamePlay2.png)
