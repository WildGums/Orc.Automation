#nullable enable
namespace Orc.Automation.ScenarioManagement.Tests;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using Catel;
using NUnit.Framework;
using Orc.Automation;

public class UserDocumentationBuilderLogger : IAutomationScenarioLogger
{
#pragma warning disable IDISP006 // Implement IDisposable
    private readonly string _documentationRootDirectory;

    private Bitmap? _bitmap;
#pragma warning restore IDISP006 // Implement IDisposable

    public UserDocumentationBuilderLogger()
    {
        _documentationRootDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "GeneratedDocumentation");
        Directory.CreateDirectory(_documentationRootDirectory);
    }

    public void LogScenarioStart(AutomationScenario scenario)
    {
        var scenarioDirectory = GetScenarioDirectory(scenario, false);
        if (Directory.Exists(scenarioDirectory))
        {
            Directory.Delete(scenarioDirectory, true);
        }
    }

    public void LogStepStart(AutomationScenarioStep step)
    {
#pragma warning disable IDISP003 // Dispose previous before re-assigning
        _bitmap = ScreenShot(step.InteractionArea);
#pragma warning restore IDISP003 // Dispose previous before re-assigning
    }

    public void LogStepFinish(AutomationScenarioStep step)
    {
        Wait.WhileProcessBusy(400);
        
        var currentScenario = ScenarioManager.CurrentScenario;
        if (currentScenario is null)
        {
            return;
        }

        var currentStepIndex = currentScenario.FinishedSteps.Count;

        var interactionArea = step.InteractionArea;
        if (_bitmap is not null)
        {
            using (new DisposableToken<UserDocumentationBuilderLogger>(this,
                       _ => {},
                       _ =>
                       {
                           _bitmap.Dispose();
                           _bitmap = null;
                       }))
            {
                for (var index = 0; index < step.Interactions.Count; index++)
                {
                    var userInteraction = step.Interactions[index];
                    var area = userInteraction.InteractionArea;
                    var actualRect = new Rectangle(new System.Drawing.Point((int)(area.X - interactionArea.X), (int)(area.Y - interactionArea.Y)),
                        new System.Drawing.Size((int)area.Width, (int)area.Height));

                    using var graphics = Graphics.FromImage(_bitmap);
#pragma warning disable IDISP004 // Don't ignore created IDisposable
                    graphics.DrawRectangle(new Pen(Color.Red, 3f), actualRect);
                    using var drawFont = new Font("Arial", 14);
                    using var drawBrush = new SolidBrush(Color.Red);

                    var userInteractionTitle = $"{index + 1}.{userInteraction.Name}";
                    var textSize = graphics.MeasureString(userInteractionTitle, drawFont);
                    graphics.DrawString(userInteractionTitle, drawFont, drawBrush, new System.Drawing.Point(actualRect.X - (int)textSize.Width + actualRect.Width, actualRect.Y - (int)textSize.Height));
#pragma warning restore IDISP004 // Don't ignore created IDisposable
                }

                var beforeImagePath = Path.Combine(GetScenarioDirectory(currentScenario), $"before_{currentStepIndex}.jpg");
                _bitmap.Save(beforeImagePath, ImageFormat.Jpeg);
            }
        }

        using var bitmap = ScreenShot(interactionArea);
        var afterImagePath = Path.Combine(GetScenarioDirectory(currentScenario), $"after_{currentStepIndex}.jpg");
        bitmap?.Save(afterImagePath, ImageFormat.Jpeg);
    }

    public void LogBeforeUserInteraction(UserInteraction userInteraction)
    {
    }

    public void LogAfterUserInteraction(UserInteraction userInteraction)
    {
    }

    public void LogScenarioFinish(AutomationScenario scenario)
    {
        var htmlStringBuilder = new StringBuilder();

        htmlStringBuilder.AppendLine("<!DOCTYPE html><html style=\"font-family:verdana\"><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        htmlStringBuilder.AppendLine("<style>");
        htmlStringBuilder.AppendLine("* {box-sizing: border-box;}");
        htmlStringBuilder.AppendLine(".column {  float: left;  width: 33.33%;  padding: 10px;}");
        htmlStringBuilder.AppendLine(".column1 {  float: left;  width: 20%;  padding: 10px;}");
        htmlStringBuilder.AppendLine(".row:after {  content: \"\";  display: table;  clear: both;}");
        htmlStringBuilder.AppendLine("</style>");
        htmlStringBuilder.AppendLine("</head><body>");
        htmlStringBuilder.AppendLine($"<h1>{scenario.Name}</h1>");
        htmlStringBuilder.AppendLine($"<h2>{scenario.Description}</h2>");
        htmlStringBuilder.AppendLine($"<div class=\"row\"><div class=\"column1\" style=\"background-color:#ccc;\"><h2>Step</h2></div><div class=\"column\" style=\"background-color:#ccc;\"><h2>Before</h2></div><div class=\"column\" style=\"background-color:#ccc;\"><h2>After</h2></div></div>");

        for (var i = 0; i < scenario.StartedSteps.Count; i++)
        {
            var step = scenario.StartedSteps[i];
            htmlStringBuilder.AppendLine($"<div class=\"row\"><div class=\"column1\"><h4>{step.Name}</h4></div><div class=\"column\"><img src=\"before_{i}.jpg\" style=\"object-fit:cover;width:100%;height:100%;border: solid 1px #CCC\"/></div><div class=\"column\"><img src=\"after_{i}.jpg\" style=\"object-fit:cover;width:100%;height:100%;border: solid 1px #CCC\"/></div></div>\r\n");
        }

        htmlStringBuilder.AppendLine("</body></html>");

        var indexHtmlPath = Path.Combine(GetScenarioDirectory(scenario), "Index.html");
        File.WriteAllText(indexHtmlPath, htmlStringBuilder.ToString());
    }

    private Bitmap? ScreenShot(Rect bounds)
    {
        try
        {
            Wait.UntilResponsive(200);

            var captureBitmap = new Bitmap((int)bounds.Width, (int)bounds.Height, PixelFormat.Format32bppArgb);
            var captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen((int)bounds.X, (int)bounds.Y, 0, 0, new System.Drawing.Size((int)bounds.Width, (int)bounds.Height));

            captureGraphics.Dispose();

            return captureBitmap;
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine(ex.ToString());
        }

        return null;
    }

    private string GetScenarioDirectory(AutomationScenario scenario, bool checkExistence = true)
    {
        var scenarioGroupDirectory = string.IsNullOrWhiteSpace(scenario.Suite)
            ? _documentationRootDirectory
            : Path.Combine(_documentationRootDirectory, scenario.Suite);

        Directory.CreateDirectory(scenarioGroupDirectory);

        var scenarioDirectory = Path.Combine(scenarioGroupDirectory, scenario.Name);
        if (checkExistence && !Directory.Exists(scenarioDirectory))
        {
            Directory.CreateDirectory(scenarioDirectory);
        }

        return scenarioDirectory;
    }
}
