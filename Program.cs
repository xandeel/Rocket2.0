using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class PortScanner
{
    static void Main()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("██╗    ██╗██╗███████╗██╗    ██████");
        Console.WriteLine("██║    ██║██║██╔════╝██║    ██╔═██");
        Console.WriteLine("██║ █╗ ██║██║███████╗██║    ██████");
        Console.WriteLine("██║███╗██║██║╚════██║██║    ██╔═██");
        Console.WriteLine("╚███╔███╔╝██║███████║██████ ██║ ██");
        Console.WriteLine(" ╚══╝╚══╝ ╚═╝╚══════╝╚═════ ╚═╝ ╚═╝");
        Console.WriteLine(" ");
        Console.WriteLine("      Uso Educativo - Proyecto Instituto Inacap");
        Console.WriteLine("      Autor: Francisco (alias nvnmmm)");
        Console.WriteLine(" ");
        Console.ResetColor();

        Console.Write("Ingrese el dominio o la dirección IP: ");
        string host = Console.ReadLine();

        // Obtener la dirección IP
        string ipAddress = GetIpAddress(host);

        Console.WriteLine($"IP: {ipAddress}");

        Console.Write("Ingrese el rango de puertos a escanear (inicio): ");
        int startPort = int.Parse(Console.ReadLine());

        Console.Write("Ingrese el rango de puertos a escanear (fin): ");
        int endPort = int.Parse(Console.ReadLine());

        string logFileName = $"{host}_ports_{startPort}-{endPort}.log";
        bool foundOpenPort = false;

        Console.WriteLine($"Escaneando puertos {startPort} a {endPort} en {host}...");
        Console.WriteLine(" ");

        // Barra de progreso
        Console.CursorVisible = false;
        int barWidth = 40;

        for (int port = startPort; port <= endPort; port++)
        {
            int progress = (int)((double)(port - startPort) / (endPort - startPort) * barWidth);
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write($"Progreso: [{new string('=', progress)}{new string(' ', barWidth - progress)}] {100 * (port - startPort) / (endPort - startPort)}%");
            Thread.Sleep(50); // Simular el escaneo
        }

        Console.WriteLine(" ");
        Console.CursorVisible = true;

        using (StreamWriter writer = new StreamWriter(logFileName))
        {
            for (int port = startPort; port <= endPort; port++)
            {
                using (TcpClient client = new TcpClient())
                {
                    client.ReceiveTimeout = 100;
                    try
                    {
                        client.Connect(host, port);
                        writer.WriteLine($"Puerto {port} está abierto");
                        foundOpenPort = true;
                        Console.WriteLine($"Puerto {port} está abierto");
                    }
                    catch (SocketException)
                    {
                        // El puerto está cerrado
                    }
                }
            }
        }

        if (foundOpenPort)
        {
            Console.WriteLine(" ");
            Console.WriteLine($"Escaneo completo. Los resultados se han guardado en '{logFileName}'");
        }
        else
        {
            Console.WriteLine(" ");
            Console.WriteLine("Perdón, fracasé buscando :(");
            File.Delete(logFileName); // Eliminar el archivo de registro vacío
        }
    }

    static string GetIpAddress(string host)
    {
        try
        {
            IPHostEntry entry = Dns.GetHostEntry(host);
            IPAddress[] addresses = entry.AddressList;
            if (addresses.Length > 0)
            {
                return addresses[0].ToString();
            }
        }
        catch (Exception)
        {
            // Error al obtener la dirección IP
        }
        return "Desconocida";
    }
}
