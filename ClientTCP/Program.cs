//NET.Socket Client
using System.Net.Sockets;
using System.Net;
using System.Text;
using CoreLib;

// Створення ендпоінту, що містить в собі адресу для сокету
IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
// Створення TCP сокету
Socket listeningSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
// Біндимо TCP сокет під ендпоінт з адресую, що був створеницй
listeningSock.Bind(ipEndPoint);
Console.WriteLine("Ожидание входящего TCP подключения...");
// Починаємо прослуховування, очікуючи підключень
listeningSock.Listen(10);

while(true)
{
    // Отримання клієнського запиту
    Socket serverSock = listeningSock.Accept();
    IPEndPoint remote = (IPEndPoint)serverSock.RemoteEndPoint;
    Console.WriteLine("Соединение установлено по адресу {0}. Входящее сообщение: ", remote.Address.ToString());

    // Отримання інформації, що була надіслана
    byte[] bufRecieve = serverSock.ReceiveAll();
    while (bufRecieve.Length == 0)
        bufRecieve = serverSock.ReceiveAll();

    // Отримання строкової репрезентації
    var recievedStr = Encoding.UTF8.GetString(bufRecieve);
    Console.WriteLine(recievedStr);

    // Сортування масиву
    var array = recievedStr.Split(';').Select(x => int.Parse(x)).OrderBy(x => x);
    var sortedArray = string.Join(';', array);
    Console.WriteLine($"Sorted array: {sortedArray}");
    // Відправка результату
    serverSock.Send(Encoding.UTF8.GetBytes(sortedArray));
    serverSock.Close(); 
    Console.WriteLine("Enter stop to stop listening");
    if(Console.ReadLine() == "stop") break;
}
// Закінчення прослуховування
listeningSock.Close();




