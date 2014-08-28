services-components-rabbitmq
============================

# Library for accessing RabbitMq using standardized interface with support for AMQP URI's

Provides support for OpenTable applications using RabbitMq.
Has one dependency beyond the .Net Base Class Libraries: RabbitMQ.Client.dll 
As of 8/2014, supports only publishing.

  * IMessageQueuePublisher is a general interface for publishing to a message queue.

Nuget package ID: OpenTable.Services.Components.RabbitMq

## Other components depending on this component

None

## Related OpenTable links

  * https://wiki.otcorp.opentable.com/display/CP/URIs+for+Delivery+to+a+Message+Queue

## Application using this component

  * https://github.com/rmacdonaldsmith/forgetmenot.git