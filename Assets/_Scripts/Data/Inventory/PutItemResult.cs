namespace Data.Inventory
{
    public record PutItemResult
    {
        public PutItemResultType ResultType { get; private set; }
        public Item Item { get; private set; }
        
        public static PutItemResult FullDeposit() => new() { ResultType = PutItemResultType.FullDeposit };
        public static PutItemResult PartialDeposit(Item item) => new() { ResultType = PutItemResultType.PartialDeposit, Item = item };
        public static PutItemResult Clobber(Item item) => new() { ResultType = PutItemResultType.Clobber, Item = item };
    }
    
    public enum PutItemResultType
    {
        FullDeposit,
        PartialDeposit,
        Clobber
    }
}