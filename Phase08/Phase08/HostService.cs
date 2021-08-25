using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic.CompilerServices;
using Phase08.Interfaces;

namespace Phase08
{
    public static class HostService
    {
        public static ServiceProvider ServiceProvider {get; set;}

        public static void InitServiceProvider()
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((_, service) =>
            {
                service.AddSingleton<IParser<string>, WordParser>();
                service.AddSingleton<IParser<string[]>,SentenceParser>();
            });
        }
        
    }


    public class ServiceFactory<TKEY>
    {
        private Dictionary<Type,Type> _dictionary = new Dictionary<Type,Type>();

        public void AddService(Type T,TKEY service)
        {
            if (typeof(TKEY) == service.GetType())
            {
                _dictionary[T] = service.GetType();
            }
        }
        
        public TKEY GetService(Type ObjectType)
        {
            var service = _dictionary[ObjectType];
            if (service == null) throw new Exception();

            return (TKEY) HostService.ServiceProvider.GetService(service);
        }
    }
    
    
    
}