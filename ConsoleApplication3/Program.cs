using System;
using System.IO.Ports;
using System.Text;

namespace ConsoleApplication3
{
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">Not used.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("ATChat");
            Console.WriteLine("Przemyslaw Fortuna:210176 Krzysztof Barden: 210139");
            Console.WriteLine("Avaiable ports:");
            foreach(var x in SerialPort.GetPortNames()) //pobranie wszystkich dostępnych portów
            {
                Console.WriteLine(x);
            }
            Console.Write("Provide port: ");
            var input = Console.ReadLine();
            Console.Write("Provide baud rate: ");
            int.TryParse(Console.ReadLine(), out int baudrate);

            foreach (var x in SerialPort.GetPortNames()) //przeszukanie czy wprowadzony port znajduje sie wsrod dostepnych portow
            {
                if (x == input && baudrate > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"Connected to {input}.");
                    PortLoop(input, baudrate); //uruchomienie metody z parametrem wybranego portu
                }
            }
            Console.WriteLine("Didnt find given port/bad baud rate value");
            Console.ReadLine();
           
        }
        /// <summary>
        /// Metoda odpowiedzialna za zczytywanie wiadomości użytkownika i wysyłanie ich do portu
        /// </summary>
        /// <param name="com">Port com.</param>
        public static void PortLoop(string com, int baudrate)
        {
            SerialPort _serialPort = new SerialPort(com, 9600); //utworzenie portu (comport, baudrate)
            _serialPort.DtrEnable = true; //Data Terminal Ready
            _serialPort.RtsEnable = true; //Request to send
            _serialPort.DataBits = 8; //rozmiar elementu
            _serialPort.Handshake = Handshake.None;
            _serialPort.Parity = Parity.None;
            _serialPort.StopBits = StopBits.One; //bit stopu = 1
            _serialPort.NewLine = System.Environment.NewLine;
            _serialPort.DataReceived += (s, e) => { //Event odebrania danych
                SerialPort sp = (SerialPort)s;
                byte[] packOfBytes = new byte[sp.BytesToRead]; //odczytana wiadomosc do tablicy bajtow
                Console.Write(Encoding.ASCII.GetString(packOfBytes, 0, sp.Read(packOfBytes, 0, packOfBytes.Length))); //konwersja bajtow na ascii
            };
            _serialPort.Open(); //otwarcie portu

            while (true) //pętla zczytujaca dane od uzytkownika
            {
                var msg = Console.ReadLine();
                if (msg == "quit")
                {
                    _serialPort.Close(); //jesli zostanie wpisany quit to zamknij port...
                    System.Environment.Exit(1); // ...i program
                }
                _serialPort.WriteLine(msg); //wyslij wiadomosc do portu
            }
        }
    }
}