using System.Runtime.InteropServices.JavaScript;

namespace My.CoreGui;

public class Answer
{
    private string _message;
    private AnswerState _state;

    public Answer(string message, AnswerState state)
    {
        _message = message;
        _state = state;
    }

    public string Message
    {
        get => _message;
    }

    public AnswerState State
    {
        get => _state;
    }
}


public enum AnswerState
{
    Ok,
    Info,
    Error,
}