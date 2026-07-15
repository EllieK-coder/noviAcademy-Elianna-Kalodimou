using Autofac;
using MediatR;
using NoviCode.Decorators;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Keep application registrations here. MediatR is registered through IServiceCollection in Program.cs.
        }
    }
}
