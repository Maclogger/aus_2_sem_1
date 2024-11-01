using AUS_Semestralna_Praca_1.BackEnd.CoreGui;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AUS_Semestralna_Praca_1.FrontEnd.GuiUtils;

public partial class MyMessageBox : Window
{
    private readonly Answer _answer;
    private Button _okButton;

    public MyMessageBox(Answer answer)
    {
        _answer = answer;
        InitializeComponent();

        _okButton = this.FindControl<Button>("okButton")!; // Assuming you have a Button with x:Name="okButton"
        _okButton.Click += OkButton_Click!;

        UpdateWindowState();
    }

    public string Message
    {
        get => _answer.Message;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Close(true); // Close the window and indicate success (optional)
    }

    private void UpdateWindowState()
    {
        switch (_answer.State)
        {
            case AnswerState.Error:
                Background = new SolidColorBrush(Colors.Red);
                Title = "Chyba";
                break;
            case AnswerState.Info:
                Background = new SolidColorBrush(Colors.LightBlue);
                Title = "Informácia";
                break;
            case AnswerState.Ok:
                Background = new SolidColorBrush(Colors.LightGreen);
                Title = "OK";
                break;
            default:
                Background = new SolidColorBrush(Colors.White);
                Title = "MyMessageBox";
                break;
        }

        messageTextBlock.Text = _answer.Message;
    }
}