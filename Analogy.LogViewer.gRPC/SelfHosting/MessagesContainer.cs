﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.gRPC.SelfHosting
{
    public class MessagesContainer
    {
        private readonly BlockingCollection<AnalogyLogMessage> messages;

        private Task _consumer;
        private readonly List<ILogConsumer> _consumers;
        private ILogger<MessagesContainer> _logger;
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();
        public MessagesContainer(GRPCLogConsumer grpcLogConsumer, ILogger<MessagesContainer> logger)
        {
            _consumers = new List<ILogConsumer> { grpcLogConsumer };
            _logger = logger;
            messages = new BlockingCollection<AnalogyLogMessage>();
            _consumer = Task.Factory.StartNew(async () =>
            {
                foreach (var msg in messages.GetConsumingEnumerable())
                {
                    try
                    {
                        _sync.EnterReadLock();
                        foreach (ILogConsumer consumer in _consumers)
                        {
                            await consumer.ConsumeLog(msg);
                        }
                    }
                    finally
                    {
                        _sync.ExitReadLock();
                    }
                }
            });
        }

        public void AddMessage(AnalogyLogMessage m) => messages.Add(m);

        public void AddConsumer(ILogConsumer consumer)
        {
            try
            {
                _sync.EnterWriteLock();
                if (!_consumers.Contains(consumer))
                {
                    _logger.LogInformation("Add new consumer: {consumer}", consumer);
                    _consumers.Add(consumer);
                }
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }

        public void RemoveConsumer(ILogConsumer consumer)
        {
            try
            {
                _sync.EnterWriteLock();
                _consumers.Remove(consumer);
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }


        public void Stop()
        {
            messages.CompleteAdding();
        }
    }
}
