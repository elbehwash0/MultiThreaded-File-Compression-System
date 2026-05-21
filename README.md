# MultiThreaded File Compression System

A multi-threaded client-server application developed in **C#** using **TCP sockets** and **WinForms**.  
The system allows multiple clients to upload files simultaneously to a server, where the files are compressed into ZIP format and returned back to the client.

---

# Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [System Architecture](#system-architecture)
- [File Transfer Protocol](#file-transfer-protocol)
- [User Interface](#user-interface)
- [Project Structure](#project-structure)
- [How to Run](#how-to-run)
- [Multi-threading Implementation](#multi-threading-implementation)
- [Compression Mechanism](#compression-mechanism)
- [Networking Concepts Used](#networking-concepts-used)
- [Error Handling](#error-handling)
- [Example Workflow](#example-workflow)
- [Future Improvements](#future-improvements)
- [License](#license)

---

# Features

- Multi-threaded TCP server
- Concurrent client handling
- Binary file transfer
- ZIP file compression
- Windows Forms graphical client
- File upload and download support
- Automatic compressed file saving
- TCP socket communication

---

# Technologies Used

- C#
- .NET
- TCP Sockets
- Multithreading
- WinForms
- System.IO.Compression

---

# System Architecture

## Server

The server is responsible for:

1. Accepting multiple client connections simultaneously
2. Receiving uploaded files
3. Compressing files into ZIP format
4. Sending compressed files back to clients

## Client

The client application allows the user to:

1. Select a local file
2. Upload the file to the server
3. Receive the compressed ZIP file
4. Save the compressed file locally

---

# File Transfer Protocol

## Client → Server

The client sends:

1. File name length
2. File name
3. File size
4. File binary data

## Server → Client

The server sends:

1. Compressed file size
2. Compressed file binary data

---

# User Interface

The client application includes a simple graphical interface that allows:

- File selection
- File sending
- Compression status display
- ZIP file receiving and saving

---

# Project Structure

```text
MultiThreaded-File-Compression-System
│
├── Client/
│   ├── Form1.cs
│   └── ...
│
├── Server/
│   ├── Program.cs
│   └── ...
│
└── CompressionProject.sln
```

---

# How to Run

## Server

1. Open the solution in Visual Studio
2. Set the **Server** project as Startup Project
3. Run the server
4. The server starts listening on port `5000`

## Client

1. Set the **Client** project as Startup Project
2. Run the client application
3. Select a file using the GUI
4. Click the **Send File** button
5. The compressed ZIP file will be received and saved automatically

---

# Multi-threading Implementation

The server uses a separate thread for each connected client:

```csharp
Thread t = new Thread(() => HandleClient(client));
t.Start();
```

This allows the server to handle multiple clients simultaneously without blocking other connections.

---

# Compression Mechanism

The server compresses files using:

```csharp
System.IO.Compression.ZipArchive
```

Each received file is compressed into ZIP format and returned back to the client through TCP binary transfer.

---

# Networking Concepts Used

- TCP Socket Programming
- NetworkStream
- Binary File Transfer
- Multi-threading
- Client-Server Architecture
- File Compression

---

# Error Handling

The application handles:

- Multiple client connections
- Binary file transfer
- File saving operations
- TCP communication
- ZIP file creation

---

# Example Workflow

```text
Client selects file
        ↓
Client sends file to server
        ↓
Server receives file
        ↓
Server compresses file into ZIP
        ↓
Server sends compressed ZIP back
        ↓
Client receives and saves ZIP file
```

---

# Future Improvements

- Progress bar for uploads/downloads
- Drag and drop file support
- Better exception handling
- File encryption support
- Multiple file compression
- Cross-device networking support

---

# License

This project is developed for educational purposes.