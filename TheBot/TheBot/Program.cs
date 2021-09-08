using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TheBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("1952790138:AAGTO8escIrN2im50ydJCF6dDVZ29PzSQTg");

            var me = await botClient.GetMeAsync();
            Console.WriteLine(
                $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            botClient.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token);
            
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

// Send cancellation request to stop bot
            cts.Cancel();

            // CreateHostBuilder(args).Build().Run();
        }
        
        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _                                       => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.EditedMessage)
            {
                var id = update.EditedMessage.Chat.Id;
                await botClient.SendTextMessageAsync(
                    chatId: id,
                    text:   "Meow"
                );
                Console.WriteLine($"Pedar sag edit kar id:{id}");
                return;
            }
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
    
            Console.WriteLine($"Received a '{update.Message.Text}' message in chat {chatId}.");

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text:   "You said:\n" + update.Message.Text
            );
        }

        
        
        
        
        
        
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}