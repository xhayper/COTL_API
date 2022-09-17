using System.Collections.Generic;
using System.Reflection;
using COTL_API.Guid;

namespace COTL_API.CustomTarotCard;

public static partial class CustomTarotCardManager
{
    public static Dictionary<TarotCards.Card, CustomTarotCard> CustomTarotCards { get; } = new();

    public static TarotCards.Card Add(CustomTarotCard card)
    {
        string guid = TypeManager.GetModIdFromCallstack(Assembly.GetCallingAssembly());

        TarotCards.Card cardType = GuidManager.GetEnumValue<TarotCards.Card>(guid, card.InternalName);
        card.CardType = cardType;
        card.ModPrefix = guid;

        CustomTarotCards.Add(cardType, card);
        if (!DataManager.AllTrinkets.Contains(cardType))
            DataManager.AllTrinkets.Add(cardType);

        return cardType;
    }
}