using System;
using System.IO.Ports;
using System.Text;

namespace ConsoleApplication3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Przemyslaw Fortuna:210176 Krzysztof Barden:210139");
                Console.WriteLine("Avaiable ports:");
                foreach (var x in SerialPort.GetPortNames()) //pobranie dostępnych portow
                {
                    Console.WriteLine(x);
                }
                Console.Write("Provide port: ");
                String input = Console.ReadLine();
                Console.Write("Provide baud rate (Default 9600): ");
                int baudrate;
                String tmp = Console.ReadLine();
                if (!int.TryParse(tmp, out baudrate))
                    baudrate = 9600;


                foreach (var x in SerialPort.GetPortNames()) //przeszukanie portow
                {
                    if (x == input && baudrate > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"Connected to {input}.");
                        PortLoop(input, baudrate); //uruchomienie z wybranym portem
                    }
                }
                Console.WriteLine("Didnt find given port/bad baud rate value\nPress enter to try again or type 'quit' to exit program");
                if (Console.ReadLine().ToLower() == "quit")
                    break;
            }
           
        }

        public static void PortLoop(string com, int baudrate)
        {
            SerialPort _serialPort = new SerialPort(com, baudrate) //utworzenie portu (comport, baudrate)
            {
                DtrEnable = true, //Data Terminal Ready
                RtsEnable = true, //Request to send
                DataBits = 8, //rozmiar elementu
                Handshake = Handshake.None,
                Parity = Parity.None,
                StopBits = StopBits.One, //bit stopu = 1
                NewLine = System.Environment.NewLine
            }; 
            _serialPort.DataReceived += (s, e) => { //Event odebrania danych
                SerialPort sp = (SerialPort)s;
                byte[] packOfBytes = new byte[sp.BytesToRead]; //odczytana wiadomosc do tablicy bajtow
                Console.Write(Encoding.ASCII.GetString(packOfBytes, 0, sp.Read(packOfBytes, 0, packOfBytes.Length))); //konwersja bajtow na ascii
            };
            _serialPort.Open(); //otwarcie portu

            while (true) //pętla zczytujaca dane od uzytkownika
            {
                String msg = Console.ReadLine();
                if (msg.ToLower() == "quit")
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close(); //zamkniecie portu
                    System.Environment.Exit(0); // zamkniecie programu
                }
                _serialPort.WriteLine(msg); //wyslij wiadomosc do portu
            }
        }
    }
}