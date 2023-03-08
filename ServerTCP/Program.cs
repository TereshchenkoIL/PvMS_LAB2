using System.Net.Sockets;
using System.Net;
using System.Text;
using CoreLib;

// Створення TCP сокету
Socket cliSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPAddress ipServ = IPAddress.Parse("127.0.0.1");
// Створення ендпоінту, що містить в собі адресу для сокету
IPEndPoint ipEndP = new IPEndPoint(ipServ, 8000);

try
{
    // Створення підключення до віддаленого сокету
    cliSock.Connect(ipEndP);
}
catch (SocketException ex)
{
    Console.WriteLine(ex);
}
var random = new Random();
// Генерація масиву для сортування
var arrayToSort = Enumerable.Range(0, 50).Select(_ => random.Next(0, 100));

Console.WriteLine($"Array to sort: {string.Join(';', arrayToSort)}");

byte[] pack = Encoding.UTF8.GetBytes(string.Join(';', arrayToSort));
// Відправка масиву для сортування
cliSock.Send(pack);

// Отримання відсортованого результату
var response = cliSock.ReceiveAll();
while(response.Length == 0 )
    response = cliSock.ReceiveAll();

Console.WriteLine($"Response : {Encoding.UTF8.GetString(response)}");
Console.WriteLine("Передача завершена. Для продолжения нажмите любую клавишу...");
Console.ReadKey();
cliSock.Close();
