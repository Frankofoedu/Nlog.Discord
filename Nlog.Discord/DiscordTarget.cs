using System;
using System.Collections.Generic;
using NLog.Common;
using NLog.Config;
using NLog.Discord.Models;
using NLog.Targets;

namespace NLog.Discord
{
    [Target("Discord")]
    public class DiscordTarget : TargetWithContext
    {
        [RequiredParameter]
        public string WebHookUrl { get; set; }

        public bool Compact { get; set; }

        public override IList<TargetPropertyWithContext> ContextProperties { get; } = new List<TargetPropertyWithContext>();

        [ArrayParameter(typeof(TargetPropertyWithContext), "field")]
        public IList<TargetPropertyWithContext> Fields => ContextProperties;

        //protected override void InitializeTarget()
        //{
        //    if (String.IsNullOrWhiteSpace(this.WebHookUrl))
        //        throw new ArgumentOutOfRangeException("WebHookUrl", "Webhook URL cannot be empty.");

        //    Uri uriResult;
        //    if (!Uri.TryCreate(this.WebHookUrl, UriKind.Absolute, out uriResult))
        //        throw new ArgumentOutOfRangeException("WebHookUrl", "Webhook URL is an invalid URL.");

        //    if (!this.Compact && this.ContextProperties.Count == 0)
        //    {
        //        this.ContextProperties.Add(new TargetPropertyWithContext("Process Name", Layout = "${machinename}\\${processname}"));
        //        this.ContextProperties.Add(new TargetPropertyWithContext("Process PID", Layout = "${processid}"));
        //    }

        //    base.InitializeTarget();
        //}

        protected override void Write(AsyncLogEventInfo info)
        {
            try
            {
                this.SendToDiscord(info);
                info.Continuation(null);
            }
            catch (Exception e)
            {
                info.Continuation(e);
            }
        }

        private void SendToDiscord(AsyncLogEventInfo info)
        {
            var message = RenderLogEvent(Layout, info.LogEvent);

            var discord = DiscordMessageBuilder
                .Build(this.WebHookUrl)
                .OnError(e => info.Continuation(e))
                .WithMessage(message);

            if (this.ShouldIncludeProperties(info.LogEvent) || this.ContextProperties.Count > 0)
            {
                var color = this.GetDiscordColorFromLogLevel(info.LogEvent.Level);
                var allProperties = this.GetAllProperties(info.LogEvent);
                foreach (var property in allProperties)
                {
                    if (string.IsNullOrEmpty(property.Key))
                        continue;

                    var propertyValue = property.Value?.ToString();
                    if (string.IsNullOrEmpty(propertyValue))
                        continue;

                }
            }

            discord.Send();
        }

        private string GetDiscordColorFromLogLevel(LogLevel level)
        {
            if (LogLevelDiscordColorMap.TryGetValue(level, out var color))
                return color;
            else
                return "#cccccc";
        }

        private static readonly Dictionary<LogLevel, string> LogLevelDiscordColorMap = new Dictionary<LogLevel, string>()
        {
            { LogLevel.Warn, "warning" },
            { LogLevel.Error, "danger" },
            { LogLevel.Fatal, "danger" },
            { LogLevel.Info, "#2a80b9" },
        };
    }
}