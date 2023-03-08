using System.Net.Sockets;
using System.Net;
using System.Text;
using CoreLib;

// Створення UDP сокету
using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

var random = new Random();
// Генерація масиву для сортування
var arrayToSort = Enumerable.Range(0, 50).Select(_ => random.Next(0, 100));

Console.WriteLine($"Array to sort: {string.Join(';', arrayToSort)}");

byte[] pack = Encoding.UTF8.GetBytes(string.Join(';', arrayToSort));
// Створення ендпоінту, що містить в собі адресу для віддаленого сокету
EndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
// Пересилання масиву для сортування на віддалений ендпоінт
int bytes = await udpSocket.SendToAsync(pack, SocketFlags.None, remotePoint);
Console.WriteLine($"Sent {bytes} bytes");


while (true)
{
    var data = new byte[65535];
    // Отримання результату
    var result = await udpSocket.ReceiveFromAsync(data, SocketFlags.None, remotePoint);

    if(result.ReceivedBytes > 0)
    {
        // Отримання строкового представлення результату
        var recievedStr = Encoding.UTF8.GetString(data.Take(result.ReceivedBytes).ToArray());
        Console.WriteLine("Result: " + recievedStr);
        break;
    }
}
Console.WriteLine("Transmission has ended");
Console.ReadKey();
