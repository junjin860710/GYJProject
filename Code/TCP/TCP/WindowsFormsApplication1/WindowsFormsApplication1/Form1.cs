using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private UdpClient udpcRecv;

        private Thread thrRecv;

        private void Form1_Load(object sender, EventArgs e)
        {
            //创建接收线程
            //IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 8080); // 本机IP和监听端口号  
            //udpcRecv = new UdpClient(localIpep);
            //thrRecv = new Thread(ReceiveMessage);
            //thrRecv.Start();

            
        }

        private void ReceiveMessage(object obj)
        {
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    string message = Encoding.UTF8.GetString(bytRecv, 0, bytRecv.Length);
                    ShowMessage(txtRecvMssg, string.Format("{0}[{1}]", remoteIpep, message));
                    
                }
                catch (Exception ex)
                {
                    ShowMessage(txtRecvMssg, ex.Message);
                    break;
                }
            }
        }

        delegate void ShowMessageDelegate(TextBox txtbox, string message);
        private void ShowMessage(TextBox txtbox, string message)
        {
            if (txtbox.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = ShowMessage;
                txtbox.Invoke(showMessageDelegate, new object[] { txtbox, message });
            }
            else
            {
                txtbox.Text += message + "\r\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string xmlStr="<?xml version=\"1.0\" encoding=\"UTF-8\"?><labels ver=\"1.0\"  map=\"地图\"><label id=\"1926582\"><x>1000</x><y>1500</y><attr>02H</attr></label></labels>";
            var doc = new XmlDocument();
            doc.LoadXml(xmlStr);

            //get Label info
            var rowNoteList = doc.SelectNodes("/labels/label");
            if (rowNoteList != null)
            {
                foreach (XmlNode rowNode in rowNoteList)
                {
                    string labelid = rowNode.Attributes["id"].Value;
                    var fieldNodeList = rowNode.ChildNodes;
                    decimal xpos = Convert.ToDecimal(fieldNodeList[0].InnerText);
                    decimal ypos = Convert.ToDecimal(fieldNodeList[1].InnerText);
                    string strstatus = fieldNodeList[2].InnerText;

                  
                }
            }
        }



    }
}
