using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomTarotCard;

public static partial class CustomTarotCardManager
{
    public static Dictionary<TarotCards.Card, CustomTarotCard> CustomTarotCardList { get; } = [];

    public static TarotCards.Card Add(CustomTarotCard card)
    {
        var guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        var cardType = GuidManager.GetEnumValue<TarotCards.Card>(guid, card.InternalName);
        card.CardType = cardType;
        card.ModPrefix = guid;

        CustomTarotCardList.Add(cardType, card);
        if (!DataManager.AllTrinkets.Contains(cardType))
            DataManager.AllTrinkets.Add(cardType);

        return cardType;
    }
}