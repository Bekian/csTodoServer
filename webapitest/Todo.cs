namespace webapitest;

public class Todo
{
    public required int ID { get; set; }

    public required string Description { get; set; }

    public required DateTime Date { get; set; }

    public required bool Completed { get; set; }
}