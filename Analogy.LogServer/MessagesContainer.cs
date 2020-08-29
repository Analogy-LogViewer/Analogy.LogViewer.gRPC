﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Analogy.LogServer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Analogy.LogServer
{
    public class MessagesContainer
    {
        private readonly BlockingCollection<AnalogyLogMessage> messages;

        private Task _consumer;
        private readonly List<ILogConsumer> _consumers;
        private ILogger<MessagesContainer> _logger;
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();

        public MessagesContainer(ILogger<MessagesContainer> logger)
        {
            _logger = logger;
            _consumers = new List<ILogConsumer>();
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
                _logger.LogInformation("Add new consumer: {consumer}", consumer);
                _sync.EnterWriteLock();
                _consumers.Add(consumer);
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
    }
}
