using System.IO.Compression;
using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);

        server.Start();

        Console.WriteLine("Server Started");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("Client Connected");

            Thread t = new Thread(() => HandleClient(client));

            t.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();

        //////////////////////////////////////////////////////
        // RECEIVE FILE NAME
        //////////////////////////////////////////////////////

        byte[] nameSizeBytes = new byte[4];
        stream.Read(nameSizeBytes, 0, 4);

        int nameLength = BitConverter.ToInt32(nameSizeBytes, 0);

        byte[] nameBytes = new byte[nameLength];
        stream.Read(nameBytes, 0, nameLength);

        string fileName = System.Text.Encoding.UTF8.GetString(nameBytes);

        //////////////////////////////////////////////////////
        // RECEIVE FILE SIZE
        //////////////////////////////////////////////////////

        byte[] sizeBytes = new byte[8];
        stream.Read(sizeBytes, 0, 8);

        long size = BitConverter.ToInt64(sizeBytes, 0);

        //////////////////////////////////////////////////////
        // RECEIVE FILE DATA
        //////////////////////////////////////////////////////

        byte[] data = new byte[size];

        int i = 0;

        while (i < size)
        {
            int r = stream.Read(data, i, (int)(size - i));

            i += r;
        }

        //////////////////////////////////////////////////////
        // SAVE FILE
        //////////////////////////////////////////////////////

        string receivedName = Guid.NewGuid().ToString() + "_" + fileName;

        File.WriteAllBytes(receivedName, data);

        Console.WriteLine("File Received");

        //////////////////////////////////////////////////////
        // COMPRESS FILE
        //////////////////////////////////////////////////////

        string zipName = Guid.NewGuid().ToString() + ".zip";

        FileStream fs = new FileStream(zipName, FileMode.Create);

        ZipArchive zip = new ZipArchive(fs, ZipArchiveMode.Create);

        zip.CreateEntryFromFile(receivedName, fileName);

        zip.Dispose();
        fs.Close();

        Console.WriteLine("File Compressed");

        //////////////////////////////////////////////////////
        // SEND ZIP SIZE
        //////////////////////////////////////////////////////

        byte[] zipData = File.ReadAllBytes(zipName);

        long zipSize = zipData.Length;

        byte[] zipSizeBytes = BitConverter.GetBytes(zipSize);

        stream.Write(zipSizeBytes, 0, 8);

        //////////////////////////////////////////////////////
        // SEND ZIP DATA
        //////////////////////////////////////////////////////

        stream.Write(zipData, 0, zipData.Length);

        Console.WriteLine("Compressed File Sent");

        //////////////////////////////////////////////////////
        // CLEANUP
        //////////////////////////////////////////////////////

        File.Delete(receivedName);
        File.Delete(zipName);

        client.Close();
    }
}