NLog.Discord
==========

An NLog target for Discord 

Installation
============
Via [NuGet](https://www.nuget.org/packages/Nlog.Discord/): ```Install-Package NLog.Discord```

... or just build it your self!

Usage
=====
1. Create a [new Discord Webhook integration](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks) in the correct channel.
2. Generate a new Webhook URL and Authorize it to post to a channel.
3. Copy your Webhook URL and configure NLog via your NLog.config file or programmatically, as below.

### NLog.config

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

  <extensions>
    <add assembly="NLog.Discord" />
  </extensions>

  <targets async="true">
    <target xsi:type="Discord"
            name="discordTarget"
            layout="${message}"
            webHookUrl="https://hooks.discord.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX"
            compact="false">

			<field name="Machine Name" layout="${machinename}" />
			<field name="Process Name" layout="${processname}" />
			<field name="Process PID" layout="${processid}" />
	</target>
  </targets>

  <rules>
    <logger name="*" minlevel="Debug" writeTo="discordTarget" />
  </rules>
</nlog>
```

Note: it's recommended to set ```async="true"``` on `targets` so if the HTTP call to Discord fails or times out it doesn't slow down your application.

### Programmatically 

```c#
var config = new LoggingConfiguration();
var discordTarget = new DiscordTarget
{
      Layout = "${message}",
      WebHookUrl = "https://hooks.discord.com/services/T00000000/B00000000/XXXXXXXXXXXXXXXXXXXXXXXX",
};

config.AddTarget("discord", discordTarget);

var discordTargetRules = new LoggingRule("*", LogLevel.Debug, discordTarget);
config.LoggingRules.Add(discordTargetRules);

LogManager.Configuration = config;
```

And you're good to go!

### Configuration Options

Key        | Description
----------:| -----------
WebHookUrl | Grab your Webhook URL (__with the token__) from your Incoming Webhooks integration in Discord
Compact    | Set to true to just send the NLog layout text (no process info, colors, etc)
