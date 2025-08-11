using System;

namespace CFAN.Common.WPF
{
    public class ConverterME<T>:IMarkupExtension<IValueConverter> 
        where T : IValueConverter, new()
    {
        public IValueConverter ProvideValue(IServiceProvider serviceProvider)
        {
            return new T();
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}