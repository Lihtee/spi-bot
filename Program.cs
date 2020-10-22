using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace spi_bot
{
    class Program
    {
        static string key = @"1111735442:AAFtHC9ciiObyvprUtFTpK3uawQcLWkqWOU";

        static ITelegramBotClient botClient;

        static Random rnd = new Random();

        static void Main(string[] args)
        {
            if (args.Length == 1) {
                ClaimSocket(args[0]);
            }
            botClient = new TelegramBotClient(key);
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Serving the bot");
            while(true) {
                try {
                    Console.ReadKey();
                } catch (Exception) {
                    // Nothing.
                }
            }
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e) {
            string in_text = e?.Message?.Text;
            if (!string.IsNullOrWhiteSpace(in_text)) {
                string out_text = "Spi";
                string imgPath = "";
                if (in_text.ToLower() == "!penis") {
                    var penisNum = rnd.Next(2, 10);
                    var longPart = string.Join("", Enumerable.Repeat("=", penisNum));
                    out_text = $"*8{longPart}D*";
                    imgPath = "https://cdn.shopify.com/s/files/1/0552/7645/products/penis-plush-toy-various-sizes-and-colors-plush-toy-light-79in-20cm-geekyget-2_1024x1024.jpg?v=1601920888";
                } else {
                    var spiNum = Math.Abs(in_text.GetHashCode() % 6) + 1;
                    out_text = string.Join(" ", Enumerable.Repeat("Spi", spiNum));
                    imgPath = "https://cs6.pikabu.ru/post_img/2014/12/06/4/1417843044_1050875513.jpg";
                }
                Console.WriteLine($"{in_text}\n{out_text}");
                await botClient.SendPhotoAsync(
                    chatId: e.Message.Chat,
                    caption: out_text,
                    parseMode: ParseMode.Markdown,
                    photo: imgPath
                );

            }
        }

        static async void ClaimSocket(string portNum) {
            int port = int.Parse(portNum);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(ipPoint);
            listenSocket.Listen(10);
            await Task.Run(() => {
                var data = new byte[256];
                while (true) {
                    Socket handler = listenSocket.Accept();
                    do 
                    { 
                        handler.Receive(data); 
                    }
                    while (handler.Available>0);
                    handler.Send(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            });
        }
    }
}
