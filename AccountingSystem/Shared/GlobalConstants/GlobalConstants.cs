namespace AccountingSystem.Shared.GlobalConstants
{
    public class GlobalConstants
    {
        public class TransactionType
        {
            public int TransactionTypeId { get; set; }
            public string TransactionTypeName { get; set; } = string.Empty;
        }

        public static List<TransactionType> TransactionTypeList = new List<TransactionType>()
        {
            new TransactionType{ TransactionTypeId = 1, TransactionTypeName= "In" },
            new TransactionType{ TransactionTypeId = 2, TransactionTypeName= "Out" },
            new TransactionType{ TransactionTypeId = 3, TransactionTypeName= "Internal Use" },
        };
    }
}