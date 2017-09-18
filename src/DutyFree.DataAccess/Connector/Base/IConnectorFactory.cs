namespace DutyFree.DataAccess.Connector.Base
{
    public interface IConnectorFactory
    {
        IConnector<Rq, Rs> GetConnector<Rq, Rs>();
    }
}