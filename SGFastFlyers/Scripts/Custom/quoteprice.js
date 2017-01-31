var QuotePrice = {

    NonMetroAddition: 200,
    BaseCostPer1000: 55,
    Quantity: 0,
    Cost: 0,
    DLSingleSided: 0,
    DLDoubleSided: 0,
    A5SingleSided: 0,
    A5DoubleSided: 0,
    IsMetro: false,
    NeedsPrint: false,
    IsDoubleSided: false,
    PrintSize: "",
    OneThousand: 1000,

    SetQuotePrice: function (isHomePage) {

        $("#quotePrice").empty();
        if (isHomePage) {
            $("#HomePageQuoteViewModel_Cost").empty();
            QuotePrice.Quantity = $("#HomePageQuoteViewModel_Quantity").val() / QuotePrice.OneThousand;
            QuotePrice.IsMetro = $("#HomePageQuoteViewModel_IsMetro").is(':checked');
            QuotePrice.NeedsPrint = $("#HomePageQuoteViewModel_NeedsPrint").is(':checked');
            QuotePrice.IsDoubleSided = $("#HomePageQuoteViewModel_IsDoubleSided").is(':checked');
            QuotePrice.PrintSize = $("#HomePageQuoteViewModel_PrintSize").val();
        } else {
            $("#Cost").empty();
            QuotePrice.Quantity = $("#Quantity").val() / QuotePrice.OneThousand;
            QuotePrice.IsMetro = $("#IsMetro").is(':checked');
            QuotePrice.NeedsPrint = $("#NeedsPrint").is(':checked');
            QuotePrice.IsDoubleSided = $("#IsDoubleSided").is(':checked');
            QuotePrice.PrintSize = $("#PrintSize").val();
        }

        if (QuotePrice.Quantity >= 1 && QuotePrice.Quantity < 20) {
            QuotePrice.Cost = 55;
            QuotePrice.DLSingleSided = 40;
            QuotePrice.DLDoubleSided = 43;
            QuotePrice.A5SingleSided = 52;
            QuotePrice.A5DoubleSided = 55.9;
        }
        if (QuotePrice.Quantity >= 20 && QuotePrice.Quantity < 50) {
            QuotePrice.Cost = 54;
            QuotePrice.DLSingleSided = 38;
            QuotePrice.DLDoubleSided = 41;
            QuotePrice.A5SingleSided = 49.4;
            QuotePrice.A5DoubleSided = 53.3;
        }
        if (QuotePrice.Quantity >= 50 && QuotePrice.Quantity < 75) {
            QuotePrice.Cost = 53;
            QuotePrice.DLSingleSided = 36;
            QuotePrice.DLDoubleSided = 41;
            QuotePrice.A5SingleSided = 46.8;
            QuotePrice.A5DoubleSided = 53.3;
        }
        if (QuotePrice.Quantity >= 75 && QuotePrice.Quantity <= 100) {
            QuotePrice.Cost = 52;
            QuotePrice.DLSingleSided = 36;
            QuotePrice.DLDoubleSided = 41;
            QuotePrice.A5SingleSided = 46.8;
            QuotePrice.A5DoubleSided = 53.3;
        }
        if (QuotePrice.Quantity >= 100 && QuotePrice.Quantity <= 200) {
            QuotePrice.Cost = 51;
            QuotePrice.DLSingleSided = 34;
            QuotePrice.DLDoubleSided = 38;
            QuotePrice.A5SingleSided = 44.2;
            QuotePrice.A5DoubleSided = 49.4;
        }
        if (QuotePrice.Quantity >= 200 && QuotePrice.Quantity < 300) {
            QuotePrice.Cost = 50;
            QuotePrice.DLSingleSided = 34;
            QuotePrice.DLDoubleSided = 38;
            QuotePrice.A5SingleSided = 44.2;
            QuotePrice.A5DoubleSided = 49.4;
        }
        if (QuotePrice.Quantity >= 300) {
            QuotePrice.Cost = 48;
            QuotePrice.DLSingleSided = 32;
            QuotePrice.DLDoubleSided = 36;
            QuotePrice.A5SingleSided = 41.6;
            QuotePrice.A5DoubleSided = 46.8;
        }

        QuotePrice.Cost = QuotePrice.Cost * QuotePrice.Quantity;
        if (QuotePrice.IsMetro == false) {
            QuotePrice.Cost = QuotePrice.Cost + QuotePrice.NonMetroAddition;
        }

        if (QuotePrice.NeedsPrint) {
            if (QuotePrice.IsDoubleSided) {
                if (QuotePrice.PrintSize == "1") {
                    QuotePrice.Cost = QuotePrice.Cost + QuotePrice.DLDoubleSided * QuotePrice.Quantity;
                }

                if (QuotePrice.PrintSize == "5") {
                    QuotePrice.Cost = QuotePrice.Cost + QuotePrice.A5DoubleSided * QuotePrice.Quantity;
                }
            }
            else {
                if (QuotePrice.PrintSize == "1") {
                    QuotePrice.Cost = QuotePrice.Cost + QuotePrice.DLSingleSided * QuotePrice.Quantity;
                }

                if (QuotePrice.PrintSize == "5") {
                    QuotePrice.Cost = QuotePrice.Cost + QuotePrice.A5SingleSided * QuotePrice.Quantity;
                }
            }
        }
        if (QuotePrice.Cost < 364) {
            QuotePrice.Cost = 400;
        }

        //GST
        QuotePrice.Cost = (QuotePrice.Cost * 0.1) + QuotePrice.Cost;
        QuotePrice.Cost = QuotePrice.Cost.toFixed(2).toLocaleString();
        $("#quotePrice").html("$" + QuotePrice.Cost);

        if (isHomePage) {
            $("#HomePageQuoteViewModel_Cost").val(QuotePrice.Cost);
        } else {
            $("#Cost").val(QuotePrice.Cost);
        }

        return QuotePrice.Cost;
    }
}