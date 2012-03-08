﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Client.Document;

namespace Homesite.App.Providers.Storage
{
    public class StorageClient : IStorageClient
    {
        readonly Lazy<DocumentStore> _docStore = new Lazy<DocumentStore>(() =>
        {
            var documentStore = new DocumentStore { ConnectionStringName = "RAVENHQ_CONNECTION_STRING" };
            documentStore.Initialize();
            return documentStore;
        }, LazyThreadSafetyMode.PublicationOnly);
        
        protected DocumentStore DocStore
        {
            get { return _docStore.Value; }
        }

        public void Store(object doc)
        {
            using(var session = DocStore.OpenSession())
            {
                session.Store(doc);
                session.SaveChanges();
            }
        }

        public IList<T> Query<T>()
        {
            using (var session = DocStore.OpenSession())
            {
                return session.Query<T>().ToList();                
            }
        }
    }
}