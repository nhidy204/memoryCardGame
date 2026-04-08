using MemoryCardGame.Models;

namespace MemoryCardGame.Services;

public class GameLogic
{
    // ── state (tương đương useState) ──────────────────────────
    public List<CardModel> Cards { get; private set; } = new();
    public int Score { get; private set; }
    public int Moves { get; private set; }
    public bool IsLocked { get; private set; }
    public bool IsGameComplete => Cards.Count > 0 && Cards.All(c => c.IsMatched);

    private List<int> _flippedCards = new();
    private readonly string[] _cardValues;

    // tương đương setState trigger re-render
    public event Action? OnStateChanged;

    public GameLogic(string[] cardValues)
    {
        _cardValues = cardValues;
        InitializeGame();
    }

    // ── initializeGame (tương đương useCallback + useEffect) ──
    public void InitializeGame()
    {
        // Fisher-Yates shuffle (giống shuffleArray trong JS)
        var shuffled = _cardValues.ToArray();
        var rng = new Random();
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        Cards = shuffled.Select((value, index) => new CardModel
        {
            Id = index,
            Value = value,
            IsFlipped = false,
            IsMatched = false
        }).ToList();

        Score = 0;
        Moves = 0;
        IsLocked = false;
        _flippedCards.Clear();
        OnStateChanged?.Invoke();
    }

    // ── handleCardClick ───────────────────────────────────────
    public async Task HandleCardClick(CardModel card)
    {
        if (card.IsFlipped || card.IsMatched || IsLocked || _flippedCards.Count == 2)
            return;

        card.IsFlipped = true;
        _flippedCards.Add(card.Id);
        OnStateChanged?.Invoke();

        if (_flippedCards.Count == 2)
        {
            IsLocked = true;
            var firstCard = Cards.First(c => c.Id == _flippedCards[0]);

            if (firstCard.Value == card.Value) // khớp
            {
                await Task.Delay(500); // tương đương setTimeout 500ms
                firstCard.IsMatched = true;
                card.IsMatched = true;
                Score++;
            }
            else // không khớp → lật lại
            {
                await Task.Delay(1000); // tương đương setTimeout 1000ms
                firstCard.IsFlipped = false;
                card.IsFlipped = false;
            }

            Moves++;
            _flippedCards.Clear();
            IsLocked = false;
            OnStateChanged?.Invoke();
        }
    }
}