
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using NServiceBus.Persistence.Sql;

namespace NSBTestHostException
{
    using System;
    using NServiceBus;

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            //TODO: NServiceBus provides multiple durable storage options, including SQL Server, RavenDB, and Azure Storage Persistence.
            // Refer to the documentation for more details on specific options.
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>().Schema("persistence");
            persistence.ConnectionBuilder(() => new SqlConnection("Server=(local); Integrated Security=SSPI; Initial Catalog=Test;MultipleActiveResultSets=true;Max Pool Size=100; Min Pool Size=0;"));
            persistence.SubscriptionSettings().CacheFor(TimeSpan.FromMinutes(1));

            endpointConfiguration.UseTransport<LearningTransport>();
            // NServiceBus will move messages that fail repeatedly to a separate "error" queue. We recommend
            // that you start with a shared error queue for all your endpoints for easy integration with ServiceControl.
            endpointConfiguration.SendFailedMessagesTo("error");

            // NServiceBus will store a copy of each successfully process message in a separate "audit" queue. We recommend
            // that you start with a shared audit queue for all your endpoints for easy integration with ServiceControl.
            endpointConfiguration.AuditProcessedMessagesTo("audit");
        }
    }
}
