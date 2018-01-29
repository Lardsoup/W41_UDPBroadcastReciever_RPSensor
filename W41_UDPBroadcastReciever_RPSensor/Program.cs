using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using W41_UDPBroadcastReciever_RPSensor.RPSensorService;

namespace W41_UDPBroadcastReciever_RPSensor
{
    class Program
    {
        private const int Port = 6968;

        static void Main(string[] args)
        {
            using (UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, Port)))
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(0, 0);
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Waiting for broadcast {0}", socket.Client.LocalEndPoint);
                        byte[] datagramReceived = socket.Receive(ref remoteEndPoint);

                        string message = Encoding.ASCII.GetString(datagramReceived, 0, datagramReceived.Length);

                        DoStuff(message);

                        //Console.WriteLine("Receives {0} bytes from {1} port {2} message {3}", datagramReceived.Length,
                        //    remoteEndPoint.Address, remoteEndPoint.Port, message);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public static void DoStuff(string message)
        {
            string Pot, Temp, Light, Analog;
            int SPot = 0, STemp = 0, SLight = 0, SAnalog = 0;
            DateTime DT = DateTime.Today;

            //Den data jeg modtager fra PI er ikke nødvendigvis det rigtige (Pot er måske Temp)
            string[] data = message.Split('\n');
            foreach (var d in data)
            {
                DT = DateTime.Parse(data[0]);
                string[] DataCollection = d.Split(' ');
                if (DataCollection[0].ToUpper().Contains("POT"))
                {
                    Pot = DataCollection[1];
                    SPot = int.Parse(Pot);
                }
                else if (DataCollection[0].ToUpper().Contains("TEMP"))
                {
                    Temp = DataCollection[1];
                    STemp = int.Parse(Temp);
                }
                else if (DataCollection[0].ToUpper().Contains("LIGHT"))
                {
                    Light = DataCollection[1];
                    SLight = int.Parse(Light);
                }
                else if (DataCollection[0].ToUpper().Contains("ANALOG"))
                {
                    Analog = DataCollection[1];
                    SAnalog = int.Parse(Analog);
                }
            }
            
            using (Service1Client RPSensorClient = new Service1Client("BasicHttpBinding_IService1"))
            {
                RPSensorClient.AddSensorData(SLight, STemp, SPot, SAnalog, DT);
            }
        }
    }
}
