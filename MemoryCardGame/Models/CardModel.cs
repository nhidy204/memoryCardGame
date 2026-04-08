namespace MemoryCardGame.Models;

public class CardModel
{
    public int Id { get; set; }
    public string Value { get; set; } = "";
    public bool IsFlipped { get; set; }
    public bool IsMatched { get; set; }
}