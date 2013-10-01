﻿using AmplaWeb.Data;
using AmplaWeb.Data.AmplaRepository;
using AmplaWeb.Data.InMemory;
using AmplaWeb.Sample.Models;
using Autofac;
using Autofac.Integration.Mvc;

namespace AmplaWeb.Sample.Modules
{
    public class ControllerInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            string type = "Ampla";

            if (type == "Ampla")
            {
                builder.Register(c => new AmplaRepositorySet("User", "password")).As<IRepositorySet>();
            }
            else
            {
                InMemoryRepositorySet repositorySet = new InMemoryRepositorySet();
                builder.RegisterInstance(repositorySet).As<IRepositorySet>().SingleInstance();
                IRepository<IngotCastModel> castRepository = repositorySet.GetRepository<IngotCastModel>();
                castRepository.Add(new IngotCastModel { CastNo = "Cast 123" });
                castRepository.Add(new IngotCastModel { CastNo = "Cast 234" });

                IRepository<IngotBundleModel> bundleRepository = repositorySet.GetRepository<IngotBundleModel>();
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 123" });
                bundleRepository.Add(new IngotBundleModel { CastNo = "Cast 234" });

            }
            
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

        }
    }
}