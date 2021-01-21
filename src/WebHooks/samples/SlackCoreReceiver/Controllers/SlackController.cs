using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebHooks;
using Microsoft.Extensions.Logging;

namespace SlackCoreReceiver.Controllers
{
    public class SlackController : ControllerBase
    {
        private readonly ILogger _logger;

        public SlackController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SlackController>();
        }

        [SlackWebHook(Id = "command")]
        public IActionResult SlackForCommand(string @event, string subtext, IFormCollection data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation(
                0,
                $"{nameof(SlackController)} / 'command' received {{Count}} properties with event '{{EventName}}'.",
                data.Count,
                @event);

            string channel = data[SlackConstants.ChannelRequestFieldName];
            string command = data[SlackConstants.CommandRequestFieldName];
            string trigger = data[SlackConstants.TriggerRequestFieldName];
            _logger.LogInformation(
                1,
                "Data contains channel '{ChannelName}', command '{Command}', and trigger '{Trigger}'.",
                channel,
                command,
                trigger);

            string text = data[SlackConstants.TextRequestFieldName];
            _logger.LogInformation(
                2,
                "Data contains text '{Text}' and subtext '{Subtext}'.",
                text,
                subtext);

            var slashCommand = SlackCommand.ParseActionWithValue(command);

            // Ignore an error parsing the remainder of the command except to keep that action value together.
            var actionValue = slashCommand.Value;
            var actionValueName = "value";
            var parameters = SlackCommand.TryParseParameters(slashCommand.Value, out var error);
            if (error == null)
            {
                actionValue = SlackCommand.GetNormalizedParameterString(parameters);
                actionValueName = "parameters";
            }

            // Create the response.
            var reply = $"Received slash command '{command}' with action '{slashCommand.Key}' and {actionValueName} " +
                $"'{actionValue}'.";

            // Slash responses can be augmented with attachments containing data, images, and more.
            var attachment = new SlackAttachment("Attachment Text", "Fallback description")
            {
                Color = "#439FE0",
                Pretext = "Hello from ASP.NET WebHooks!",
                Title = "Attachment title",
            };

            // Slash attachments can contain tabular data as well
            attachment.Fields.Add(new SlackField("Field1", "1234"));
            attachment.Fields.Add(new SlackField("Field2", "5678"));

            return new JsonResult(new SlackSlashResponse(reply, attachment));
        }

        [SlackWebHook(Id = "trigger")]
        public IActionResult SlackForTrigger(string @event, string subtext, IFormCollection data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation(
                3,
                $"{nameof(SlackController)} / 'trigger' received {{Count}} properties with event '{{EventName}}'.",
                data.Count,
                @event);

            var channel = data[SlackConstants.ChannelRequestFieldName];
            var command = data[SlackConstants.CommandRequestFieldName];
            var trigger = data[SlackConstants.TriggerRequestFieldName];
            _logger.LogInformation(
                4,
                "Data contains channel '{ChannelName}', command '{Command}', and trigger '{Trigger}'.",
                channel,
                command,
                trigger);

            var text = data[SlackConstants.TextRequestFieldName];
            _logger.LogInformation(
                5,
                "Data contains text '{Text}' and subtext '{Subtext}'.",
                text,
                subtext);

            // Create the response.
            var triggerCommand = SlackCommand.ParseActionWithValue(subtext);

            // Information can be returned using a SlackResponse.
            var reply = string.Format(
                "Received trigger '{0}' with action '{1}' and value '{2}'",
                trigger,
                triggerCommand.Key,
                triggerCommand.Value);

            return new JsonResult(new SlackResponse(reply));
        }

        [SlackWebHook]
        public IActionResult Slack(string id, string @event, string subtext, IFormCollection data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation(
                6,
                $"{nameof(SlackController)} / '{{ReceiverId}}' received {{Count}} properties with event " +
                "'{EventName}'.",
                id,
                data.Count,
                @event);

            var channel = data[SlackConstants.ChannelRequestFieldName];
            var command = data[SlackConstants.CommandRequestFieldName];
            var trigger = data[SlackConstants.TriggerRequestFieldName];
            _logger.LogInformation(
                7,
                "Data contains channel '{ChannelName}', command '{Command}', and trigger '{Trigger}'.",
                channel,
                command,
                trigger);

            var text = data[SlackConstants.TextRequestFieldName];
            _logger.LogInformation(
                8,
                "Data contains text '{Text}' and subtext '{Subtext}'.",
                text,
                subtext);

            return Ok();
        }
    }
}
