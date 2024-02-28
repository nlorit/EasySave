using System.Net.Sockets;
using System.Net;
using System.Text;

public static class ServerService
{
    public static string? message;
    private static TcpListener listener = new TcpListener(IPAddress.Any, 52286);
    private static bool isRunning;


    public static void Start()
    {
        isRunning = true;
        Console.WriteLine("Serveur démarré. En attente de connexions...");

        listener.Start();

        while (isRunning)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Nouvelle connexion acceptée.");

                // Récupérer les données envoyées par le client
                byte[] buffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Message reçu du client : {receivedMessage}");
                message = receivedMessage;
                // Traiter la connexion ici

                // Envoyer une réponse au client
                byte[] response = Encoding.ASCII.GetBytes("Message reçu par le serveur.");
                stream.Write(response, 0, response.Length);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Erreur lors de la connexion du client : {ex.Message}");
            }
        }
    }

    public static void Stop()
    {
        isRunning = false;
        listener.Stop();
        Console.WriteLine("Serveur arrêté.");
    }
}
