using System.Text.RegularExpressions;

public class Day07 : IDay
{
    private string[] lines;

    private const string CARD_TYPES = @"(2|3|4|5|6|7|8|9|T|J|Q|K|A)";

    private const string FIVE_OF_KIND = CARD_TYPES + @"\1{4}";
    private const string FOUR_OF_KIND = CARD_TYPES + @"\1{3}";
    private const string FULL_HOUSE = @"((" + CARD_TYPES + @"\3" + CARD_TYPES + @"\4{2})|(" + CARD_TYPES + @"\6{2}" + CARD_TYPES + @"\7))";
    private const string THREE_OF_KIND = CARD_TYPES + @"\1{2}";
    private const string TWO_PAIR = CARD_TYPES + @"\1" + "(.*)" + CARD_TYPES + @"\3";
    private const string ONE_PAIR = CARD_TYPES + @"\1";
    private const string HIGH_CARD = CARD_TYPES;

    
    List<string> pokerHands = new()
    {
        FIVE_OF_KIND,
        FOUR_OF_KIND,
        FULL_HOUSE,
        THREE_OF_KIND,
        TWO_PAIR,
        ONE_PAIR,
        HIGH_CARD
    };

    Dictionary<string, int> pokerHandValues = new()
    {
        {FIVE_OF_KIND, 7},
        {FOUR_OF_KIND, 6},
        {FULL_HOUSE, 5},
        {THREE_OF_KIND, 4},
        {TWO_PAIR, 3},
        {ONE_PAIR, 2},
        {HIGH_CARD, 1}
    };

    Dictionary<char, int> cardValues = new()
    {
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'T', 10},
        {'J', 11},
        {'Q', 12},
        {'K', 13},
        {'A', 14}
    };

    public Day07(string filePath)
    {
        lines = File.ReadAllLines(filePath);
    }

    public object SolveOne()
    {
        IEnumerable<(string Hand, int HandValue, int Bid)> hands = ParseHands(lines).ToList();

        return Sum(hands);
    }

    private object Sum(IEnumerable<(string Hand, int HandValue, int Bid)> hands)
    {
        hands = hands.OrderBy(hand => hand.HandValue)
                    .ThenBy(hand => cardValues[hand.Hand[0]])
                    .ThenBy(hand => cardValues[hand.Hand[1]])
                    .ThenBy(hand => cardValues[hand.Hand[2]])
                    .ThenBy(hand => cardValues[hand.Hand[3]])
                    .ThenBy(hand => cardValues[hand.Hand[4]]);

        // foreach ((string Hand, int HandValue, int Bid) hand in hands)
        //     Console.WriteLine($"{hand.Hand} {hand.HandValue} {hand.Bid}");

        return hands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
    }

    private IEnumerable<(string, int, int)> ParseHands(string[] lines)
    {
        foreach(string line in lines)
        {
            string[] split = line.Split(' ');

            string part1 = split[0];
            string part2 = split[1];

            int pokerHandValue = GetPokerHandValue(part1);

            yield return (part1, pokerHandValue, int.Parse(part2));
        }
    }

    public object SolveTwo()
    {
        cardValues['J'] = 1;

        IEnumerable<(string Hand, int HandValue, int Bid)> hands = ParseHandsTwo(lines).ToList();

        return Sum(hands);
    }

    private List<(string, int, int)> ParseHandsTwo(string[] lines)
    {
        List<(string, int, int)> hands = new();

        foreach(string line in lines)
        {
            string[] split = line.Split(' ');
            
            string part1 = split[0];
            string part2 = split[1];
            
            char strongestSuit = '2';
            int strongestHand = 0;

            string part1_replaced;

            if (part1.Contains('J'))
            {
                foreach (char cardValue in cardValues.Keys.Where(k => k != 'J'))
                {
                    part1_replaced = part1.Replace('J', cardValue);
                    
                    int pokerHandValue = GetPokerHandValue(part1_replaced);

                    if (pokerHandValue > strongestHand)
                    {
                        strongestHand = pokerHandValue;
                        strongestSuit = cardValue;
                    }
                }

                hands.Add((part1, strongestHand, int.Parse(part2)));
            }
            else
            {
                int pokerHandValue = GetPokerHandValue(part1);

                hands.Add((part1, pokerHandValue, int.Parse(part2)));
            }
        }

        return hands;
    }

    private int GetPokerHandValue(string part1)
    {
        string orderedHand = string.Concat(part1.OrderBy(x => cardValues[x]));

        return pokerHandValues[pokerHands.First(hand => Regex.IsMatch(orderedHand, hand))];
    }
}