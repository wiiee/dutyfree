namespace DutyFree.Platform.Enum
{
    public enum ProductStatus
    {
        //入库
        Inbound,
        //过审，可以售卖
        Onsale,
        //暂停销售
        Suspend,
        //出库，不再销售
        Outbound
    }
}