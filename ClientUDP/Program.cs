// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;
using CoreLib;

// Створення UDP сокету
using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
// Створення ендпоінту, що містить в собі адресу для сокету
var localIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
// Біндимо UDP сокет під ендпоінт з адресую, що був створеницй
udpSocket.Bind(localIP);
Console.WriteLine("UDP port is listening");


while (true)
{
    var data = new byte[65535];
    // Отримання інформації з запиту користувача
    var result = await udpSocket.ReceiveFromAsync(data, SocketFlags.None, localIP);

    var recievedStr = Encoding.UTF8.GetString(data.Take(result.ReceivedBytes).ToArray());
    Console.WriteLine(recievedStr);

    // Сортування масиву
    var array = recievedStr.Split(';').Select(x => int.Parse(x)).OrderBy(x => x);
    var sortedArray = string.Join(';', array);
    Console.WriteLine($"Sorted array: {sortedArray}");
    // Відправка масиву на ендпоінт, що ініціював передачу
    int bytes = await udpSocket.SendToAsync(Encoding.UTF8.GetBytes(sortedArray), SocketFlags.None, result.RemoteEndPoint);
    Console.WriteLine($"Sent {bytes} bytes");
    Console.WriteLine("Enter stop to stop listening");
    if (Console.ReadLine() == "stop") break;
}
// Припинення отримання запитів
udpSocket.Close();
