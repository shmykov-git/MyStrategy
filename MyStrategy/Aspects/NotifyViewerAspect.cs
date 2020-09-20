using System;
using System.Reflection;
using AspectInjector.Broker;
using MyStrategy.DataModel;
using MyStrategy.Tools;
using Suit;
using Suit.Logs;

namespace MyStrategy.Aspects
{
    // TODO: performance
	[Aspect(Scope.Global)]
	[Injection(typeof(NotifyViewerAspect))]
	public class NotifyViewerAspect : Attribute
    {
        private IViewer _viewer;
        private IViewer viewer => _viewer ?? (_viewer = IoC.Get<IViewer>());

        private ILog _log;
		private ILog log => _log ?? (_log = IoC.Get<ILog>());

        [Advice(Kind.Around, Targets = Target.Setter)]
		public object Decorator(
			[Argument(Source.Metadata)] MethodBase methodInfo,
            [Argument(Source.Instance)] object instance,
            [Argument(Source.Target)] Func<object[], object> method,
			[Argument(Source.Arguments)] object[] args)
		{
			if (!IoC.IsConfigured)
				return method(args);

            var propertyInfo = typeof(Unit).GetProperty(methodInfo.Name.Substring(4));

            method(args);

            viewer.OnPropertyChange((Unit)instance, propertyInfo, args[0]);

            return null;
        }
    }
}
