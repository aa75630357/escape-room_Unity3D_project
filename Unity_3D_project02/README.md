Escape Room 3D (3D密室逃脫)
這是一款使用 Unity 3D 開發的第一人稱解謎逃脫遊戲。玩家需要在充滿未知與機關的 3D 空間中，透過敏銳的觀察力與邏輯推理，尋找隱藏線索、收集關鍵道具，並破解多重連鎖謎題，最終找出成功逃離密室的方法。

📂 專案核心目錄導覽
本專案採用標準化 Unity 目錄結構進行管理，確保場景物件與程式邏輯的解耦，大幅提升專案的程式碼可讀性與後續維護效率：

Assets/Models_and_Materials/: 包含場景環境的 3D 模型、貼圖與材質球。

Assets/Scenes/:

MainRoom.unity: 遊戲主密室場景，整合所有解謎機關、燈光與環境佈置。

Assets/Scripts/ ⬅️ 【核心邏輯】面試官請點此檢視代碼

InteractController.cs: 處理玩家視角射線檢測與場景物件互動邏輯。

InventorySystem.cs: 玩家背包系統，管理道具的拾取、檢視與使用。

PuzzleManager.cs: 核心解謎管理器，負責判定密碼鎖或特定通關條件是否達成。

ProjectSettings/: Unity 專案的輸入、物理與標籤等核心設定檔。

🛠 關鍵技術實現
在這個 Unity 3D 專案中，我重點開發了以下技術模組：

物件互動與射線檢測 (Raycast Interaction) * 技術： 熟練運用 Unity 的 Physics.Raycast 與 C# 介面 (Interface) 概念。

實現： 實作了精準的第一人稱互動機制，透過攝影機中心向準心發射射線，判定前方物件是否具備可互動屬性（如點擊開門、拾取道具），確保互動判定的高效性並將邏輯獨立封裝。

狀態管理與解謎觸發 (State & Puzzle Event)

技術： 結合 C# 事件委派 (Action/Delegate) 與 UGUI 系統。

實現： 建立解謎管理器 (PuzzleManager)，當玩家在正確位置使用特定道具或解開密碼時，透過事件廣播無縫觸發場景機關的動畫與音效，達成各個系統之間的低耦合 (Low coupling) 設計。
