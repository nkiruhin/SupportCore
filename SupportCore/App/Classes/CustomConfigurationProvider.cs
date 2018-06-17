using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupportCore.Models;

namespace CustomConfigurationProvider
{
    public class EFConfigProvider : ConfigurationProvider
    {
        public EFConfigProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        Action<DbContextOptionsBuilder> OptionsAction { get; }

        // Load config data from EF DB.
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<Context>();
            OptionsAction(builder);

            using (var dbContext = new Context(builder.Options))
            {
                dbContext.Database.Migrate();
                //if (!dbContext.Configuration.Any())
                //{
                //    dbContext.Configuration.Add(new Configuration { Name = "Signature", Value = "С Уважением служба технической поддержки" });
                //    dbContext.SaveChangesAsync();
                //}
                //Data = !dbContext.Configuration.Any()
                //    ? null
                //    : dbContext.Configuration.ToDictionary(c => c.Name, c => c.Value);
                Data = dbContext.Configuration.ToDictionary(c => c.Name, c => c.Value);
            }
        }

        //private static IDictionary<string, string> CreateAndSaveDefaultValues(
        //    Context dbContext)
        //{
        //    var configValues = new Dictionary<string, string>
        //        {
        //            { "key1", "value_from_ef_1" },
        //            { "key2", "value_from_ef_2" }
        //        };
        //    dbContext.Configuration.AddRange(configValues
        //        .Select(kvp => new Configuration { Name = kvp.Key, Value = kvp.Value })
        //        .ToArray());
        //    dbContext.SaveChanges();
        //    return configValues;
        //}
    }


    public class EFConfigSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public EFConfigSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigProvider(_optionsAction);
        }
    }
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(
            this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EFConfigSource(setup));
        }
    }
}