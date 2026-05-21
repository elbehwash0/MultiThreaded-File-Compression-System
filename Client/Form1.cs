using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////
        // SELECT FILE
        //////////////////////////////////////////////////////

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();

            if (op.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = op.FileName;

                label1.Text = "Status: File Selected";
            }
        }

        //////////////////////////////////////////////////////
        // SEND FILE
        //////////////////////////////////////////////////////

        private void button2_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);

            NetworkStream stream = client.GetStream();

            //////////////////////////////////////////////////
            // SEND FILE NAME
            //////////////////////////////////////////////////

            string fileName = Path.GetFileName(textBox1.Text);

            byte[] nameBytes =
                System.Text.Encoding.UTF8.GetBytes(fileName);

            byte[] nameSize =
                BitConverter.GetBytes(nameBytes.Length);

            stream.Write(nameSize, 0, 4);

            stream.Write(nameBytes, 0, nameBytes.Length);

            //////////////////////////////////////////////////
            // READ FILE
            //////////////////////////////////////////////////

            byte[] data = File.ReadAllBytes(textBox1.Text);

            long size = data.Length;

            //////////////////////////////////////////////////
            // SEND FILE SIZE
            //////////////////////////////////////////////////

            byte[] sizeBytes = BitConverter.GetBytes(size);

            stream.Write(sizeBytes, 0, 8);

            //////////////////////////////////////////////////
            // SEND FILE DATA
            //////////////////////////////////////////////////

            stream.Write(data, 0, data.Length);

            label1.Text = "Status: File Sent";

            //////////////////////////////////////////////////
            // RECEIVE ZIP SIZE
            //////////////////////////////////////////////////

            byte[] zipSizeBytes = new byte[8];

            stream.Read(zipSizeBytes, 0, 8);

            long zipSize =
                BitConverter.ToInt64(zipSizeBytes, 0);

            //////////////////////////////////////////////////
            // RECEIVE ZIP DATA
            //////////////////////////////////////////////////

            byte[] zipData = new byte[zipSize];

            int i = 0;

            while (i < zipSize)
            {
                int r =
                    stream.Read(zipData, i, (int)(zipSize - i));

                i += r;
            }

            //////////////////////////////////////////////////
            // SAVE ZIP
            //////////////////////////////////////////////////

            string zipPath =
                Application.StartupPath + "\\received.zip";

            File.WriteAllBytes(zipPath, zipData);

            //////////////////////////////////////////////////
            // SHOW MESSAGE
            //////////////////////////////////////////////////

            label1.Text = "Status: ZIP Received";

            MessageBox.Show(
                "Compressed file saved at:\n\n" + zipPath);

            client.Close();
        }
    }
}