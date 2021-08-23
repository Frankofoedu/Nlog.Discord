using System;
using NLog.Discord.Models;

namespace NLog.Discord
{
    public class DiscordMessageBuilder
    {
        private readonly string _webHookUrl;

        private readonly DiscordClient _client;

        private readonly Payload _payload;

        public DiscordMessageBuilder(string webHookUrl)
        {
            this._webHookUrl = webHookUrl;
            this._client = new DiscordClient();
            this._payload = new Payload();
        }

        public static DiscordMessageBuilder Build(string webHookUrl)
        {
            return new DiscordMessageBuilder(webHookUrl);
        }

        public DiscordMessageBuilder WithMessage(string message)
        {
            this._payload.Content = message;

            return this;
        }


        public DiscordMessageBuilder OnError(Action<Exception> error)
        {
            this._client.Error += error;

            return this;
        }

        public void Send()
        {
            this._client.Send(this._webHookUrl, this._payload.ToJson());
        }
    }
}